# .NET SDK ve Runtime imajlarını kullan
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# SDK imajını kullanarak uygulamayı derle
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["restaurant-backend.csproj", "restaurant-backend/"]
RUN dotnet restore "restaurant-backend/restaurant-backend.csproj"

WORKDIR "/src/restaurant-backend"
COPY . .
RUN dotnet build "restaurant-backend.csproj" -c Release -o /app/build

# Yayınlama aşaması
FROM build AS publish
RUN dotnet publish "restaurant-backend.csproj" -c Release -o /app/publish

# Yayınlanan uygulamayı çalışma imajına kopyala
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "restaurant-backend.dll"]

