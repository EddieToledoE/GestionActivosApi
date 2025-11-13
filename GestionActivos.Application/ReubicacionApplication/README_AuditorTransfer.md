# Flujo Completo: Transferencia de Activo por Auditor

## Descripción
Este flujo permite a un auditor transferir directamente un activo de un usuario a otro sin necesidad de crear una solicitud de aprobación. Es una transferencia administrativa directa que genera notificaciones automáticas para ambos usuarios involucrados.

**Nota:** Las notificaciones son genéricas y no tienen referencia directa al activo. La información del activo se incluye en el mensaje de texto de la notificación para mantener flexibilidad en el uso de notificaciones para otros fines no relacionados con activos.

---

## Endpoint

### URL
```
POST /api/reubicaciones/auditor-transfer
```

### Content-Type
```
application/json
```

---

## Request Body

| Campo | Tipo | Requerido | Descripción |
|-------|------|-----------|-------------|
| `idAuditor` | int | Sí | ID del auditor que realiza la transferencia |
| `idActivo` | int | Sí | ID del activo a transferir |
| `idUsuarioDestino` | int | Sí | ID del usuario que recibirá el activo |
| `motivo` | string | Sí | Motivo de la transferencia (máx 500 caracteres) |

### Ejemplo de Request

```bash
curl -X POST "https://localhost:7000/api/reubicaciones/auditor-transfer" \
  -H "Content-Type: application/json" \
  -d '{
    "idAuditor": 10,
    "idActivo": 5,
    "idUsuarioDestino": 8,
    "motivo": "Reorganización de equipos por cambio de estructura organizacional"
  }'
```

**Request Body:**
```json
{
  "idAuditor": 10,
  "idActivo": 5,
  "idUsuarioDestino": 8,
  "motivo": "Reorganización de equipos por cambio de estructura organizacional"
}
```

---

## Respuesta Exitosa (200 OK)

```json
{
  "success": true,
  "message": "Transferencia realizada correctamente."
}
```

---

## Respuestas de Error

### 404 Not Found - Auditor No Encontrado

```json
{
  "message": "No se encontró el auditor con ID 10."
}
```

### 404 Not Found - Activo No Encontrado

```json
{
  "message": "No se encontró el activo con ID 5."
}
```

### 404 Not Found - Usuario Destino No Encontrado

```json
{
  "message": "No se encontró el usuario destino con ID 8."
}
```

### 400 Bad Request - Activo No Activo

```json
{
  "message": "El activo 'LAPTOP-001' no está activo. Estado actual: Inactivo"
}
```

### 400 Bad Request - Mismo Usuario

```json
{
  "message": "El activo ya está asignado al usuario destino."
}
```

### 400 Bad Request - Validación Fallida

```json
{
  "errors": {
    "Motivo": ["El motivo de la transferencia es obligatorio."]
  }
}
```

---

## Flujo de Transacción

```
1. BEGIN TRANSACTION
   ?
2. Validar que auditor existe
   ?
3. Obtener activo por ID
   ?
4. Validar que activo exista y esté activo
   ?
5. Obtener usuario destino por ID
   ?
6. Validar que usuario destino existe
   ?
7. Validar que no sea el mismo responsable
   ?
8. Construir descripción del activo para mensajes
   ?
9. Crear Reubicación
   - IdActivo: 5
   - IdUsuarioAnterior: 3 (responsable actual)
   - IdUsuarioNuevo: 8
   - Fecha: DateTime.Now
   - Motivo: "[AUDITOR] {motivo}"
   ?
10. Actualizar Activo.ResponsableId = 8
   ?
11. Crear Notificación #1 (Usuario Anterior)
    - Tipo: "Revocación de Activo"
    - Título: "Activo reasignado por auditoría"
    - Mensaje: Incluye información completa del activo
   ?
12. Crear Notificación #2 (Usuario Nuevo)
    - Tipo: "Asignación de Activo"
    - Título: "Nuevo activo asignado"
    - Mensaje: Incluye información completa del activo
   ?
13. SAVE CHANGES
    ?
14. COMMIT TRANSACTION
```

**Si ocurre error en cualquier paso:**
```
ROLLBACK TRANSACTION
Lanzar excepción con mensaje descriptivo
```

---

## Reglas de Negocio

### Validaciones

1. ? **Auditor debe existir**: Se valida la existencia del auditor en la base de datos
2. ? **Activo debe existir**: Se valida la existencia del activo
3. ? **Activo debe estar activo**: Solo se pueden transferir activos con estado "Activo"
4. ? **Usuario destino debe existir**: Se valida la existencia del usuario destino
5. ? **No mismo usuario**: El usuario destino no puede ser el mismo que el responsable actual
6. ? **Motivo obligatorio**: El motivo de la transferencia es requerido (máx 500 caracteres)

### Operaciones Ejecutadas

1. ? **Creación de Reubicación**:
   - Registro histórico del movimiento
   - Se guarda el usuario anterior y el nuevo
   - Motivo prefijado con "[AUDITOR]"

