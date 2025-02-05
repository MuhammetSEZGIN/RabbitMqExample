FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY RabbitMqService/*.csproj ./RabbitMqService/
RUN dotnet restore ./RabbitMqService/RabbitMqService.csproj

COPY RabbitMqService/. ./RabbitMqService/
WORKDIR /app/RabbitMqService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/RabbitMqService/out ./

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 80
ENTRYPOINT ["dotnet", "RabbitMqService.dll"]