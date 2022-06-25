using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using taskSquare.DAL;
using taskSquare.Models;

namespace taskSquare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TitleAreasController : Controller
    {
        private readonly AppDbContext _context;

        public TitleAreasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/TitleAreas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TitleArea.ToListAsync());
        }

        // GET: Admin/TitleAreas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titleArea = await _context.TitleArea
                .FirstOrDefaultAsync(m => m.Id == id);
            if (titleArea == null)
            {
                return NotFound();
            }

            return View(titleArea);
        }

        // GET: Admin/TitleAreas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TitleAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,SubTitle,Text,Id")] TitleArea titleArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(titleArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(titleArea);
        }

        // GET: Admin/TitleAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titleArea = await _context.TitleArea.FindAsync(id);
            if (titleArea == null)
            {
                return NotFound();
            }
            return View(titleArea);
        }

        // POST: Admin/TitleAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,SubTitle,Text,Id")] TitleArea titleArea)
        {
            if (id != titleArea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(titleArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TitleAreaExists(titleArea.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(titleArea);
        }

        // GET: Admin/TitleAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titleArea = await _context.TitleArea
                .FirstOrDefaultAsync(m => m.Id == id);
            if (titleArea == null)
            {
                return NotFound();
            }

            return View(titleArea);
        }

        // POST: Admin/TitleAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var titleArea = await _context.TitleArea.FindAsync(id);
            _context.TitleArea.Remove(titleArea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TitleAreaExists(int id)
        {
            return _context.TitleArea.Any(e => e.Id == id);
        }
    }
}
