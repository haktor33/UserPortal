FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -o /src/published-app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime

WORKDIR /src
COPY --from=build /src/published-app /src

ENTRYPOINT [ "dotnet", "Management.dll" ]