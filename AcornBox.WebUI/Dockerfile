#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AcornBox.WebUI/AcornBox.WebUI.csproj", "AcornBox.WebUI/"]
COPY ["AcornBox.Persistence/AcornBox.Persistence.csproj", "AcornBox.Persistence/"]
COPY ["AcornBox.Application/AcornBox.Application.csproj", "AcornBox.Application/"]
COPY ["AcornBox.Domain/AcornBox.Domain.csproj", "AcornBox.Domain/"]
COPY ["AcornBox.Common/AcornBox.Common.csproj", "AcornBox.Common/"]
RUN dotnet restore "AcornBox.WebUI/AcornBox.WebUI.csproj"
COPY . .
WORKDIR "/src/AcornBox.WebUI"
RUN dotnet build "AcornBox.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcornBox.WebUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcornBox.WebUI.dll"]
