#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AcornBox.Worker/AcornBox.Worker.csproj", "AcornBox.Worker/"]
COPY ["AcornBox.Core/AcornBox.Core.csproj", "AcornBox.Core/"]
COPY ["AcornBox.Common/AcornBox.Common.csproj", "AcornBox.Common/"]
RUN dotnet restore "AcornBox.Worker/AcornBox.Worker.csproj"
COPY . .
WORKDIR "/src/AcornBox.Worker"
RUN dotnet build "AcornBox.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcornBox.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcornBox.Worker.dll"]
