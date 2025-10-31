#!/bin/bash
# Integration test script for YARP-EE
# This script verifies the complete functionality of the YARP-EE system

set -e

API_URL="${API_URL:-http://localhost:8080}"
TIMEOUT=60
MAX_RETRIES=30

echo "🔍 YARP-EE Integration Test"
echo "Testing API at: $API_URL"
echo ""

# Wait for API to be ready
echo "⏳ Waiting for API to be ready..."
for i in $(seq 1 $MAX_RETRIES); do
    if curl -s -f "$API_URL/health/ready" > /dev/null 2>&1; then
        echo "✅ API is ready!"
        break
    fi
    if [ $i -eq $MAX_RETRIES ]; then
        echo "❌ API failed to become ready within timeout"
        exit 1
    fi
    sleep 2
done

echo ""
echo "🧪 Running tests..."
echo ""

# Test 1: Health checks
echo "Test 1: Health checks"
if curl -s -f "$API_URL/health" | grep -q "Healthy"; then
    echo "  ✅ Liveness check passed"
else
    echo "  ❌ Liveness check failed"
    exit 1
fi

if curl -s -f "$API_URL/health/ready" | grep -q "Healthy"; then
    echo "  ✅ Readiness check passed"
else
    echo "  ❌ Readiness check failed"
    exit 1
fi

# Test 2: Get hosts
echo ""
echo "Test 2: List hosts"
HOSTS=$(curl -s "$API_URL/api/hosts")
if [ -n "$HOSTS" ] && [ "$HOSTS" != "[]" ]; then
    echo "  ✅ Hosts retrieved successfully"
    HOST_ID=$(echo "$HOSTS" | grep -o '"id":"[^"]*"' | head -1 | cut -d'"' -f4)
    echo "  📝 Host ID: $HOST_ID"
else
    echo "  ❌ Failed to retrieve hosts"
    exit 1
fi

# Test 3: Create a cluster
echo ""
echo "Test 3: Create cluster"
CLUSTER_RESPONSE=$(curl -s -X POST "$API_URL/api/clusters" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "test-cluster-'$(date +%s)'",
    "loadBalancingPolicy": "RoundRobin",
    "destinations": [
      {
        "address": "http://httpbin.org",
        "healthPath": "/status/200"
      }
    ]
  }')

if echo "$CLUSTER_RESPONSE" | grep -q '"id"'; then
    echo "  ✅ Cluster created successfully"
    CLUSTER_ID=$(echo "$CLUSTER_RESPONSE" | grep -o '"id":"[^"]*"' | head -1 | cut -d'"' -f4)
    echo "  📝 Cluster ID: $CLUSTER_ID"
else
    echo "  ❌ Failed to create cluster"
    echo "  Response: $CLUSTER_RESPONSE"
    exit 1
fi

# Test 4: List clusters
echo ""
echo "Test 4: List clusters"
CLUSTERS=$(curl -s "$API_URL/api/clusters")
if echo "$CLUSTERS" | grep -q "$CLUSTER_ID"; then
    echo "  ✅ Cluster appears in list"
else
    echo "  ❌ Cluster not found in list"
    exit 1
fi

# Test 5: Create a route
echo ""
echo "Test 5: Create route"
ROUTE_RESPONSE=$(curl -s -X POST "$API_URL/api/routes" \
  -H "Content-Type: application/json" \
  -d "{
    \"hostId\": \"$HOST_ID\",
    \"clusterId\": \"$CLUSTER_ID\",
    \"path\": \"/test-$(date +%s)/{**catch-all}\",
    \"order\": 0,
    \"enabled\": true,
    \"methods\": [\"GET\", \"POST\"]
  }")

if echo "$ROUTE_RESPONSE" | grep -q '"id"'; then
    echo "  ✅ Route created successfully"
    ROUTE_ID=$(echo "$ROUTE_RESPONSE" | grep -o '"id":"[^"]*"' | head -1 | cut -d'"' -f4)
    echo "  📝 Route ID: $ROUTE_ID"
else
    echo "  ❌ Failed to create route"
    echo "  Response: $ROUTE_RESPONSE"
    exit 1
fi

# Test 6: List routes
echo ""
echo "Test 6: List routes"
ROUTES=$(curl -s "$API_URL/api/routes")
if echo "$ROUTES" | grep -q "$ROUTE_ID"; then
    echo "  ✅ Route appears in list"
else
    echo "  ❌ Route not found in list"
    exit 1
fi

# Test 7: Update route
echo ""
echo "Test 7: Update route"
UPDATE_RESPONSE=$(curl -s -X PUT "$API_URL/api/routes/$ROUTE_ID" \
  -H "Content-Type: application/json" \
  -d '{
    "enabled": false
  }')

if echo "$UPDATE_RESPONSE" | grep -q '"enabled":false'; then
    echo "  ✅ Route updated successfully"
else
    echo "  ❌ Failed to update route"
    exit 1
fi

# Test 8: Reload proxy configuration
echo ""
echo "Test 8: Reload proxy configuration"
RELOAD_RESPONSE=$(curl -s -X POST "$API_URL/api/proxy/reload")
if echo "$RELOAD_RESPONSE" | grep -q "successfully"; then
    echo "  ✅ Proxy configuration reloaded"
else
    echo "  ❌ Failed to reload proxy configuration"
    exit 1
fi

# Test 9: Delete route
echo ""
echo "Test 9: Delete route"
if curl -s -X DELETE "$API_URL/api/routes/$ROUTE_ID" -o /dev/null -w "%{http_code}" | grep -q "204"; then
    echo "  ✅ Route deleted successfully"
else
    echo "  ❌ Failed to delete route"
    exit 1
fi

# Test 10: Delete cluster
echo ""
echo "Test 10: Delete cluster"
if curl -s -X DELETE "$API_URL/api/clusters/$CLUSTER_ID" -o /dev/null -w "%{http_code}" | grep -q "204"; then
    echo "  ✅ Cluster deleted successfully"
else
    echo "  ❌ Failed to delete cluster"
    exit 1
fi

echo ""
echo "🎉 All tests passed!"
echo ""
echo "Summary:"
echo "  ✅ Health checks working"
echo "  ✅ CRUD operations for clusters working"
echo "  ✅ CRUD operations for routes working"
echo "  ✅ Proxy configuration reload working"
echo "  ✅ Data persistence verified"
