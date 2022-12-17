
using JKRESERVAS.Entity;
using JKRESERVAS.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JKRESERVAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly SampleContext _context;
        private cifrado _cifrado;
        public UsuarioController(SampleContext context,cifrado cifrado_)
        {
            _context=context;
            _cifrado = cifrado_;
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetUsuariosAsync(Usuario usuario)
        {
            var query = await _context.empresa.FirstOrDefaultAsync(res => res.descripcion.Equals(usuario.empresa) && res.app.Equals("RESERVAS"));
            if (query == null)
            {
                return Problem("No se encontro la empresa");
            }
            if (query.cadenaconexion == null)
            {
                return Problem("No se encontro la empresa");
            }
            using (var context = new SampleContext(query.cadenaconexion))
            {
                var res = await context.usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(usuario.nombreusuario) && res.contrasena.Equals(usuario.contrasena));
                if (res == null)
                {
                    return Problem("No se encontro ningun usuario");
                }
                var cifrado = _cifrado.EncryptStringAES(usuario.empresa + " " + usuario.nombreusuario + " " + usuario.contrasena);
                var result = await (from a in context.local
                                    join b in context.usuario_local on a.id equals b.localid
                                    where b.usuarioid == res.usuarioid
                                    select new { token_restaurant=a.token, token= cifrado }).ToListAsync();
                if (result == null)
                {
                    return Problem("El usuario no tiene permiso para este local");
                }
                return Ok(result);
                //return Ok("{\"token\":\"" + cifrado + "\",\"token_restaurant\":\"" + cifrado + "\"}");
            }
        }
        [HttpGet("permiso")]
        public async Task<IActionResult> ObtenerPermiso([FromHeader] string token) {
            var vtoken = _cifrado.validarToken(token);
            if (vtoken == null)
            {
                return Problem("El token no es valido!");
            }
            var empresa = await _context.empresa.FirstOrDefaultAsync(x => x.descripcion == vtoken[0] && x.app.Equals("RESERVAS"));
            if (empresa == null)
            {
                return Problem("La empresa ingresada no es válida.");
            }
            if (empresa.cadenaconexion == null)
            {
                return Problem("La empresa ingresada no es válida.");
            }
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                var usuario = await context.usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(vtoken[1]) && res.contrasena.Equals(vtoken[2]));
                if (usuario == null)
                {
                    return Problem("El usuario ingresado no es valido");
                }
                var usuario_locales = await context.usuario_local.Where(res => res.usuarioid.Equals(usuario.usuarioid)).ToListAsync();
                if (usuario_locales == null)
                {
                    return Problem("No hay locales asignados");
                }
                if (context.reservas == null)
                {
                    return NotFound();
                }
                var result = await (from a in context.local
                                    join b in context.usuario_local on a.id equals b.localid
                                    where b.usuarioid == usuario.usuarioid
                                    select new { a.token }).ToListAsync();

                return Ok(result);
            }

        }

    }
}
