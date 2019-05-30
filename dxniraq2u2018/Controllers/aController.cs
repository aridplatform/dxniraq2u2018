using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dxniraq2u2018.Controllers
{
    public class aController : Controller
    {
        public IActionResult Index()
        {
                        return RedirectToAction("Display", "BranchAdvertismentScreenks");
        }
    }
}