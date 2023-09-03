FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 5553

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
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messaging.Server.dll"]