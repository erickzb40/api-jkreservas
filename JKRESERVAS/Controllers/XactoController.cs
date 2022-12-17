using JKRESERVAS.Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace JKRESERVAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XactoController : Controller
    {   
        [HttpGet]
        public async Task<IActionResult> Index(string fechaInicio)
        {
            var client = new RestClient("http://api.xactoperu.com:6099/api/documentocabeceras/covermanager");
            var request = new RestRequest("",Method.Post);
            var fechaFin = DateTime.Parse(fechaInicio).AddDays(1).ToString("yyyy/MM/dd").Replace("/", "-");
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(new { localId= 97, fechaInicio= fechaInicio, fechaFin= fechaFin });
            RestResponse response = client.Execute(request);
            return Ok(response.Content);
        }
    }
}
