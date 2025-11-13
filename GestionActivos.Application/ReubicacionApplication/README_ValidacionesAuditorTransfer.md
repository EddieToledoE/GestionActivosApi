# Validaciones del Endpoint POST /api/reubicaciones/auditor-transfer

## Descripción
Se han agregado validaciones de seguridad e integridad de datos al endpoint de transferencia directa por auditor para garantizar que solo se procesen operaciones válidas.

---

## Validaciones Implementadas

### ? 1. Auditor Activo
**Validación:** El auditor que realiza la transferencia debe estar activo en el sistema.

**Error (400 Bad Request):**
```json
{
  "message": "El auditor 'Carlos Martínez López' está inactivo y no puede realizar transferencias."
}
```

**Razón:** Un auditor inactivo no debe poder realizar operaciones administrativas en el sistema.

---

### ? 2. Activo Activo (Estado)
**Validación:** El activo debe tener estado "Activo".

**Error (400 Bad Request):**
```json
{
  "message": "El activo 'LAPTOP-001' no está activo (Estado: Inactivo). Solo se pueden transferir activos activos."
}
```

**Razón:** No se deben transferir activos que han sido dados de baja o están inactivos.

---

### ? 3. Usuario Anterior (Responsable Actual) Activo
**Validación:** El usuario que actualmente tiene asignado el activo debe estar activo.

**Error (400 Bad Request):**
```json
{
  "message": "El usuario actual responsable 'Juan Pérez García' está inactivo. No se puede procesar la transferencia."
}
```

**Razón:** Si el responsable actual está inactivo, indica un problema de datos que debe resolverse antes de transferir.

---

### ? 4. Usuario Destino Activo
**Validación:** El usuario que recibirá el activo debe estar activo en el sistema.

**Error (400 Bad Request):**
```json
{
  "message": "El usuario destino 'María López Sánchez' está inactivo. No se pueden asignar activos a usuarios inactivos."
}
```

**Razón:** No tiene sentido transferir activos a usuarios que ya no operan en el sistema.

---

### ? 5. Validar que no sea el Mismo Usuario
**Validación:** El usuario destino no puede ser el mismo que el responsable actual.

**Error (400 Bad Request):**
```json
{
  "message": "El activo 'LAPTOP-001' ya está asignado al usuario destino."
}
```

**Razón:** No tiene sentido crear una transferencia si el activo ya pertenece al usuario destino.

---

## Flujo de Validación Completo

```
1. ¿Auditor existe?
   ? No: NotFoundException (404)
   ?
2. ¿Auditor está activo?
   ? No: BusinessException (400)
   ?
3. ¿Activo existe?
   ? No: NotFoundException (404)
   ?
4. ¿Activo está activo?
   ? No: BusinessException (400)
   ?
5. ¿Usuario anterior existe?
   ? No: NotFoundException (404)
   ?
6. ¿Usuario anterior está activo?
   ? No: BusinessException (400)
   ?
7. ¿Usuario destino existe?
   ? No: NotFoundException (404)
   ?
8. ¿Usuario destino está activo?
   ? No: BusinessException (400)
   ?
9. ¿Es el mismo usuario?
   ? Sí: BusinessException (400)
   ?
10. ? Procesar transferencia
    - Crear reubicación
    - Actualizar activo
    - Crear 2 notificaciones
    - Commit transacción
```

---

## Ejemplos de Casos de Error

### Caso 1: Auditor Inactivo
**Request:**
```json
{
  "idAuditor": 10,      // Auditor inactivo
  "idActivo": 5,
  "idUsuarioDestino": 8,
  "motivo": "Reorganización"
}
```

**Response (400):**
```json
{
  "message": "El auditor 'Carlos Martínez López' está inactivo y no puede realizar transferencias."
}
```

---

### Caso 2: Activo Inactivo
**Request:**
```json
{
  "idAuditor": 10,
  "idActivo": 15,       // Activo con estado "Inactivo"
  "idUsuarioDestino": 8,
  "motivo": "Reorganización"
}
```

**Response (400):**
```json
{
  "message": "El activo 'MONITOR-034' no está activo (Estado: Inactivo). Solo se pueden transferir activos activos."
}
```

