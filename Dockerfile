FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy everything
COPY bin /app

ENTRYPOINT ["dotnet", "PublicSpeaker.Web.dll"]