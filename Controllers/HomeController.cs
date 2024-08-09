using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Chefs_N_Dishes.Models;
using Chefs_N_Dishes.Context;
using Chefs_N_Dishes.ViewModels; // Add this using directive
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Chefs_N_Dishes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;
        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var chefs = _context.Chefs.ToList();
            return View(chefs);
        }

        [HttpGet("chefs")]
        public IActionResult Chefs()
        {
            var chefs = _context.Chefs.Include(c => c.Dishes).ToList();
            return View("Index", chefs);
        }

        [HttpGet("dishes")]
        public IActionResult Dishes()
        {
            var dishes = _context.Dishes.Include(d => d.Chef).ToList();
            return View(dishes);
        }

        [HttpGet("chefs/new")]
        public IActionResult NewChef()
        {
            return View();
        }

        [HttpPost("chefs/create")]
        public IActionResult CreateChef(Chef chef)
        {
            if (ModelState.IsValid)
            {
                _context.Chefs.Add(chef);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("NewChef");
        }

        [HttpGet("dishes/new")]
        public IActionResult NewDish()
        {
            ViewBag.Chefs = _context.Chefs.ToList();
            return View();
        }

        [HttpPost("dishes/create")]
        public IActionResult CreateDish(Dish dish)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                Console.WriteLine(message);
            }
            if (ModelState.IsValid)
            {
                _context.Dishes.Add(dish);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Chefs = _context.Chefs.ToList();
            return View("NewDish", dish);
        }

        [HttpGet("dishes/{id}/edit")]
        public IActionResult EditDish(int id)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.DishId == id);
            if (dish == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Chefs = _context.Chefs.ToList();
            return View(dish);
        }

        [HttpPost("dishes/{id}/update")]
        public IActionResult UpdateDish(int id, Dish updatedDish)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.DishId == id);
            if (dish == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                dish.Name = updatedDish.Name;
                dish.ChefId = updatedDish.ChefId;
                dish.Tastiness = updatedDish.Tastiness;
                dish.Calories = updatedDish.Calories;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Chefs = _context.Chefs.ToList();
            return View("EditDish", dish);
        }

        [HttpPost("dishes/{id}/delete")]
        public IActionResult DeleteDish(int id)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.DishId == id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
