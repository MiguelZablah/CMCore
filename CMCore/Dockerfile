FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ./CMCore.csproj CMCore/
COPY ./Properties/NoGet.Config ./
RUN dotnet restore CMCore/CMCore.csproj --configfile ./NoGet.Config
COPY . CMCore/
WORKDIR "/src/CMCore"
RUN dotnet build "CMCore.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CMCore.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CMCore.dll"]