2. ? **Actualización de Activo**:
   - El campo `ResponsableId` se actualiza al nuevo usuario
   - El activo mantiene su estado y demás propiedades

3. ? **Notificaciones Automáticas**:
   - **Usuario Anterior**: Notificación de tipo "Revocación de Activo"
   - **Usuario Nuevo**: Notificación de tipo "Asignación de Activo"
   - Ambas marcadas como no leídas
   - Origen: "Auditoría"
   - **La información del activo se incluye en el mensaje de texto**

---

## Diferencias con Transferencia por Solicitud

| Característica | Por Solicitud | Por Auditor |
|----------------|---------------|-------------|
| **Requiere aprobación** | Sí | No |
| **Estado intermedio** | Pendiente ? Aceptada | Directa |
| **Crea solicitud** | Sí | No |
| **Crea reubicación** | Al aceptar | Inmediata |
| **Notificaciones** | Al aprobar/rechazar | Inmediatas (2) |
| **Puede revertirse** | Sí (rechazar) | No (directa) |
| **Quién autoriza** | Receptor | Auditor |
| **Motivo** | Opcional | Obligatorio |

---

## Estructura de Notificaciones

### Notificación para Usuario Anterior (Revocación)

```json
{
  "idNotificacion": 45,
  "idUsuarioDestino": 3,
  "origen": "Auditoría",
  "tipo": "Revocación de Activo",
  "titulo": "Activo reasignado por auditoría",
  "mensaje": "El activo 'LAPTOP-001' - Laptop Dell Latitude (Dell Latitude 7420) ha sido reasignado por un auditor. Motivo: Reorganización de equipos",
  "fecha": "2024-11-12T14:30:00",
  "leida": false
}
```

### Notificación para Usuario Nuevo (Asignación)

```json
{
  "idNotificacion": 46,
  "idUsuarioDestino": 8,
  "origen": "Auditoría",
  "tipo": "Asignación de Activo",
  "titulo": "Nuevo activo asignado",
  "mensaje": "Se te ha asignado el activo 'LAPTOP-001' - Laptop Dell Latitude (Dell Latitude 7420). Motivo: Reorganización de equipos",
  "fecha": "2024-11-12T14:30:00",
  "leida": false
}
```

### Formato del Mensaje

El mensaje incluye la información del activo en este formato:
```
'{Etiqueta}' - {Descripción} ({Marca} {Modelo})
```

Ejemplos:
- `'LAPTOP-001' - Laptop ejecutiva (Dell Latitude 7420)`
- `'MONITOR-034' - Monitor de 27 pulgadas (Samsung S27)`
- `'TECLADO-123' - Sin descripción`

---

## Componentes Creados

### Domain Layer
- ? `INotificacionRepository.cs` - Interface del repositorio de notificaciones

### Infrastructure Layer
- ? `NotificacionRepository.cs` - Implementación del repositorio
- ? `ActivosUnitOfWork.cs` - Actualizado con INotificacionRepository

### Application Layer
- ? `AuditorTransferCommand.cs` - Command CQRS
- ? `AuditorTransferCommandValidator.cs` - Validador FluentValidation
- ? `AuditorTransferCommandHandler.cs` - Handler con transacción completa

### API Layer
- ? `ReubicacionesController.cs` - Controlador con endpoint POST

### Dependency Injection
- ? `DependencyInjectionExtensions.cs` - Registro de INotificacionRepository

---

## Uso del Unit of Work

Este flujo utiliza el patrón **Unit of Work** para garantizar que todas las operaciones se ejecuten de forma atómica:

```csharp
await _uow.BeginTransactionAsync();

try 
{
    // 1. Obtener y validar entidades
    var auditor = await _uow.Usuarios.GetByIdAsync(idAuditor);
    var activo = await _uow.Activos.GetByIdAsync(idActivo);
    var usuarioDestino = await _uow.Usuarios.GetByIdAsync(idUsuarioDestino);
    
    // 2. Construir información descriptiva del activo
    string infoActivo = $"'{activo.Etiqueta}' - {activo.Descripcion} ({activo.Marca} {activo.Modelo})";
    
    // 3. Crear reubicación
    await _uow.Reubicaciones.AddAsync(reubicacion);
    
    // 4. Actualizar activo
    await _uow.Activos.UpdateAsync(activo);
    
    // 5. Crear notificación #1 (anterior) con info del activo en mensaje
    await _uow.Notificaciones.AddAsync(notificacionAnterior);
    
    // 6. Crear notificación #2 (nuevo) con info del activo en mensaje
    await _uow.Notificaciones.AddAsync(notificacionNuevo);
    
    // 7. Guardar y confirmar TODO
    await _uow.SaveChangesAsync();
    await _uow.CommitAsync();
} 
catch 
{
    await _uow.RollbackAsync(); // Revierte TODO
    throw;
}
```

---

## Arquitectura

