# Endpoints: Obtener Solicitudes Pendientes

## Descripción
Estos endpoints permiten obtener las solicitudes con estado "Pendiente" filtradas por emisor o receptor.

---

## 1. Obtener Solicitudes Pendientes por Emisor

### URL
```
GET /api/Solicitud/pendientes/emisor/{emisorId}
```

### Parámetros de URL

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `emisorId` | int | ID del usuario emisor |

### Descripción
Retorna todas las solicitudes con estado "Pendiente" que fueron enviadas por el usuario especificado.

### Ejemplo de Request

```bash
curl -X GET "https://localhost:7000/api/Solicitud/pendientes/emisor/1"
```

### Respuesta Exitosa (200 OK)

```json
[
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
  },
  {
    "idSolicitud": 18,
    "idEmisor": 1,
    "nombreEmisor": "Juan Pérez García",
    "idReceptor": 5,
    "nombreReceptor": "Carlos Ramírez Torres",
    "idActivo": 12,
    "etiquetaActivo": "MONITOR-034",
    "descripcionActivo": "Monitor Dell 27 pulgadas",
    "tipo": "Baja",
    "mensaje": "El monitor presenta fallas irreparables",
    "fecha": "2024-11-12T19:15:00",
    "estado": "Pendiente"
  }
]
```

### Respuesta cuando no hay solicitudes pendientes (200 OK)

```json
[]
```

---

## 2. Obtener Solicitudes Pendientes por Receptor

### URL
```
GET /api/Solicitud/pendientes/receptor/{receptorId}
```

### Parámetros de URL

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `receptorId` | int | ID del usuario receptor |

### Descripción
Retorna todas las solicitudes con estado "Pendiente" que fueron recibidas por el usuario especificado. Estas son las solicitudes que el usuario debe revisar y aprobar/rechazar.

### Ejemplo de Request

```bash
curl -X GET "https://localhost:7000/api/Solicitud/pendientes/receptor/2"
```

### Respuesta Exitosa (200 OK)

```json
[
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
  },
  {
    "idSolicitud": 20,
    "idEmisor": 7,
    "nombreEmisor": "Ana Martínez Gómez",
    "idReceptor": 2,
    "nombreReceptor": "María López Sánchez",
    "idActivo": 8,
    "etiquetaActivo": "IMPRESORA-012",
    "descripcionActivo": "Impresora HP LaserJet",
    "tipo": "Diagnóstico",
    "mensaje": "Necesito diagnóstico técnico del equipo",
    "fecha": "2024-11-12T20:00:00",
    "estado": "Pendiente"
  }
]
```

### Respuesta cuando no hay solicitudes pendientes (200 OK)

```json
[]
```

---

## Campos de Respuesta

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `idSolicitud` | int | ID único de la solicitud |
| `idEmisor` | int | ID del usuario que envió la solicitud |
| `nombreEmisor` | string | Nombre completo del emisor |
| `idReceptor` | int | ID del usuario que recibe la solicitud |
| `nombreReceptor` | string | Nombre completo del receptor |
| `idActivo` | int | ID del activo relacionado |
| `etiquetaActivo` | string | Etiqueta única del activo |
| `descripcionActivo` | string | Descripción del activo |
| `tipo` | string | Tipo de solicitud (Transferencia, Baja, Diagnóstico, Auditoría) |
| `mensaje` | string | Mensaje descriptivo de la solicitud |
| `fecha` | DateTime | Fecha y hora de creación |
| `estado` | string | Siempre "Pendiente" en estos endpoints |

---

## Casos de Uso

### 1. Bandeja de Salida (Emisor)
**Endpoint:** `/api/Solicitud/pendientes/emisor/{emisorId}`

**Uso:** Ver las solicitudes que YO he enviado y están pendientes de respuesta.

**Escenario:**
```
Usuario Juan (ID: 1) quiere ver qué solicitudes ha enviado y aún no han sido procesadas.
```

**Request:**
```bash
GET /api/Solicitud/pendientes/emisor/1
```

**Resultado:**
- Muestra todas las solicitudes enviadas por Juan que aún no han sido aceptadas o rechazadas
- Juan puede hacer seguimiento de sus solicitudes

### 2. Bandeja de Entrada (Receptor)
**Endpoint:** `/api/Solicitud/pendientes/receptor/{receptorId}`

**Uso:** Ver las solicitudes que ME han enviado y necesitan mi aprobación.

**Escenario:**
```
Usuario María (ID: 2) quiere ver qué solicitudes ha recibido y debe atender.
```

**Request:**
```bash
GET /api/Solicitud/pendientes/receptor/2
```

**Resultado:**
- Muestra todas las solicitudes recibidas por María que requieren su atención
- María puede decidir aceptar o rechazar cada solicitud

