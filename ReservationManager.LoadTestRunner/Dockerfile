FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ReservationManager.LoadTestRunner/ReservationManager.LoadTestRunner.csproj", "ReservationManager.LoadTestRunner/"]
COPY ["ReservationManager.Core/ReservationManager.Core.csproj", "ReservationManager.Core/"]
COPY ["ReservationManager.DomainModel/ReservationManager.DomainModel.csproj", "ReservationManager.DomainModel/"]
RUN dotnet restore "ReservationManager.LoadTestRunner/ReservationManager.LoadTestRunner.csproj"
COPY . .
WORKDIR "/src/ReservationManager.LoadTestRunner"
RUN dotnet build "ReservationManager.LoadTestRunner.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ReservationManager.LoadTestRunner.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReservationManager.LoadTestRunner.dll"]
