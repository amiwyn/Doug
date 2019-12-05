FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Doug/Doug.csproj", "Doug/"]
RUN dotnet restore "Doug/Doug.csproj"
COPY . .
WORKDIR "/src/Doug"
RUN dotnet build "Doug.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Doug.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Doug.dll"]