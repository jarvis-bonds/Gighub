using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    using GigHub.Models;
    using GigHub.ViewModels;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;

    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            this._context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = this._context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"
            };

            return this.View("Gigs", viewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = this.User.Identity.GetUserId();

            var gigs = this._context.Gigs
                .Where(g => g.ArtistId == userId
                && g.DateTime > DateTime.Now
                && !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();

            return this.View(gigs);
        }

        // GET: Gigs
        [Authorize]
        public ActionResult Create()
        {

            var viewModel = new GigFormViewModel
            {
                Genres = this._context.Genres.ToList(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var gig = this._context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            var viewModel = new GigFormViewModel
            {
                Heading = "Edit a Gig",
                Id = gig.Id,
                Genres = this._context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue
            };

            return View("GigForm", viewModel);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = this._context.Genres.ToList();
                return this.View("GigForm", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = this.User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            this._context.Gigs.Add(gig);
            this._context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = this._context.Genres.ToList();
                return this.View("GigForm", viewModel);
            }

            var userId = this.User.Identity.GetUserId();
            var gig = this._context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            this._context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

    }
}