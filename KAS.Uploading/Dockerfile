#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-nanoserver-sac2016 AS build
WORKDIR /src
COPY ["KAS.TruongThinh/KAS.TruongThinh.csproj", "KAS.TruongThinh/"]
RUN dotnet restore "KAS.TruongThinh/KAS.TruongThinh.csproj"
COPY . .
WORKDIR "/src/KAS.TruongThinh"
RUN dotnet build "KAS.TruongThinh.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KAS.TruongThinh.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KAS.TruongThinh.dll"]