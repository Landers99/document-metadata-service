FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore src/DocumentMetadataService.Api/DocumentMetadataService.Api.csproj
RUN dotnet publish src/DocumentMetadataService.Api/DocumentMetadataService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "DocumentMetadataService.Api.dll"]
