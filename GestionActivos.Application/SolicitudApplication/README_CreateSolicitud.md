# Endpoint: Crear Solicitud

## Descripción
Este endpoint permite crear una nueva solicitud entre dos usuarios (emisor y receptor) relacionada con un activo específico.

---

## URL
```
POST /api/Solicitud
```

## Content-Type
```
application/json
```

## Parámetros del Body

| Campo | Tipo | Requerido | Descripción |
|-------|------|-----------|-------------|
| `IdEmisor` | int | Sí | ID del usuario que envía la solicitud |
| `IdReceptor` | int | Sí | ID del usuario que recibe la solicitud |
| `IdActivo` | int | Sí | ID del activo relacionado con la solicitud |
| `Tipo` | string | Sí | Tipo de solicitud (Transferencia, Baja, Diagnóstico, Auditoría) |
| `Mensaje` | string | No | Mensaje descriptivo de la solicitud (máx 300 caracteres) |

---

## Reglas de Negocio

1. **Validación de Usuarios y Activo**:
   - El `IdEmisor` debe corresponder a un usuario existente
   - El `IdReceptor` debe corresponder a un usuario existente
   - El `IdActivo` debe corresponder a un activo existente
   - El emisor y receptor deben ser diferentes

2. **Tipos de Solicitud Permitidos**:
   - `Transferencia`: Solicitud para transferir un activo
   - `Baja`: Solicitud para dar de baja un activo
   - `Diagnóstico`: Solicitud para diagnóstico de un activo
   - `Auditoría`: Solicitud relacionada con auditoría

3. **Estado por Defecto**:
   - Todas las solicitudes se crean con estado `"Pendiente"`
   - La fecha se establece automáticamente al momento de creación

4. **Validación de Campos**:
   - `IdEmisor`, `IdReceptor` y `IdActivo` deben ser mayores a 0
   - `Tipo` no puede estar vacío y debe ser uno de los valores permitidos
   - `Mensaje` es opcional pero no puede exceder 300 caracteres

---

## Ejemplo de Request

```bash
curl -X POST "https://localhost:7000/api/Solicitud" \
  -H "Content-Type: application/json" \
  -d '{
    "idEmisor": 1,
    "idReceptor": 2,
    "idActivo": 5,
    "tipo": "Transferencia",
    "mensaje": "Solicito la transferencia del activo LAPTOP-001 por cambio de proyecto"
  }'
```

### Request Body

```json
{
  "idEmisor": 1,
  "idReceptor": 2,
  "idActivo": 5,
  "tipo": "Transferencia",
  "mensaje": "Solicito la transferencia del activo LAPTOP-001 por cambio de proyecto"
}
```

---

## Respuesta Exitosa (200 OK)

```json
{
  "message": "Solicitud creada correctamente",
  "id": 15
}
```

---

## Respuestas de Error

### 400 Bad Request - Validación Fallida

**Ejemplo 1: Emisor y receptor son iguales**
```json
{
  "errors": {
    "IdReceptor": ["El receptor no puede ser el mismo que el emisor."]
  }
}
```

**Ejemplo 2: Tipo de solicitud inválido**
```json
{
  "errors": {
    "Tipo": ["El tipo debe ser: Transferencia, Baja, Diagnóstico o Auditoría."]
  }
}
```

**Ejemplo 3: Falta ID del activo**
```json
{
  "errors": {
    "IdActivo": ["El ID del activo es obligatorio y debe ser mayor a 0."]
  }
}
```

**Ejemplo 4: Mensaje muy largo**
```json
{
  "errors": {
    "Mensaje": ["El mensaje no puede exceder 300 caracteres."]
  }
}
```

### 404 Not Found - Entidad No Encontrada

**Usuario emisor:**
```json
{
  "message": "No se encontró el usuario emisor con ID 999."
}
```

**Usuario receptor:**
```json
{
  "message": "No se encontró el usuario receptor con ID 888."
}
```

**Activo:**
```json
{
  "message": "No se encontró el activo con ID 555."
}
```

---

## Ejemplos de Uso por Tipo

### 1. Solicitud de Transferencia

```json
{
  "idEmisor": 5,
  "idReceptor": 8,
  "idActivo": 12,
  "tipo": "Transferencia",
  "mensaje": "Necesito transferir el activo debido a cambio de área"
}
```

### 2. Solicitud de Baja

```json
{
  "idEmisor": 3,
  "idReceptor": 10,
  "idActivo": 7,
  "tipo": "Baja",
  "mensaje": "El equipo ya no funciona, solicito dar de baja"
}
```

### 3. Solicitud de Diagnóstico

```json
{
  "idEmisor": 7,
  "idReceptor": 4,
  "idActivo": 15,
  "tipo": "Diagnóstico",
  "mensaje": "El activo presenta fallas, necesito un diagnóstico técnico"
}
```

### 4. Solicitud de Auditoría

```json
{
  "idEmisor": 2,
  "idReceptor": 6,
  "idActivo": 20,
  "tipo": "Auditoría",
  "mensaje": "Solicito auditoría del activo asignado"
}
```

