#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MyKudos.Gateway/MyKudos.Gateway.csproj", "MyKudos.Gateway/"]
COPY ["MyKudos.Gateway.Domain/MyKudos.Gateway.Domain.csproj", "MyKudos.Gateway.Domain/"]
COPY ["MyKudos.Kudos.Domain/MyKudos.Kudos.Domain.csproj", "MyKudos.Kudos.Domain/"]
COPY ["MyKudos.MessageSender/MyKudos.MessageSender.csproj", "MyKudos.MessageSender/"]
COPY ["MyKudos.RestHelper/MyKudos.Communication.Helper.csproj", "MyKudos.RestHelper/"]
RUN dotnet restore "MyKudos.Gateway/MyKudos.Gateway.csproj"
COPY . .
WORKDIR "/src/MyKudos.Gateway"
RUN dotnet build "MyKudos.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyKudos.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyKudos.Gateway.dll"]