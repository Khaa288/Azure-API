# Azure API

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Run on Local Envronment](#run-on-local-envronment)
- [Run on Staging Environment (Docker Compose)](#run-on-staging-environment-docker-compose)
- [Run on Production Environment (Azure)](#run-on-production-environment-azure)

## Overview
RESTful API project built with .NET Core enable the capability to deploy into multiple environments, including **Development (local), Staging (Docker), Production (Azure Web App Service)**. 

## Features
- **CRUD Operations**, including: 
  - **GET:**    `api/note`
  - **POST:**   `api/note`
    ```json
    {
      "noteTitle": "title here",
      "noteDetail": "detail here"
    }
    ```
  - **PUT:**    `api/note{noteId}`
    ```json
    {
      "noteTitle": "title here",
      "noteDetail": "detail here"
    }
    ```
  - **DELETE:** `api/note/{noteId}`
- **Database Integration**: Supports [SQL Server, Azure SQL].

## Prerequisites
Before running this project, ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or any supported database
- [Docker](https://www.docker.com/products/docker-desktop/) (for staging environment)
- [Azure Account](https://azure.microsoft.com/en-us/get-started/azure-portal) (for production environment)
<br>

## Run on Local Envronment
Configure the database connection: Update the `appsettings.Development.json` file with your database

```json
"ConnectionStrings": {
  "LOCAL_SQL_CONNECTIONSTRING": "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=azure-api-db;Integrated Security=True;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;"
}
```

Start the application using the following command
```cmd
dotnet run
```

or 
```cmd
dotnet run --environment Development
```

Test the API
```http
http://localhost:5059/api/note
```

By default, the API will run at http://localhost:5059
<br>
<br>

## Run on Staging Environment (Docker Compose)
Configure the database connection: Update the `appsettings.Staging.json` file with your database

```json
"ConnectionStrings": {
  "STAGING_SQL_CONNECTIONSTRING": "Server=azure-staging-database-container,1433;Initial Catalog=AuthModuleDb;User Id=sa;Password=4zureD0ckerD4t4b4s3;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;"
}
```

**Note:** The connection string here is the connection to the database within `dockercompose` container. 

Configure `docker-compose.yml` file, by default will be:
```dockerfile
services:
  azure-api:
    image: azure-api-image
    container_name: azure-api-container
    environment:
      ASPNETCORE_ENVIRONMENT: Staging
    build:
      context: .
      dockerfile: ./Dockerfile
    ports:
      - 8000:5059
    restart: on-failure

  azure-staging-database:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: azure-staging-database-container
    environment:
      MSSQL_SA_PASSWORD: "4zureD0ckerD4t4b4s3"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
    volumes:
      - staging_mssql_data:/var/lib/mssql
    restart: on-failure
  
volumes:
  staging_mssql_data:
```

Run docker compose
```dockerfile
docker compose up
```

Test the API
```http
http://localhost:8000/api/note
```

By default, the API will run at http://localhost:8000, port **8000** is the binded from port **5059** within `docker-compose.yml`
<br>
<br>

## Run on Production Environment (Azure)
**Prerequisite**: An active Azure SQL Database

Configure the database connection: Update the `appsettings.json` file with your database

```json
"ConnectionStrings": {
    "AZURE_SQL_CONNECTIONSTRING": "Connection string to Azure SQL Database";
  }
```

Start the application using the following command
```cmd
dotnet run --environment Production
```

Or push the docker image into Docker Hub 

Build docker image
```dockerfile
docker build -t your-dockerhub-account/azure-prod-api .
```


Login to docker hub account
```dockerfile
docker login
```

Push docker image into docker hub
```dockerfile
docker push your-dockerhub-account/azure-prod-api:tagname
```

And deploy into [Azure Web App Service](https://learn.microsoft.com/en-us/azure/app-service/). 

Test the API (example)
```http
https://azure-production-api.azurewebsites.net/api/note
```
