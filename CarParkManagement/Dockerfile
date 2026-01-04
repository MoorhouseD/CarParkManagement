# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarParkManagement/CarParkManagement.csproj", "CarParkManagement/"]
RUN dotnet restore "./CarParkManagement/CarParkManagement.csproj"
COPY . .
WORKDIR "/src/CarParkManagement"
RUN dotnet build "./CarParkManagement.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarParkManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
# Ensure the container runs in Development environment by default
ENV ASPNETCORE_ENVIRONMENT=Development
# Expose the app on port 8080 by default
ENV ASPNETCORE_URLS="http://+:8080"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarParkManagement.dll"]