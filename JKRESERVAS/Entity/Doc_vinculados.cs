using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JKRESERVAS.Entity
{
    public class Doc_vinculados
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        [Column(name: "Id_reserva")]
        [ForeignKey("Reservas")]
        public int Reservasid { get; set; }
        [Key, Column(Order = 1)]
        public string Serie_doc { get; set; }
        public string? Tipo_doc { get; set; }
        public DateTime fechaEmision { get; set; }
        public string? numeroDocumentoAdquirente { get; set; }
        public string? razonSocialAdquiriente { get; set; }
        public Decimal? totalVenta { get; set; }
        public List<documentoDetalles>? documentoDetalles { get; set; }
    }
}
