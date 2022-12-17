using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JKRESERVAS.Entity;
using JKRESERVAS.services;
using System.Text.Json;

namespace JKRESERVAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly SampleContext _context;
        private cifrado _cifrado;
        public reservasController(SampleContext context, cifrado cifrado_)
        {
            _context = context;
            _cifrado = cifrado_;
        }

        [HttpGet("rango")]
        public async Task<IActionResult> GetreservasRango([FromHeader] string token, string fecha1, string fecha2,string restaurant)
        {
            var vtoken = _cifrado.validarToken(token);
            if (vtoken == null)
            {
                return Problem("El token no es valido!");
            }
            var empresa = await _context.empresa.FirstOrDefaultAsync(x => x.descripcion == vtoken[0] && 
            x.app.Equals("RESERVAS"));
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
                if (context.reservas == null)
                {
                    return NotFound();
                }
                //var result = await context.reservas.Include(a => a.doc_vinculados).ThenInclude(a => a.documentoDetalles).Join(context.estado_mesa, // the source table of the inner join
                //   post => post.status,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                //   meta => meta.codigo,   // Select the foreign key (the second part of the "on" clause)
                //   (post, meta) => new { estado_nombre = meta.descripcion }) // selection
                //   .ToListAsync();    // where statement
                var result = await (from a in context.reservas
                                    join e in context.estado_mesa on a.status equals e.codigo
                                    where a.date > DateTime.Parse(fecha1) && a.date < DateTime.Parse(fecha2).AddDays(1).AddSeconds(-1) && a.restaurant.Equals(restaurant)
                                    select new
                                    {
                                        a.id,
                                        a.id_reserv,
                                        a.date_add,
                                        a.date,
                                        a.status,
                                        a.user_name,
                                        a.pax,
                                        a.mesa,
                                        a.user_phone,
                                        a.provenance,
                                        a.origin,
                                        a.commentary_client,
                                        a.usuario,
                                        a.email,
                                        estado_nombre = e.descripcion,
                                        a.restaurant,
                                        a.tipo,
                                        doc_vinculados = a.doc_vinculados.Select(b => new
                                        {
                                            b.Id,
                                            b.Reservasid,
                                            b.Serie_doc,
                                            b.Tipo_doc,
                                            b.fechaEmision,
                                            b.numeroDocumentoAdquirente,
                                            b.razonSocialAdquiriente,
                                            b.totalVenta,
                                            documentoDetalles = b.documentoDetalles.Select(b => new { b.id, b.cantidad, b.totalventa, b.tipoDocumento, b.codigoProducto, b.descripcion }).ToList(),
                                        }).ToList(),
                                        a.date_sit
                                    }).ToListAsync();
                //var result = await context.reservas.Where(a => a.date > DateTime.Parse(fecha1) && a.date < DateTime.Parse(fecha2).AddDays(1).AddSeconds(-1) && a.restaurant.Equals(restaurant)).ToListAsync();
                
                return Ok(JsonSerializer.Serialize(result));
            }
        }

        [HttpGet("sentados")]
        public async Task<IActionResult> GetreservasRangoSentados([FromHeader] string token, string fecha1, string fecha2, string restaurant)
        {
            var vtoken = _cifrado.validarToken(token);
            if (vtoken == null)
            {
                return Problem("El token no es valido!");
            }
            var empresa = await _context.empresa.FirstOrDefaultAsync(x => x.descripcion == vtoken[0] &&
            x.app.Equals("RESERVAS"));
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
                if (context.reservas == null)
                {
                    return NotFound();
                }
                var result = await (from a in context.reservas
                                    join e in context.estado_mesa on a.status equals e.codigo
                                    where a.date > DateTime.Parse(fecha1) && a.date < DateTime.Parse(fecha2).AddDays(1).AddSeconds(-1) && a.restaurant.Equals(restaurant)
                                    select new
                                    {
                                        a.id,
                                        a.id_reserv,
                                        a.date_add,
                                        a.date,
                                        a.status,
                                        a.user_name,
                                        a.pax,
                                        a.mesa,
                                        a.user_phone,
                                        a.provenance,
                                        a.origin,
                                        a.commentary_client,
                                        a.usuario,
                                        a.email,
                                        estado_nombre = e.descripcion,
                                        a.restaurant,
                                        a.tipo,
                                        a.date_sit
                                    }).ToListAsync();
                return Ok(JsonSerializer.Serialize(result));
            }
        }
    

        // GET: api/reservas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Getreservas([FromHeader] string token,int id)
        {
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
                if (context.reservas == null)
                {
                    return NotFound();
                }
                var reservas = await context.reservas.FindAsync(id);

                if (reservas == null)
                {
                    return NotFound();
                }

                return Ok(reservas);
            }
             
        }

        [HttpPut]
        public async Task<ActionResult> Putreservas([FromHeader] string token,Reservas _reservas)
        {
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
                if (_reservas.id == 0) { return Problem(); }
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
                var query = await context.reservas.FirstOrDefaultAsync(res => res.id.Equals(_reservas.id));
                if (query == null)
                {
                    return Problem("No se encontro el registro");
                }
                if (_reservas.pax != null)
                {
                    query.pax = _reservas.pax;
                    context.SaveChanges();
                }
                if (_reservas.mesa != null)
                {
                    query.mesa = _reservas.mesa;
                    context.SaveChanges();
                }
                if (_reservas.status != null)
                {
                    query.status = _reservas.status;
                    context.SaveChanges();
                }
                if (_reservas.user_name != null)
                {
                    query.user_name = _reservas.user_name;
                    context.SaveChanges();
                }
                if (_reservas.user_phone != null)
                {
                    query.user_phone = _reservas.user_phone;
                    context.SaveChanges();
                }
                if (_reservas.status == "3") query.date_sit = DateTime.Now;
                query.usuario = usuario.usuarioid;
                context.SaveChanges();
                return Ok(query);
            }
            

        }

        // POST: api/reservas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("update")]
        public async Task<ActionResult<Reservas>> Postreservas([FromHeader] string token,string Restaurant, Reservas[] param)
        {
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
                if (param.Length == 0)
                {
                    return Problem("No hay reservas por insertar");
                }
                foreach (Reservas Reservas in param)
                {
                    var val = await context.reservas.Where(res => Reservas.id_reserv == res.id_reserv).ToListAsync();
                    if (val.Count < 1)
                    {
                        if (Reservas.mesa == "") Reservas.mesa = "-";
                        Reservas.tipo = "RESERVA";
                        Reservas.restaurant = Restaurant;
                        context.reservas.Add(Reservas);
                        await context.SaveChangesAsync();
                    }
                }
                return Ok();
            }
              
        }

        [HttpPost("actualizarDoc")]
        public async Task<ActionResult<Reservas>> UpdateDoc([FromHeader] string token, string Restaurant, Reservas[] param)
        {
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
                if (param.Length == 0)
                {
                    return Problem("No hay reservas por insertar");
                }
                foreach (Reservas Reservas in param)
                {       
                    var val = await context.reservas.Where(res => Reservas.id_reserv == res.id_reserv).FirstOrDefaultAsync();
                    if (val.status!="3") 
                    {
                        context.Remove(val);
                        context.SaveChanges();
                        val = null;
                    }
                    if (val==null)
                    {
                        if (Reservas.mesa == "") Reservas.mesa = "-";
                        Reservas.tipo = "RESERVA";
                        Reservas.restaurant = Restaurant;
                        context.reservas.Add(Reservas);
                        await context.SaveChangesAsync();
                    }
                }
                return Ok();
            }

        }


        [HttpPost("create")]
        public async Task<ActionResult> CrearReservas([FromHeader] string token,Reservas reservas)
        {
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
                if (reservas == null)
                {
                    return Problem("No se ingreso ningun dato");
                }
                
                reservas.date_add = DateTime.Now;
                reservas.status = "1";
                await context.reservas.AddAsync(reservas);
                context.SaveChanges();
                reservas.id_reserv= reservas.id.ToString("D10");
                context.Update(reservas);
                context.SaveChanges();
                return Ok();
            }
             
        }
        [HttpPost("vincular")]
        public async Task<ActionResult> vincularReservas([FromHeader] string token, Doc_vinculados doc)
        {
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
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                if (doc == null)
                {
                    return Problem("No se ingreso ningun dato");
                }
                var validarDocumento = await context.doc_vinculados.FirstOrDefaultAsync(res => res.Serie_doc.Equals(doc.Serie_doc));
                if (validarDocumento != null) { return Problem("El documento ya ha sido vinculado"); }
                else
                {
                    if (doc.documentoDetalles!=null) {
                        var val = context.documentoDetalles.Where(res => res.serieNumero.Equals(doc.Serie_doc)).ToList();
                        if (val != null) {
                        foreach (var det in val) { 
                                context.documentoDetalles.Remove(det);
                                context.SaveChanges();
                            }
                        }
                        foreach (var item in doc.documentoDetalles)
                        {   
                            await context.documentoDetalles.AddAsync(item);
                            context.SaveChanges();
                            }
                         }
                    await context.doc_vinculados.AddAsync(doc);
                    context.SaveChanges();
                    return Ok();
                }

            }

        }

        [HttpPost("vincularDocumentos")]
        public async Task<ActionResult> vincularReservasDocumentos([FromHeader] string token, Doc_vinculados[] doc)
        {
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
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                if (doc == null)
                {
                    return Problem("No se ingreso ningun dato");
                }
                foreach (var d in doc) {
                    var validarDocumento = await context.doc_vinculados.FirstOrDefaultAsync(res => res.Serie_doc.Equals(d.Serie_doc));
                    if (validarDocumento != null) {  }
                    else
                    {
                        if (d.documentoDetalles != null)
                        {
                            var val = context.documentoDetalles.Where(res => res.serieNumero.Equals(d.Serie_doc)).ToList();
                            if (val != null)
                            {
                                foreach (var det in val)
                                {
                                    context.documentoDetalles.Remove(det);
                                    context.SaveChanges();
                                }
                            }
                            foreach (var item in d.documentoDetalles)
                            {
                                await context.documentoDetalles.AddAsync(item);
                                context.SaveChanges();
                            }
                        }
                        await context.doc_vinculados.AddAsync(d);
                        context.SaveChanges();
                    }
                }
                return Ok();
            }

        }


        [HttpPost("eliminarVinculo")]
        public async Task<ActionResult> EliminarVinculo([FromHeader] string token, Doc_vinculados doc)
        {
            var vtoken = _cifrado.validarToken(token);
            if (vtoken == null) return Problem("El token no es valido!"); 
            var empresa = await _context.empresa.FirstOrDefaultAsync(x => x.descripcion == vtoken[0] && x.app.Equals("RESERVAS"));
            if (empresa == null)return Problem("La empresa ingresada no es válida.");
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                if (doc == null)return Problem("No se ingreso ningun dato");
                var validarDocumento = await context.doc_vinculados.FindAsync(doc.Id);
                if (validarDocumento != null)
                {
                    context.doc_vinculados.Remove(validarDocumento);
                    context.SaveChanges();
                    var detalles = await context.documentoDetalles.Where(res => res.serieNumero.Equals(validarDocumento.Serie_doc)).ToListAsync();
                    if (detalles != null)
                    {
                        foreach (var det in detalles)
                        {
                            context.documentoDetalles.Remove(det);
                            context.SaveChanges();
                        }
                    }
                    return Ok();
                }
                else {
                    return Problem("No existe el documento en el registro!");
                }
             
            }

        }

        [HttpGet("prueba")]
        public async Task<ActionResult> prueba()
        {
            var vtoken = _cifrado.validarToken("8+s8Hwji6CXImvEKjKModQ==");
            if (vtoken == null) return Problem("El token no es valido!");
            var empresa = await _context.empresa.FirstOrDefaultAsync(x => x.descripcion == vtoken[0] && x.app.Equals("RESERVAS"));
            if (empresa == null) return Problem("La empresa ingresada no es válida.");
            using (var context = new SampleContext(empresa.cadenaconexion))
            {

                //var result = await context.reservas.Select(a => new
                //{
                //    a.id,
                //    a.id_reserv,
                //    a.date_add,
                //    a.date,
                //    a.status,
                //    a.user_name,
                //    a.pax,
                //    a.mesa,
                //    a.user_phone,
                //    a.provenance,
                //    a.origin,
                //    a.commentary_client,
                //    a.usuario,
                //    a.email,
                //    a.restaurant,
                //    a.tipo,
                //    doc_vinculados = a.doc_vinculados.Select(b => new
                //    {
                //        b.Id,
                //        b.Reservasid,
                //        b.Serie_doc,
                //        b.Tipo_doc,
                //        b.fechaEmision,
                //        b.numeroDocumentoAdquirente,
                //        b.razonSocialAdquiriente,
                //        b.totalVenta,
                //        documentoDetalles = b.documentoDetalles.Select(b => new { b.id, b.cantidad, b.totalventa, b.tipoDocumento, b.codigoProducto, b.descripcion }).ToList(),
                //    }).ToList(),
                //    a.date_sit
                //});
                var result = await (from a in context.reservas
                                    join e in context.estado_mesa on a.status equals e.codigo
                                    where a.restaurant.Equals("cala-restaurante")
                                    select new
                                    {
                                        a.id,
                                        a.id_reserv,
                                        a.date_add,
                                        a.date,
                                        a.status,
                                        a.user_name,
                                        a.pax,
                                        a.mesa,
                                        a.user_phone,
                                        a.provenance,
                                        a.origin,
                                        a.commentary_client,
                                        a.usuario,
                                        a.email,
                                        estado_nombre = e.descripcion,
                                        a.restaurant,
                                        a.tipo,
                                        doc_vinculados = a.doc_vinculados.Select(b => new
                                        {
                                            b.Id,
                                            b.Reservasid,
                                            b.Serie_doc,
                                            b.Tipo_doc,
                                            b.fechaEmision,
                                            b.numeroDocumentoAdquirente,
                                            b.razonSocialAdquiriente,
                                            b.totalVenta,
                                            documentoDetalles = b.documentoDetalles.Select(b => new { b.id, b.cantidad, b.totalventa, b.tipoDocumento, b.codigoProducto, b.descripcion }).ToList(),
                                        }).ToList(),
                                        a.date_sit
                                    }).ToListAsync();
                //return Ok(await context.reservas.Include(a=>a.doc_vinculados).ThenInclude(a=>a.documentoDetalles).ToListAsync());
                return Ok(result);
            }

        }
    }
}
