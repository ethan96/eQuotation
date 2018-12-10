using eQuotation.Entities;
using eQuotation.Models.Material;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Controllers
{
    public class MaterialController : AppControllerBase
    {
        [Authorize]
        public ActionResult ProductManager(string id)
        {
            var model = new ProductManager();
            if (!string.IsNullOrWhiteSpace(id))
            {
                model.GetValueByID(id);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ProductManager(ProductManager model)
        {
            model.AddOrUpdate();
            return RedirectToAction("ProductManager", new { id = model.Header.ID });
        }

	}
}