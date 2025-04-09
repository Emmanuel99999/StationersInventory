# 📦 Sistema de Gestión de Inventario - MVC/Blazor
Aplicación web para gestión de inventarios desarrollada con Blazor (MudBlazor) en frontend y ASP.NET Core MVC en backend.
<div align="center">
  <img src="./docs/images/system-preview.png" alt="Vista previa" width="800">
</div>


## 📜 Índice

- [📦 Sistema de Gestión de Inventario - MVC/Blazor](#-sistema-de-gestión-de-inventario---mvcblazor)
  - [📜 Índice](#-índice)
  - [🌟 Características Principales](#-características-principales)
  - [🛠 Stack Tecnológico](#-stack-tecnológico)
  - [🚀 Primeros Pasos {#sección-3}](#-primeros-pasos-sección-3)
    - [Requisitos {#subsección-3-1}](#requisitos-subsección-3-1)
    - [Instalación {#subsección-3-2}](#instalación-subsección-3-2)
  - [🚀 Cómo Iniciar {#sección-4}](#-cómo-iniciar-sección-4)

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

## 🚀 Primeros Pasos {#sección-3}

### Requisitos {#subsección-3-1}
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- IDE:
  - **Visual Studio 2022** (Recomendado)
  - **VS Code** con extensiones:
    - C# Dev Kit
    - Blazor WASM Tools

### Instalación {#subsección-3-2}
1. **Clonar repositorio**:
   ```bash
   git clone https://github.com/tu-usuario/gestion-inventario.git
   cd gestion-inventario

## 🚀 Cómo Iniciar {#sección-4}
1. **Visual Studio 2022**:
   ```bash
   dotnet restore
   dotnet run

