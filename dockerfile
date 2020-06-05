FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR ../Tibos.Test
COPY . .
WORKDIR "/Tibos.Test"
RUN dotnet publish "Tibos.Test.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
RUN rm -rf appsettings.Development.json
RUN cp /usr/share/zoneinfo/Asia/Shanghai /etc/localtime \
&& echo 'Asia/Shanghai' >/etc/timezone
ENTRYPOINT ["dotnet", "Tibos.Test.dll","-b","0.0.0.0"]