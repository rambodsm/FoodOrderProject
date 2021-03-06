
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FoodOrder.Presentation/FoodOrder.Presentation.csproj", "FoodOrder.Presentation/"]
COPY ["FoodOrder.WebFramework/FoodOrder.WebFramework.csproj", "FoodOrder.WebFramework/"]
COPY ["FoodOrder.Service/FoodOrder.Service.csproj", "FoodOrder.Service/"]
COPY ["FoodOrder.Domain/FoodOrder.Domain.csproj", "FoodOrder.Domain/"]
COPY ["FoodOrder.Common/FoodOrder.Common.csproj", "FoodOrder.Common/"]
COPY ["FoodOrder.Infrastructure/FoodOrder.Infrastructure.csproj", "FoodOrder.Infrastructure/"]
RUN dotnet restore "FoodOrder.Presentation/FoodOrder.Presentation.csproj"
COPY . .
WORKDIR "/src/FoodOrder.Presentation"
RUN dotnet build "FoodOrder.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodOrder.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodOrder.Presentation.dll"]





#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
#WORKDIR /app
#
#
#COPY *.csproj ./
#RUN dotnet restore "FoodOrder.Presentation/FoodOrder.Presentation.csproj"
#
#
#COPY . ./
#RUN dotnet publish -c Release -o out
#
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0
#WORKDIR /app
#COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "aspnetapp.dll"]