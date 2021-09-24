#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-nanoserver-sac2016 AS build
WORKDIR /src
COPY ["KAS.Uploading.API/KAS.Uploading.API.csproj", "KAS.Uploading.API/"]
RUN dotnet restore "KAS.Uploading.API/KAS.Uploading.API.csproj"
COPY . .
WORKDIR "/src/KAS.Uploading.API"
RUN dotnet build "KAS.Uploading.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KAS.Uploading.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KAS.Uploading.API.dll"]