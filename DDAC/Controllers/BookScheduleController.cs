using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using DDAC.Models;
using DDAC.ViewModels;

namespace DDAC.Controllers
{
    public class BookScheduleController : Controller
    {
        private ApplicationDbContext _context;
        private static BookScheduleModel bm;

        public BookScheduleController()
        {
            if (bm == null)
            {
                bm = new BookScheduleModel() {ContainerModels = new List<ContainerModels>()};
            }

            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: BookSchedule
        public ActionResult Index()
        {
            var currentbookschedule = _context.BookScheduleModels.Include(b => b.ScheduleDetails).Include(b => b.ScheduleDetails.ShipDetails).Include(b => b.CustomerModels).ToList();
            return View(currentbookschedule);
        }

        public ActionResult ViewSchedule()
        {   
            var availableschedule = _context.ScheduleDetails.Include(b => b.ShipDetails).ToList();

            return View(availableschedule);
        }


        public ActionResult Booking(int id)
        {
            var viewschedule = _context.ScheduleDetails.Include(b => b.ShipDetails).SingleOrDefault(b => b.Id == id);
            var viewcustomer = _context.CustomerModels.Where(b=>b.AspNetUsersId == User.Identity.Name);

            BookScheduleViewModel bsvm = new BookScheduleViewModel
            {
                ScheduleDetails = viewschedule,
                CustomerModels = viewcustomer
            };

            return View(bsvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBooking([Bind(Exclude = "Origin,Destination,StartDate,EndDate")]BookScheduleViewModel bsvm)
        {
            var viewschedule = _context.ScheduleDetails.Include(b => b.ShipDetails).SingleOrDefault(b => b.Id == bsvm.ScheduleDetails.Id);
            var viewcustomer = _context.CustomerModels.Where(b => b.AspNetUsersId == User.Identity.Name);

            BookScheduleViewModel bsvm1 = new BookScheduleViewModel
            {
                ScheduleDetails = viewschedule,
                CustomerModels = viewcustomer
            };

            var addbooking = new BookScheduleModel
            {
                ContainerModels = bm.ContainerModels,
                CustomerModelsId = bsvm.BookScheduleModels.CustomerModelsId,
                ScheduleDetailsId = bsvm.ScheduleDetails.Id,
                IsDelivered = false
            };

            var getBay = _context.ScheduleDetails.Include(db => db.ShipDetails)
                .SingleOrDefault(db => db.Id == addbooking.ScheduleDetailsId);

            var deductBay = _context.ShipDetails.SingleOrDefault(db => db.Id == getBay.ShipDetailsId);

            var BayUsed = 0;
            foreach (var i in bm.ContainerModels)
            {
                BayUsed += i.NumberOfBayUsed;
            }

            addbooking.totalBayUsed = BayUsed;
                
                deductBay.RemainingBaySize = deductBay.RemainingBaySize - BayUsed;

                if (deductBay.RemainingBaySize == 0)
                {
                    deductBay.Availability = false;
                }

                _context.SaveChanges();


            _context.BookScheduleModels.Add(addbooking);

            try
            {
                _context.SaveChanges();

                ViewBag.IsSuccess = true;
                ViewBag.Message = "Booking has been made successfully.";
                bm.ContainerModels.Clear();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Create Failed.\nError: " + ex.Message;

            }

            return RedirectToAction("Index");
        }

        public ActionResult AddContainer(int id, string remain, string container, string bay)
        {
            var viewschedule = _context.ScheduleDetails.Include(b => b.ShipDetails).SingleOrDefault(b => b.Id == id);

            BookScheduleViewModel bsvm = new BookScheduleViewModel
            {
                ScheduleDetails = viewschedule
            };

            int baysize = 0;
            if (string.IsNullOrWhiteSpace(bay) || string.IsNullOrWhiteSpace(container))
            {
                return Json(new { success = false, message = "Please fill in all the fields completely."}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                baysize = Convert.ToInt32(bay);
            }
            
            var total = 0;
            var ori = Convert.ToInt32(remain);
            ContainerModels c = new ContainerModels
            {
                ContainerType = container,
                NumberOfBayUsed = baysize
            };

            total = total + baysize;

            if (bm.ContainerModels.Count > 0) // if there are exisiting container type with number of bay in the list
            {
                foreach (var i in bm.ContainerModels)
                {
                    total += i.NumberOfBayUsed;
                }               
            }

            if ( ori >= total)
            {
                bm.ContainerModels.Add(c);
            }
            else
            {
                //ViewBag.IsSuccess = false;
                //ViewBag.Message = "The bay size of the ship selected is lesser than the amount you wanted.";
                //return View("Booking", bsvm);
                return Json(new { success = false, message = "The bay size of the ship selected is lesser than the amount you wanted." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}