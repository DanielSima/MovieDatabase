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

        public IActionResult OnGetTestList()
        {
            var x = new MovieRepository().GetMultiple();
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
