using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ARPatients.Models;

namespace ARPatients.Controllers
{
    /// <summary>
    /// Creates and maintains the Concentration Unit Table
    /// Created Sept 2019 Aaron Reimer
    /// </summary>
    public class ARConcentrationUnitController : Controller
    {
        private readonly Patients _context;
        //Connects database to controller
        public ARConcentrationUnitController(Patients context)
        {
            _context = context;
        }

        // GET: ARConcentrationUnit
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConcentrationUnit.ToListAsync());
        }

        // GET: ARConcentrationUnit/Details/5
        /// <summary>
        /// Generates Details view
        /// </summary>
        /// <param name="id">Id of concentration unit</param>
        /// <returns>The Details view</returns>
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        // GET: ARConcentrationUnit/Create
        /// <summary>
        /// Creates view
        /// </summary>
        /// <returns>Creates view</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: ARConcentrationUnit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Creates new concentration unit
        /// </summary>
        /// <param name="concentrationUnit">Name of the concentration unit to be added</param>
        /// <returns>The page with new concentration unit</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concentrationUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concentrationUnit);
        }

        // GET: ARConcentrationUnit/Edit/5
        /// <summary>
        /// Retrieves the concentration unit and displays
        /// </summary>
        /// <param name="id">ID of concentration unit</param>
        /// <returns>Generates view</returns>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }
            return View(concentrationUnit);
        }

        // POST: ARConcentrationUnit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edits a concentration unit given its id
        /// </summary>
        /// <param name="id">ID of concentration unit to edit</param>
        /// <param name="concentrationUnit">Concentration unit to edit</param>
        /// <returns>The updated page with edited concentration unit</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (id != concentrationUnit.ConcentrationCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concentrationUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcentrationUnitExists(concentrationUnit.ConcentrationCode))
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
            return View(concentrationUnit);
        }

        // GET: ARConcentrationUnit/Delete/5
        /// <summary>
        /// Retrieves the concentration unit and displays
        /// </summary>
        /// <param name="id">Id of con. unit to delete</param>
        /// <returns>Calls NotFound if not found, otherwise shows updated list</returns>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        // POST: ARConcentrationUnit/Delete/5
        /// <summary>
        /// Deletes a concentration unit
        /// </summary>
        /// <param name="id">Id of unit to be deleted</param>
        /// <returns>The index view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            _context.ConcentrationUnit.Remove(concentrationUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Checks if a concentration unit exists given an id
        /// </summary>
        /// <param name="id">The id of the concentration unit</param>
        /// <returns>The Concentration Unit view</returns>
        private bool ConcentrationUnitExists(string id)
        {
            return _context.ConcentrationUnit.Any(e => e.ConcentrationCode == id);
        }
    }
}
