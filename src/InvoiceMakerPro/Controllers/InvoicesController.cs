using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using InvoiceMakerPro.Models;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace InvoiceMakerPro.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InvoicesController> _logger;
        private readonly IConfiguration _config;

        public InvoicesController(ApplicationDbContext context, ILogger<InvoicesController> logger, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetInvoices()
        {
            try
            {
                var model =
                    _context.Invoice.Include(i => i.Customer).Include(i => i.Store)
                        .Include(i => i.InvoiceDetails)
                        .ThenInclude(p => p.Article)
                        .ToList();
                return Json(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}{Environment.NewLine}{ex.InnerException}");
            }

            return null;
        }

        public JsonResult GetCustomerInvoices(Guid CustomerId)
        {
            try
            {
                var model =
                    _context.Invoice.Include(i => i.Customer).Where(i => i.Customer.CustomerId == CustomerId)
                        .Include(i => i.InvoiceDetails)
                        .ThenInclude(p => p.Article)
                        .ToList();
                return Json(model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}{Environment.NewLine}{ex.InnerException}");
            }

            return null;
        }

        public void DeleteInvoice(Guid InvoiceId)
        {
            var invoice = _context.Invoice.Include(i => i.InvoiceDetails).FirstOrDefault(i => i.InvoiceId == InvoiceId);
            if (invoice != null)
            {
                _context.InvoiceDetails.RemoveRange(invoice.InvoiceDetails);
                _context.Invoice.Remove(invoice);

                _context.SaveChanges();
            }
        }

        public void ReleaseInvoice(Guid InvoiceId)
        {
            var invoice = _context.Invoice.FirstOrDefault(i => i.InvoiceId == InvoiceId);
            if (invoice != null)
            {
                invoice.InvoiceState = InvoiceState.Released;
                _context.Entry(invoice).State = EntityState.Modified;

                _context.SaveChanges();
            }
        }

        public ActionResult EditInvoice(Guid id)
        {
            if (id == Guid.Empty)
            {
                var max = _context.Invoice.ToList().Max(f => f.InvoiceNumber);

                return View(new Invoice
                {
                    InvoiceId = Guid.Empty,
                    InvoiceNumber = (max ?? 0) + 1
                });
            }
            else
            {
                var model = _context.Invoice.Include(i => i.Customer).Include(i => i.Store).Include(i => i.InvoiceDetails).ThenInclude(p => p.Article).FirstOrDefault(i => i.InvoiceId == id);
                foreach (var detail in model.InvoiceDetails)
                {
                    detail.Invoice = null;
                }
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult SaveInvoice([FromBody] Invoice invoice)
        {
            try
            {
                if (invoice.Customer == null)
                    throw new Exception("Please select a Customer.");

                if (invoice.Store == null)
                    throw new Exception("Please select a Store.");

                var cleanList = invoice.InvoiceDetails.ToList();
                cleanList.RemoveAll(article => article.Article == null);
                invoice.InvoiceDetails = cleanList;

                if (invoice.InvoiceDetails == null || invoice.InvoiceDetails.Count == 0)
                    throw new Exception("Please select at least one article.");

                var attachedEntity = _context.Invoice.SingleOrDefault(i => i.InvoiceId == invoice.InvoiceId);
                if (attachedEntity != null)
                {
                    DeleteInvoice(invoice.InvoiceId);
                }

                _context.Invoice.Add(invoice);

                invoice.User = User.Identity.Name;
                _context.SaveChanges();
                return Json(new { success = true, id = invoice.InvoiceId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult Print(Guid id)
        {
            ViewBag.Print = true;
            var companyInfo = _config.GetSection("CompanyInfo").GetChildren();
            ViewBag.MyCompany = companyInfo.FirstOrDefault(k => k.Key == "MyCompanyName")?.Value;
            ViewBag.MyCompanyID = companyInfo.FirstOrDefault(k => k.Key == "MyCompanyID")?.Value;
            ViewBag.MyCompanyAddress = companyInfo.FirstOrDefault(k => k.Key == "MyCompanyAddress")?.Value;
            ViewBag.MyCompanyPhone = companyInfo.FirstOrDefault(k => k.Key == "MyCompanyPhone")?.Value;
            ViewBag.MyEmail = companyInfo.FirstOrDefault(k => k.Key == "MyEmail")?.Value;
            ViewBag.MyBankAccount = companyInfo.FirstOrDefault(k => k.Key == "MyBankAccount")?.Value;

            Invoice invoice = _context.Invoice.Include(i => i.Customer).Include(i => i.InvoiceDetails).ThenInclude(p => p.Article).FirstOrDefault(i => i.InvoiceId == id);
            return View(invoice);
        }
    }
}
