FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY ./src/ EvaExchange.API/
# Restore as distinct layers
RUN dotnet restore EvaExchange.API/
# Build and publish a release
RUN dotnet publish EvaExchange.API/ -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "EvaExchange.API.dll"]