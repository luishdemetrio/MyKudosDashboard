#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MyKudos.Recognition.Api/MyKudos.Recognition.Api.csproj", "MyKudos.Recognition.Api/"]
COPY ["MyKudos.Recognition.App/MyKudos.Recognition.App.csproj", "MyKudos.Recognition.App/"]
COPY ["MyKudos.Recognition.Domain/MyKudos.Recognition.Domain.csproj", "MyKudos.Recognition.Domain/"]
COPY ["MyKudos.Domain.Core/MyKudos.Domain.Core.csproj", "MyKudos.Domain.Core/"]
COPY ["MyKudos.Recognition.Data/MyKudos.Recognition.Data.csproj", "MyKudos.Recognition.Data/"]
RUN dotnet restore "MyKudos.Recognition.Api/MyKudos.Recognition.Api.csproj"
COPY . .
WORKDIR "/src/MyKudos.Recognition.Api"
RUN dotnet build "MyKudos.Recognition.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyKudos.Recognition.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyKudos.Recognition.Api.dll"]