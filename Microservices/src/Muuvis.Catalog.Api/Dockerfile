FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY Muuvis.Catalog.Api/Muuvis.Catalog.Api.csproj Muuvis.Catalog.Api/
COPY Muuvis.Catalog.EntityFramework/Muuvis.Catalog.EntityFramework.csproj Muuvis.Catalog.EntityFramework/
COPY Muuvis.Repository/Muuvis.Repository.csproj Muuvis.Repository/
COPY Muuvis.DataAccessObject/Muuvis.DataAccessObject.csproj Muuvis.DataAccessObject/
COPY Muuvis.Catalog.ReadModel/Muuvis.Catalog.ReadModel.csproj Muuvis.Catalog.ReadModel/
COPY Muuvis.EntityFramework/Muuvis.EntityFramework.csproj Muuvis.EntityFramework/
COPY Muuvis.Catalog.DomainModel/Muuvis.Catalog.DomainModel.csproj Muuvis.Catalog.DomainModel/
COPY Muuvis.Cqrs/Muuvis.Cqrs.csproj Muuvis.Cqrs/
COPY Muuvis.Catalog.Cqrs/Muuvis.Catalog.Cqrs.csproj Muuvis.Catalog.Cqrs/
RUN dotnet restore
COPY . .
WORKDIR /src/Muuvis.Catalog.Api
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Muuvis.Catalog.Api.dll"]
