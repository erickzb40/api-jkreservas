using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JKRESERVAS.Entity
{
    [Table("RESERVAS")]
    public class Reservas
    {   [Key]
        public int id { get; set; }
        public string? id_reserv { get; set; }
        public DateTime? date_add { get; set; }
        public DateTime? date { get; set; }
        public DateTime? date_sit { get; set; }
        public string? tipo { get; set; }
        public string? email { get; set; }
        [Column(name: "estado")]
        public string? status { get; set; }
        [Column(name: "nombre")]
        public string? user_name { get; set; }
        public string? turno { get; set; }
        public string? pax { get; set; }
        public string? mesa { get; set; }
        public string? codigo { get; set; }
        [Column(name: "telefono")]
        public string? user_phone { get; set; }
        public string? is_group { get; set; }
        public string? provenance { get; set; }
        public string? origin { get; set; }
        [Column("ref")]
        public string? referencia { get; set; }
        public string? restaurant { get; set; }
        [Column("observaciones")]
        public string? commentary_client { get; set; }
        public string? integration_id_order { get; set; }
        public int usuario { get; set; }
        public List<Doc_vinculados>? doc_vinculados { get; set; }
        [NotMapped]
        public string? nombre_usuario { get; set; }
    }
}
