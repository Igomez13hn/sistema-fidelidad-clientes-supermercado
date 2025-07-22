# Sistema de Fidelidad de Clientes - Supermercado

## Descripción

Sistema completo de fidelidad de clientes desarrollado en **Visual Basic 2010** con base de datos **Microsoft SQL Server**. El sistema permite gestionar clientes, registrar compras, calcular puntos automáticamente (1 punto por cada $1 comprado), canjear puntos y generar reportes detallados.

## Características Principales

### ✅ Gestión de Clientes
- Agregar, editar y eliminar clientes
- Información completa: nombre, email, teléfono, dirección
- Seguimiento de puntos actuales por cliente

### ✅ Registro de Compras
- Registro rápido de compras por cliente
- Cálculo automático de puntos (1 punto = $1)
- Actualización automática del saldo de puntos
- Historial de compras recientes

### ✅ Canje de Puntos
- Validación de puntos disponibles
- Canje seguro con confirmación
- Deducción automática de puntos
- Registro de comentarios por canje
- Historial de canjes realizados

### ✅ Reportes Completos
- Reporte detallado por cliente
- Estadísticas generales del sistema
- Filtros de búsqueda
- Exportación a CSV
- Métricas de rendimiento

### ✅ Características Técnicas
- Interfaz moderna y limpia
- Validaciones completas de datos
- Manejo robusto de errores
- Procedimientos almacenados para operaciones críticas
- Arquitectura escalable y mantenible

## Requisitos del Sistema

### Software Necesario
- **Visual Studio 2010** o superior
- **Microsoft SQL Server 2008** o superior (Express, Standard, o Enterprise)
- **.NET Framework 4.0** o superior
- **Windows 7** o superior

### Hardware Mínimo
- **Procesador:** Intel Pentium 4 o equivalente
- **RAM:** 2 GB mínimo (4 GB recomendado)
- **Disco Duro:** 500 MB de espacio libre
- **Resolución:** 1024x768 mínimo

## Instalación y Configuración

### 1. Configuración de la Base de Datos

1. **Abrir SQL Server Management Studio**
2. **Ejecutar el script de base de datos:**
   ```sql
   -- Ejecutar el archivo: LoyaltySystem_DB.sql
   ```
3. **Verificar la creación de:**
   - Base de datos: `LoyaltySystemDB`
   - Tablas: `Customers`, `Purchases`, `Redemptions`
   - Procedimientos: `sp_RegisterPurchase`, `sp_RedeemPoints`
   - Vista: `vw_CustomerReport`

### 2. Configuración del Proyecto

1. **Abrir Visual Studio 2010**
2. **Abrir el proyecto:** `LoyaltySystem.vbproj`
3. **Configurar la cadena de conexión en `DatabaseHelper.vb`:**
   ```vb
   Private Shared connectionString As String = "Data Source=TU_SERVIDOR;Initial Catalog=LoyaltySystemDB;Integrated Security=True;"
   ```
4. **Compilar el proyecto:** Build → Build Solution (F6)

### 3. Configuración de Conexión

Actualizar la cadena de conexión según tu configuración:

**Para SQL Server local:**
```
Data Source=localhost;Initial Catalog=LoyaltySystemDB;Integrated Security=True;
```

**Para SQL Server con usuario/contraseña:**
```
Data Source=servidor;Initial Catalog=LoyaltySystemDB;User ID=usuario;Password=contraseña;
```

**Para SQL Server Express:**
```
Data Source=.\SQLEXPRESS;Initial Catalog=LoyaltySystemDB;Integrated Security=True;
```

## Estructura del Proyecto

```
LoyaltySystem/
├── LoyaltySystem_DB.sql          # Script de base de datos
├── DatabaseHelper.vb             # Clase de acceso a datos
├── MainForm.vb                   # Formulario principal (menú)
├── CustomersForm.vb              # Gestión de clientes
├── PurchasesForm.vb              # Registro de compras
├── RedemptionsForm.vb            # Canje de puntos
├── ReportsForm.vb                # Reportes y estadísticas
├── App.config                    # Configuración de la aplicación
├── LoyaltySystem.vbproj          # Archivo de proyecto
└── README.md                     # Documentación
```

## Guía de Uso

