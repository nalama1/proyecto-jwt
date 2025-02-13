# Imagen base para compilar la aplicación 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

RUN apt-get update && apt-get install -y curl wget iputils-ping

# Copiar la solución y los archivos de proyecto
COPY *.sln ./
COPY MSPersona.API/ MSPersona.API/
COPY MSPersona.Data/ MSPersona.Data/
COPY MSPersona.Dominio/ MSPersona.Dominio/
COPY MSPersona.Services/ MSPersona.Services/
#COPY Common/ Common/ 

RUN dotnet restore "MSPersona.API/MSPersona.API.csproj"
RUN dotnet build "MSPersona.API/MSPersona.API.csproj" -c Release -o /app/build
RUN dotnet publish "MSPersona.API/MSPersona.API.csproj" -c Release -o /app/publish

# Imagen base para ejecutar la aplicación (más ligera)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar solo los archivos publicados desde la etapa "build"
COPY --from=build /app/publish .


EXPOSE 8080 

# Comando de inicio de la aplicación
CMD ["dotnet", "MSPersona.API.dll"]
