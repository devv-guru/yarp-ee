FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY YarpEe.sln ./
COPY src/YarpEe.Domain/YarpEe.Domain.csproj ./src/YarpEe.Domain/
COPY src/YarpEe.Application/YarpEe.Application.csproj ./src/YarpEe.Application/
COPY src/YarpEe.Adapters.Persistence.Postgres/YarpEe.Adapters.Persistence.Postgres.csproj ./src/YarpEe.Adapters.Persistence.Postgres/
COPY src/YarpEe.Adapters.Proxy.Yarp/YarpEe.Adapters.Proxy.Yarp.csproj ./src/YarpEe.Adapters.Proxy.Yarp/
COPY src/YarpEe.WebApi/YarpEe.WebApi.csproj ./src/YarpEe.WebApi/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build and publish
WORKDIR /src/src/YarpEe.WebApi
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "YarpEe.WebApi.dll"]
