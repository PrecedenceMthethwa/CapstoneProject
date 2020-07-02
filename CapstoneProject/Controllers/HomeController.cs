using StudentBussines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace CapstoneProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var tablebusiness = new StudentBusiness.StudentBusiness();
            return View(tablebusiness.GetAllCustomer("customer"));
        }

        public ActionResult Emails()
        {
            var tablebusiness = new StudentBusiness.StudentBusiness();
            return View(tablebusiness.GetCustomerEmails("customer"));
        }

        public async Task<ActionResult> IndexAsync()
        {
            var tablebusiness = new StudentBusiness.StudentBusiness();
            return View("Index", await tablebusiness.GetAllCustomersAsync("customer"));
        }

        public ActionResult AddCustomer()
        {
            ViewBag.Message = "Upload page.";

            return View();
        }
        [HttpPost]
        public ActionResult AddCustomer(AzureTablesModel.StudentEntity customer)
        {
            if (ModelState.IsValid)
            {
                var AzureTable = new StudentBusiness.AzureTablesBusiness();
                AzureTable.InsertCustomer("customer", customer);
            }

            return RedirectToAction("Index");
        }

        //search customer
        [HttpPost]
        public ActionResult SearchCustomer(string name)
        {
            if ((ModelState.IsValid) && (!string.IsNullOrEmpty(name)))
            {
                var AzureTable = new StudentBusiness.AzureTablesBusiness();
                return View("Index", AzureTable.GetCustomerByName("customer", name));
            }

            return RedirectToAction("Index");
        }
    }