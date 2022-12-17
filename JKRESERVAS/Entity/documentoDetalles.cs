using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JKRESERVAS.Entity
{
    public class documentoDetalles
    {
        [Key]
        public int id { get; set; }
        public decimal cantidad { get; set; }
        public string codigoProducto  { get; set; }
        [MaxLength(250)]
        public string descripcion  { get; set; }
        [MaxLength(5)]
        public string tipoDocumento { get; set; }
        public decimal totalventa  { get; set; }
        [MaxLength(50)]
        public string? serieNumero { get; set; }
    }
}
