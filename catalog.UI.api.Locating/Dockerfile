#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_CONNECTIONSTRINGS__Locating="SqlServerConnectionString"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["catalog.UI.api.Locating/catalog.UI.api.Locating.csproj", "catalog.UI.api.Locating/"]
COPY ["catalog.Persistence.DBLocating/catalog.Persistence.DBLocating.csproj", "catalog.Persistence.DBLocating/"]
COPY ["catalog.Core.Locating.Application/catalog.Core.Locating.Application.csproj", "catalog.Core.Locating.Application/"]
COPY ["catalog.Core.Application.Common/catalog.Core.Application.Common.csproj", "catalog.Core.Application.Common/"]
COPY ["catalog.Core.Locating.Domain/catalog.Core.Locating.Domain.csproj", "catalog.Core.Locating.Domain/"]
COPY ["catalog.Core.SharedKernel/catalog.Core.SharedKernel.csproj", "catalog.Core.SharedKernel/"]
COPY ["catalog.Infrastructure.Cache/catalog.Infrastructure.Cache.csproj", "catalog.Infrastructure.Cache/"]
COPY ["catalog.Infrastructure.Library.Extensions/catalog.Infrastructure.Library.Extensions.csproj", "catalog.Infrastructure.Library.Extensions/"]
RUN dotnet restore "catalog.UI.api.Locating/catalog.UI.api.Locating.csproj"
COPY . .
WORKDIR "/src/catalog.UI.api.Locating"
RUN dotnet build "catalog.UI.api.Locating.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "catalog.UI.api.Locating.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "catalog.UI.api.Locating.dll"]