---

## Cambios en la Estructura

### ? Migración Aplicada: `AddIdActivoToSolicitud`

**Columna Agregada:**
- `IdActivo` (int, NOT NULL) - Relación con MOV_ACTIVO

**Índice Creado:**
- `IX_MOV_SOLICITUD_IdActivo` - Para optimizar búsquedas por activo

**Foreign Key:**
- `FK_MOV_SOLICITUD_MOV_ACTIVO_IdActivo` con `ON DELETE RESTRICT`

---

## Estructura Actualizada

### Capa de Dominio
- ? `Solicitud` - Agregada propiedad `IdActivo` y relación de navegación `Activo`
- ? `ISolicitudRepository` - Interface del repositorio

### Capa de Aplicación

**Commands:**
- ? `CreateSolicitudCommand` - Validación de existencia del activo

**DTOs:**
- ? `CreateSolicitudDto` - Agregado campo `IdActivo`
- ? `SolicitudDto` - Agregados campos `IdActivo`, `EtiquetaActivo`, `DescripcionActivo`

**Validators:**
- ? `CreateSolicitudCommandValidator` - Validación de `IdActivo > 0`

**Profiles:**
- ? `SolicitudProfile` - Mapeo de información del activo (etiqueta y descripción)

### Capa de Infraestructura
- ? `SolicitudRepository` - Include de relación `Activo` en todos los métodos
- ? `SolicitudConfiguration` - Configuración de relación con Activo

### Capa de API
- ? `SolicitudController` - Endpoint POST actualizado

---

## Notas Técnicas

1. **Relación con Activo**:
   - Cada solicitud debe estar relacionada con un activo específico
   - La relación es obligatoria (NOT NULL)
   - Se usa `DeleteBehavior.Restrict` para evitar eliminaciones en cascada

2. **Información del Activo en Respuesta**:
   - El DTO de respuesta incluye:
     - `IdActivo`: ID del activo
     - `EtiquetaActivo`: Etiqueta única del activo
     - `DescripcionActivo`: Descripción del activo
   - Esto facilita mostrar información contextual sin consultas adicionales

3. **Validaciones en Capas**:
   - **Validador**: Verifica que `IdActivo > 0`
   - **Command Handler**: Verifica existencia del activo en BD

4. **Include de Relaciones**:
   - El repositorio siempre incluye `Emisor`, `Receptor` y `Activo`
   - Esto evita lazy loading y N+1 queries

5. **Estado y Fecha Automáticos**:
   - Estado: "Pendiente" (establecido en AutoMapper)
   - Fecha: DateTime.Now (establecido en AutoMapper)

---

## Arquitectura

- ? Patrón CQRS con MediatR
- ? Validación con FluentValidation
- ? Mapeo con AutoMapper
- ? Repository Pattern con Include de relaciones
- ? Manejo centralizado de excepciones
- ? DTOs para separación de concerns

---

## Flujo de Creación Actualizado

```
1. Usuario envía POST /api/Solicitud con datos (incluyendo IdActivo)
   ?
2. FluentValidation valida el DTO
   ?
3. MediatR envía comando al Handler
   ?
4. Handler verifica que emisor existe
   ?
5. Handler verifica que receptor existe
   ?
6. Handler verifica que activo existe ? NUEVO
   ?
7. AutoMapper mapea DTO ? Entidad
   - Estado: "Pendiente"
   - Fecha: DateTime.Now
   ?
8. Repository guarda en BD con FK a Activo
   ?
9. Retorna ID de la solicitud creada
```

---

## Ejemplo de Respuesta Completa (GET)

Cuando implementes los endpoints GET, la respuesta incluirá:

```json
{
  "idSolicitud": 15,
  "idEmisor": 1,
  "nombreEmisor": "Juan Pérez García",
  "idReceptor": 2,
  "nombreReceptor": "María López Sánchez",
  "idActivo": 5,
  "etiquetaActivo": "LAPTOP-001",
  "descripcionActivo": "Laptop Dell Latitude 7420",
  "tipo": "Transferencia",
  "mensaje": "Solicito la transferencia del activo por cambio de proyecto",
  "fecha": "2024-11-12T18:30:00",
  "estado": "Pendiente"
}
```

---

## Próximos Endpoints Sugeridos

Para completar el CRUD de solicitudes:

- `GET /api/Solicitud` - Obtener todas las solicitudes
- `GET /api/Solicitud/{id}` - Obtener solicitud por ID
- `GET /api/Solicitud/emisor/{emisorId}` - Solicitudes enviadas por un usuario
- `GET /api/Solicitud/receptor/{receptorId}` - Solicitudes recibidas por un usuario
- `GET /api/Solicitud/activo/{activoId}` - Solicitudes relacionadas con un activo ? NUEVO
- `PUT /api/Solicitud/{id}/aceptar` - Aceptar una solicitud
- `PUT /api/Solicitud/{id}/rechazar` - Rechazar una solicitud
