#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ReservationManager.API/ReservationManager.API.csproj", "ReservationManager.API/"]
COPY ["ReservationManager.Core/ReservationManager.Core.csproj", "ReservationManager.Core/"]
COPY ["ReservationManager.Core/ReservationManager.Cache.csproj", "ReservationManager.Cache/"]
COPY ["ReservationManager.DomainModel/ReservationManager.DomainModel.csproj", "ReservationManager.DomainModel/"]
COPY ["ReservationManager.Persistence/ReservationManager.Persistence.csproj", "ReservationManager.Persistence/"]
RUN dotnet restore "ReservationManager.API/ReservationManager.API.csproj"
COPY . .
WORKDIR "/src/ReservationManager.API"
RUN dotnet build "ReservationManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReservationManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReservationManager.API.dll"]