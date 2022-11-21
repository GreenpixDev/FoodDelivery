FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7141
EXPOSE 5258

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FoodDelivery.csproj", "./"]
RUN dotnet restore "FoodDelivery.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "FoodDelivery.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodDelivery.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodDelivery.dll"]
