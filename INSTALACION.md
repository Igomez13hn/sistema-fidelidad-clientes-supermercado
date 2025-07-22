# Guía Rápida de Instalación - Sistema de Fidelidad

## Pasos de Instalación Rápida

### 1. Preparar el Entorno
```bash
# Verificar que tienes instalado:
- Visual Studio 2010 o superior
- SQL Server 2008 o superior
- .NET Framework 4.0
```

### 2. Configurar Base de Datos
```sql
-- 1. Abrir SQL Server Management Studio
-- 2. Ejecutar el script completo: LoyaltySystem_DB.sql
-- 3. Verificar que se creó la base de datos "LoyaltySystemDB"
```

### 3. Configurar Proyecto
```vb
' 1. Abrir LoyaltySystem.vbproj en Visual Studio
' 2. Actualizar cadena de conexión en DatabaseHelper.vb:
Private Shared connectionString As String = "Data Source=TU_SERVIDOR;Initial Catalog=LoyaltySystemDB;Integrated Security=True;"

' 3. Compilar: Build → Build Solution (F6)
' 4. Ejecutar: Debug → Start Debugging (F5)
```

### 4. Probar el Sistema
1. **Agregar un cliente de prueba**
2. **Registrar una compra**
3. **Verificar puntos ganados**
4. **Probar canje de puntos**
5. **Ver reportes**

## Configuraciones Comunes de Conexión

### SQL Server Local
```
Data Source=localhost;Initial Catalog=LoyaltySystemDB;Integrated Security=True;
```

### SQL Server Express
```
Data Source=.\SQLEXPRESS;Initial Catalog=LoyaltySystemDB;Integrated Security=True;
```

### SQL Server con Usuario/Contraseña
```
Data Source=servidor;Initial Catalog=LoyaltySystemDB;User ID=usuario;Password=contraseña;
```

## Solución Rápida de Problemas

### ❌ Error de Conexión
- Verificar que SQL Server esté ejecutándose
- Comprobar la cadena de conexión
- Verificar permisos de usuario

### ❌ Error de Compilación
- Verificar .NET Framework 4.0
- Limpiar y recompilar el proyecto
- Verificar referencias del proyecto

### ❌ Formularios no se Abren
- Verificar que todas las clases están compiladas
- Revisar errores en el código de los formularios
- Verificar que MainForm está configurado como formulario de inicio

## Datos de Prueba

El script de base de datos incluye 3 clientes de ejemplo:
- Juan Pérez
- María García  
- Carlos López

¡El sistema está listo para usar!
