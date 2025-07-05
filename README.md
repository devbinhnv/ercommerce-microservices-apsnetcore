## Ercommerse Microservices with ASP.Net Core

** Developement Environment **
## Prepare environment

* Install dotnet core version in file `global.json`
* IDE: Visual Studio 2022+, Rider or Visual Studio Code
* Docker Desktop
## How to run the project

Run command for build project
```Powershell
dotnet build
```
Go to folder contain file `docker-compose`

1. Using docker-compose
```Powershell
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans
```

## Application URLs - LOCAL Environment (Docker Container):
- Product API: http://localhost:6002/api/products
- Customer API: http://localhost:6003/api/customers
- Basket API: http://localhost:6003/api/baskets
- Ordering API: http://localhost:6005/api/v1/Order

## Docker Application URLs - LOCAL Environment (Docker Container):
- Portainer: http://localhost:9000 - username: admin ; pass: admin1234
- Kibana: http://localhost:5601 - username: elastic ; pass: admin
- RabbitMQ: http://localhost:15672 - username: guest ; pass: guest

2. Using Visual Studio 2022
- Open ercommerce-microservices-apsnetcore.sln - `ercommerce-microservices-apsnetcore.sln`
- Run Compound to start multi projects
---
## Application URLs - DEVELOPMENT Environment:
- Product API: http://localhost:5002/api/products
- Customer API: http://localhost:5003/api/customers
- Basket API: http://localhost:5004/api/baskets
- Ordering API: http://localhost:5005/api/v1/Order
---
## Application URLs - PRODUCTION Environment:
- Not available
---
## Packages References

## Install Environment

- https://dotnet.microsoft.com/download/dotnet/8.0
- https://visualstudio.microsoft.com/

## References URLS

## Docker Commands: (cd into folder contain file `docker-compose.yml`, `docker-compose.override.yml`)

- Up & running:
```Powershell
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans --build
```
- Stop & Removing:
```Powershell
docker-compose down
```

## Useful commands:

- ASPNETCORE_ENVIRONMENT=Production dotnet ef database update
- dotnet watch run --environment "Development"
- dotnet restore
- dotnet build
- Migration commands for Ordering API:
  - cd into Ordering folder
  - (Terminal) "dotnet ef migrations add "SampleMigration" -p Ordering.Infrastructure --startup-project Ordering.API --output-dir Persistence/Migrations"
	(Package Manager Console/Visual Studio) "Add-Migration SampleMigration -p Ordering.Infrastructure -startupProject Ordering.API -OutputDir Persistence/Migrations"
  - dotnet ef migrations remove -p Ordering.Infrastructure --startup-project Ordering.API
  - dotnet ef database update -p Ordering.Infrastructure --startup-project Ordering.API
  - dotnet ef migrations add InitialCreate --project "service".Infrastructure --startup-project "service".API --output-dir Migrations
## Run redis cli with docker on Windows:
 - docker excec -it basketdb sh