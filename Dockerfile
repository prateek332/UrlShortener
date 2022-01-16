FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
COPY bin/Release/net6.0/publish/ app/
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000
ENTRYPOINT ["dotnet", "UrlShortner.dll"]