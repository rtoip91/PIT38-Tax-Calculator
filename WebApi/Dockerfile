FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN mkdir -p /var/ExcelFiles
RUN mkdir -p /var/ResultFiles

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["../WebApi/WebApi.csproj", "../WebApi/"]
COPY ["../Calculations/Calculations.csproj", "../Calculations/"]
COPY ["../Database/Database.csproj", "../Database/"]
COPY ["../EtoroExcelReader/ExcelReader.csproj", "../EtoroExcelReader/"]
COPY ["TaxCalculatingService.csproj", "./"]
COPY ["../ResultPresenter/ResultsPresenter.csproj", "../ResultPresenter/"]
ENV ASPNETCORE_ENVIRONMENT=Development

RUN dotnet restore "../WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/../WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
