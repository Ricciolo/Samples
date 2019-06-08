FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
ENV TZ=Etc/UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Recipes/Recipes.WebApi/Recipes.WebApi.csproj", "Recipes/Recipes.WebApi/"]
COPY ["Recipes/Recipes.Cqrs.Handlers/Recipes.Cqrs.Handlers.csproj", "Recipes/Recipes.Cqrs.Handlers/"]
COPY ["Recipes/Recipes.DomainModel/Recipes.DomainModel.csproj", "Recipes/Recipes.DomainModel/"]
COPY ["Shared/DomainModel/DomainModel.csproj", "Shared/DomainModel/"]
COPY ["Shared/DataAccessObject/DataAccessObject.csproj", "Shared/DataAccessObject/"]
COPY ["Shared/ReadModel/ReadModel.csproj", "Shared/ReadModel/"]
COPY ["Shared/Linq/Linq.csproj", "Shared/Linq/"]
COPY ["Shared/Repository/Repository.csproj", "Shared/Repository/"]
COPY ["Shared/Core/Core.csproj", "Shared/Core/"]
COPY ["Recipes/Recipes.ReadModel/Recipes.ReadModel.csproj", "Recipes/Recipes.ReadModel/"]
COPY ["Recipes/Recipes.Cqrs/Recipes.Cqrs.csproj", "Recipes/Recipes.Cqrs/"]
COPY ["Shared/Cqrs/Cqrs.csproj", "Shared/Cqrs/"]
COPY ["Shared/Logging/Logging.csproj", "Shared/Logging/"]
COPY ["Shared/Web.Cqrs/Web.Cqrs.csproj", "Shared/Web.Cqrs/"]
COPY ["Shared/Web/Web.csproj", "Shared/Web/"]
COPY ["Recipes/Recipes.Sqlite/Recipes.Sqlite.csproj", "Recipes/Recipes.Sqlite/"]
COPY ["Shared/Linq.EntityFramework/Linq.EntityFramework.csproj", "Shared/Linq.EntityFramework/"]
COPY ["Shared/EntityFramework/EntityFramework.csproj", "Shared/EntityFramework/"]
RUN dotnet restore "Recipes/Recipes.WebApi/Recipes.WebApi.csproj"
COPY . .
WORKDIR "/src/Recipes/Recipes.WebApi"
RUN dotnet build "Recipes.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Recipes.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Muuvis.Catalog.WebApi.dll"]
