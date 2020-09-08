FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY *.sln .
COPY xsolla-revenue-calculator/*.csproj xsolla-revenue-calculator/

RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/xsolla-revenue-calculator
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# heroku uses the following

CMD ASPNETCORE_URLS=http://*:$PORT dotnet xsolla-revenue-calculator.dll