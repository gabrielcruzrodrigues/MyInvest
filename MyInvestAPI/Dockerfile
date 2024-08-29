FROM mcr.microsoft.com/dotnet.sdk:8.0 AS build-env
WORKDIR /app

COPY ./src/MyInvestApi/*.csproj ./src/MyInvestApi
RUN dotnet restore ./src/MyInvestApi/MyInvestApi.csproj

COPY ./src/MyInvestApi/ ./src/MyInvestApi
RUN dotnet publish ./src/MyInvestApi/MyInvestApi.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/src/MyInvestApi/out .

EXPOSE 80

ENTRYPOINT [ "dotnet", "MyInvestApi.dll" ]