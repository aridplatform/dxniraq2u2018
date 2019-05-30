using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dxniraq2u2018.Controllers
{
  
    public class MobilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}