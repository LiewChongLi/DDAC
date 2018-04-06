using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDAC.Models;

namespace DDAC.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext _context;

        public CustomerController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register(CustomerModels cm)
        {
            CustomerModels c = new CustomerModels();
            if (!ModelState.IsValid)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Customer Register Failed.";

                return View("Index", c);
            }

            cm.AspNetUsersId = User.Identity.Name;
            _context.CustomerModels.Add(cm);

            try
            {
                _context.SaveChanges();

                ViewBag.IsSuccess = true;
                ViewBag.Message = "Customer registered successfully. Thanks for using our service";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Error in adding Customer! \nError: " + ex.Message;
            }

            return View("Index", c);
        }

        public ActionResult ViewCustomer()
        {
            if (User.IsInRole("Admin"))
            {
                var customer = _context.CustomerModels.ToList();
                return View(customer);
            }
            else
            {
                var customer = _context.CustomerModels.Where(c => c.AspNetUsersId == User.Identity.Name).ToList();
                return View(customer);

            }
            
        }
    }
}