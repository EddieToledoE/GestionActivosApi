# Validaciones del Endpoint POST /api/Solicitud

## Descripción
Se han agregado validaciones de seguridad y consistencia de datos al endpoint de creación de solicitudes para garantizar la integridad del sistema.

---

## Validaciones Implementadas

### ? 1. Usuario Emisor Activo
**Validación:** El usuario emisor debe estar activo en el sistema.

**Error (400 Bad Request):**
```json
{
  "message": "El usuario emisor 'Juan Pérez García' está inactivo y no puede crear solicitudes."
}
```

**Razón:** Un usuario inactivo no debe poder realizar operaciones en el sistema.

---

### ? 2. Usuario Receptor Activo
**Validación:** El usuario receptor debe estar activo en el sistema.

**Error (400 Bad Request):**
```json
{
  "message": "El usuario receptor 'María López Sánchez' está inactivo y no puede recibir solicitudes."
}
```

**Razón:** No tiene sentido transferir activos a usuarios inactivos que ya no operan en el sistema.

---

### ? 3. Activo Activo (Estado)
**Validación:** El activo debe tener estado "Activo".

**Error (400 Bad Request):**
```json
{
  "message": "El activo 'LAPTOP-001' no está activo (Estado: Inactivo). Solo se pueden crear solicitudes para activos activos."
}
```

**Razón:** No se deben transferir activos que han sido dados de baja o están inactivos.

---

### ? 4. Sin Solicitudes Pendientes Duplicadas
**Validación:** No debe existir otra solicitud pendiente para el mismo activo.

**Error (400 Bad Request):**
```json
{
  "message": "Ya existe una solicitud pendiente para el activo 'LAPTOP-001'. No se pueden crear solicitudes duplicadas hasta que la anterior sea procesada."
}
```

**Razón:** Evita conflictos y asegura que solo haya una solicitud activa por activo a la vez.

---

### ? 5. Emisor es Responsable Actual
**Validación:** Solo el responsable actual del activo puede crear una solicitud de transferencia.

**Error (400 Bad Request):**
```json
{
  "message": "Solo el responsable actual del activo 'LAPTOP-001' puede crear una solicitud de transferencia. Responsable actual: ID 5"
}
```

**Razón:** Solo quien tiene el activo asignado puede solicitar transferirlo.

---

## Flujo de Validación Completo

```
1. ¿Solicitud es nula?
   ? Sí: ArgumentNullException
   ?
2. ¿Emisor existe?
   ? No: NotFoundException (404)
   ?
3. ¿Emisor está activo?
   ? No: BusinessException (400)
   ?
4. ¿Receptor existe?
   ? No: NotFoundException (404)
   ?
5. ¿Receptor está activo?
   ? No: BusinessException (400)
   ?
6. ¿Activo existe?
   ? No: NotFoundException (404)
   ?
7. ¿Activo está activo?
   ? No: BusinessException (400)
   ?
8. ¿Ya existe solicitud pendiente para este activo?
   ? Sí: BusinessException (400)
   ?
9. ¿Emisor es el responsable actual?
   ? No: BusinessException (400)
   ?
10. ? Crear solicitud
```

---

## Ejemplos de Casos de Error

### Caso 1: Emisor Inactivo
**Request:**
```json
{
  "idEmisor": 5,       // Usuario inactivo
  "idReceptor": 2,
  "idActivo": 10,
  "tipo": "Transferencia",
  "mensaje": "Necesito transferir este equipo"
}
```

**Response (400):**
```json
{
  "message": "El usuario emisor 'Pedro González' está inactivo y no puede crear solicitudes."
}
```

---

### Caso 2: Activo Inactivo
**Request:**
```json
{
  "idEmisor": 1,
  "idReceptor": 2,
  "idActivo": 15,      // Activo con estado "Inactivo"
  "tipo": "Transferencia",
  "mensaje": "Solicitud de transferencia"
}
```

**Response (400):**
```json
{
  "message": "El activo 'MONITOR-034' no está activo (Estado: Inactivo). Solo se pueden crear solicitudes para activos activos."
}
```

---

### Caso 3: Solicitud Pendiente Duplicada
**Request:**
```json
{
  "idEmisor": 1,
  "idReceptor": 3,
  "idActivo": 5,       // Ya tiene una solicitud pendiente
  "tipo": "Transferencia",
  "mensaje": "Segunda solicitud"
}
```

**Response (400):**
```json
{
  "message": "Ya existe una solicitud pendiente para el activo 'LAPTOP-001'. No se pueden crear solicitudes duplicadas hasta que la anterior sea procesada."
}
```

---

### Caso 4: Emisor No es Responsable
**Request:**
```json
{
  "idEmisor": 3,       // No es el responsable
  "idReceptor": 2,
  "idActivo": 5,       // Responsable actual: ID 1
  "tipo": "Transferencia",
  "mensaje": "Intento transferir activo ajeno"
}
```

**Response (400):**
```json
{
  "message": "Solo el responsable actual del activo 'LAPTOP-001' puede crear una solicitud de transferencia. Responsable actual: ID 1"
}
```

---

## Cambios en el Repositorio

### ISolicitudRepository
```csharp
Task<bool> ExisteSolicitudPendienteParaActivoAsync(int idActivo);
```

### SolicitudRepository
```csharp
public async Task<bool> ExisteSolicitudPendienteParaActivoAsync(int idActivo)
{
    return await _context.Solicitudes
        .AnyAsync(s => s.IdActivo == idActivo && s.Estado == "Pendiente");
}
```

