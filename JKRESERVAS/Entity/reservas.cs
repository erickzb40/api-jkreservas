using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static JKRESERVAS.SampleContext;

namespace JKRESERVAS.Entity
{
    public class reservas
    {   [Key]
        public int id { get; set; }
        [StringLength(20)]
        public string? id_reserv { get; set; }
        public DateTime date_add { get; set; }
        public DateTime time_add { get; set; }
        public DateTime date { get; set; }
        public DateTime time { get; set; }
        [StringLength(20)]
        public string? tipo { get; set; }
        [StringLength(50)]
        public string? estado { get; set; }
        [StringLength(50)]
        public string? nombre { get; set; }
        [StringLength(50)]
        public string? first_name { get; set; }
        [StringLength(50)]
        public string? last_name { get; set; }
        [StringLength(50)]
        public string? turno { get; set; }
        [StringLength(20)]
        public string? pax { get; set; }
        [StringLength(20)]
        public string? mesa { get; set; }
        [StringLength(20)]
        public string? anotadopor { get; set; }
        [StringLength(20)]
        public string? codigo { get; set; }
        [StringLength(20)]
        public string? telefono { get; set; }
        [StringLength(10)]
        public string? is_group { get; set; }
        [StringLength(20)]
        public string? provenance { get; set; }
        [StringLength(20)]
        public string? origin { get; set; }
        [StringLength(20)]
        [Column("ref")]
        public string? referencia { get; set; }
        [StringLength(50)]
        public string? restaurant { get; set; }
        [StringLength(100)]
        public string? observaciones { get; set; }
        [StringLength(20)]
        public string? integration_id_order { get; set; }
        public int usuario { get; set; }
        [NotMapped]
        public string? nombre_usuario { get; set; }
        [NotMapped]
        public string? token { get; set; }
    }
}
