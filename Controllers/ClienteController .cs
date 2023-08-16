using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
       
        private readonly ILogger<AutoController> _logger;

        public ClienteController(ILogger<AutoController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Mostrar todos los objetos de la lista
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Object> GetTodos()
        {
            //List<Cliente> cliente = null;

            using (var context = new MiejemploContext())
            {
                //Mostrar una lista de clientes
                var a = context.Clientes.ToList();

                //Mostrar solo algunos campos del objeto
                var cliente= context.Clientes
                    .Select(x => new { Id = x.Clave, ruta = x.Rut })
                    .ToList();
                //Mostrar campos de la clase ClienteDTO
                var cliente2 = context.Clientes
                    .Select(x => new ClienteDTO { Clave = x.Clave })
                    .ToList();

                return a;
                
            }
        }
        /// <summary>
        /// Mostrar un solo objeto de la lista, introduciendo su parametro.
        /// </summary>
        /// <param name="patente"></param>
        /// <returns></returns>
        [HttpGet("{ruta}")]
        public Cliente GetUnElemento(string ruta)
        {
            Cliente cte = null;

            using (var context = new MiejemploContext())
            {
                cte = context.Clientes.Single(b => b.Rut.Equals(ruta));

            }
            return cte;
        }

        /// <summary>
        /// Guardar un elemento haciendo solo uso de la capa intermedia de datos DTO e incorporando la informacion en el BODY
        /// </summary>
        /// <param name="auto"></param>
        /// <returns></returns>
        [HttpPost]
        public ClienteDTO PostElemento(ClienteDTO ctedto)
        {
            using (MiejemploContext context = new MiejemploContext())
            {
                Cliente cte = new Cliente {Rut= ctedto.Rut, Nombre= ctedto.Nombre,Apellido=ctedto.Apellido,Clave=ctedto.Clave };
                context.Clientes.Add(cte);
                context.SaveChanges();

            }
            return ctedto;
        }

        [HttpPut("{ruta}")]
        public ClienteDTO UpdateElemento(ClienteDTO ctedto, string ruta)
        {


            using (MiejemploContext context = new MiejemploContext())
            {
                Cliente cte;
                cte = context.Clientes.Single(b => b.Rut.Equals(ruta));
                cte.Nombre = ctedto.Nombre;
                cte.Apellido = ctedto.Apellido;
                cte.Clave = ctedto.Clave;
                context.SaveChanges();

            }
            return ctedto;
        }
        /// <summary>
        /// Eliminar un elemento por ruta.
        /// </summary>
        /// <param name="auto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{ruta}")]
        public string DeleteElemento(string ruta)
        {
            using (MiejemploContext context = new MiejemploContext())
            {
                Cliente cte;
                cte = context.Clientes.Single(b => b.Rut.Equals(ruta));
                context.Clientes.Remove(cte);
                context.SaveChanges();
            }
            return ruta;
        }

        

    }
}