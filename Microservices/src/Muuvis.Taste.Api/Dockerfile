FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY Muuvis.Taste.Api/Muuvis.Taste.Api.csproj Muuvis.Taste.Api/
COPY Muuvis.Taste.DomainModel/Muuvis.Taste.DomainModel.csproj Muuvis.Taste.DomainModel/
COPY Muuvis.Repository/Muuvis.Repository.csproj Muuvis.Repository/
COPY Muuvis.DataAccessObject/Muuvis.DataAccessObject.csproj Muuvis.DataAccessObject/
COPY Muuvis.Common/Muuvis.Common.csproj Muuvis.Common/
COPY Muuvis.Taste.Cqrs/Muuvis.Taste.Cqrs.csproj Muuvis.Taste.Cqrs/
COPY Muuvis.Cqrs/Muuvis.Cqrs.csproj Muuvis.Cqrs/
COPY Muuvis.Taste.ReadModel/Muuvis.Taste.ReadModel.csproj Muuvis.Taste.ReadModel/
COPY Muuvis.EntityFramework/Muuvis.EntityFramework.csproj Muuvis.EntityFramework/
COPY Muuvis.Taste.EntityFramework/Muuvis.Taste.EntityFramework.csproj Muuvis.Taste.EntityFramework/
COPY Muuvis.Taste.Cqrs.Handlers/Muuvis.Taste.Cqrs.Handlers.csproj Muuvis.Taste.Cqrs.Handlers/
COPY Muuvis.Catalog.Cqrs/Muuvis.Catalog.Cqrs.csproj Muuvis.Catalog.Cqrs/
RUN dotnet restore
COPY . .
WORKDIR /src/Muuvis.Taste.Api
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Muuvis.Taste.Api.dll"]
