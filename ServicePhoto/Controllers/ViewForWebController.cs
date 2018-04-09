using System;
using System.Linq;
using System.Web.Mvc;
using PhotoConsole.Domain.Abstract;
using PhotoConsole.Domain.Data;
using PhotoConsole.Domain.Entities;
using Models.SortHelper;
using System.Net;

namespace Controllers.ForWeb
{
	public class ViewForWebController : Controller
	{
		public ActionResult ForWebGridMethod()
		{
			return View();
		}
		public int PageSize = 10;

		private IProductRepository repository;

		public ViewForWebController(IProductRepository productRepository)
		{
			repository = productRepository;
		}

		public JsonResult GetPhoto(UploadCapture Photos)
		{
			string fullPath = $"\\\\{ Dns.GetHostName() }\\Photo\\{Photos.FolderName}\\{Photos.Capture}";

			if (System.IO.File.Exists(fullPath))
			{
				return Json(new { fullPath }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(false);
			}
		}

		//public string GetPhoto(UploadCapture Photos)
		//{
		//	string fullPath = $"\\\\{ Dns.GetHostName() }\\Photo\\{Photos.FolderName}\\{Photos.Capture}";

		//	if (System.IO.File.Exists(fullPath))
		//	{
		//		using (Image image = Image.FromFile(fullPath))
		//		{
		//			using (MemoryStream m = new MemoryStream())
		//			{
		//				image.Save(m, image.RawFormat);
		//				byte[] imageBytes = m.ToArray();

		//				string base64String = Convert.ToBase64String(imageBytes);

		//				return base64String;
		//			}
		//		}
		//	}
		//	else
		//	{
		//		return null;
		//	}
		//}

		public JsonResult ForWebReadMethod(int? page, int? limit, string sortBy, string direction, string searchString = null)
		{
			int total = 0;
			DateTime dt;
			DateTime strDate;
			if (repository != null)
			{
				var maxDate = repository.InCaptures.Max(x => x.DateUpload);
				strDate = Convert.ToDateTime(maxDate).AddDays(-60);
			}
			else
			{
				strDate = DateTime.Now;
			}
			using (var context = new RenFilesEntities())
			{
				var record = (from p in context.UploadCapture
							  select new InCaptures
							  {
								  Id = p.Id,
								  Capture = p.Capture,
								  Dateinto = p.DateUpload,
								  FolderName = p.FolderName,
							  }).AsQueryable();

				if (!string.IsNullOrWhiteSpace(searchString))
				{
					if (DateTime.TryParse(searchString, out dt))
					{
						record = record.Where(s => s.Dateinto == dt);
					}
					else
					{
						record = record.Where(s => s.Capture.Contains(searchString));
					}
				}
				total = record.Count();

				page = 1 <= (total / limit.Value) ? page : 1;

				if (page.HasValue && limit.HasValue)
				{
					int start = (page.Value - 1) * limit.Value;
					record = record.OrderByDescending(g => g).Skip(start).Take(limit.Value);
				}

				if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
				{
					if (direction.Trim().ToLower() == "asc")
					{
						record = SortHelper.OrderBy(record, sortBy);
					}
					else
					{
						record = SortHelper.OrderByDescending(record, sortBy);
					}
				}

				var records = record.ToList();
				return Json(new { records, total }, JsonRequestBehavior.AllowGet);
			}
			//public ViewResult List(int page = 1)
			//{
			//    DateTime strDate;
			//    if (repository != null)
			//    {
			//        var maxDate = repository.InCaptures.Max(x => x.Dateinto);
			//        strDate = Convert.ToDateTime(maxDate).AddDays(-1);
			//    }
			//    else
			//    {
			//        strDate = DateTime.Now;
			//    }
			//        var viewModel = new ForWebModel
			//        {
			//            ForWebTests = repository.ForWebTests
			//            .OrderBy(p => p.Data)
			//            //>= strDate)
			//            .Skip((page - 1) * PageSize)
			//            .Take(PageSize),
			//            pageinfo = new Pageinfo
			//            {
			//                CurrentPage = page,
			//                ItemsPerPage = PageSize,
			//                TotalItems = repository.ForWebTests.Count(s => s.Data >= strDate)
			//            }
			//        };
			//        return View(viewModel);
			//}

		}
	}

}