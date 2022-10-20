using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JKRESERVAS.Entity;

namespace JKRESERVAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly SampleContext _context;
        public reservasController(SampleContext context)
        {
            _context = context;
        }
        // GET: api/reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<reservas>>> Getreservas()
        {
          if (_context.reservas == null)
          {
              return NotFound();
          }
            return await _context.reservas.ToListAsync();
        }
        [HttpGet("rango")]
        public async Task<ActionResult<IEnumerable<reservas>>> GetreservasRango(String fecha1,String fecha2)
        {
            if (_context.reservas == null)
            {
                return NotFound();
            }
            var result= await _context.reservas.Where(res => res.date> DateTime.Parse(fecha1) && res.date< DateTime.Parse(fecha2).AddDays(1)).ToListAsync();
            return result;
        }

        // GET: api/reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<reservas>> Getreservas(int id)
        {
          if (_context.reservas == null)
          {
              return NotFound();
          }
            var reservas = await _context.reservas.FindAsync(id);

            if (reservas == null)
            {
                return NotFound();
            }

            return reservas;
        }

        [HttpPut]
        public async Task<ActionResult> Putreservas(reservas _reservas)
        {
            if (_reservas.id == 0) { return Problem(); }
            var user = await _context.usuarios.FirstOrDefaultAsync(res => res.usuario.Equals(_reservas.nombre_usuario) && res.password.Equals(_reservas.token));
            if (user==null)
            {
                return Problem("No tiene los permisos para realizar la acción!");
            }
            var query = await _context.reservas.FirstOrDefaultAsync(res => res.id.Equals(_reservas.id));
            if (query == null)
            {
                return Problem("No se encontro el registro");
            }
            if (_reservas.pax != null)
            {
                query.pax = _reservas.pax;
                _context.SaveChanges();
            }
            if (_reservas.mesa != null)
            {
                query.mesa = _reservas.mesa;
                _context.SaveChanges();
            }
            if (_reservas.estado != null)
            {
                query.estado = _reservas.estado;
                _context.SaveChanges();
            }
            if (_reservas.nombre != null)
            {
                query.nombre = _reservas.nombre;
                _context.SaveChanges();
            }
            if (_reservas.telefono != null)
            {
                query.telefono = _reservas.telefono;
                _context.SaveChanges();
            }
            query.usuario = user.id;
            _context.SaveChanges();
            return Ok(query);

        }

        // POST: api/reservas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("update")]
        public async Task<ActionResult<reservas>> Postreservas(reservas[] param)
        {
            if (param.Length==0)
            {
                return Problem("No hay reservas por insertar");
            }
            foreach (reservas Reservas in param)
            {
               var val= _context.reservas.Where(res=>Reservas.id_reserv==res.id_reserv).ToList();
                if (val.Count<1)
                {
                    if (Reservas.mesa == "") Reservas.mesa = "-"; 
                    _context.reservas.Add(Reservas);
                    await _context.SaveChangesAsync();
                }
            }
            return Ok("Se actualizó con exito");
        }

        // DELETE: api/reservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletereservas(int id)
        {
            if (_context.reservas == null)
            {
                return NotFound();
            }
            var reservas = await _context.reservas.FindAsync(id);
            if (reservas == null)
            {
                return NotFound();
            }

            _context.reservas.Remove(reservas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool reservasExists(int id)
        {
            return (_context.reservas?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
