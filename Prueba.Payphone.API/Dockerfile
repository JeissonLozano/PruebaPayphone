FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER app
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development
COPY Prueba.Payphone.API/ /app/

ENTRYPOINT ["dotnet", "Prueba.Payphone.API.dll"] 