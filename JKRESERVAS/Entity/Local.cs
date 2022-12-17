using System.ComponentModel.DataAnnotations;
namespace JKRESERVAS.Entity
{
    public class Local
    {   [Key]
        public int id { get; set; }
        public string? descripcion { get; set; }
        public int ruc { get; set; }
        public string? token { get; set; }
    }
}
