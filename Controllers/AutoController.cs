using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System.Reflection.Metadata;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutoController : ControllerBase
    {
       
        private readonly ILogger<AutoController> _logger;

        public AutoController(ILogger<AutoController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Mostrar todos los objetos de la lista
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Auto> GetTodos()
        {
            List<Auto> autos = null;

            using (var context = new MiejemploContext())
            {
                autos= context.Autos.ToList();
                
            }
            return autos;
        }
        /// <summary>
        /// Mostrar un solo objeto de la lista, introduciendo su parametro.
        /// </summary>
        /// <param name="patente"></param>
        /// <returns></returns>
        [HttpGet("{patente}")]
        public Auto GetUnElemento(string patente)
        {
            Auto auto = null;

            using (var context = new MiejemploContext())
            {
                auto = context.Autos.Single(b => b.Patente.Equals(patente));
                 
            }
            return auto;
        }
        ///// <summary>
        ///// De esta manera le pasamos a POST todos los parametros por la URL sin capa DTO
        ///// </summary>
        ///// <param name="patente"></param>
        ///// <param name="marca"></param>
        ///// <param name="modelo"></param>
        ///// <param name="ruta"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public Auto PostElemento(string patente, string marca, string modelo, string ruta)
        //{
        //    using (var context = new MiejemploContext())
        //    {
        //        var coche = new Auto { Patente = patente, Marca = marca, Modelo = modelo, RutCliente = ruta };
        //        context.Autos.Add(coche);
        //        context.SaveChanges();
        //        return coche;
        //    }
        //}

        /// <summary>
        /// Guardar un elemento haciendo solo uso de la capa intermedia de datos DTO e incorporando la informacion en el BODY
        /// </summary>
        /// <param name="auto"></param>
        /// <returns></returns>
        [HttpPost]
        public AutoDTO PostElemento(AutoDTO auto)
        {
            using (MiejemploContext context = new MiejemploContext())
            {
                Auto coche = new Auto { Patente = auto.Patente, Marca = auto.Marca, Modelo = auto.Modelo, RutCliente=auto.RutCliente };
                context.Autos.Add(coche);
                context.SaveChanges();
                
            }return auto;
        }

        ///// <summary>
        ///// Muestra todo el elemento a actualizar y actualiza el Modelo
        ///// </summary>
        ///// <param name="auto"></param>
        ///// <returns></returns>
        //[HttpPut]
        //public AutoDTO UpdateElemento(AutoDTO auto)
        //{
        //    using (MiejemploContext context = new MiejemploContext())
        //    {
        //        Auto coche = context.Autos.Single(b => b.Patente.Equals(auto.Patente));
        //        coche.Modelo = auto.Modelo;
        //        context.SaveChanges();
                
        //    }
        //    return auto;
        //}

        /// <summary>
        /// Modifica paremetros desde BODY enviando una matricula"patente".
        /// </summary>
        /// <param name="auto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public AutoDTO UpdateElemento(AutoDTO auto, string id)
        {
            

            using (MiejemploContext context = new MiejemploContext())
            {
                Auto coche;
                coche = context.Autos.Single(b => b.Patente.Equals(id));
                coche.Modelo = auto.Modelo ;
                coche.Marca = auto.Marca ;
                coche.RutCliente = auto.RutCliente ;
                context.SaveChanges();

            }
            return auto;
        }
        /// <summary>
        /// Eliminar un elemento por matricula.
        /// </summary>
        /// <param name="auto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")] 
        public string DeleteElemento(string id)
        {
            using (MiejemploContext context = new MiejemploContext())
            {
                Auto coche;
                coche = context.Autos.Single(b => b.Patente.Equals(id));
                context.Autos.Remove(coche);
                context.SaveChanges();
            }
            return id;
        }

        [HttpGet("ElementoRelacionado")]
        public Object MostrarRelacionElementos(string patente)
        {
            object coche = null;
            using (MiejemploContext context = new MiejemploContext())
            {
                //ClienteDTO cte=null;
                //var coches2 = context.Autos
                //    .FirstOrDefault(x => x.Patente.Equals(patente));

                var coches = context.Autos
                    .Include(x => x.RutClienteNavigation)
                    .FirstOrDefault(x => x.Patente.Equals(patente));

                coche = new { Modelo = coches.Modelo, Cliente = coches.RutClienteNavigation?.Apellido } ;
            }
            return coche;
        }

        [HttpPost("ElementoRelacionado")]
        public Object PostRelacionElemento(Auto car)
        {
            using (MiejemploContext context = new MiejemploContext())
            {
                //Auto car = null;
                //var cte = context.Clientes
                //    .FirstOrDefault(x=>x.Rut==auto.RutCliente);
                var cte = new Cliente
                {
                    Nombre = car.RutClienteNavigation.Nombre,
                    Apellido = car.RutClienteNavigation.Apellido,
                    Clave = car.RutClienteNavigation.Clave,
                    Rut = car.RutClienteNavigation.Rut
                };
                var coche = new Auto 
                { 
                    Marca=car.Marca,
                    Modelo=car.Modelo,
                    Patente=car.Patente,
                    RutClienteNavigation =cte
                };

                context.Clientes.Add(cte);
                context.Autos.Add(coche);
                context.SaveChanges();
            }
            return Ok();
        }

        /// <summary>
        /// Modifica paremetros desde BODY enviando una matricula"patente".
        /// </summary>
        /// <param name="auto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("UpdateRelacional/{id}")]
        public Object UpdateElementoRelacional(AutoDTO auto, string id)
        {


            using (MiejemploContext context = new MiejemploContext())
            {
                Cliente cte;
                Auto coche;

                var upd = context.Autos
                    .Include(x => x.RutClienteNavigation)
                    .Single(b => b.Patente.Equals(id));

                upd.Modelo = auto.Modelo;
                upd.RutClienteNavigation.Nombre = auto.Modelo;

                context.SaveChanges();

                    //.FirstOrDefault(x => x.RutCliente == x.RutClienteNavigation.Rut);
               
                //coche.Modelo=upd.Modelo


                ////coche = context.Autos.Single(b => b.Patente.Equals(id));
                ////cte = context.Clientes.Single(x => x.Rut == coche.RutCliente);
                ////coche.Modelo = auto.Modelo;
                ////coche.Marca = auto.Marca;
                ////coche.RutCliente = auto.RutCliente;
                ////cte.Nombre=cliente.Nombre;
                ////cte.Apellido=cliente.Apellido;

                //context.SaveChanges();

            }
            return auto;
        }


    }
}