#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["pg_Sample/src/WebApi/WebApi.csproj", "pg_Sample/src/WebApi/"]
COPY ["pg_Sample/src/Application/Application.csproj", "pg_Sample/src/Application/"]
COPY ["pg_Sample/src/Domain/Domain.csproj", "pg_Sample/src/Domain/"]
COPY ["pg_Sample/src/Infrastructure/Infrastructure.csproj", "pg_Sample/src/Infrastructure/"]
RUN dotnet restore "./pg_Sample/src/WebApi/./WebApi.csproj"
COPY . .
WORKDIR "/src/pg_Sample/src/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Add version argument
ARG VERSION=latest
# Label the image with the version
LABEL version=$VERSION


ENTRYPOINT ["dotnet", "WebApi.dll"]