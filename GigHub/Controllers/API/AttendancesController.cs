namespace GigHub.API
{
    using GigHub.Dtos;
    using GigHub.Models;
    using Microsoft.AspNet.Identity;
    using System.Linq;
    using System.Web.Http;

    public class AttendancesController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController()
        {
            this._context = new ApplicationDbContext();
        }


        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {

            var userId = this.User.Identity.GetUserId();

            if (this._context.Attendances
                .Any(a => a.AttendeeId == userId
                          && a.GigId == dto.GigId))
                return this.BadRequest("The attendance already exists.");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            this._context.Attendances.Add(attendance);
            this._context.SaveChanges();

            return this.Ok();
        }
    }
}
