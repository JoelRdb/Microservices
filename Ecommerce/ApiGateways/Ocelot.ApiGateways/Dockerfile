# Consultez https://aka.ms/customizecontainer pour savoir comment personnaliser votre conteneur de débogage et comment Visual Studio utilise ce Dockerfile pour générer vos images afin d’accélérer le débogage.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ApiGateways/Ocelot.ApiGateways/Ocelot.ApiGateways.csproj", "ApiGateways/Ocelot.ApiGateways/"]

RUN dotnet restore "ApiGateways/Ocelot.ApiGateways/Ocelot.ApiGateways.csproj"
COPY . .
WORKDIR "/src/ApiGateways/Ocelot.API"
RUN dotnet build "Ocelot.ApiGateways.csproj" -c Release -o /app/build

FROM buils AS publish
RUN dotnet publish "Ocelot.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ocelot.ApiGateway.dll"]