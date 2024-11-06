FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ShopManager/ShopManager.csproj", "ShopManager/"]
RUN dotnet restore "ShopManager/ShopManager.csproj"
COPY . .
WORKDIR "/src/ShopManager"
RUN dotnet build "ShopManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopManager.dll"]