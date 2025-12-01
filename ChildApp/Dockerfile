# ChildApp Dockerfile - multi-stage build for a console app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore ChildApp/ChildApp.csproj
RUN dotnet publish ChildApp/ChildApp.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ChildApp.dll"]
