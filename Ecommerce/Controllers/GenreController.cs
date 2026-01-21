using Ecommerce.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class GenreController : Controller
    {
        private readonly IGenreRepo _genreRepo;

        public GenreController(IGenreRepo genreRepo)
        {
            _genreRepo = genreRepo;
        }
        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepo.GetGenres();
            return View(genres);
        }
        public IActionResult AddGenre()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddGenre(GenreDto genre)
        {
            if (!ModelState.IsValid)
            {
                return View(genre);
            }
            try
            {
                var genreToAdd = new Genre { GenreName = genre.GenreName, Id = genre.id };
                await _genreRepo.AddGenre(genreToAdd);
                TempData["msg"] = "Genre Added Successfuly";
                return RedirectToAction(nameof(AddGenre));
            }
            catch (Exception ex)
            {
                TempData["errmsg"] = "Genre not Added";
                return View(genre);
            }
        }
        public async Task<IActionResult> UpdateGenre(int id)
        {
            var genre = await _genreRepo.GetGenreById(id);
            if (genre is null)

                throw new InvalidOperationException($"Genre with id : {id} does not found");
            var genreToUpdate = new GenreDto
            {
                id = genre.Id,
                GenreName = genre.GenreName
            };
            return View(genreToUpdate);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateGenre(GenreDto genreToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(genreToUpdate);
            }
            try
            {
                var genre = new Genre { GenreName = genreToUpdate.GenreName, Id = genreToUpdate.id };
                await _genreRepo.UpdateGenre(genre);
                TempData["msg"] = "Genre Updated Successfuly";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["errmsg"] = "Genre could not updated";
                return View(genreToUpdate);
            }
        }
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreRepo.GetGenreById(id);
            if (genre is null)
                throw new InvalidOperationException($"Genre with id: {id} does not found");
            await _genreRepo.DeleteGenre(genre);
            return RedirectToAction(nameof(Index));
        }
    }
}
