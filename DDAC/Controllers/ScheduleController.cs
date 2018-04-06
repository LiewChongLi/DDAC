using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using DDAC.Models;
using DDAC.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace DDAC.Controllers
{
    public class ScheduleController : Controller
    {
        private ApplicationDbContext _context;

        public ScheduleController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Schedule
        public ActionResult Index()
        {
            var currentSchedule = _context.ScheduleDetails.Include(s => s.ShipDetails).ToList();
            
            return View(currentSchedule);
        }

        [HttpGet]
        public ActionResult AddSchedule()
        {
            AddScheduleViewModel asvm = new AddScheduleViewModel
            {
                Locations = _context.Location
                    .DistinctBy(l => l.Origin)
                    .ToList()
            };
            
            return View(asvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSchedule([Bind(Exclude = "Id")] ScheduleDetails scheduleDetails)
        {

            AddScheduleViewModel asvm = new AddScheduleViewModel
            {
                Locations = _context.Location
                    .DistinctBy(l => l.Origin)
                    .ToList()
            };

            if (!ModelState.IsValid)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Schedule Create Failed.";

                return View("AddSchedule", asvm);
            }

            _context.ScheduleDetails.Add(scheduleDetails);

            try
            {
                _context.SaveChanges();

                ViewBag.IsSuccess = true;
                ViewBag.Message = "Schedule has been created successfully.";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Create Failed.\nError: " + ex.Message;
                ModelState.Remove("StartDate");
                ModelState.Remove("EndDate");
            }

            return View("AddSchedule", asvm);

        }

        public ActionResult FindDestination(string origin)
        {
            var destination = _context.Location
                .Where(l => l.Origin == origin)
                .ToList()
                .DistinctBy(l => l.Destination);

            return Json(destination, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindShip(string origin, string destination)
        {
            var ship = _context.ShipDetails
                .Where(s => s.Origin == origin && s.Destination == destination)
                .ToList();

            return Json(ship, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSchedule(int id)
        {
            var schedule = _context.ScheduleDetails.Include(s => s.ShipDetails).SingleOrDefault(s => s.Id == id);
            AddScheduleViewModel asvm = new AddScheduleViewModel
            {
                ScheduleDetails = schedule,
                Locations = _context.Location
                    .DistinctBy(l => l.Origin)
                    .ToList()
            };
            ViewBag.Name = "Edit";
            
            return View("AddSchedule", asvm);

        }

        public ActionResult Edit(ScheduleDetails scheduleDetails)
        {
            AddScheduleViewModel asvm = new AddScheduleViewModel
            {
                Locations = _context.Location
                    .DistinctBy(l => l.Origin)
                    .ToList()
            };

            if (!ModelState.IsValid)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Schedule Update Failed.";

                return View("AddSchedule", asvm);
            }

            var update = _context.ScheduleDetails.Include(s => s.ShipDetails).SingleOrDefault(s => s.Id == scheduleDetails.Id);
            update.StartDate = scheduleDetails.StartDate;
            update.EndDate = scheduleDetails.EndDate;
            update.Origin = scheduleDetails.Origin;
            update.Destination = scheduleDetails.Destination;
            update.ShipDetailsId = scheduleDetails.ShipDetailsId;
            _context.SaveChanges();

            ViewBag.IsSuccess = true;
            ViewBag.Message = "Schedule has been edited successfully.";
            ModelState.Clear();

            return View("AddSchedule", asvm);
        }

    }
}