using AutoMapper;
using GestionActivos.Application.ActivoApplication.DTOs;
using GestionActivos.Domain.Entities;
using GestionActivos.Domain.Exceptions;
using GestionActivos.Domain.Interfaces;
using MediatR;
using System;

namespace GestionActivos.Application.ActivoApplication.Commands
{
    public record CreateActivoCommand(CreateActivoDto Activo) : IRequest<Guid>;

    public class CreateActivoHandler : IRequestHandler<CreateActivoCommand, Guid>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public CreateActivoHandler(
            IActivoRepository activoRepository,
            IUsuarioRepository usuarioRepository,
            ICategoriaRepository categoriaRepository,
            IFileStorageService fileStorageService,
            IMapper mapper
        )
        {
            _activoRepository = activoRepository;
            _usuarioRepository = usuarioRepository;
            _categoriaRepository = categoriaRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(
            CreateActivoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Activo == null)
            {
                throw new ArgumentNullException(
                    nameof(request.Activo),
                    "El activo no puede ser nulo."
                );
            }

            // Validar que el responsable existe
            var responsable = await _usuarioRepository.GetByIdAsync(
                request.Activo.ResponsableId
            );
            if (responsable == null)
            {
                throw new NotFoundException(
                    $"No se encontró el usuario responsable con ID {request.Activo.ResponsableId}."
                );
            }

            // Validar que la categoría existe
            var categoria = await _categoriaRepository.GetByIdAsync(
                request.Activo.IdCategoria
            );
            if (categoria == null)
            {
                throw new NotFoundException(
                    $"No se encontró la categoría con ID {request.Activo.IdCategoria}."
                );
            }

            // Validar que la etiqueta sea única
            if (!string.IsNullOrEmpty(request.Activo.Etiqueta))
            {
                var existeEtiqueta = await _activoRepository.ExistsByEtiquetaAsync(
                    request.Activo.Etiqueta
                );
                if (existeEtiqueta)
                {
                    throw new BusinessException(
                        $"Ya existe un activo con la etiqueta '{request.Activo.Etiqueta}'."
                    );
                }
            }

            // Validar que el número de serie sea único (si se proporciona)
            if (!string.IsNullOrEmpty(request.Activo.NumeroSerie))
            {
                var existeNumeroSerie = await _activoRepository.ExistsByNumeroSerieAsync(
                    request.Activo.NumeroSerie
                );
                if (existeNumeroSerie)
                {
                    throw new BusinessException(
                        $"Ya existe un activo con el número de serie '{request.Activo.NumeroSerie}'."
                    );
                }
            }

            // Mapear el DTO a la entidad
            var activo = _mapper.Map<Activo>(request.Activo);

            // Manejar la carga de la imagen si existe
            if (request.Activo.Imagen != null && request.Activo.Imagen.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await request.Activo.Imagen.CopyToAsync(memoryStream, cancellationToken);
                var fileBytes = memoryStream.ToArray();

                // Generar nombre con formato: activo/imagen/fecha/guid.extension
                var extension = Path.GetExtension(request.Activo.Imagen.FileName);
                var fileName = GenerateFileName("activo", "imagen", extension);

                // Subir la imagen a Minio
                var imageUrl = await _fileStorageService.UploadAsync(
                    fileBytes,
                    fileName,
                    request.Activo.Imagen.ContentType
                );

                activo.ImagenUrl = imageUrl;
            }

            // Manejar la carga de FacturaPDF si existe
            if (request.Activo.FacturaPDF != null && request.Activo.FacturaPDF.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await request.Activo.FacturaPDF.CopyToAsync(memoryStream, cancellationToken);
                var fileBytes = memoryStream.ToArray();

                // Generar nombre con formato: activo/factura_pdf/fecha/guid.extension
                var extension = Path.GetExtension(request.Activo.FacturaPDF.FileName);
                var fileName = GenerateFileName("activo", "factura_pdf", extension);

                var facturaPdfUrl = await _fileStorageService.UploadAsync(
                    fileBytes,
                    fileName,
                    request.Activo.FacturaPDF.ContentType
                );

                activo.FacturaPDF = facturaPdfUrl;
            }

            // Manejar la carga de FacturaXML si existe
            if (request.Activo.FacturaXML != null && request.Activo.FacturaXML.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await request.Activo.FacturaXML.CopyToAsync(memoryStream, cancellationToken);
                var fileBytes = memoryStream.ToArray();

                // Generar nombre con formato: activo/factura_xml/fecha/guid.extension
                var extension = Path.GetExtension(request.Activo.FacturaXML.FileName);
                var fileName = GenerateFileName("activo", "factura_xml", extension);

                var facturaXmlUrl = await _fileStorageService.UploadAsync(
                    fileBytes,
                    fileName,
                    request.Activo.FacturaXML.ContentType
                );

                activo.FacturaXML = facturaXmlUrl;
            }

            // Si es donación, limpiar los campos de factura, cuenta contable, valor y fecha de adquisición
            if (activo.Donacion)
            {
                activo.FacturaPDF = null;
                activo.FacturaXML = null;
                activo.CuentaContable = null;
                activo.ValorAdquisicion = null;
                activo.FechaAdquisicion = null;
            }

            // Si no porta etiqueta, CuentaContableEtiqueta debe ser false
            if (!activo.PortaEtiqueta)
            {
                activo.CuentaContableEtiqueta = false;
            }

            await _activoRepository.AddAsync(activo);
            return activo.IdActivo;
        }

        /// <summary>
        /// Genera un nombre de archivo con el formato: entidad/campo/fecha/guid.extension
        /// Ejemplo: activo/factura_xml/20231114/a1b2c3d4-e5f6-7890-abcd-ef1234567890.pdf
        /// </summary>
        private static string GenerateFileName(string entidad, string campo, string extension)
        {
            var fecha = DateTime.UtcNow.ToString("yyyyMMdd");
            var guid = Guid.NewGuid().ToString();
            return $"{entidad}/{campo}/{fecha}/{guid}{extension}";
        }
    }
}
