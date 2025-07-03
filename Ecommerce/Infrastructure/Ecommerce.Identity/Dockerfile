# Consultez https://aka.ms/customizecontainer pour savoir comment personnaliser votre conteneur de débogage et comment Visual Studio utilise ce Dockerfile pour générer vos images afin d’accélérer le débogage.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 9009

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Infrastructure/Ecommerce.Identity/Ecommerce.Identity.csproj", "Infrastructure/Ecommerce.Identity/"]
RUN dotnet restore "Infrastructure/Ecommerce.Identity/Ecommerce.Identity.csproj"
COPY . .
WORKDIR "/src/Infrastructure/Ecommerce.Identity"
RUN dotnet build "Ecommerce.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Identity.dll"]