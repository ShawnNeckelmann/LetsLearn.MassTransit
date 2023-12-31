#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Build.targets", "."]
COPY ["services/inventory/BurgerLink.Inventory/BurgerLink.Inventory.csproj", "services/inventory/BurgerLink.Inventory/"]
COPY ["shared/BurgerLink.Shared/BurgerLink.Shared.csproj", "shared/BurgerLink.Shared/"]
COPY ["services/inventory/BurgerLink.Inventory.Contracts/BurgerLink.Inventory.Contracts.csproj", "services/inventory/BurgerLink.Inventory.Contracts/"]
RUN dotnet restore "services/inventory/BurgerLink.Inventory/BurgerLink.Inventory.csproj"
COPY . .
WORKDIR "/src/services/inventory/BurgerLink.Inventory"
RUN dotnet build "BurgerLink.Inventory.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BurgerLink.Inventory.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BurgerLink.Inventory.dll"]