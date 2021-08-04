using TwenyTwoRT.EfStuff;
using TwenyTwoRT.EfStuff.Model;
using TwenyTwoRT.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.Controllers
{
    public class CertificateController : Controller
    {
        private SpaceDbContext _dbContext;
        public CertificateController(SpaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        
    }
}
