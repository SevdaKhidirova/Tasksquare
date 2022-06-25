using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using taskSquare.DAL;
using taskSquare.ViewModels;

namespace taskSquare.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            TeamViewModel tvm = new TeamViewModel
            {
                teamMembers = await db.TeamMembers.ToListAsync(),
                titleArea = await db.TitleArea.OrderByDescending(x=>x.Id).LastAsync()
            }; 
            return View(tvm);
        }

    }
}
