#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.targets", "."]
COPY ["api/BurgerLink.Api/BurgerLink.Api.csproj", "api/BurgerLink.Api/"]
COPY ["services/inventory/BurgerLink.Inventory.Contracts/BurgerLink.Inventory.Contracts.csproj", "services/inventory/BurgerLink.Inventory.Contracts/"]
COPY ["services/order/BurgerLink.Order.Contracts/BurgerLink.Order.Contracts.csproj", "services/order/BurgerLink.Order.Contracts/"]
COPY ["shared/BurgerLink.Shared/BurgerLink.Shared.csproj", "shared/BurgerLink.Shared/"]
RUN dotnet restore "./api/BurgerLink.Api/./BurgerLink.Api.csproj"
COPY . .
WORKDIR "/src/api/BurgerLink.Api"
RUN dotnet build "./BurgerLink.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BurgerLink.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BurgerLink.Api.dll"]