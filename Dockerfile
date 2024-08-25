FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY *.sln ./
COPY MessageService.API/*.csproj MessageService.API/
COPY MessageClient1/*.csproj MessageClient1/
COPY MessageClient2/*.csproj MessageClient2/
COPY MessageClient3/*.csproj MessageClient3/

RUN dotnet restore

COPY . ./
RUN dotnet build --configuration Release

RUN dotnet publish MessageService.API -c Release -o /app/MessageService.API/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/MessageService.API/out .

EXPOSE 5103

ENTRYPOINT ["dotnet", "MessageService.API.dll"]
