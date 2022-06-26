using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using taskSquare.DAL;
using taskSquare.Models;

namespace taskSquare.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeamMembersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment env;

        public TeamMembersController(AppDbContext context, IWebHostEnvironment _env)
        {
            _context = context;
            env = _env;
        }

        // GET: Admin/TeamMembers
        public async Task<IActionResult> Index()
        {
            return View(await _context.TeamMembers.ToListAsync());
        }

        // GET: Admin/TeamMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // GET: Admin/TeamMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TeamMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMember teamMember)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!teamMember.Img.ContentType.Contains("image"))
            {
                ModelState.AddModelError("img", "Fole is not an image");
                return View();
            }

            if (teamMember.Img.Length / 1024 > 1000)
            {
                ModelState.AddModelError("Img", "File is too big");
                return View();
            }

            string path = env.WebRootPath + @"\images";
            string fileName = Guid.NewGuid().ToString() + teamMember.Img.FileName;
            string final = Path.Combine(path, fileName);

            using(FileStream stream=new FileStream(final, FileMode.Create))
            {
                await teamMember.Img.CopyToAsync(stream);
            }

            teamMember.Image = fileName;

            _context.Add(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/TeamMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }
            return View(teamMember);
        }

        // POST: Admin/TeamMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,TeamMember teamMember)
        {
            if (id != teamMember.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (teamMember.Img != null)
            {
                if (!teamMember.Img.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("img", "Fole is not an image");
                    return View();
                }

                if (teamMember.Img.Length / 1024 > 1000)
                {
                    ModelState.AddModelError("Img", "File is too big");
                    return View();
                }

                string path = env.WebRootPath + @"\images";
                string fileName = Guid.NewGuid().ToString() + teamMember.Img.FileName;
                string final = Path.Combine(path, fileName);

                if (System.IO.File.Exists(Path.Combine(path, teamMember.Image)))
                {
                    System.IO.File.Delete(Path.Combine(path, teamMember.Image));
                }

                using (FileStream stream = new FileStream(final, FileMode.Create))
                {
                    await teamMember.Img.CopyToAsync(stream);
                }

                teamMember.Image = fileName;
            }
            
            _context.Update(teamMember);
                    await _context.SaveChangesAsync();
           
                return RedirectToAction(nameof(Index));
        }

        // GET: Admin/TeamMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // POST: Admin/TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamMember = await _context.TeamMembers.FindAsync(id);
            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamMemberExists(int id)
        {
            return _context.TeamMembers.Any(e => e.Id == id);
        }
    }
}
