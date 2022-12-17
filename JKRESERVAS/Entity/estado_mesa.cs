using System.ComponentModel.DataAnnotations;

namespace JKRESERVAS.Entity
{
    public class estado_mesa
    {   [Key]
        public int id { get; set; }
        [MaxLength(50)]
        public string? descripcion { get; set; }
        [MaxLength(5)]
        public string? codigo { get; set; }
    }
}
