#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Api/BurgerLink.Api/BurgerLink.Api.csproj", "Api/BurgerLink.Api/"]
COPY ["services/inventory/BurgerLink.Inventory.Contracts/BurgerLink.Inventory.Contracts.csproj", "services/inventory/BurgerLink.Inventory.Contracts/"]
COPY ["services/order/BurgerLink.Order.Contracts/BurgerLink.Order.Contracts.csproj", "services/order/BurgerLink.Order.Contracts/"]
COPY ["shared/BurgerLink.Shared/BurgerLink.Shared.csproj", "shared/BurgerLink.Shared/"]
RUN dotnet restore "Api/BurgerLink.Api/BurgerLink.Api.csproj"
COPY . .
WORKDIR "/src/Api/BurgerLink.Api"
RUN dotnet build "BurgerLink.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BurgerLink.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BurgerLink.Api.dll"]