using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PSTest.Areas.Database.Controllers
{
    public class UpdateController : Controller
    {
        // GET: Database/Update
        public ActionResult Index()
        {
            return View(Execute());
        }

        public static List<string> Execute()
        {
            var returnable = new List<string>();
            
            var assembly = Assembly.GetExecutingAssembly();

            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var assemblyLocation = Path.GetDirectoryName(path);
            
            string resourcePath = "";

            string ModuleName = assembly.ManifestModule.Name;
            ModuleName = ModuleName.Substring(0, ModuleName.LastIndexOf("."));
            ModuleName = ModuleName.Replace(' ', '_').Replace(".", "");

            string FolderPath = "Areas.Database.SQL";
            FolderPath = FolderPath.Replace(' ', '_');

            if (FolderPath != null && FolderPath.Length > 0 && FolderPath[FolderPath.Length - 1] == '.')
                FolderPath = FolderPath.Substring(0, FolderPath.Length - 1);

            StringBuilder filepath = new StringBuilder();
            filepath.Append(ModuleName);
            if (FolderPath != null && FolderPath.Length > 0)
            {
                filepath.Append('.' + FolderPath);
                filepath.Append('.');
            }
            resourcePath = filepath.ToString();

            string[] resourceNames = assembly.GetManifestResourceNames();
            foreach (var resourceName in resourceNames)
            {
                if (Regex.Match(resourceName, "^" + resourcePath).Success)
                {
                    returnable.Add(resourceName);
                }
            }

            var orderedFileNames = new List<string>();
            if (returnable != null && returnable.Any())
            {
                orderedFileNames = returnable.OrderBy(q => q).ToList();
            }
            else
            {
                returnable.Add("No files found");
            }

            return returnable;
        }
    }
}