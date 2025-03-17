# Use the official .NET runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["TestOgSikkerhed.csproj", "./"]
RUN dotnet restore "TestOgSikkerhed.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TestOgSikkerhed.dll"]
