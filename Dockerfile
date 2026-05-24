FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN PROJECT_PATH=$(find . -name "AquaVivarium.csproj" | head -n 1) && \
    echo "¡Archivo encontrado en: $PROJECT_PATH!" && \
    dotnet restore "$PROJECT_PATH" && \
    dotnet publish "$PROJECT_PATH" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AquaVivarium.dll"]