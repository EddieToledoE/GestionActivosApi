# Endpoints: Aceptar y Rechazar Solicitudes

## Descripción
Estos endpoints permiten a un usuario receptor procesar las solicitudes pendientes que ha recibido, ya sea aceptándolas o rechazándolas.

---

## 1. Aceptar Solicitud

### URL
```
PUT /api/Solicitud/{id}/aceptar
```

### Content-Type
```
application/json
```

### Parámetros

**URL:**
| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `id` | int | ID de la solicitud a aceptar |

**Body:**
| Campo | Tipo | Requerido | Descripción |
|-------|------|-----------|-------------|
| `idUsuarioAprobador` | int | Sí | ID del usuario que acepta la solicitud (debe ser el receptor) |

### Descripción
Acepta una solicitud pendiente y ejecuta las siguientes acciones en una transacción:
1. Cambia el estado de la solicitud a "Aceptada"
2. Crea un registro de reubicación del activo
3. Actualiza el responsable del activo al nuevo usuario

### Ejemplo de Request

```bash
curl -X PUT "https://localhost:7000/api/Solicitud/15/aceptar" \
  -H "Content-Type: application/json" \
  -d '{
    "idUsuarioAprobador": 2
  }'
```

**Request Body:**
```json
{
  "idUsuarioAprobador": 2
}
```

### Respuesta Exitosa (200 OK)

```json
{
  "message": "Solicitud aceptada correctamente"
}
```

---

## 2. Rechazar Solicitud

### URL
```
PUT /api/Solicitud/{id}/rechazar
```

### Content-Type
```
application/json
```

### Parámetros

**URL:**
| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `id` | int | ID de la solicitud a rechazar |

**Body:**
| Campo | Tipo | Requerido | Descripción |
|-------|------|-----------|-------------|
| `idUsuarioAprobador` | int | Sí | ID del usuario que rechaza la solicitud (debe ser el receptor) |
| `motivoRechazo` | string | No | Motivo por el cual se rechaza la solicitud |

### Descripción
Rechaza una solicitud pendiente, cambiando su estado a "Rechazada". Opcionalmente puede agregar un motivo de rechazo.

### Ejemplo de Request

```bash
curl -X PUT "https://localhost:7000/api/Solicitud/15/rechazar" \
  -H "Content-Type: application/json" \
  -d '{
    "idUsuarioAprobador": 2,
    "motivoRechazo": "El equipo está asignado a un proyecto crítico"
  }'
```

**Request Body:**
```json
{
  "idUsuarioAprobador": 2,
  "motivoRechazo": "El equipo está asignado a un proyecto crítico"
}
```

### Respuesta Exitosa (200 OK)

```json
{
  "message": "Solicitud rechazada correctamente"
}
```

---

## Reglas de Negocio

### Validaciones Comunes

1. **Estado de la Solicitud**:
   - Solo se pueden procesar solicitudes en estado "Pendiente"
   - Si la solicitud ya fue procesada, se lanza una excepción

2. **Autorización**:
   - Solo el **receptor** de la solicitud puede aceptarla o rechazarla
   - Si otro usuario intenta procesarla, se lanza una excepción

3. **Existencia**:
   - La solicitud debe existir en la base de datos
   - El activo relacionado debe existir

### Acciones al Aceptar

1. ? Actualiza `Estado` de "Pendiente" a "Aceptada"
2. ? Crea un registro en la tabla `Reubicaciones`:
   - `IdActivo`: El activo involucrado
   - `IdUsuarioAnterior`: Responsable actual del activo
   - `IdUsuarioNuevo`: El receptor de la solicitud
   - `Fecha`: Fecha actual
   - `Motivo`: "Transferencia por solicitud #{IdSolicitud}"
3. ? Actualiza `ResponsableId` del activo al nuevo usuario

### Acciones al Rechazar

1. ? Actualiza `Estado` de "Pendiente" a "Rechazada"
2. ? Agrega el motivo de rechazo al campo `Mensaje` (si se proporciona)
3. ? NO crea reubicación
4. ? NO cambia el responsable del activo

---

## Respuestas de Error

### 404 Not Found - Solicitud No Encontrada

```json
{
  "message": "No se encontró la solicitud con ID 999."
}
```

### 400 Bad Request - Solicitud Ya Procesada

```json
{
  "message": "La solicitud ya ha sido procesada. Estado actual: Aceptada"
}
```

### 403 Forbidden - Usuario No Autorizado

```json
{
  "message": "Solo el receptor de la solicitud puede aceptarla."
}
```

O

```json
{
  "message": "Solo el receptor de la solicitud puede rechazarla."
}
```

### 404 Not Found - Activo No Encontrado (Solo en Aceptar)

```json
{
  "message": "No se encontró el activo con ID 555."
}
```

---

## Flujo de Transacción (Aceptar)

```
1. BEGIN TRANSACTION
   ?
2. Obtener Solicitud por ID
   ?
3. Validar Estado = "Pendiente"
   ?
4. Validar IdUsuarioAprobador = IdReceptor
   ?
5. Cambiar Estado a "Aceptada"
   ?
6. Obtener Activo por ID
   ?
7. Crear Reubicación
   - IdActivo
   - IdUsuarioAnterior (responsable actual)
   - IdUsuarioNuevo (receptor)
   - Fecha, Motivo
   ?
8. Actualizar Activo.ResponsableId
   ?
9. SAVE CHANGES
   ?
10. COMMIT TRANSACTION
```

