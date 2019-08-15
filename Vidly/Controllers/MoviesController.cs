using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;


namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private VidlyContext _context;

        public MoviesController()
        {
            _context = new VidlyContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: movies/random
        public ActionResult Index()
        {
            var movies = _context.Movies.Include(m => m.Genre).ToList();

            return View(movies);
            /*return Content("Hello World!");
              return HttpNotFound();
              return new EmptyResult();
              return RedirectToAction("Index", "home", new { page = 1, SortBy = "name"});*/
        }

        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            return View(movie);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var genres = _context.Genres.ToList();
            var viewMode = new MovieFormViewModel
            {
                Genres = genres
            };
            return View("MovieForm", viewMode);
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            if (movie.Id == 0)
                _context.Movies.Add(movie);
            else
            {
                var movieDb = _context.Movies.Single(m => m.Id == movie.Id);
                movieDb.Name = movie.Name;
                movieDb.GenreId = movie.GenreId;
                movieDb.ReleaseDate = movie.ReleaseDate;
                movieDb.DateAdded = movie.DateAdded;
                movieDb.NumInStock = movie.NumInStock;
            }

            _context.SaveChanges();
            return RedirectToAction("index", "Movies");
        }
        // Get: movies/edit/{id} || movies/edit?id={id}
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
            if (movie == null)
                return HttpNotFound();

            var viewModel = new MovieFormViewModel
            {
                Movie = movie,
                Genres = _context.Genres.ToList()
            };

            return View("MovieForm", viewModel);
            //return Content("id= " + id);
        }

        // Get: movies/index
        //public ActionResult Index(int? pageIndex, string sortBy)
        //{
        //    if (!pageIndex.HasValue)
        //        pageIndex = 1;
        //    if (String.IsNullOrWhiteSpace(sortBy))
        //        sortBy = "Name";
        //    return Content(string.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        //}

        // Get: movies/released/{year}/{month}
        //[Route("movies/released/{year}/{month}")]
        //public ActionResult ByReleaseDate(int year, int month)
        //{
        //    return Content(String.Format("{0}/{1}",year, month));
        //}
    }
}