### 1. Gestión de Clientes
1. Desde el menú principal, hacer clic en **"Gestión de Clientes"**
2. **Agregar cliente:** Completar los campos y hacer clic en "Agregar Cliente"
3. **Editar cliente:** Seleccionar cliente de la lista, modificar datos y hacer clic en "Actualizar Cliente"
4. **Eliminar cliente:** Seleccionar cliente y hacer clic en "Eliminar Cliente" (requiere confirmación)

### 2. Registro de Compras
1. Hacer clic en **"Registro de Compras"**
2. **Seleccionar cliente** del menú desplegable
3. **Ingresar monto** de la compra
4. **Verificar puntos** a ganar (se calculan automáticamente)
5. Hacer clic en **"Registrar Compra"**

### 3. Canje de Puntos
1. Hacer clic en **"Canje de Puntos"**
2. **Seleccionar cliente** (solo aparecen clientes con puntos disponibles)
3. **Ingresar cantidad** de puntos a canjear
4. **Agregar comentarios** (opcional)
5. **Confirmar el canje**

### 4. Reportes
1. Hacer clic en **"Reportes"**
2. **Ver estadísticas generales** en la parte superior
3. **Buscar clientes** usando el filtro de búsqueda
4. **Exportar datos** a CSV usando el botón "Exportar"

## Funcionalidades Avanzadas

### Procedimientos Almacenados

**sp_RegisterPurchase:** Registra una compra y actualiza puntos automáticamente
```sql
EXEC sp_RegisterPurchase @CustomerID = 1, @Amount = 150.00
```

**sp_RedeemPoints:** Procesa el canje de puntos con validaciones
```sql
EXEC sp_RedeemPoints @CustomerID = 1, @RedeemPoints = 100, @Comments = 'Descuento en compra'
```

### Vista de Reportes

**vw_CustomerReport:** Vista consolidada con información completa de clientes
- Puntos actuales
- Total de compras realizadas
- Puntos ganados históricos
- Puntos canjeados
- Número de transacciones

### Validaciones Implementadas

- **Campos obligatorios:** Nombre del cliente, monto de compra
- **Formatos numéricos:** Solo números en campos de monto y puntos
- **Puntos suficientes:** Validación antes de canjear puntos
- **Conexión a BD:** Verificación de conectividad al iniciar

## Solución de Problemas

### Error de Conexión a Base de Datos
1. **Verificar que SQL Server esté ejecutándose**
2. **Comprobar la cadena de conexión** en `DatabaseHelper.vb`
3. **Verificar permisos** del usuario en la base de datos
4. **Probar conexión** desde SQL Server Management Studio

### Error al Compilar
1. **Verificar .NET Framework 4.0** esté instalado
2. **Comprobar referencias** del proyecto
3. **Limpiar y recompilar:** Build → Clean Solution, luego Build → Rebuild Solution

### Problemas de Rendimiento
1. **Verificar índices** en las tablas de la base de datos
2. **Optimizar consultas** si hay muchos registros
3. **Considerar paginación** para reportes grandes

## Personalización

### Modificar Ratio de Puntos
En `PurchasesForm.vb`, cambiar la línea:
```vb
Dim earnedPoints As Integer = CInt(Math.Floor(amount))
```

### Cambiar Colores de la Interfaz
Modificar los valores `Color.FromArgb()` en cada formulario.

### Agregar Nuevos Campos
1. **Modificar tabla** en la base de datos
2. **Actualizar formularios** correspondientes
3. **Modificar consultas** en `DatabaseHelper.vb`

## Seguridad

- **Consultas parametrizadas** para prevenir SQL Injection
- **Validación de entrada** en todos los formularios
- **Manejo de errores** robusto
- **Transacciones** para operaciones críticas

## Soporte y Mantenimiento

### Respaldos Recomendados
- **Base de datos:** Respaldo diario automático
- **Código fuente:** Control de versiones (Git)
- **Configuración:** Documentar cambios en cadenas de conexión

### Actualizaciones Futuras
- Implementar autenticación de usuarios
- Agregar niveles de fidelidad (Bronze, Silver, Gold)
- Integrar con sistemas de punto de venta
- Desarrollar aplicación web complementaria

## Licencia

Este sistema fue desarrollado como una solución completa de fidelidad de clientes. Puede ser modificado y distribuido según las necesidades del negocio.

---

**Desarrollado en Visual Basic 2010 con Microsoft SQL Server**  
**Versión:** 1.0.0  
**Fecha:** 2024
