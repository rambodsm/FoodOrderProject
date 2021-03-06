##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
##Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
##For more information, please see https://aka.ms/containercompat
#
#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["FoodOrder.Presentation/FoodOrder.Presentation.csproj", "FoodOrder.Presentation/"]
#COPY ["FoodOrder.WebFramework/FoodOrder.WebFramework.csproj", "FoodOrder.WebFramework/"]
#COPY ["FoodOrder.Service/FoodOrder.Service.csproj", "FoodOrder.Service/"]
#COPY ["FoodOrder.Domain/FoodOrder.Domain.csproj", "FoodOrder.Domain/"]
#COPY ["FoodOrder.Common/FoodOrder.Common.csproj", "FoodOrder.Common/"]
#COPY ["FoodOrder.Infrastructure/FoodOrder.Infrastructure.csproj", "FoodOrder.Infrastructure/"]
#RUN dotnet restore "FoodOrder.Presentation/FoodOrder.Presentation.csproj"
#COPY . .
#WORKDIR "/src/FoodOrder.Presentation"
#RUN dotnet build "FoodOrder.Presentation.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "FoodOrder.Presentation.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "FoodOrder.Presentation.dll"]





FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app


COPY *.csproj ./
#RUN dotnet restore "FoodOrder.Presentation.csproj"


COPY . ./
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 4040
ENV ASPNETCORE_URLS=http://localhost:4000
ENTRYPOINT ["dotnet", "FoodOrder.Presentation.dll"]