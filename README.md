# Proyecto JWT
Este repositorio contiene la implementación de un sistema basado en JWT que incluye un frontend para el registro de Personas y de Usuarios, otro frontend para el login de usuarios, un backend en .NET Core 8 y una base de datos en SQL Server.

# Requisitos previos

Herramientas que deben estar instaladas antes de ejecutar el proyecto.

Node.js (para ejecutar el frontend)

Angular CLI (para manejar los proyectos Angular)

Docker (para contenedores)

Visual Studio 2022 (para el backend)

SQL Server Management Studio


## 📁 Estructura del proyecto

![image](https://github.com/user-attachments/assets/2757feb3-9938-4b1d-8e1c-053d471af6d1)


## 📁 Estructura de carpetas detallado

![image](https://github.com/user-attachments/assets/75c7f56c-ab95-450f-abec-9b0085e82917)


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
5. Compilar el proyecto y seleccionar la opción Pack (se generará un archivo `
.nupkg`).
6. Subir el archivo generado a NuGet y esperar el proceso de indexación (aprox. 1 hora).

![image](https://github.com/user-attachments/assets/63459bb2-e741-4695-8a27-51c91f3ef035)

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


### Dentro de Docker Desktop ejecutar el contenedor con variables de entorno (en este escenario no se encuentra incluido el archivo .env)
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

### Ejecutando todo el aplicativo con Docker Compose (en este escenario no se encuentra incluido el archivo .env)
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

![Captura de pantalla 2025-02-13 114458](https://github.com/user-attachments/assets/044e8607-e72e-45ee-acf5-e13e83879d4b)

![Captura de pantalla 2025-02-13 114444](https://github.com/user-attachments/assets/889ab8f8-8684-4d46-9b2c-6069b4a1b5a6)




# Docker Hub:
En este link se encuentran las cuatro imágenes contenerizadas publicadas en Docker Hub:

https://hub.docker.com/repositories/acujilem

acujilem/proyecto-jwt-sqlserver

acujilem/proyecto-jwt-test_cujilema

acujilem/proyecto-jwt-login

acujilem/proyecto-jwt-registro_usuario

![Captura de pantalla 2025-02-13 120256](https://github.com/user-attachments/assets/5ffaf5c7-1869-4432-9e7a-a8cb226d761f)



# Capturas de pantalla 

Capturas de pantalla del aplicativo.
![image](https://github.com/user-attachments/assets/9546ffb4-cf4b-4ba3-b3f4-cb1cbb6e701a)

![image](https://github.com/user-attachments/assets/0404ccbf-a8b5-4d16-84fa-0bb91789f7ae)

![image](https://github.com/user-attachments/assets/5aa00ad1-bc29-434a-b49e-a33f7190d8d6)

![image](https://github.com/user-attachments/assets/862fa696-cf36-4ca5-af7f-ac1b67ed08df)

![ListadoPersonas](https://github.com/user-attachments/assets/844ee809-9841-487c-a0fb-51abba969bb6)















