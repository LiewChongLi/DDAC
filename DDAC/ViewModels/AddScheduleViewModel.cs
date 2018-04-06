using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDAC.Models;

namespace DDAC.ViewModels
{
    public class AddScheduleViewModel
    {
        public ScheduleDetails ScheduleDetails { get; set; }
        public IEnumerable<ShipDetails> ShipDetails { get; set; }
        public IEnumerable<Location> Locations { get; set; }
    }
}