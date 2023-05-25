#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MyKudos.Kudos.Api/MyKudos.Kudos.Api.csproj", "MyKudos.Kudos.Api/"]
COPY ["MyKudos.Kudos.App/MyKudos.Kudos.App.csproj", "MyKudos.Kudos.App/"]
COPY ["MyKudos.Kudos.Domain/MyKudos.Kudos.Domain.csproj", "MyKudos.Kudos.Domain/"]
COPY ["MyKudos.Kudos.Data/MyKudos.Kudos.Data.csproj", "MyKudos.Kudos.Data/"]
RUN dotnet restore "MyKudos.Kudos.Api/MyKudos.Kudos.Api.csproj"
COPY . .
WORKDIR "/src/MyKudos.Kudos.Api"
RUN dotnet build "MyKudos.Kudos.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyKudos.Kudos.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyKudos.Kudos.Api.dll"]