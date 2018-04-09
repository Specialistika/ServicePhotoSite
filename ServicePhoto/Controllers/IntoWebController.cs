using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repair.context;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
//using PhotoConsole.Domain.Entities;
using PhotoConsole.Domain.Data;

namespace UnZipFileForWeb.Controllers
{
    public class IntoWebController : RapairController
    {
        public ActionResult Read_IntoCap([DataSourceRequest] DataSourceRequest request)
        {
            return Json(context.ProcessCapture.ToDataSourceResult(request, e => new ProcessCapture
			{ 
                Id = e.Id,
                ToCapture = e.ToCapture,
				DateProcess = Convert.ToDateTime(e.DateProcess)
            }));
        }
        public ActionResult Update_IntoCap(ProcessCapture employee)
        {
            if (employee != null && ModelState.IsValid)
            {
                var target = GetEmployeeByID(employee.Id);
                target.ToCapture = employee.ToCapture;
                target.DateProcess = employee.DateProcess;
                context.SaveChanges();
            }

            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult Create_IntoCap(ProcessCapture employee)
        {
            if (employee != null && ModelState.IsValid)
            {
                var target = new ProcessCapture
				{
                Id = employee.Id,
                ToCapture = employee.ToCapture,
				DateProcess = employee.DateProcess
                };
                context.ProcessCapture.Add(target);
                context.SaveChanges();

                employee.Id = target.Id;
            }

            return Json(new[] { employee }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        public ActionResult Destroy_IntoCap(int employeeID)
        {
            var target = GetEmployeeByID(employeeID);
            context.ProcessCapture.Remove(target);
            context.SaveChanges();

            return Json(ModelState.ToDataSourceResult());
        }
        private ProcessCapture GetEmployeeByID(int id)
        {
            return context.ProcessCapture.FirstOrDefault(e => e.Id == id);
        }
    }
}