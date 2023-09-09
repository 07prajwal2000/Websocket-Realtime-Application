#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Messaging.Server/Messaging.Server.csproj", "Messaging.Server/"]
COPY ["Messaging.Shared/Messaging.Shared.csproj", "Messaging.Shared/"]
RUN dotnet restore "Messaging.Server/Messaging.Server.csproj"
COPY . .
WORKDIR "/src/Messaging.Server"
RUN dotnet build "Messaging.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messaging.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
EXPOSE 5555
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messaging.Server.dll"]