using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Experimental1.Controllers
{
    public class UploaderBaseController : Controller
    {
        // GET: UploaderBase
        public ActionResult Index(string uid="23ab123e22290d1a9dff")
        {
            return View();
        }
        public ActionResult Description()
        {
            return View();
        }
    }
}