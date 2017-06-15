using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.API
{
    using GigHub.Dtos;
    using GigHub.Models;

    using Microsoft.AspNet.Identity;

    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public FollowingsController()
        {
            this._context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = this.User.Identity.GetUserId();

            if (this._context.Followings
                .Any(f => f.FolloweeId == userId 
                && f.FolloweeId == dto.FolloweeId))

                return BadRequest("Following already exists. ");

            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = dto.FolloweeId
            };

            this._context.Followings.Add(following);
            this._context.SaveChanges();

            return this.Ok();
        }
    }

}
