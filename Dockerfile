# Stage 1 - Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore packages
COPY . .
RUN dotnet restore

# Publish the app
RUN dotnet publish -c Release -o out

# Stage 2 - Run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/out .

# Tell ASP.NET to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "StudyMonitor.dll"]