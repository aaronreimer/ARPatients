using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ARPatients.Models;
using Microsoft.AspNetCore.Http;

namespace ARPatients.Controllers
{
    /// <summary>
    /// Creates and maintains the Countries
    /// Created Sept 2019 Aaron Reimer
    /// </summary>
    public class ARCountryController : Controller
    {
        public IActionResult Binder()
        {
            ViewData["Section"] = "Section Albums Binder Action";

            TempData["id"] = "Album TempData id";

            HttpContext.Session.SetString("id", "Album Session id");

            return View();
        }
        private readonly Patients _context;
        //Connects database to controller
        public ARCountryController(Patients context)
        {
            _context = context;
        }

        // GET: ARCountry
        public async Task<IActionResult> Index()
        {
            return View(await _context.Country.ToListAsync());
        }

        // GET: ARCountry/Details/5
        /// <summary>
        /// Shows details of country
        /// </summary>
        /// <param name="id">ID of country</param>
        /// <returns>Details of country</returns>
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: ARCountry/Create
        /// <summary>
        /// Creates and shows index view
        /// </summary>
        /// <returns>Index View</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: ARCountry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Creates a new country
        /// </summary>
        /// <param name="country">Name of Country</param>
        /// <returns>Redirects to index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: ARCountry/Edit/5
        /// <summary>
        /// Checks if country exists to be edited
        /// </summary>
        /// <param name="id">ID of country</param>
        /// <returns>Generates Country view</returns>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: ARCountry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edits a country's info
        /// </summary>
        /// <param name="id">Id of country</param>
        /// <param name="country"></param>
        /// <returns>Redirects to index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax")] Country country)
        {
            if (id != country.CountryCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryCode))
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
            return View(country);
        }

        // GET: ARCountry/Delete/5
        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="id">id of country to be deleted</param>
        /// <returns>Redirects to index</returns>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: ARCountry/Delete/5
        /// <summary>
        /// Confirms deletion of country
        /// </summary>
        /// <param name="id">Id of country to be deleted</param>
        /// <returns>Redirects to index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var country = await _context.Country.FindAsync(id);
            _context.Country.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Checks if a country exists
        /// </summary>
        /// <param name="id">Id of country</param>
        /// <returns></returns>
        private bool CountryExists(string id)
        {
            return _context.Country.Any(e => e.CountryCode == id);
        }
    }
}
