using System;
using System.Collections.Generic;
using System.Linq;
using PhotoConsole.Domain.Entities;
using PhotoConsole.Domain.Data;
using Constructors.PathString;
using Models.SortHelper;
using System.IO;

namespace Constructors.GridAction
{
    public class GridModel
    {
        private string FilePath;
        private string FilePathNew;
        public List<OutInfos> GetPlayers(int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            DateTime dt;
            using (var context = new RenFilesEntities())
            {
                var records = (from p in context.OutInfo
                               select new OutInfos
                               {
                                   Id = p.Id,
                                   Barlog = p.Barlog,
                                   Codelog = p.Codelog,
                                   Datelog = p.Datelog,
                                   Renlog = p.Renlog
                               }).AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    if (DateTime.TryParse(searchString, out dt))
                    {
                        records = records.Where(p => p.Datelog == dt);
                    }
                    else
                    {
                        records = records.Where(p => p.Barlog.Contains(searchString) || p.Codelog.Contains(searchString));
                    }
                }
                    total = records.Count();

                    page = 1 <= (total / limit.Value) ? page : 1;

                if (page.HasValue && limit.HasValue)
                {
                    int start = (page.Value - 1) * limit.Value;
                    records = records.OrderByDescending(g => g).Skip(start).Take(limit.Value);
                }

                if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
                {
                    if (direction.Trim().ToLower() == "asc")
                    {
                        records = SortHelper.OrderBy(records, sortBy);
                    }
                    else
                    {
                        records = SortHelper.OrderByDescending(records, sortBy);
                    }
                }
                
                return records.ToList();
            }
        }

        public void Save(OutInfo Cap)
        {
            using (var context = new RenFilesEntities())
            {
                if (Cap.Id > 0)
                {
                    OutInfo Icp = context.OutInfo
                        .Where(c => c.Id == Cap.Id)
                        .FirstOrDefault();

                    if (Icp != null)
                    {
                        FilePath = VariableConfig.ImagesPath + Icp.Codelog + ".png";
                        FilePathNew = VariableConfig.ImagesPath + Cap.Codelog + ".png";
                        try
                        {
                            if (File.Exists(FilePath))
                            {
                                File.Move(FilePath, FilePathNew);
                            }
                            Icp.Codelog = Cap.Codelog;
                            context.SaveChanges();
                        }
                        catch (System.IO.IOException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else
                {
                }                
            }
        }

        public void Remove(OutInfos Codelog)
        {
            using (var context = new RenFilesEntities())
            {
                var Icp = (from p in context.OutInfo
                                 where p.Codelog == Codelog.Codelog
                                 select p);
                if (Icp != null)
                {
                    FilePath = VariableConfig.ImagesPath + Codelog.Codelog + ".png";
                    try
                    {
                        if (File.Exists(FilePath))
                        {
                            File.Delete(FilePath);
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    foreach (var names in Icp.ToList())
                    {
                        context.OutInfo.Remove(names);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}