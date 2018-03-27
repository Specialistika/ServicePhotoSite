using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoConsole.Domain.Data;

namespace Repair.context
{
    public class RapairController : Controller
    {
        protected RenFilesEntities context = new RenFilesEntities();
    }
}