using System.Collections.Generic;
using System.Web.Mvc;

namespace NezamEquipment.Web.Controllers
{
    public partial class ErrorController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult HttpError404()
        {
            return View();
        }

        public virtual ActionResult NoPermission(int? id = null)
        {
            return View(id ?? 0);
        }

        public virtual ActionResult ListOfErrors(IList<string> errors)
        {
            return View(errors);
        }

    }
}