---

## Orden de Validaciones

Las validaciones están ordenadas estratégicamente:

1. **Validaciones de existencia** (404): Primero verificamos que las entidades existan
2. **Validaciones de estado** (400): Luego validamos que estén activas
3. **Validaciones de negocio** (400): Finalmente validamos reglas de negocio complejas

Este orden permite mensajes de error más claros y lógicos para el usuario.

---

## Reglas de Negocio Aplicadas

| Regla | Descripción | Validación |
|-------|-------------|------------|
| **RN-01** | Solo usuarios activos pueden crear solicitudes | ? Emisor.Activo == true |
| **RN-02** | Solo usuarios activos pueden recibir solicitudes | ? Receptor.Activo == true |
| **RN-03** | Solo activos activos pueden ser transferidos | ? Activo.Estatus == "Activo" |
| **RN-04** | Un activo solo puede tener una solicitud pendiente | ? No existe otra pendiente |
| **RN-05** | Solo el responsable puede solicitar transferencia | ? Activo.ResponsableId == IdEmisor |

---

## Impacto en el Sistema

### Antes ?
- Se podían crear solicitudes con usuarios inactivos
- Se podían transferir activos dados de baja
- Se podían crear múltiples solicitudes pendientes para el mismo activo
- Cualquier usuario podía solicitar transferir cualquier activo

### Ahora ?
- Solo usuarios activos participan en solicitudes
- Solo activos activos pueden ser transferidos
- Un activo solo puede tener una solicitud pendiente a la vez
- Solo el responsable actual puede solicitar transferir su activo

---

## Casos de Uso Válidos

### Caso Válido 1: Transferencia Normal
```json
{
  "idEmisor": 1,          // Usuario activo, responsable del activo
  "idReceptor": 2,        // Usuario activo
  "idActivo": 5,          // Activo activo, sin solicitudes pendientes
  "tipo": "Transferencia",
  "mensaje": "Cambio de proyecto"
}
```

? **Resultado:** Solicitud creada exitosamente

---

### Caso Válido 2: Diagnóstico
```json
{
  "idEmisor": 3,          // Usuario activo, responsable del activo
  "idReceptor": 10,       // Técnico activo
  "idActivo": 12,         // Activo activo, sin solicitudes pendientes
  "tipo": "Diagnóstico",
  "mensaje": "El equipo presenta fallas"
}
```

? **Resultado:** Solicitud creada exitosamente

---

## Flujo de Aceptación de Solicitud

Una vez que una solicitud pendiente es aceptada o rechazada:

1. **Si se acepta**:
   - Estado cambia a "Aceptada"
   - Se crea la reubicación
   - El activo cambia de responsable
   - ? Se puede crear una nueva solicitud para ese activo

2. **Si se rechaza**:
   - Estado cambia a "Rechazada"
   - NO se crea reubicación
   - El activo mantiene su responsable
   - ? Se puede crear una nueva solicitud para ese activo

3. **Mientras está pendiente**:
   - ? NO se pueden crear nuevas solicitudes para ese activo

---

## Consultas Útiles

### Verificar si un activo tiene solicitudes pendientes
```sql
SELECT * FROM MOV_SOLICITUD 
WHERE IdActivo = 5 AND Estado = 'Pendiente';
```

### Ver todas las solicitudes pendientes
```sql
SELECT s.*, a.Etiqueta, 
       e.Nombres as EmisorNombre, 
       r.Nombres as ReceptorNombre
FROM MOV_SOLICITUD s
INNER JOIN MOV_ACTIVO a ON s.IdActivo = a.IdActivo
INNER JOIN CAT_USUARIO e ON s.IdEmisor = e.IdUsuario
INNER JOIN CAT_USUARIO r ON s.IdReceptor = r.IdUsuario
WHERE s.Estado = 'Pendiente';
```

---

## Componentes Modificados

### Domain Layer
- ? `ISolicitudRepository.cs` - Agregado método `ExisteSolicitudPendienteParaActivoAsync`

### Infrastructure Layer
- ? `SolicitudRepository.cs` - Implementado método de verificación

### Application Layer
- ? `CreateSolicitudCommand.cs` - Agregadas 5 validaciones nuevas

---

## Testing Recomendado

### Pruebas Unitarias
1. ? Crear solicitud con emisor inactivo ? Error
2. ? Crear solicitud con receptor inactivo ? Error
3. ? Crear solicitud con activo inactivo ? Error
4. ? Crear solicitud con solicitud pendiente existente ? Error
5. ? Crear solicitud sin ser responsable ? Error
6. ? Crear solicitud válida ? Éxito

### Pruebas de Integración
1. ? Flujo completo: Crear ? Aceptar ? Crear nueva para mismo activo
2. ? Flujo completo: Crear ? Rechazar ? Crear nueva para mismo activo
3. ? Intentar crear duplicada ? Error ? Procesar pendiente ? Reintentar con éxito

---

## Estado Final

- ? **5 Validaciones agregadas**
- ? **Método en repositorio implementado**
- ? **Compilación exitosa**
- ? **Integridad de datos garantizada**
- ? **Reglas de negocio aplicadas**
- ? **Sistema más robusto y consistente** ??

¡El endpoint POST /api/Solicitud ahora es mucho más seguro y confiable!
