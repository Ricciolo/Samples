FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
ENV TZ=Etc/UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Shared/Http/Http.csproj", "Shared/Http/"]
COPY ["Catalog/Catalog.WebApi/Catalog.WebApi.csproj", "Catalog/Catalog.WebApi/"]
COPY ["Shared/Web.Cqrs/Web.Cqrs.csproj", "Shared/Web.Cqrs/"]
COPY ["Shared/Web/Web.csproj", "Shared/Web/"]
COPY ["Shared/Depedencies/Depedencies.csproj", "Shared/Depedencies/"]
COPY ["Shared/Logging/Logging.csproj", "Shared/Logging/"]
COPY ["Shared/DomainModel/DomainModel.csproj", "Shared/DomainModel/"]
COPY ["Shared/Cqrs/Cqrs.csproj", "Shared/Cqrs/"]
COPY ["Shared/Core/Core.csproj", "Shared/Core/"]
COPY ["Catalog/Catalog.DomainModel/Catalog.DomainModel.csproj", "Catalog/Catalog.DomainModel/"]
COPY ["Catalog/Catalog.Cqrs.Handlers/Catalog.Cqrs.Handlers.csproj", "Catalog/Catalog.Cqrs.Handlers/"]
COPY ["Catalog/Catalog.Cqrs/Catalog.Cqrs.csproj", "Catalog/Catalog.Cqrs/"]
COPY ["Catalog/Catalog.ReadModel/Catalog.ReadModel.csproj", "Catalog/Catalog.ReadModel/"]
COPY ["Shared/DataAccessObject/DataAccessObject.csproj", "Shared/DataAccessObject/"]
COPY ["Shared/Repository/Repository.csproj", "Shared/Repository/"]
COPY ["Catalog/Catalog.EntityFramework/Catalog.EntityFramework.csproj", "Catalog/Catalog.EntityFramework/"]
COPY ["Shared/EntityFramework/EntityFramework.csproj", "Shared/EntityFramework/"]
RUN dotnet restore "Catalog/Catalog.WebApi/Catalog.WebApi.csproj"
COPY . .
WORKDIR "/src/Catalog/Catalog.WebApi"
RUN dotnet build "Catalog.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Catalog.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Muuvis.Catalog.WebApi.dll"]
