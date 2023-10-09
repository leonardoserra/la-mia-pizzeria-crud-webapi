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
            List<Pizza> pizzas = _db.Pizzas.Include(pizza=> pizza.Category).Include(pizza=>pizza.Ingredients).ToList();
            return Ok(pizzas);
        }

        [HttpGet]
        public IActionResult SearchPizzasByName(string search)
        {
            List<Pizza> pizzas = _db.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).Where(pizza=> pizza.Name.ToLower().Contains(search.ToLower())).ToList();
            return Ok(pizzas);
        }

    }
}
