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
        private RenFilesEntities1 contexts = new RenFilesEntities1();

        public IQueryable<IntoCapture> IntoCaptures
        {
            get { return contexts.IntoCapture; }
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
        public IQueryable<InCapture> InCaptures
        {
            get { return contexts.InCapture; }
        }
        public IQueryable<OutInCapture> OutInCapture
        {
            get { return contexts.OutInCapture; }
        }
        public IQueryable<Not_inBase> Not_inBase
        {
            get { return contexts.Not_inBase; }
        }
        public IQueryable<InBarcode> InBarcode
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
