FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SmartDepot.API/SmartDepot.API.csproj", "SmartDepot.API/"]
COPY ["SmartDepot.Persistence/SmartDepot.Persistence.csproj", "SmartDepot.Persistence/"]
COPY ["SmartDepot.Domain/SmartDepot.Domain.csproj", "SmartDepot.Domain/"]
COPY ["SmartDepot.Application/SmartDepot.Application.csproj", "SmartDepot.Application/"]
COPY ["SmartDepot.Infrastructure/SmartDepot.Infrastructure.csproj", "SmartDepot.Infrastructure/"]
RUN dotnet restore "SmartDepot.API/SmartDepot.API.csproj"
COPY . .
WORKDIR "/src/SmartDepot.API"
RUN dotnet build "SmartDepot.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SmartDepot.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartDepot.API.dll"]
