using System;
using System.Collections.Generic;
using System.Linq;
using PhotoConsole.Domain.Entities;
using PhotoConsole.Domain.Data;
using Constructors.PathString;
using Models.SortHelper;
using System.IO;

namespace Constructors.NotRenameFiles
{
    public class NotRenFiles
    {
        private DateTime date = DateTime.Now;
        public List<Not_inBases> WithoutRenFilesData(int? page, int? limit, string sortBy, string direction, out int total)
        {
            using (var context = new RenFilesEntities())
            {
                var RenFilesRecords = (from p in context.Not_inBase
                               where p.Datelog > date.Date
                               orderby p.Datelog
                               select new Not_inBases
                               {
                                   Idd = p.id,
                                   NotCapture = p.Capture,
                                   DatelogNotData = p.Datelog,
                               }).AsQueryable();


                total = RenFilesRecords.Count();


                if (page.HasValue && limit.HasValue)
                {
                    int start = (page.Value - 1) * limit.Value;
                    RenFilesRecords = RenFilesRecords.OrderByDescending(g => g).Skip(start).Take(limit.Value);
                }

                if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
                {
                    if (direction.Trim().ToLower() == "asc")
                    {
                        RenFilesRecords = SortHelper.OrderBy(RenFilesRecords, sortBy);
                    }
                    else
                    {
                        RenFilesRecords = SortHelper.OrderByDescending(RenFilesRecords, sortBy);
                    }
                }
                return RenFilesRecords.ToList();
            }
        }
    }
}