using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public List<Cliente> ObtenerClientes()
        {
            List<Cliente> clientes = ClienteService.ObtenerClientes();
            return clientes;
        }
        [HttpGet]
        [Route("[action]")]
        public Cliente ObtenerCliente(int Id)
        {
            Cliente cliente = ClienteService.ObtenerCliente(Id);
            return cliente;
        }
        [HttpPost]
        [Route("[action]")]
        public string GuardarCliente(int Id, string Nombre, string Correo, string Pwd, string Telefono)
        {
            Cliente cliente = new Cliente();
            cliente.Id = Id;
            cliente.Nombre = Nombre;
            cliente.Correo = Correo;
            cliente.Pwd = Pwd;
            cliente.Telefono = Telefono;

            string mensaje  = ClienteService.GuardarCliente(cliente);
            return mensaje;
        }
    }
}
