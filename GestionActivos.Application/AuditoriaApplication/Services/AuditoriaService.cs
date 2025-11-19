using GestionActivos.Domain.Entities;
using System.Globalization;

namespace GestionActivos.Application.AuditoriaApplication.Services
{
    /// <summary>
    /// Servicio que contiene la lógica de negocio para determinar si un usuario tiene auditorías pendientes.
    /// Calcula según el periodo configurado (Mensual, Bimestral, Quincenal, Semanal).
    /// </summary>
    public class AuditoriaService
    {
        /// <summary>
        /// Determina si un usuario tiene una auditoría pendiente según su configuración de centro de costo.
        /// </summary>
        /// <param name="config">Configuración de auditoría del centro de costo</param>
        /// <param name="ultimaAuditoria">Última auditoría realizada (null si nunca ha tenido)</param>
        /// <returns>True si tiene auditoría pendiente</returns>
        public bool EsAuditoriaPendiente(ConfigAuditoria config, Auditoria? ultimaAuditoria)
        {
            // Si no hay configuración activa, no hay auditoría pendiente
            if (config == null || !config.Activa)
                return false;

            var ahora = DateTime.Now;

            // Si nunca ha tenido auditoría, está pendiente
            if (ultimaAuditoria == null)
                return true;

            var fechaUltimaAuditoria = ultimaAuditoria.Fecha;

            return config.Periodo.ToLower() switch
            {
                "mensual" => !EstanEnMismoMes(fechaUltimaAuditoria, ahora),
                "bimestral" => !EstanEnMismoBimestre(fechaUltimaAuditoria, ahora),
                "quincenal" => !EstanEnMismaQuincena(fechaUltimaAuditoria, ahora),
                "semanal" => !EstanEnMismaSemanaISO(fechaUltimaAuditoria, ahora),
                _ => false // Periodo desconocido, no está pendiente
            };
        }

        /// <summary>
        /// Obtiene un mensaje descriptivo del estado de la auditoría.
        /// </summary>
        public string ObtenerMensajeEstado(ConfigAuditoria? config, bool pendiente, Auditoria? ultimaAuditoria)
        {
            if (config == null || !config.Activa)
                return "No tienes configuración de auditoría activa.";

            if (ultimaAuditoria == null)
                return $"No tienes auditorías registradas. Tu periodo configurado es {config.Periodo}.";

            if (pendiente)
                return $"Tienes una auditoría pendiente este periodo ({config.Periodo}).";

            return $"Tu auditoría está al día. Última realizada: {ultimaAuditoria.Fecha:dd/MM/yyyy}.";
        }

        #region Métodos Privados de Cálculo de Periodos

        /// <summary>
        /// Verifica si dos fechas están en el mismo mes y año.
        /// </summary>
        private bool EstanEnMismoMes(DateTime fecha1, DateTime fecha2)
        {
            return fecha1.Year == fecha2.Year && fecha1.Month == fecha2.Month;
        }

        /// <summary>
        /// Verifica si dos fechas están en el mismo bimestre del año.
        /// Bimestres: 1-2 (Ene-Feb), 3-4 (Mar-Abr), 5-6 (May-Jun), etc.
        /// </summary>
        private bool EstanEnMismoBimestre(DateTime fecha1, DateTime fecha2)
        {
            if (fecha1.Year != fecha2.Year)
                return false;

            int bimestre1 = (fecha1.Month - 1) / 2; // 0=Ene-Feb, 1=Mar-Abr, etc.
            int bimestre2 = (fecha2.Month - 1) / 2;

            return bimestre1 == bimestre2;
        }

        /// <summary>
        /// Verifica si dos fechas están en la misma quincena del mes.
        /// Primera quincena: días 1-15
        /// Segunda quincena: días 16-fin de mes
        /// </summary>
        private bool EstanEnMismaQuincena(DateTime fecha1, DateTime fecha2)
        {
            if (!EstanEnMismoMes(fecha1, fecha2))
                return false;

            bool primerQuincena1 = fecha1.Day <= 15;
            bool primerQuincena2 = fecha2.Day <= 15;

            return primerQuincena1 == primerQuincena2;
        }

        /// <summary>
        /// Verifica si dos fechas están en la misma semana ISO 8601.
        /// La semana ISO comienza en lunes y puede abarcar dos años diferentes.
        /// </summary>
        private bool EstanEnMismaSemanaISO(DateTime fecha1, DateTime fecha2)
        {
            var cal = CultureInfo.CurrentCulture.Calendar;
            var dfi = DateTimeFormatInfo.CurrentInfo;

            int semana1 = cal.GetWeekOfYear(fecha1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            int semana2 = cal.GetWeekOfYear(fecha2, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            int año1 = cal.GetYear(fecha1);
            int año2 = cal.GetYear(fecha2);

            return semana1 == semana2 && año1 == año2;
        }

        #endregion
    }
}
