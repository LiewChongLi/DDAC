using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDAC.Models;

namespace DDAC.ViewModels
{
    public class AddShipViewModel
    {
        public ShipDetails ShipDetails { get; set; }
        public IEnumerable<Location> Location { get; set; }
    }
}