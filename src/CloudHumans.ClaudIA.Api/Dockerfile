FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CloudHumans.ClaudIA.Api/CloudHumans.ClaudIA.Api.csproj", "src/CloudHumans.ClaudIA.Api/"]
RUN dotnet restore "src/CloudHumans.ClaudIA.Api/CloudHumans.ClaudIA.Api.csproj"
COPY . .
WORKDIR "/src/src/CloudHumans.ClaudIA.Api"
RUN dotnet build "CloudHumans.ClaudIA.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CloudHumans.ClaudIA.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudHumans.ClaudIA.Api.dll"]
