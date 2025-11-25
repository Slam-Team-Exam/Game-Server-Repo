# Use SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["SLAMServer.csproj", "."]
RUN dotnet restore "SLAMServer.csproj"

# Copy all source files and publish
COPY . .
RUN dotnet publish "SLAMServer.csproj" -c Release -o /app/publish

# Use the ASP.NET runtime for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Set the entrypoint
ENTRYPOINT ["dotnet", "SLAMServer.dll"]
