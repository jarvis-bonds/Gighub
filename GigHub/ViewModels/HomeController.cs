namespace GigHub.ViewModels
{
    using GigHub.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            this._context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var upcomingGigs = this._context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCanceled);

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = upcomingGigs,
                ShowActions = this.User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs"
            };

            return this.View("Gigs", viewModel);
        }

        public ActionResult About()
        {
            this.ViewBag.Message = "Your application description page.";

            return this.View();
        }

        public ActionResult Contact()
        {
            this.ViewBag.Message = "Your contact page.";

            return this.View();
        }
    }
}