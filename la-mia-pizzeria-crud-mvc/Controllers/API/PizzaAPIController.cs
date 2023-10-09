using la_mia_pizzeria_crud.Database;
using la_mia_pizzeria_crud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_crud.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaAPIController : ControllerBase
    {
        private PizzeriaContext _db = new PizzeriaContext();

        public PizzaAPIController(PizzeriaContext db) 
        { 
            this._db = db;
        }

        [HttpGet]
        public IActionResult GetPizzas()
        {
            List<Pizza>? pizzas = _db.Pizzas.Include(pizza=> pizza.Category)
                                .Include(pizza=>pizza.Ingredients)
                                .ToList();
            if (pizzas == null)
                return NotFound(new { message = "Nessuna pizza trovata." });
            
            return Ok(pizzas);
        }

        [HttpGet]
        public IActionResult SearchPizzasByName(string? search)
        {
            if(search == null)
                return BadRequest(new { message = "Ricerca non valida." });

            List<Pizza>? pizzas = _db.Pizzas.Include(pizza => pizza.Category)
                                .Include(pizza => pizza.Ingredients)
                                .Where(pizza=> pizza.Name.ToLower()
                                .Contains(search.ToLower()))
                                .ToList();
            if(pizzas == null)
                return NotFound(new { message = "Nessuna pizza trovata."});
            
            return Ok(pizzas);
        }


        [HttpGet]
        public IActionResult GetPizzaById(int? id)
        {
            if(id == null)
                return BadRequest(new { message = "Ricerca non valida." });
            
            Pizza? foundedPizza = _db.Pizzas.Include(pizza => pizza.Category)
                                .Include(pizza => pizza.Ingredients)
                                .Where(pizza => pizza.Id==id)
                                .FirstOrDefault();

            if (foundedPizza == null)
                return NotFound(new { message = "Non ci sono pizze con questo id." });
            
            return Ok(foundedPizza);
        }

        [HttpPost]
        public IActionResult Create([FromBody]Pizza? newPizza)
        {
            if(newPizza == null)
                return BadRequest(new { message = "Dati inviati non validi" });
            
            _db.Pizzas.Add(newPizza);
            int success = _db.SaveChanges();
            
            if (success != 1)
                return BadRequest(new { message = "Dati inviati non validi" });

            return Ok(newPizza);
        }
    }
}
