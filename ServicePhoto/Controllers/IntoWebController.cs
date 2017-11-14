using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repair.context;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using PhotoConsole.Domain.Entities;
using PhotoConsole.Domain.Data;

namespace UnZipFileForWeb.Controllers
{
    public class IntoWebController : RapairController
    {
        public ActionResult Read_IntoCap([DataSourceRequest] DataSourceRequest request)
        {
            return Json(context.IntoCapture.ToDataSourceResult(request, e => new IntoCap
            { 
                Id = e.Id,
                ToCapture = e.ToCapture,
                Dateloginto = Convert.ToDateTime(e.Dateloginto)
            }));
        }
        public ActionResult Update_IntoCap(IntoCap employee)
        {
            if (employee != null && ModelState.IsValid)
            {
                var target = GetEmployeeByID(employee.Id);
                target.ToCapture = employee.ToCapture;
                target.Dateloginto = employee.Dateloginto;
                context.SaveChanges();
            }

            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult Create_IntoCap(IntoCap employee)
        {
            if (employee != null && ModelState.IsValid)
            {
                var target = new IntoCapture{
                Id = employee.Id,
                ToCapture = employee.ToCapture,
                Dateloginto = employee.Dateloginto
                };
                context.IntoCapture.Add(target);
                context.SaveChanges();

                employee.Id = target.Id;
            }

            return Json(new[] { employee }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        public ActionResult Destroy_IntoCap(int employeeID)
        {
            var target = GetEmployeeByID(employeeID);
            context.IntoCapture.Remove(target);
            context.SaveChanges();

            return Json(ModelState.ToDataSourceResult());
        }
        private IntoCapture GetEmployeeByID(int id)
        {
            return context.IntoCapture.FirstOrDefault(e => e.Id == id);
        }
    }
}