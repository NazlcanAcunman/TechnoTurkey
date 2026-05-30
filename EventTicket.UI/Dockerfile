FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["EventTicket.UI/EventTicket.UI.csproj", "EventTicket.UI/"]
COPY ["EventTicket.Core/EventTicket.Core.csproj", "EventTicket.Core/"]

RUN dotnet restore "EventTicket.UI/EventTicket.UI.csproj"

COPY . .
WORKDIR "/src/EventTicket.UI"
ARG RAILWAY_GIT_COMMIT_SHA=unknown
ARG RENDER_GIT_COMMIT=unknown
RUN echo "Build: ${RAILWAY_GIT_COMMIT_SHA}${RENDER_GIT_COMMIT}" && dotnet build "EventTicket.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventTicket.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventTicket.UI.dll"]
