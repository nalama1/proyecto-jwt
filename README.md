# Proyecto JWT
Este repositorio contiene la implementación de un sistema basado en JWT que incluye un frontend para el registro de Personas y de Usuarios, otro frontend para el login de usuarios, un backend en .NET y una base de datos en SQL Server.

# Requisitos previos

Herramientas que deben estar instaladas antes de ejecutar el proyecto.

Node.js (para ejecutar el frontend)

Angular CLI (para manejar los proyectos Angular)

Docker (para contenedores)

Visual Studio 2022 (para el backend)

SQL Server Management Studio


## 📁 Estructura del proyecto
proyecto-jwt

├── RegistroUsuarioJwt    # FRONTEND: Proyecto Angular para registrar personas y usuarios

├── Login                 # FRONTEND: Proyecto Angular para el login de usuarios

├── TestCujilema          # BACKEND: API en .NET para la autenticación y datos

├── sql                   # BD: Scripts de base de datos y configuración SQL

├── docker-compose.yml    # Archivo para levantar todos los contenedores

├── env                   # Archivo de variables de entorno


## 📁 Estructura de carpetas detallado

proyecto-jwt/

├── frontend-registro/

│   ├── Dockerfile

│   └── ... (código del proyecto RegistroUsuarioJwt)

├── frontend-login/

│   ├── Dockerfile

│   └── ... (código del proyecto Login)

├── backend/

│   ├── Dockerfile

│   └── ... (código del proyecto TestCujilema)

├── sql/

│   ├── Dockerfile

│   ├── base_datos.sql

├── docker-compose.yml

└── .env



## 🚀 Instrucciones para ejecutar el proyecto

### Frontend

#### 1️⃣ Ejecutar el proyecto de Creación de Persona y Usuario
ng serve --port 4200

#### 2️⃣ Ejecutar el proyecto de Login
ng serve --port 4201

### Backend

#### 1️⃣ Ejecutar el proyecto API Backend
El backend se ejecuta en el puerto 4438

### Base de datos
El proyecto incluye scripts SQL para la creación de la base de datos. 
Asegúrate de tener instalado SQL Server Management Studio (SSMS) 19.1.56.0.

## 📦 Desarrollo de una librería NuGet

1. Crear una cuenta en [NuGet](https://www.nuget.org)
2. Generar una API Key para subir paquetes a la galería.
3. Crear un proyecto Library e incluir las clases y modelos.
4. Configurar los metadatos en Propiedades -> Package (versión, licencia, autor, descripción e imagen).
5. Compilar el proyecto y seleccionar la opción Pack (se generará un archivo `.nupkg`).
6. Subir el archivo generado a NuGet y esperar el proceso de indexación (aprox. 1 hora).

## 🛠️ Instalación de paquetes NuGet

### 🔹 Serilog (para logging)
Install-Package Serilog
Install-Package Serilog.AspNetCore
Install-Package Serilog.Sinks.Console

### 🔹 JWT (para autenticación)
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer

## 📐 Arquitectura de software utilizada

### 🔹 Frontend
- Angular CLI: 19.1.5
- Node.js: 22.11.0
- Package Manager: npm 10.9.0
- Sistema Operativo: Windows 64-bit

### 🔹 Backend
- Microsoft Visual Studio Community 2022 (Versión 17.8.3)
- .NET Framework: 4.8.09032

### 🔹 Base de datos
- SQL Server Management Studio: 19.1.56.0


## ⚠️ Consideraciones

- La política de CORS permite los siguientes accesos:
  - Frontend RegistroUsuarioJwt en el puerto 4200
  - Frontend Login en el puerto 4201
- Asegúrate de tener configuradas correctamente las dependencias antes de ejecutar el proyecto.



## 📌 Configuración de SQL Server

Para que el backend funcione correctamente, sigue estos pasos:

### Crear la base de datos
Ejecutar el script base_datos_proyecto-jwt.sql.

### Configurar SQL Server para conexiones remotas

Habilitar TCP/IP en SQL Server Configuration Manager.

Asegurar que el usuario de SQL (Security > Login > Status) esté habilitado.

Configurar el firewall de Windows para permitir el tráfico entrante en el puerto 1433.


### Crear variables de entorno para Docker
DB_SERVER: Servidor de la base de datos.
DB_NAME: Nombre de la base de datos.
DB_USER: Usuario de la base de datos.
DB_PASSWORD: Contraseña de la base de datos.

### Dentro de Docker Desktop ejecutar el contenedor con variables de entorno (en este escenario no estaba incluido el archivo .env)
docker run -p 5000:8080 \
  -e DB_SERVER=host.docker.internal \
  -e DB_NAME=TestCujilema1 \
  -e DB_USER=sa \
  -e DB_PASSWORD=Contraseña de la base de datos \
  acujilem/backend-jwt:1.0

acujilem: es un nombre de usuario creado en el sitio web Docker HUB.
backend-jwt: es un nombre descriptivo 
1.0: el usuario escribe la versión en la que se encuentra su contenedor.

### Descargar la imagen del backend desde Docker Hub
https://hub.docker.com/r/ acujilem/backend-jwt

Comando para descargar la imagen:
docker pull acujilem/backend-jwt:latest

### Ejecutando todo el aplicativo con Docker Compose (en este escenario no estaba incluido el archivo .env)
docker-compose up -d \
  -e DB_SERVER=host.docker.internal \
  -e DB_NAME=TestCujilema1 \
  -e DB_USER=ingresar_usuario_de_base_de_datos \
  -e DB_PASSWORD=ingresar_contraseña_de_base_de_datos

host.docker.internal: Le indica a Docker que no utilice su localhost, sino que utilice el host de la máquina local.


# Gestión de Contenedores con Docker Compose

Para levantar y administrar los contenedores correctamente, se utilizaron los siguientes comandos:

#🛑 Detener y eliminar contenedores

docker-compose down -v

down: Detiene y elimina los contenedores, redes y volúmenes creados por docker-compose up.

-v: Elimina los volúmenes asociados, lo que borra cualquier dato persistente almacenado en contenedores.


#🚀 Construir y levantar contenedores en segundo plano

docker-compose up --build -d

--build: Fuerza la reconstrucción de las imágenes antes de iniciar los contenedores. Útil cuando has realizado cambios en el código o en los Dockerfiles.

-d: Ejecuta los contenedores en segundo plano (modo "detached"), para que sigan corriendo sin bloquear la terminal.


#🔍 Verificar los contenedores en ejecución
docker ps

Muestra una lista de contenedores activos, incluyendo su ID, nombre, estado y puertos asignados.


Si quieres ver todos los contenedores (incluso los detenidos), usa:

docker ps -a


#📜 Ver los logs de un contenedor específico
docker-compose logs <nombre_del_contenedor>


Permite ver los logs del contenedor para depuración.

Ejemplo: Si el backend se llama backend-jwt, usa:

docker-compose logs backend-jwt


o

utilizar el contenedorID

docker-compose logs contenedorID


#📜 un comando adicional utilizado, pero con ¡precaución al usarlo!
docker-compose system prune -a 


Comando que eliminó todas las imágenes y contenedores dentro del docker desktop.

Se utilizó el comando porque no estaba tomando unos valores nuevos, posiblemente porque hubo caché en Docker. En este caso se limpio todo.



# Docker Hub:
En este link se encuentran las cuatro imágenes contenerizadas publicadas en Docker Hub:

https://hub.docker.com/repositories/acujilem

acujilem/proyecto-jwt-sqlserver

acujilem/proyecto-jwt-test_cujilema

acujilem/proyecto-jwt-login

acujilem/proyecto-jwt-registro_usuario



















