FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["NetCoreUserInterface.csproj", ""]
RUN dotnet restore "NetCoreUserInterface.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "NetCoreUserInterface.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NetCoreUserInterface.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetCoreUserInterface.dll"]