---

### Caso 3: Usuario Destino Inactivo
**Request:**
```json
{
  "idAuditor": 10,
  "idActivo": 5,
  "idUsuarioDestino": 3,  // Usuario inactivo
  "motivo": "Reorganización"
}
```

**Response (400):**
```json
{
  "message": "El usuario destino 'María López Sánchez' está inactivo. No se pueden asignar activos a usuarios inactivos."
}
```

---

### Caso 4: Responsable Actual Inactivo
**Request:**
```json
{
  "idAuditor": 10,
  "idActivo": 12,       // Activo asignado a usuario inactivo
  "idUsuarioDestino": 8,
  "motivo": "Reasignación"
}
```

**Response (400):**
```json
{
  "message": "El usuario actual responsable 'Pedro González' está inactivo. No se puede procesar la transferencia."
}
```

**Nota:** Este caso indica un problema de datos que debe resolverse. Si un activo está asignado a un usuario inactivo, primero debe reasignarse manualmente o mediante un proceso de limpieza de datos.

---

### Caso 5: Mismo Usuario
**Request:**
```json
{
  "idAuditor": 10,
  "idActivo": 5,        // Ya asignado al usuario 8
  "idUsuarioDestino": 8,
  "motivo": "Confirmación"
}
```

**Response (400):**
```json
{
  "message": "El activo 'LAPTOP-001' ya está asignado al usuario destino."
}
```

---

## Comparación con Endpoint de Solicitudes

| Validación | POST /api/solicitud | POST /api/reubicaciones/auditor-transfer |
|------------|---------------------|------------------------------------------|
| Usuario emisor activo | ? | ? (Auditor) |
| Usuario receptor activo | ? | ? (Usuario destino) |
| Activo activo | ? | ? |
| Usuario anterior activo | ? (N/A) | ? (Responsable actual) |
| Sin solicitudes duplicadas | ? | ? (No aplica, es transferencia directa) |
| Emisor es responsable | ? | ? (Auditor tiene privilegios) |

---

## Reglas de Negocio Aplicadas

| Regla | Descripción | Validación |
|-------|-------------|------------|
| **RN-01** | Solo auditores activos pueden hacer transferencias | ? Auditor.Activo == true |
| **RN-02** | Solo activos activos pueden ser transferidos | ? Activo.Estatus == "Activo" |
| **RN-03** | Usuario anterior debe estar activo | ? UsuarioAnterior.Activo == true |
| **RN-04** | Usuario destino debe estar activo | ? UsuarioDestino.Activo == true |
| **RN-05** | No transferir al mismo usuario | ? ResponsableId ? IdUsuarioDestino |

---

## Diferencias con Transferencia por Solicitud

### Transferencia por Solicitud
- ? Requiere aprobación del receptor
- ? Solo el responsable puede iniciar
- ? Valida que no haya solicitudes duplicadas
- ? No valida usuario anterior (es el mismo que el emisor)

### Transferencia por Auditor
- ? No requiere aprobación (privilegio administrativo)
- ? El auditor puede transferir cualquier activo
- ? No valida solicitudes duplicadas (es directa)
- ? Valida usuario anterior (puede ser diferente al auditor)

---

## Impacto en el Sistema

### Antes ?
- Auditor inactivo podía hacer transferencias
- Se podían transferir activos a usuarios inactivos
- Se podían transferir activos desde usuarios inactivos
- No se validaba el estado del responsable actual

### Ahora ?
- Solo auditores activos pueden hacer transferencias
- Solo usuarios activos pueden recibir activos
- Se valida que el responsable actual esté activo
- Sistema más consistente y seguro

---

## Caso Especial: Usuario Anterior Inactivo

### ¿Qué hacer si el responsable actual está inactivo?

Este caso indica un **problema de integridad de datos**. Si un activo está asignado a un usuario inactivo, significa que:

1. El usuario fue desactivado pero no se reasignaron sus activos
2. Existe una inconsistencia en los datos

### Soluciones Recomendadas:

