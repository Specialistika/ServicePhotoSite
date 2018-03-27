using System;
using System.Collections.Generic;
using System.Linq;
using PhotoConsole.Domain.Entities;
using PhotoConsole.Domain.Data;
using Models.SortHelper;

namespace Constructors.OutInfoGrid
{
    public class GridModelOutInfo
    {
        private DateTime date = DateTime.Now;
        public List<OutInfos> OutInfos(int? page, int? limit, string sortB, string direction, out int total)
        {
            using (var context = new RenFilesEntities())
            {
                var records = (from p in context.OutInfo
                                       where p.Datelog > date.Date
                                       orderby p.Datelog
                                       select new OutInfos
                                       {
                                           Id = p.Id,
                                           Barlog = p.Barlog,
                                           Codelog = p.Codelog,
                                           Datelog = p.Datelog,
                                           Renlog = p.Renlog
                                       }).AsQueryable();

                total = records.Count();

                //if (!string.IsNullOrWhiteSpace(searchStrin))
                //{
                //    if (DateTime.TryParse(searchStrin, out dt))
                //    {
                //        records = records.Where(p => p.Datelog == dt);
                //    }
                //    else
                //    {
                //        records = records.Where(p => p.Barlog.Contains(searchStrin));
                //    }
                //}

                if (page.HasValue && limit.HasValue)
                {
                    int start = (page.Value - 1) * limit.Value;
                    records = records.OrderByDescending(g => g).Skip(start).Take(limit.Value);
                }

                if (!string.IsNullOrEmpty(sortB) && !string.IsNullOrEmpty(direction))
                {
                    if (direction.Trim().ToLower() == "asc")
                    {
                        records = SortHelper.OrderBy(records, sortB);
                    }
                    else
                    {
                        records = SortHelper.OrderByDescending(records, sortB);
                    }
                }
                return records.ToList();
            }
        }


    }
}