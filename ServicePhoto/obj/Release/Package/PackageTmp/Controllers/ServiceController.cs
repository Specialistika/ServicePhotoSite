using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoConsole.Domain.Data;
using PhotoConsole.Domain.Entities;
using Constructors.GridAction;

namespace Controllers.GridForView
{
    [NoCache]
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult ServiceView()
        {
            return View();
        }
               [HttpGet]
        public JsonResult GetPlayers(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total;
            var records = new GridModel().GetPlayers(page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(OutInfo player)
        {
            new GridModel().Save(player);
            return Json(true);
        }

        [HttpPost]
        public JsonResult Remove(OutInfos Codelog)
        {
            new GridModel().Remove(Codelog);
            return Json(true);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }
    
}