**Si ocurre un error en cualquier paso:**
```
ROLLBACK TRANSACTION
Lanzar excepción
```

---

## Flujo de Transacción (Rechazar)

```
1. BEGIN TRANSACTION
   ?
2. Obtener Solicitud por ID
   ?
3. Validar Estado = "Pendiente"
   ?
4. Validar IdUsuarioAprobador = IdReceptor
   ?
5. Cambiar Estado a "Rechazada"
   ?
6. Agregar MotivoRechazo al Mensaje (opcional)
   ?
7. SAVE CHANGES
   ?
8. COMMIT TRANSACTION
```

**Si ocurre un error en cualquier paso:**
```
ROLLBACK TRANSACTION
Lanzar excepción
```

---

## Uso del Unit of Work (UoW)

Estos endpoints utilizan el patrón **Unit of Work** para garantizar:

? **Atomicidad**: Todas las operaciones se completan o ninguna  
? **Consistencia**: Los datos mantienen su integridad  
? **Aislamiento**: Las transacciones no interfieren entre sí  
? **Durabilidad**: Los cambios confirmados son permanentes  

### Ejemplo de Código Interno

```csharp
await _uow.BeginTransactionAsync();
try {
    // 1. Actualizar solicitud
    solicitud.Estado = "Aceptada";
    await _uow.Solicitudes.UpdateAsync(solicitud);
    
    // 2. Crear reubicación
    await _uow.Reubicaciones.AddAsync(reubicacion);
    
    // 3. Actualizar activo
    await _uow.Activos.UpdateAsync(activo);
    
    // 4. Guardar y confirmar
    await _uow.SaveChangesAsync();
    await _uow.CommitAsync();
} catch {
    await _uow.RollbackAsync();
    throw;
}
```

---

## Ejemplos de Uso en UI

### Dashboard de Solicitudes Recibidas

```typescript
// Usuario ve sus solicitudes pendientes
GET /api/Solicitud/pendientes/receptor/2

// Usuario acepta una solicitud
PUT /api/Solicitud/15/aceptar
{
  "idUsuarioAprobador": 2
}

// Usuario rechaza una solicitud con motivo
PUT /api/Solicitud/18/rechazar
{
  "idUsuarioAprobador": 2,
  "motivoRechazo": "El activo está en uso crítico"
}
```

### Flujo Completo

```
1. Usuario A (ID: 1) crea solicitud de transferencia
   POST /api/Solicitud
   {
     "idEmisor": 1,
     "idReceptor": 2,
     "idActivo": 5,
     "tipo": "Transferencia",
     "mensaje": "Necesito este activo para mi proyecto"
   }

2. Usuario B (ID: 2) ve la solicitud en su bandeja
   GET /api/Solicitud/pendientes/receptor/2

3. Usuario B acepta la solicitud
   PUT /api/Solicitud/{id}/aceptar
   {
     "idUsuarioAprobador": 2
   }

4. Sistema ejecuta:
   - Cambia estado a "Aceptada"
   - Crea reubicación
   - Activo.ResponsableId = 2 (Usuario B)
```

---

## Componentes Creados

### Commands
- ? `AceptarSolicitudCommand` - Procesa aceptación con UoW
- ? `RechazarSolicitudCommand` - Procesa rechazo con UoW

### Handlers
- ? `AceptarSolicitudHandler` - Maneja transacción completa
- ? `RechazarSolicitudHandler` - Maneja actualización simple

### Controller
- ? `PUT /api/Solicitud/{id}/aceptar` - Endpoint para aceptar
- ? `PUT /api/Solicitud/{id}/rechazar` - Endpoint para rechazar

### Request DTOs
- ? `AceptarSolicitudRequest` - { idUsuarioAprobador }
- ? `RechazarSolicitudRequest` - { idUsuarioAprobador, motivoRechazo? }

---

## Estados de Solicitud

| Estado | Descripción | Transiciones Permitidas |
|--------|-------------|------------------------|
| `Pendiente` | Solicitud creada, esperando respuesta | ? Aceptada, Rechazada |
| `Aceptada` | Solicitud aprobada, activo transferido | (Estado final) |
| `Rechazada` | Solicitud denegada | (Estado final) |

---

## Arquitectura

- ? **CQRS** con MediatR
- ? **Unit of Work Pattern** para transacciones
- ? **Repository Pattern** para acceso a datos
- ? **Transacciones ACID** con EF Core
- ? **Manejo de excepciones** centralizado
- ? **Validaciones de negocio** en handlers

---

## Endpoints Completos de Solicitud

| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | `/api/Solicitud` | Crear solicitud |
| GET | `/api/Solicitud/pendientes/emisor/{id}` | Solicitudes enviadas pendientes |
| GET | `/api/Solicitud/pendientes/receptor/{id}` | Solicitudes recibidas pendientes |
| PUT | `/api/Solicitud/{id}/aceptar` | Aceptar solicitud |
| PUT | `/api/Solicitud/{id}/rechazar` | Rechazar solicitud |
