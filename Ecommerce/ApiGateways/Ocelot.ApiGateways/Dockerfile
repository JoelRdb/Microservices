# Consultez https://aka.ms/customizecontainer pour savoir comment personnaliser votre conteneur de d�bogage et comment Visual�Studio utilise ce Dockerfile pour g�n�rer vos images afin d�acc�l�rer le d�bogage.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ApiGateways/Ocelot.ApiGateways/Ocelot.ApiGateways.csproj", "ApiGateways/Ocelot.ApiGateways/"]

RUN dotnet restore "ApiGateways/Ocelot.ApiGateways/Ocelot.ApiGateways.csproj"
COPY . .
WORKDIR "/src/ApiGateways/Ocelot.ApiGateways"
RUN dotnet build "Ocelot.ApiGateways.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ocelot.ApiGateways.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ocelot.ApiGateways.dll"]