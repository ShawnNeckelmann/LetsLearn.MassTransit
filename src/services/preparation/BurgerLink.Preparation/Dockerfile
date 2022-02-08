#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/preparation/BurgerLink.Preparation/BurgerLink.Preparation.csproj", "services/preparation/BurgerLink.Preparation/"]
COPY ["services/preparation/BurgerLink.Preparation.Contracts/BurgerLink.Preparation.Contracts.csproj", "services/preparation/BurgerLink.Preparation.Contracts/"]
COPY ["shared/BurgerLink.Shared/BurgerLink.Shared.csproj", "shared/BurgerLink.Shared/"]
RUN dotnet restore "services/preparation/BurgerLink.Preparation/BurgerLink.Preparation.csproj"
COPY . .
WORKDIR "/src/services/preparation/BurgerLink.Preparation"
RUN dotnet build "BurgerLink.Preparation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BurgerLink.Preparation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BurgerLink.Preparation.dll"]
