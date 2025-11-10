# Endpoint: Crear Activo

## Descripción
Este endpoint permite crear un nuevo activo con soporte para carga de imágenes a través de MinIO.

## URL
```
POST /api/Activo
```

## Content-Type
```
multipart/form-data
```

## Parámetros del Body

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
| `Factura` | string | Condicional* | Número de factura (máx 100 caracteres) |
| `ValorAdquisicion` | decimal | Condicional* | Valor de adquisición del activo |
| `FechaAdquisicion` | DateTime | Condicional* | Fecha de adquisición |
| `PortaEtiqueta` | bool | No | Indica si porta etiqueta (default: false) |

\* **Condicional**: Estos campos son obligatorios SOLO cuando `Donacion = false`

## Reglas de Negocio

1. **Validación de Donación**:
   - Si `Donacion = true`: Los campos `Factura`, `ValorAdquisicion` y `FechaAdquisicion` quedan vacíos automáticamente
 - Si `Donacion = false`: Los campos `Factura`, `ValorAdquisicion` y `FechaAdquisicion` son obligatorios

2. **Validación de Unicidad**:
   - La `Etiqueta` debe ser única en el sistema
   - El `NumeroSerie` (si se proporciona) debe ser único

3. **Validación de Existencia**:
   - El `ResponsableId` debe corresponder a un usuario existente
   - El `IdCategoria` debe corresponder a una categoría existente

4. **Validación de Imagen**:
   - Tamaño máximo: 5MB
   - Tipo de archivo: debe ser una imagen válida (image/*)

## Ejemplo de Request (Donación)

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
  -F "PortaEtiqueta=true"
```

## Ejemplo de Request (No Donación)

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
  -F "Factura=FAC-2024-001" \
  -F "ValorAdquisicion=25000.50" \
  -F "FechaAdquisicion=2024-01-15" \
  -F "PortaEtiqueta=true"
```

## Respuesta Exitosa (200 OK)

```json
{
  "message": "Activo creado correctamente",
  "id": 1
}
```

## Respuestas de Error

### 400 Bad Request - Validación Fallida
```json
{
  "errors": {
    "Etiqueta": ["La etiqueta es obligatoria."],
    "Factura": ["La factura es obligatoria cuando no es donación."],
    "Imagen": ["La imagen no puede exceder 5MB."]
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

## Estructura Creada

### Capa de Dominio
- ? `IActivoRepository` - Interface del repositorio

### Capa de Aplicación
- ? `CreateActivoDto` - DTO para crear activo
- ? `ActivoDto` - DTO de respuesta
- ? `CreateActivoCommand` - Comando MediatR
- ? `CreateActivoCommandValidator` - Validador FluentValidation
- ? `ActivoProfile` - Perfil de AutoMapper

### Capa de Infraestructura
- ? `ActivoRepository` - Implementación del repositorio
- ? `MinioStorageService` - Servicio de almacenamiento de archivos

### Capa de API
- ? `ActivoController` - Controlador REST

## Notas Técnicas

1. **Manejo de Imágenes**: 
- Las imágenes se suben a MinIO con un nombre único generado con GUID
   - La URL de la imagen se almacena en el campo `ImagenUrl` del activo

2. **Campos Automáticos**:
   - `Estatus`: Se establece automáticamente como "Activo"
   - `FechaAlta`: Se establece automáticamente con la fecha actual

3. **Arquitectura**:
   - Patrón CQRS con MediatR
 - Validación con FluentValidation
   - Mapeo con AutoMapper
   - Manejo centralizado de excepciones