#### Opción 1: Script de Limpieza
```sql
-- Reasignar activos de usuarios inactivos a un usuario administrativo
UPDATE MOV_ACTIVO
SET ResponsableId = @IdUsuarioAdmin
WHERE ResponsableId IN (
    SELECT IdUsuario 
    FROM CAT_USUARIO 
    WHERE Activo = 0
);
```

#### Opción 2: Proceso Manual
1. Identificar activos con responsables inactivos
2. Reasignar manualmente mediante auditor-transfer
3. Actualizar estado del usuario anterior

#### Opción 3: Endpoint Especial (Futuro)
```
POST /api/reubicaciones/auditor-transfer-force
```
Con flag `allowInactiveSource: true` para casos excepcionales.

---

## Casos de Uso Válidos

### Caso Válido 1: Reorganización Normal
```json
{
  "idAuditor": 10,        // Auditor activo
  "idActivo": 5,          // Activo activo, responsable activo
  "idUsuarioDestino": 8,  // Usuario destino activo
  "motivo": "Reorganización de equipos por cambio de estructura"
}
```

? **Resultado:** Transferencia exitosa

---

### Caso Válido 2: Reasignación Urgente
```json
{
  "idAuditor": 10,
  "idActivo": 12,
  "idUsuarioDestino": 5,
  "motivo": "Reasignación urgente por ausencia del responsable"
}
```

? **Resultado:** Transferencia exitosa con notificaciones automáticas

---

## Consultas Útiles

### Identificar activos con responsables inactivos
```sql
SELECT a.IdActivo, a.Etiqueta, a.Descripcion,
       u.IdUsuario, u.Nombres, u.ApellidoPaterno
FROM MOV_ACTIVO a
INNER JOIN CAT_USUARIO u ON a.ResponsableId = u.IdUsuario
WHERE u.Activo = 0 AND a.Estatus = 'Activo';
```

### Ver todas las transferencias realizadas por auditores
```sql
SELECT r.*, a.Etiqueta, 
       ua.Nombres as UsuarioAnterior,
       un.Nombres as UsuarioNuevo,
       aud.Nombres as Auditor
FROM H_REUBICACION r
INNER JOIN MOV_ACTIVO a ON r.IdActivo = a.IdActivo
INNER JOIN CAT_USUARIO ua ON r.IdUsuarioAnterior = ua.IdUsuario
INNER JOIN CAT_USUARIO un ON r.IdUsuarioNuevo = un.IdUsuario
LEFT JOIN CAT_USUARIO aud ON r.Motivo LIKE '[AUDITOR]%'
WHERE r.Motivo LIKE '[AUDITOR]%'
ORDER BY r.Fecha DESC;
```

---

## Componentes Modificados

### Application Layer
- ? `AuditorTransferCommandHandler.cs` - Agregadas 4 validaciones de usuarios activos

---

## Testing Recomendado

### Pruebas Unitarias
1. ? Transferir con auditor inactivo ? Error
2. ? Transferir activo inactivo ? Error
3. ? Transferir con usuario anterior inactivo ? Error
4. ? Transferir a usuario destino inactivo ? Error
5. ? Transferir al mismo usuario ? Error
6. ? Transferir válida con todos activos ? Éxito

### Pruebas de Integración
1. ? Flujo completo de transferencia con notificaciones
2. ? Verificar creación de reubicación
3. ? Verificar actualización de responsable
4. ? Verificar que ambas notificaciones se crean

---

## Orden de Validaciones

Las validaciones están ordenadas lógicamente:

1. **Validaciones de auditor** (quien ejecuta)
2. **Validaciones de activo** (qué se transfiere)
3. **Validaciones de usuario anterior** (desde dónde)
4. **Validaciones de usuario destino** (hacia dónde)
5. **Validaciones de lógica** (reglas de negocio)

Este orden permite mensajes de error más claros y procesamiento eficiente.

---

## Estado Final

- ? **4 Validaciones de usuarios activos agregadas**
- ? **1 Validación de activo activo (ya existía)**
- ? **1 Validación de mismo usuario (ya existía)**
- ? **Compilación exitosa**
- ? **Sistema más robusto y consistente**
- ? **Integridad de datos garantizada** ??

¡El endpoint POST /api/reubicaciones/auditor-transfer ahora es más seguro y confiable!
