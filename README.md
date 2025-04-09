# 📦 Sistema de Gestión de Inventario - MVC/Blazor

<div align="center">
  <img src="./docs/images/system-preview.png" alt="Vista previa" width="800">
</div>

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

### 🚀 Cómo Iniciar
2. **Restaurar paquetes NuGet** en tu IDE.
  ```bash
dotnet restore

### 🚀 Abrir en navegador
3. **Compilar proyecto**
  ```bash
dotnet run
