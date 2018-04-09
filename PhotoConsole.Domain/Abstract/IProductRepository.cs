using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoConsole.Domain.Data;
//using PhotoConsole.Domain.Entities;

namespace PhotoConsole.Domain.Abstract
{
    public interface IProductRepository
    {
        IQueryable<ProcessCapture> IntoCaptures { get; }
        IQueryable<Categories> Category { get; }
        IQueryable<OutInfo> OutInfos { get; }
        IQueryable<ForWebTest> ForWebTests { get; }
        IQueryable<UploadCapture> InCaptures { get; }
        IQueryable<OutInCapture> OutInCapture { get; }
        IQueryable<Not_inBase> Not_inBases { get; }
        IQueryable<InBarcode> InBarcodes { get; }
    }
}
