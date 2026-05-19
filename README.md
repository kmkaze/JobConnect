# JobConnect 🔍
Aplicación Android para búsqueda de empleo.

## Tecnologías
- Android (Kotlin)
- .NET 10 Web API
- PostgreSQL

## Cómo ejecutar el proyecto

### 1. Base de datos
- Instalar PostgreSQL y pgAdmin
- Ejecutar el archivo `database.sql`

### 2. Backend
- Instalar .NET 8 SDK
- Copiar `appsettings.example.json` → `appsettings.json`
- Poner tu contraseña de PostgreSQL en `appsettings.json`
- Ejecutar: `dotnet run`

### 3. Android
- Abrir la carpeta en Android Studio
- Cambiar la IP en `RetrofitClient.kt` por la IP de tu PC
- Ejecutar en emulador o celular

## Usuario administrador
- Correo: admin@jobconnect.com
- Contraseña: admin123
