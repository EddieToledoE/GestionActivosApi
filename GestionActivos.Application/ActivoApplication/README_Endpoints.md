# Endpoints: Gestión de Activos

## Descripción
Estos endpoints permiten la gestión completa de activos con soporte para carga de imágenes y documentos (Facturas PDF/XML) a través de MinIO.

---

## 1. Crear Activo

### URL
```
POST /api/Activo
```

### Content-Type
```
multipart/form-data
```

### Parámetros del Body

| Campo | Tipo | Requerido | Descripción |
|-------|------|-----------|-------------|
| `Imagen` | IFormFile | No | Archivo de imagen del activo (máx 5MB) |
| `ResponsableId` | int | Sí | ID del usuario responsable |
| `IdCategoria` | int | Sí | ID de la categoría |
| `Marca` | string | No | Marca del activo (máx 100 caracteres) |
| `Modelo` | string | No | Modelo del activo (máx 100 caracteres) |
| `Descripcion` | string | No | Descripción del activo (máx 200 caracteres) |
| `Etiqueta` | string | Sí | Etiqueta única del activo (máx 50 caracteres) |
| `NumeroSerie` | string | No | Número de serie único (máx 100 caracteres) |
| `Donacion` | bool | No | Indica si es donación (default: false) |
| `FacturaPDF` | IFormFile | Condicional* | Archivo PDF de la factura (máx 10MB) |
| `FacturaXML` | IFormFile | Condicional* | Archivo XML de la factura (máx 5MB) |
| `CuentaContable` | string | Condicional* | Cuenta contable (máx 100 caracteres) |
| `ValorAdquisicion` | decimal | Condicional* | Valor de adquisición del activo |
| `FechaAdquisicion` | DateTime | Condicional* | Fecha de adquisición |
| `PortaEtiqueta` | bool | No | Indica si porta etiqueta (default: false) |
| `CuentaContableEtiqueta` | string | Condicional** | Cuenta contable de etiqueta (máx 100 caracteres) |

\* **Condicional (Donación)**: 
- Si `Donacion = false`: 
  - Al menos UNA factura (PDF o XML) es obligatoria
  - `CuentaContable` es obligatorio
  - `ValorAdquisicion` es obligatorio
  - `FechaAdquisicion` es obligatorio

\*\* **Condicional (Porta Etiqueta)**:
- Si `PortaEtiqueta = true`: `CuentaContableEtiqueta` es obligatorio

### Reglas de Negocio

1. **Validación de Donación**:
 - Si `Donacion = true`: Los campos `FacturaPDF`, `FacturaXML`, `CuentaContable`, `ValorAdquisicion` y `FechaAdquisicion` quedan vacíos automáticamente
   - Si `Donacion = false`: 
     - Al menos una factura (PDF o XML) es obligatoria
     - `CuentaContable`, `ValorAdquisicion` y `FechaAdquisicion` son obligatorios

2. **Validación de Porta Etiqueta**:
   - Si `PortaEtiqueta = true`: `CuentaContableEtiqueta` es obligatorio
   - Si `PortaEtiqueta = false`: `CuentaContableEtiqueta` queda vacío automáticamente

3. **Validación de Unicidad**:
   - La `Etiqueta` debe ser única en el sistema
   - El `NumeroSerie` (si se proporciona) debe ser único

4. **Validación de Existencia**:
   - El `ResponsableId` debe corresponder a un usuario existente
   - El `IdCategoria` debe corresponder a una categoría existente

