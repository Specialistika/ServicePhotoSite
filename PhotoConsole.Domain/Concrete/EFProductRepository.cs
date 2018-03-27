using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoConsole.Domain.Abstract;
using PhotoConsole.Domain.Data;

namespace PhotoConsole.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private RenFilesEntities contexts = new RenFilesEntities();

        public IQueryable<ProcessCapture> IntoCaptures
        {
            get { return contexts.ProcessCapture; }
        }
        public IQueryable<Categories> Category
        {
            get { return contexts.Categories; }
        }
        public IQueryable<OutInfo> OutInfos
        {
            get { return contexts.OutInfo; }
        }
        public IQueryable<ForWebTest> ForWebTests
        {
            get { return contexts.ForWebTest; }
        }
        public IQueryable<UploadCapture> InCaptures
        {
            get { return contexts.UploadCapture; }
        }
        public IQueryable<OutInCapture> OutInCapture
        {
            get { return contexts.OutInCapture; }
        }
        public IQueryable<Not_inBase> Not_inBases
        {
            get { return contexts.Not_inBase; }
        }
        public IQueryable<InBarcode> InBarcodes
        {
            get { return contexts.InBarcode; }
        }
        //public void SaveInfo(ForWebTest forwebtest)
        //{
        //    if (forwebtest.Id == 0)
        //    {
        //        contexts.ForWebTest.Add(forwebtest);
        //    }
        //        contexts.SaveChanges();
        //}
    }
}