---

## Diferencias entre los Endpoints

| Aspecto | Pendientes por Emisor | Pendientes por Receptor |
|---------|----------------------|------------------------|
| **Filtro** | `IdEmisor = {id}` | `IdReceptor = {id}` |
| **Perspectiva** | Solicitudes que YO envié | Solicitudes que ME enviaron |
| **Acción del usuario** | Hacer seguimiento | Aprobar/Rechazar |
| **Rol** | Solicitante | Aprobador |
| **Notificaciones** | "Tus solicitudes enviadas" | "Solicitudes que requieren tu atención" |

---

## Estructura de las Queries

### Query por Emisor
```csharp
public record GetSolicitudesPendientesByEmisorQuery(int EmisorId) 
    : IRequest<IEnumerable<SolicitudDto>>;
```

### Query por Receptor
```csharp
public record GetSolicitudesPendientesByReceptorQuery(int ReceptorId) 
    : IRequest<IEnumerable<SolicitudDto>>;
```

### Lógica de Filtrado
```csharp
// 1. Obtiene todas las solicitudes del usuario (emisor o receptor)
var solicitudes = await _solicitudRepository.GetBy[Emisor|Receptor]IdAsync(id);

// 2. Filtra solo las que están en estado "Pendiente"
var pendientes = solicitudes.Where(s => s.Estado == "Pendiente");

// 3. Mapea a DTO y retorna
return _mapper.Map<IEnumerable<SolicitudDto>>(pendientes);
```

---

## Notas Técnicas

1. **Filtro Automático**:
   - El filtro por estado "Pendiente" se aplica en la capa de aplicación
   - Esto permite reutilizar los métodos del repositorio existentes

2. **Incluye Relaciones**:
   - Todos los queries incluyen:
     - `Emisor` (Usuario)
     - `Receptor` (Usuario)
     - `Activo`
   - Esto se configura en el repositorio con `.Include()`

3. **Performance**:
   - El filtro `Where(s => s.Estado == "Pendiente")` se ejecuta en memoria
   - Para grandes volúmenes, considera agregar métodos específicos en el repositorio

4. **Ordenamiento**:
   - Las solicitudes se retornan en el orden de la base de datos
   - Puedes agregar `.OrderByDescending(s => s.Fecha)` para orden cronológico

---

## Ejemplo de Implementación en Frontend

### Dashboard de Usuario

```typescript
// Obtener mis solicitudes enviadas (bandeja de salida)
const solicitudesEnviadas = await fetch(
  `/api/Solicitud/pendientes/emisor/${userId}`
);

// Obtener solicitudes que debo atender (bandeja de entrada)
const solicitudesRecibidas = await fetch(
  `/api/Solicitud/pendientes/receptor/${userId}`
);
```

### Notificaciones
```typescript
// Mostrar badge con número de solicitudes pendientes
const response = await fetch(`/api/Solicitud/pendientes/receptor/${userId}`);
const solicitudes = await response.json();
const numPendientes = solicitudes.length;

// Mostrar: "Tienes 3 solicitudes pendientes"
```

---

## Próximas Mejoras Sugeridas

1. **Paginación**:
   ```
   GET /api/Solicitud/pendientes/emisor/{id}?page=1&pageSize=10
   ```

2. **Ordenamiento**:
   ```
   GET /api/Solicitud/pendientes/receptor/{id}?orderBy=fecha&desc=true
   ```

3. **Filtro por Tipo**:
   ```
   GET /api/Solicitud/pendientes/receptor/{id}?tipo=Transferencia
   ```

4. **Estadísticas**:
   ```
   GET /api/Solicitud/estadisticas/{usuarioId}
   // Retorna: { enviadas: 5, recibidas: 3, aceptadas: 10, rechazadas: 2 }
   ```

---

## Arquitectura

- ? Patrón CQRS con MediatR
- ? Queries separadas por responsabilidad
- ? Mapeo con AutoMapper
- ? Reutilización de repositorio existente
- ? Filtrado en capa de aplicación

---

## Endpoints Relacionados

- `POST /api/Solicitud` - Crear nueva solicitud
- `GET /api/Solicitud/pendientes/emisor/{id}` - Solicitudes enviadas pendientes
- `GET /api/Solicitud/pendientes/receptor/{id}` - Solicitudes recibidas pendientes

### Próximos endpoints sugeridos:
- `PUT /api/Solicitud/{id}/aceptar` - Aceptar solicitud
- `PUT /api/Solicitud/{id}/rechazar` - Rechazar solicitud
- `GET /api/Solicitud/emisor/{id}` - Todas las solicitudes enviadas (cualquier estado)
- `GET /api/Solicitud/receptor/{id}` - Todas las solicitudes recibidas (cualquier estado)
