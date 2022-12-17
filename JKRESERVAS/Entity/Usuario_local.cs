using System.ComponentModel.DataAnnotations;
namespace JKRESERVAS.Entity
{
    public class Usuario_local
    {   [Key]
        public int usuarioid { get; set; }
        
        public int localid { get; set; }
    }
}
