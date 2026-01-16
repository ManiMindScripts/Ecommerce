using System.Diagnostics;
using System.Threading.Tasks;
using Ecommerce.Models;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepo _homeRepo;
        public HomeController(ILogger<HomeController> logger,IHomeRepo homeRepo)
        {
            _homeRepo = homeRepo;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string sTerm="", int genreId=0)
        {
            IEnumerable<Book>  books = await _homeRepo.DisplayBooks(sTerm, genreId);
            IEnumerable<Genre> genres = await _homeRepo.Genres();
            BookDisplay bookModel = new BookDisplay
            {
                Books = books,
                Genres = genres,
                STerm = sTerm,
                GenreId = genreId,
            };
            return View(bookModel);
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
