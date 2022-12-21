FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["kittyshop.csproj", "./"]
RUN dotnet restore "kittyshop.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "kittyshop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "kittyshop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kittyshop.dll"]
