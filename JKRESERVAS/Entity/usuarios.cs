using System.ComponentModel.DataAnnotations;

namespace JKRESERVAS.Entity
{
    public class usuarios
    {   [Key]
        public int id { set; get; }
        public string usuario { set; get; }
        public string password { set; get; }
        public string? nombre { set; get; }
        public int dni { set; get; }
        public int local { set; get; }
        public int rol { set; get; }
    }
}
