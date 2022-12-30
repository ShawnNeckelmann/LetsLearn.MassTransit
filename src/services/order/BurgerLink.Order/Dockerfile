#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["services/order/BurgerLink.Order/BurgerLink.Order.csproj", "services/order/BurgerLink.Order/"]
COPY ["shared/BurgerLink.Shared/BurgerLink.Shared.csproj", "shared/BurgerLink.Shared/"]
COPY ["services/preparation/BurgerLink.Preparation.Contracts/BurgerLink.Preparation.Contracts.csproj", "services/preparation/BurgerLink.Preparation.Contracts/"]
COPY ["services/order/BurgerLink.Order.Contracts/BurgerLink.Order.Contracts.csproj", "services/order/BurgerLink.Order.Contracts/"]
RUN dotnet restore "services/order/BurgerLink.Order/BurgerLink.Order.csproj"
COPY . .
WORKDIR "/src/services/order/BurgerLink.Order"
RUN dotnet build "BurgerLink.Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BurgerLink.Order.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BurgerLink.Order.dll"]