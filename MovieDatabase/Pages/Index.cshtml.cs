using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MovieDatabase.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This method is called by client via HTTP GET and returns list of requested movies from DB.
        /// </summary>
        public IActionResult OnGetDefaultList()
        {
            var where = "";
            var orderBy = "";
            foreach (var item in Request?.Query)
            {
                if (item.Key == "release_date")
                {
                    if (where == "") where += $"year(release_date) = '{item.Value}'";
                    else where += $"and year(release_date) = '{item.Value}'";
                }
                /*if (item.Key == "genre")
                {
                    if (where == "") where += $"release_date = {item.Value}";
                    else where += $"and release_date = {item.Value}";
                }*/

            }
            
            var x = new MovieRepository().GetMultiple(where: where);
            var y = JsonConvert.SerializeObject(x);
            var z = Content(y);
            return z;
        }

        public void OnPost()
        {
            //Movie.Test();
        }
    }
}
