using JKRESERVAS.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JKRESERVAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly SampleContext _context;
        public UsuarioController(SampleContext context)
        {
            _context=context;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<usuarios>>> Login(usuarios user){
            var result = await _context.usuarios.Where(res => res.usuario.Equals(user.usuario) && res.password.Equals(user.password)).ToListAsync();
            if (result==null)
            {
                return Problem("No se encontro ningun usuario");
            }
            return Ok(result);
        }

    }
}
