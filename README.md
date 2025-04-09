# 📦 Sistema de Gestión de Inventario - MVC/Blazor
Aplicación web para gestión de inventarios desarrollada con Blazor (MudBlazor) en frontend y ASP.NET Core MVC en backend.
<div align="center">
  <img src="./docs/images/system-preview.png" alt="Vista previa" width="800">
</div>


## 📜 Índice

1. [Características Principales](#user-content-características-principales)
2. [Stack Tecnológico](#user-content-stack-tecnológico)
3. [Primeros Pasos](#user-content-primeros-pasos)
   - [Requisitos](#user-content-requisitos)
   - [Instalación](#user-content-instalación)
4. [Iniciar el Proyecto](#user-content-cómo-iniciar)
5. [Documentación Técnica](ARCHITECTURE.md)
6. [Guía de Desarrollo](DEVELOPMENT_GUIDE.md)

## 🌟 Características Principales
- **CRUD completo** de productos con validaciones
- **Seguimiento automático** de movimientos (entradas/salidas)
- **Interfaz profesional** con MudBlazor
- **Arquitectura en capas** (MVC + Servicios)
- **Manejo de errores** especializado (DuplicateProductException)

## 🛠 Stack Tecnológico
| Componente       | Tecnología               | Versión |
|------------------|--------------------------|---------|
| Frontend         | Blazor (MudBlazor)       | 8.5.1   |
| Backend          | ASP.NET Core MVC         | 8.0     |
| Base de datos    | SQL Server               | -       |
| Validaciones     | DataAnnotations          | -       |

## 🚀 Primeros Pasos

### Requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- IDE:
  - **Visual Studio 2022** (Recomendado)
  - **VS Code** con extensiones:
    - C# Dev Kit
    - Blazor WASM Tools

### Instalación
1. **Clonar repositorio**:
   ```bash
   git clone https://github.com/tu-usuario/gestion-inventario.git
   cd gestion-inventario

## 🚀 Cómo Iniciar
1. **Visual Studio 2022**:
   ```bash
   dotnet restore

## 🚀 Abrir en navegador
3. **compilar proyecto**
  ```bash
    dotnet run

