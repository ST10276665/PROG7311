# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore
COPY PROG7311.API/PROG7311.API.csproj PROG7311.API/
RUN dotnet restore PROG7311.API/PROG7311.API.csproj

# copy everything and publish
COPY . .
WORKDIR /src/PROG7311.API
RUN dotnet publish PROG7311.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "PROG7311.API.dll"]
