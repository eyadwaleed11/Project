FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
COPY bin/Debug/net8.0/ ./
ENTRYPOINT ["dotnet", "YourApiProjectName.dll"]