using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelSystem;
using TravelSystem.Models;

namespace TravelSystem.Controllers
{
    public class ClintsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClintsController(AppDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this._webHostEnvironment = webHostEnvironment;
        }

        // GET: Clints
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clints.ToListAsync());
        }

        // GET: Clints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clint = await _context.Clints
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clint == null)
            {
                return NotFound();
            }

            return View(clint);
        }

        // GET: Clints/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Age,DocumntPath,Documnt")] Clint clint)
        {
            if (ModelState.IsValid)
            {
                if (clint.Documnt != null)
                {
                    string folder = "clints/docs/";
                    clint.DocumntPath = await UploadImage(folder, clint.Documnt);
                }
                _context.Add(clint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clint);
        }

        // GET: Clints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clint = await _context.Clints.FindAsync(id);
            if (clint == null)
            {
                return NotFound();
            }
            return View(clint);
        }

        // POST: Clints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Age,DocumntPath,Documnt")] Clint clint)
        {
            if (id != clint.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (clint.Documnt != null)
                    {
                        string folder = "clints/docs/";
                        clint.DocumntPath = await UploadImage(folder, clint.Documnt);
                    }
                    _context.Update(clint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClintExists(clint.Id))
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
            return View(clint);
        }

        // GET: Clints/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clint = await _context.Clints
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clint == null)
            {
                return NotFound();
            }

            return View(clint);
        }

        // POST: Clints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clint = await _context.Clints.FindAsync(id);
            if (clint != null)
            {
                _context.Clints.Remove(clint);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClintExists(int id)
        {
            return _context.Clints.Any(e => e.Id == id);
        }
        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }
    }
}