5. **Validación de Archivos**:
   - **Imagen**: Tamaño máximo 5MB, debe ser tipo image/*
   - **FacturaPDF**: Tamaño máximo 10MB, debe ser application/pdf
   - **FacturaXML**: Tamaño máximo 5MB, debe ser application/xml o text/xml

### Ejemplo de Request (No Donación - Con ambas facturas)

```bash
curl -X POST "https://localhost:7000/api/Activo" \
  -H "Content-Type: multipart/form-data" \
  -F "Imagen=@/ruta/imagen.jpg" \
  -F "ResponsableId=1" \
  -F "IdCategoria=2" \
  -F "Marca=Dell" \
  -F "Modelo=Latitude 7420" \
  -F "Descripcion=Laptop ejecutiva" \
  -F "Etiqueta=LAPTOP-002" \
  -F "NumeroSerie=SN987654321" \
  -F "Donacion=false" \
  -F "FacturaPDF=@/ruta/factura.pdf" \
  -F "FacturaXML=@/ruta/factura.xml" \
  -F "CuentaContable=1234567890" \
  -F "ValorAdquisicion=25000.50" \
  -F "FechaAdquisicion=2024-01-15" \
  -F "PortaEtiqueta=true" \
  -F "CuentaContableEtiqueta=9876543210"
```

### Ejemplo de Request (Donación)

```bash
curl -X POST "https://localhost:7000/api/Activo" \
  -H "Content-Type: multipart/form-data" \
  -F "Imagen=@/ruta/imagen.jpg" \
  -F "ResponsableId=1" \
  -F "IdCategoria=2" \
  -F "Marca=HP" \
  -F "Modelo=ProBook 450" \
  -F "Descripcion=Laptop para desarrollo" \
  -F "Etiqueta=LAPTOP-001" \
  -F "NumeroSerie=SN123456789" \
  -F "Donacion=true" \
  -F "PortaEtiqueta=false"
```

### Respuesta Exitosa (200 OK)

```json
{
  "message": "Activo creado correctamente",
  "id": 1
}
```

---

## 2. Obtener Todos los Activos

### URL
```
GET /api/Activo
```

### Respuesta Exitosa (200 OK)

```json
[
  {
    "idActivo": 1,
    "imagenUrl": "http://localhost:9000/activos-bucket/activo_imagen_abc123.jpg",
    "responsableId": 1,
    "idCategoria": 2,
    "marca": "Dell",
    "modelo": "Latitude 7420",
    "descripcion": "Laptop ejecutiva",
    "etiqueta": "LAPTOP-001",
 "numeroSerie": "SN123456789",
    "donacion": false,
    "facturaPDF": "http://localhost:9000/activos-bucket/activo_factura_pdf_def456.pdf",
    "facturaXML": "http://localhost:9000/activos-bucket/activo_factura_xml_ghi789.xml",
    "cuentaContable": "1234567890",
    "valorAdquisicion": 25000.50,
    "estatus": "Activo",
    "fechaAdquisicion": "2024-01-15T00:00:00",
    "fechaAlta": "2024-11-10T15:30:00",
    "portaEtiqueta": true,
    "cuentaContableEtiqueta": "9876543210"
  }
]
```

---

## 3. Obtener Activo por ID

### URL
```
GET /api/Activo/{id}
```

### Parámetros de URL

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `id` | int | ID del activo a consultar |

### Ejemplo de Request

```bash
curl -X GET "https://localhost:7000/api/Activo/1"
```

### Respuesta Exitosa (200 OK)

```json
{
  "idActivo": 1,
  "imagenUrl": "http://localhost:9000/activos-bucket/activo_imagen_abc123.jpg",
  "responsableId": 1,
  "idCategoria": 2,
  "marca": "Dell",
  "modelo": "Latitude 7420",
  "descripcion": "Laptop ejecutiva",
  "etiqueta": "LAPTOP-001",
  "numeroSerie": "SN123456789",
  "donacion": false,
  "facturaPDF": "http://localhost:9000/activos-bucket/activo_factura_pdf_def456.pdf",
  "facturaXML": "http://localhost:9000/activos-bucket/activo_factura_xml_ghi789.xml",
  "cuentaContable": "1234567890",
  "valorAdquisicion": 25000.50,
  "estatus": "Activo",
  "fechaAdquisicion": "2024-01-15T00:00:00",
  "fechaAlta": "2024-11-10T15:30:00",
  "portaEtiqueta": true,
  "cuentaContableEtiqueta": "9876543210"
}
```

### Respuesta de Error (404 Not Found)

```json
{
  "message": "No se encontró el activo con ID 999."
}
```

---

## 4. Obtener Activos por Responsable

### URL
```
GET /api/Activo/responsable/{responsableId}
```

### Parámetros de URL

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `responsableId` | int | ID del usuario responsable |

### Ejemplo de Request

```bash
curl -X GET "https://localhost:7000/api/Activo/responsable/1"
```

### Respuesta Exitosa (200 OK)

```json
[
  {
    "idActivo": 1,
    "imagenUrl": "http://localhost:9000/activos-bucket/activo_imagen_abc123.jpg",
    "responsableId": 1,
    "idCategoria": 2,
    "marca": "Dell",
    "modelo": "Latitude 7420",
 "descripcion": "Laptop ejecutiva",
    "etiqueta": "LAPTOP-001",
    "numeroSerie": "SN123456789",
    "donacion": false,
    "facturaPDF": "http://localhost:9000/activos-bucket/activo_factura_pdf_def456.pdf",
    "facturaXML": "http://localhost:9000/activos-bucket/activo_factura_xml_ghi789.xml",
    "cuentaContable": "1234567890",
    "valorAdquisicion": 25000.50,
    "estatus": "Activo",
    "fechaAdquisicion": "2024-01-15T00:00:00",
    "fechaAlta": "2024-11-10T15:30:00",
  "portaEtiqueta": true,
    "cuentaContableEtiqueta": "9876543210"
  }
]
```

---

## 5. Desactivar Activo (Cambiar Estatus a Inactivo)

### URL
```
DELETE /api/Activo/{id}
```

### Parámetros de URL

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `id` | int | ID del activo a desactivar |

### Descripción
Este endpoint NO elimina físicamente el activo de la base de datos. Solo cambia su estatus a "Inactivo".

### Ejemplo de Request

```bash
curl -X DELETE "https://localhost:7000/api/Activo/1"
```

### Respuesta Exitosa (200 OK)

```json
{
  "message": "Activo desactivado correctamente"
}
```

### Respuesta de Error (404 Not Found)

```json
{
  "message": "No se encontró el activo con ID 999."
}
```

---

## Respuestas de Error Comunes

### 400 Bad Request - Validación Fallida

**Ejemplo 1: Falta factura cuando no es donación**
```json
{
  "errors": {
    "Activo": ["Debe proporcionar al menos una factura (PDF o XML) cuando no es donación."],
    "CuentaContable": ["La cuenta contable es obligatoria cuando no es donación."]
  }
}
```

**Ejemplo 2: Falta cuenta contable de etiqueta**
```json
{
  "errors": {
    "CuentaContableEtiqueta": ["La cuenta contable de etiqueta es obligatoria cuando porta etiqueta."]
  }
}
```

**Ejemplo 3: Archivo inválido**
```json
{
  "errors": {
    "FacturaPDF": ["El archivo debe ser un PDF válido."],
    "FacturaXML": ["La factura XML no puede exceder 5MB."]
  }
}
```

### 404 Not Found - Entidad No Encontrada
```json
{
  "message": "No se encontró el usuario responsable con ID 999."
}
```

### 409 Conflict - Violación de Unicidad
```json
{
  "message": "Ya existe un activo con la etiqueta 'LAPTOP-001'."
}
```

---

## Estructura Actualizada

### Cambios en la Base de Datos (Migración)

**Columnas Eliminadas:**
- `Factura` (string)

**Columnas Agregadas:**
- `FacturaPDF` (nvarchar(400)) - URL del PDF en MinIO
- `FacturaXML` (nvarchar(400)) - URL del XML en MinIO
- `CuentaContable` (nvarchar(100)) - Cuenta contable del activo
- `CuentaContableEtiqueta` (nvarchar(100)) - Cuenta contable de la etiqueta

### Capa de Dominio
- ? `IActivoRepository` - Interface del repositorio
- ? `Activo` - Entidad actualizada con nuevos campos

### Capa de Aplicación

**Commands:**
- ? `CreateActivoCommand` - Crear activo con manejo de múltiples archivos
- ? `DeleteActivoCommand` - Desactivar activo

**Queries:**
- ? `GetActivosQuery` - Obtener todos los activos
- ? `GetActivoByIdQuery` - Obtener activo por ID
- ? `GetActivosByResponsableIdQuery` - Obtener activos por responsable

**DTOs:**
- ? `CreateActivoDto` - DTO actualizado con campos de facturas y cuentas contables
- ? `ActivoDto` - DTO de respuesta actualizado

**Validators:**
- ? `CreateActivoCommandValidator` - Validador actualizado con:
  - Validación de al menos una factura cuando no es donación
  - Validación de cuenta contable cuando no es donación
  - Validación de cuenta contable de etiqueta cuando porta etiqueta
  - Validación de formatos de archivos PDF y XML
  - Validación de tamaños máximos de archivos

**Profiles:**
- ? `ActivoProfile` - Perfil actualizado ignorando archivos en mapeo

### Capa de Infraestructura
- ? `ActivoRepository` - Implementación del repositorio
- ? `MinioStorageService` - Servicio de almacenamiento de archivos
- ? `ActivoConfiguration` - Configuración EF Core actualizada

### Capa de API
- ? `ActivoController` - Controlador REST con todos los endpoints

---

## Notas Técnicas

1. **Manejo de Archivos**: 
   - Imágenes, PDFs y XMLs se suben a MinIO con nombres únicos generados con GUID
   - Las URLs se almacenan en los campos correspondientes del activo
   - Prefijos para archivos:
     - Imagen: `activo_imagen_{guid}.ext`
     - Factura PDF: `activo_factura_pdf_{guid}.pdf`
     - Factura XML: `activo_factura_xml_{guid}.xml`

2. **Campos Automáticos**:
   - `Estatus`: Se establece automáticamente como "Activo"
   - `FechaAlta`: Se establece automáticamente con la fecha actual

3. **Limpieza Automática de Campos**:
   - Si `Donacion = true`: Se limpian `FacturaPDF`, `FacturaXML`, `CuentaContable`, `ValorAdquisicion` y `FechaAdquisicion`
   - Si `PortaEtiqueta = false`: Se limpia `CuentaContableEtiqueta`

4. **Soft Delete**:
   - El endpoint DELETE no elimina físicamente el activo
   - Solo cambia el campo `Estatus` de "Activo" a "Inactivo"

5. **Arquitectura**:
   - Patrón CQRS con MediatR
   - Validación con FluentValidation
   - Mapeo con AutoMapper
   - Manejo centralizado de excepciones
   - Sin Unit of Work para queries simples (solo lectura)

6. **Validaciones Condicionales**:
   - Las validaciones cambian dinámicamente según los valores de `Donacion` y `PortaEtiqueta`
   - FluentValidation maneja estas reglas de negocio complejas de forma declarativa
