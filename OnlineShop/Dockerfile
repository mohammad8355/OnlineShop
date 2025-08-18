# ----------------------------
# Base runtime image
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ----------------------------
# Build stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app_build

# Copy project files for caching
COPY OnlineShop/PresentationLayer.csproj OnlineShop/
COPY BusinessLogicLayer/BusinessLogicLayer.csproj BusinessLogicLayer/
COPY DataAccessLayer/DataAccessLayer.csproj DataAccessLayer/
COPY Utility/Utility.csproj Utility/
COPY Infrustructure/Infrustructure.csproj Infrustructure/

# Restore dependencies
RUN dotnet restore OnlineShop/PresentationLayer.csproj

# Copy the full source code
COPY . .

# Set working directory to the PresentationLayer project
WORKDIR /app_build/OnlineShop
RUN dotnet build PresentationLayer.csproj -c $BUILD_CONFIGURATION -o /app/build

# ----------------------------
# Publish stage
# ----------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish PresentationLayer.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ----------------------------
# Final image
# ----------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PresentationLayer.dll"]
