using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FoodOrder.Common.Utilities
{
    public static class FileWriter
    {
        public static async Task<string> WriteFile(this IFormFile file, string url)
        {
            if (file is null)
                return null;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                string fileNamelocal = Path.GetRandomFileName() + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), url, fileNamelocal);
                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }

                return String.IsNullOrEmpty(fileNamelocal) ? null : fileNamelocal;
            }
            catch (Exception)
            {
                //TO DO:Log
                return null;
            }
        }
    }
}
