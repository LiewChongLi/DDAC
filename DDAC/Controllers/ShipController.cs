using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using DDAC.Models;
using DDAC.ViewModels;
using Microsoft.Ajax.Utilities;

namespace DDAC.Controllers
{
    public class ShipController : Controller
    {
        private ApplicationDbContext _context;

        public ShipController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Ship
        public ActionResult Index()
        {
            var existingShip = _context.ShipDetails.Where(s => s.Availability == true).ToList();
            return View(existingShip);
        }

        [HttpGet]
        public ActionResult AddShip()
        {
            AddShipViewModel asvm = new AddShipViewModel
            {
               Location = _context.Location
                .DistinctBy(l=>l.Origin)
                .ToList()
            };
            return View(asvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateShip(ShipDetails shipDetails)
        {
            AddShipViewModel asvm = new AddShipViewModel
            {
                Location = _context.Location
                    .DistinctBy(l => l.Origin)
                    .ToList()
            };

            if (!ModelState.IsValid)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Ship Create Failed.";

                return View("AddShip", asvm);
            }
            shipDetails.RemainingBaySize = shipDetails.BaySize;
            shipDetails.Availability = true;


            _context.ShipDetails.Add(shipDetails);

            try
            {
                _context.SaveChanges();

                ViewBag.IsSuccess = true;
                ViewBag.Message = "Ship added successfully. Thanks for using our service";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Error in adding ship! \nError: " + ex.Message;
            }


            return View("AddShip", asvm);

        }

        [HttpGet]
        public ActionResult FindDestination(string origin)
        {
            var destination = _context.Location
                .Where(l => l.Origin == origin)
                .ToList()
                .DistinctBy(l => l.Destination);

            return Json(destination, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteShip(int id)
        {
            var schedule = _context.ScheduleDetails.Include(s => s.ShipDetails).SingleOrDefault(s => s.ShipDetailsId == id);
            var selected = _context.ShipDetails.SingleOrDefault(s => s.Id == id);
            

            if (schedule == null)
            {
                selected.Availability = false;
                _context.SaveChanges();
                var existingShip = _context.ShipDetails.Where(s => s.Availability == true).ToList();
                TempData["delete"] = "The ship has been successfully deleted.";
                return View("Index", existingShip);
            }
            else
            {
                var existingShip = _context.ShipDetails.Where(s => s.Availability == true).ToList();
                TempData["delete-not-success"] = "The ship has existing schedule, cannot be deleted";
                return View("Index", existingShip);
            }
            
        }
    }

}