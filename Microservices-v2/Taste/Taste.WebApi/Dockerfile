FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
ENV TZ=Etc/UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Taste/Taste.WebApi/Taste.WebApi.csproj", "Taste/Taste.WebApi/"]
COPY ["Shared/Web.Cqrs/Web.Cqrs.csproj", "Shared/Web.Cqrs/"]
COPY ["Shared/Web/Web.csproj", "Shared/Web/"]
COPY ["Shared/Depedencies/Depedencies.csproj", "Shared/Depedencies/"]
COPY ["Shared/Logging/Logging.csproj", "Shared/Logging/"]
COPY ["Shared/DomainModel/DomainModel.csproj", "Shared/DomainModel/"]
COPY ["Shared/Cqrs/Cqrs.csproj", "Shared/Cqrs/"]
COPY ["Shared/Core/Core.csproj", "Shared/Core/"]
COPY ["Taste/Taste.Cqrs.Handlers/Taste.Cqrs.Handlers.csproj", "Taste/Taste.Cqrs.Handlers/"]
COPY ["Catalog/Catalog.Cqrs/Catalog.Cqrs.csproj", "Catalog/Catalog.Cqrs/"]
COPY ["Taste/Taste.ReadModel/Taste.ReadModel.csproj", "Taste/Taste.ReadModel/"]
COPY ["Taste/Taste.Cqrs/Taste.Cqrs.csproj", "Taste/Taste.Cqrs/"]
COPY ["Shared/DataAccessObject/DataAccessObject.csproj", "Shared/DataAccessObject/"]
COPY ["Taste/Taste.DomainModel/Taste.DomainModel.csproj", "Taste/Taste.DomainModel/"]
COPY ["Shared/Repository/Repository.csproj", "Shared/Repository/"]
COPY ["Taste/Taste.EntityFramework/Taste.EntityFramework.csproj", "Taste/Taste.EntityFramework/"]
COPY ["Shared/EntityFramework/EntityFramework.csproj", "Shared/EntityFramework/"]
RUN dotnet restore "Taste/Taste.WebApi/Taste.WebApi.csproj"
COPY . .
WORKDIR "/src/Taste/Taste.WebApi"
RUN dotnet build "Taste.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Taste.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Muuvis.Taste.WebApi.dll"]