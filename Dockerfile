FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first to cache dependencies
COPY ["QuantityMeasurementApp.slnx", "./"]
COPY ["QuantityMeasurementWebAPI/QuantityMeasurementWebAPI.csproj", "QuantityMeasurementWebAPI/"]
COPY ["QuantityMeasurementApp.Service/QuantityMeasurementApp.Service.csproj", "QuantityMeasurementApp.Service/"]
COPY ["QuantityMeasurementApp.Repository/QuantityMeasurementApp.Repository.csproj", "QuantityMeasurementApp.Repository/"]
COPY ["QuantityMeasurementApp.Entity/QuantityMeasurementApp.Entity.csproj", "QuantityMeasurementApp.Entity/"]
COPY ["QuantityMeasurementApp.Tests/QuantityMeasurementApp.Tests.csproj", "QuantityMeasurementApp.Tests/"]

# Restore dependencies
RUN dotnet restore "QuantityMeasurementWebAPI/QuantityMeasurementWebAPI.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish the Web API project
WORKDIR "/src/QuantityMeasurementWebAPI"
RUN dotnet publish "QuantityMeasurementWebAPI.csproj" -c Release -o /app/publish

# Use ASP.NET runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (Render sets PORT environment variable, usually defaults to 8080 or 80)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "QuantityMeasurementWebAPI.dll"]
