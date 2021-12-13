FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine
WORKDIR /app
COPY  ./publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80

ENTRYPOINT [ "dotnet", "ToDoApp.dll"]