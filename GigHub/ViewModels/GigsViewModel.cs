namespace GigHub.ViewModels
{
    using System.Collections.Generic;

    using GigHub.Models;

    public class GigsViewModel
    {
        public IEnumerable<Gig> UpcomingGigs { get; set; }

        public bool ShowActions { get; set; }

        public string Heading { get; set; }
    }
}