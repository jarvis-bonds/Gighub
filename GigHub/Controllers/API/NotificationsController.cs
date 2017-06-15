using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    using GigHub.Dtos;
    using GigHub.Models;
    using Microsoft.AspNet.Identity;
    using System.Data.Entity;

    [Authorize]
    public class NotificationsController : ApiController
    {
        private ApplicationDbContext _context;

        public NotificationsController()
        {
            this._context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();

            var notifications =
                this._context.UserNotifications
                    .Where(un => un.UserId == userId && !un.IsRead)
                    .Select(un => un.Notification)
                    .Include(n => n.Gig.Artist)
                    .ToList();

            return notifications.Select(n => new NotificationDto()
            {
                DateTime = n.DateTime,
                Gig = new GigDto()
                {
                    Artist = new UserDto
                    {
                        Id = n.Gig.Artist.Id,
                        Name = n.Gig.Artist.Name
                    },
                    DateTime = n.Gig.DateTime,
                    Id = n.Gig.Id,
                    IsCanceled = n.Gig.IsCanceled,
                    Venue = n.Gig.Venue
                },
                OriginalDateTime = n.OriginalDateTime,
                OriginalVenue = n.OriginalVenue,
                Type = n.Type
            });
        }
    }
}
