FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Services/Ordering/Ordering.API/Ordering.API.csproj","Services/Ordering/Ordering.API/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj","BuildingBlocks/Common.Logging/"]
COPY ["Services/Ordering/Ordering.Application/Ordering.Application.csproj","Services/Ordering/Ordering.Application/"]
COPY ["Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj","Services/Ordering/Ordering.Infrastructure/"]


RUN dotnet restore "Services/Ordering/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/Services/Ordering/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]
