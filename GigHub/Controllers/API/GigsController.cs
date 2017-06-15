using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    using GigHub.Models;

    using Microsoft.AspNet.Identity;

    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext _context;

        public GigsController()
        {
            this._context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var gig = this._context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == id
               && g.ArtistId == userId);

            if (gig.IsCanceled)
                return this.NotFound();

            gig.Cancel();
            
            this._context.SaveChanges();

            return this.Ok();
        }
    }
}