- ? **CQRS** con MediatR (Command pattern)
- ? **Unit of Work** para transacciones atómicas
- ? **Repository Pattern** para acceso a datos
- ? **FluentValidation** para validaciones
- ? **Clean Architecture** (Domain ? Application ? Infrastructure ? API)
- ? **Transacciones ACID** con EF Core
- ? **Notificaciones genéricas** sin dependencias a entidades específicas

---

## Diseño de Notificaciones

### Características

1. **Genéricas**: La entidad `Notificacion` no tiene FK a `Activo`
2. **Flexibles**: Pueden usarse para cualquier tipo de notificación
3. **Descriptivas**: Toda la información necesaria va en el mensaje de texto
4. **Reutilizables**: El mismo sistema sirve para notificaciones de:
   - Activos (asignación, revocación)
   - Sistema (mantenimiento, alertas)
   - Administración (anuncios, recordatorios)
   - Auditorías (inspecciones, cambios)

### Ventajas del Diseño

? **Sin acoplamiento**: No depende de entidades específicas  
? **Extensible**: Fácil agregar nuevos tipos de notificaciones  
? **Simple**: Estructura plana, fácil de consultar  
? **Completa**: Toda la info necesaria en campos de texto  
? **Histórica**: Se mantiene el mensaje completo aunque se elimine el activo  

---

## Ejemplo de Flujo Completo

### Escenario
Un auditor necesita reasignar el activo "LAPTOP-001" del Usuario 3 al Usuario 8 debido a una reorganización.

### Request
```bash
POST /api/reubicaciones/auditor-transfer
{
  "idAuditor": 10,
  "idActivo": 5,
  "idUsuarioDestino": 8,
  "motivo": "Reorganización de equipos por cambio de estructura"
}
```

### Resultado en Base de Datos

**Tabla: H_REUBICACION**
```
IdReubicacion: 25
IdActivo: 5
IdUsuarioAnterior: 3
IdUsuarioNuevo: 8
Fecha: 2024-11-12 14:30:00
Motivo: "[AUDITOR] Reorganización de equipos por cambio de estructura"
```

**Tabla: MOV_ACTIVO**
```
IdActivo: 5
ResponsableId: 8 (actualizado de 3 a 8)
Etiqueta: "LAPTOP-001"
Descripcion: "Laptop Dell Latitude"
Marca: "Dell"
Modelo: "Latitude 7420"
```

**Tabla: MOV_NOTIFICACION** (2 registros)
```
1. IdNotificacion: 45
   IdUsuarioDestino: 3
   Origen: "Auditoría"
   Tipo: "Revocación de Activo"
   Titulo: "Activo reasignado por auditoría"
   Mensaje: "El activo 'LAPTOP-001' - Laptop Dell Latitude (Dell Latitude 7420) ha sido reasignado por un auditor. Motivo: Reorganización de equipos por cambio de estructura"
   Fecha: 2024-11-12 14:30:00
   Leida: false

2. IdNotificacion: 46
   IdUsuarioDestino: 8
   Origen: "Auditoría"
   Tipo: "Asignación de Activo"
   Titulo: "Nuevo activo asignado"
   Mensaje: "Se te ha asignado el activo 'LAPTOP-001' - Laptop Dell Latitude (Dell Latitude 7420). Motivo: Reorganización de equipos por cambio de estructura"
   Fecha: 2024-11-12 14:30:00
   Leida: false
```

---

## Próximos Pasos Sugeridos

1. **Endpoints de Notificaciones**:
   - `GET /api/notificaciones/usuario/{id}` - Obtener todas las notificaciones
   - `GET /api/notificaciones/no-leidas/{id}` - Obtener no leídas
   - `PUT /api/notificaciones/{id}/marcar-leida` - Marcar como leída
   - `GET /api/notificaciones/count/{id}` - Contador de no leídas

2. **Endpoints de Reubicaciones**:
   - `GET /api/reubicaciones/activo/{id}` - Historial de un activo
   - `GET /api/reubicaciones/usuario/{id}` - Reubicaciones de un usuario
   - `GET /api/reubicaciones/recientes` - Últimas reubicaciones

3. **Validación de Roles**:
   - Agregar middleware para verificar que el usuario sea auditor
   - `[Authorize(Roles = "Auditor")]`

4. **Logs y Auditoría**:
   - Registrar quién realizó la transferencia
   - Timestamp de la operación
   - IP o información del cliente

5. **Notificaciones Push** (futuro):
   - SignalR para notificaciones en tiempo real
   - Email para notificaciones importantes
   - SMS para alertas críticas

---

## Estado Final

- ? **Command creado**
- ? **Validator implementado**
- ? **Handler con transacciones**
- ? **Controller con endpoint**
- ? **Repositorio de notificaciones**
- ? **UoW actualizado**
- ? **DI configurado**
- ? **Notificaciones genéricas sin FK a activos**
- ? **Listo para compilar y usar** ??

¡El flujo completo de transferencia por auditor está implementado con notificaciones genéricas y flexibles!
