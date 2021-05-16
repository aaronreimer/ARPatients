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
    public class ARPatientTreatmentController : Controller
    {
        private readonly Patients _context;

        public ARPatientTreatmentController(Patients context)
        {
            _context = context;
        }
        // GET: ARPatientTreatment
        public async Task<IActionResult> Index(int? diagnosisId, string patientFirst, string patientLast, string diagnosisName)
        {
            if(diagnosisId != null)
            {
                Response.Cookies.Append("diagnosisId", diagnosisId.ToString());
                Response.Cookies.Append("patientFirst", patientFirst.ToString());
                Response.Cookies.Append("patientLast", patientLast.ToString());
                Response.Cookies.Append("diagnosisName", diagnosisName.ToString());
            }
            else
            {
                if(Request.Cookies["diagnosisId"] == null)
                {
                    TempData["Message"] = "error";
                    return RedirectToAction("Index", "ARPatientTreatment");
                }
                else
                {
                    diagnosisId = Convert.ToInt32(Request.Cookies["diagnosisId"]);
                    patientFirst = Convert.ToString(Request.Cookies["patientFirst"]);
                    patientLast = Convert.ToString(Request.Cookies["patientLast"]);
                    diagnosisName = Convert.ToString(Request.Cookies["diagnosisName"]);
                    
                }
            }
            ViewBag.diagnosisName = diagnosisName;
            ViewBag.patientFirst = patientFirst;
            ViewBag.patientLast = patientLast;
            var patients = _context.PatientTreatment.Where(p => p.PatientDiagnosisId == diagnosisId)
                .Include(p => p.PatientDiagnosis).Include(p => p.Treatment).OrderByDescending(p => p.DatePrescribed);
            return View(await patients.ToListAsync());
        }

        // GET: ARPatientTreatment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.patientFirst = Request.Cookies["patientLast"].ToString();
            ViewBag.patientLast = Request.Cookies["patientFirst"].ToString();
            ViewBag.diagnosisName = Request.Cookies["diagnosisName"].ToString();

            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // GET: ARPatientTreatment/Create
        public IActionResult Create()
        {
            ViewBag.patientFirst = Request.Cookies["patientLast"].ToString();
            ViewBag.patientLast = Request.Cookies["patientFirst"].ToString();
            ViewBag.diagnosisName = Request.Cookies["diagnosisName"].ToString();

            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId");
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(p => p.DiagnosisId == int.Parse(Request.Cookies["diagnosisId"])), "TreatmentId", "TreatmentId");
            return View();
        }

        // POST: ARPatientTreatment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
           // if (patientTreatment.Comments =="1") ModelState.AddModelError("Comments", "Comment is 1");
            var a = ModelState.First(p => p.Key == "Comments");
            a.Value.Errors.Add("Duplicate");
            if (ModelState.IsValid)
            {
                _context.Add(patientTreatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "Name");

            return View(patientTreatment);
        }

        // GET: ARPatientTreatment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment.FindAsync(id);
            if (patientTreatment == null)
            {
                return NotFound();
            }
            ViewBag.patientFirst = Request.Cookies["patientLast"].ToString();
            ViewBag.patientLast = Request.Cookies["patientFirst"].ToString();
            ViewBag.diagnosisName = Request.Cookies["diagnosisName"].ToString();

            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(p => p.DiagnosisId == int.Parse(Request.Cookies["diagnosisId"])), "TreatmentId", "Name", patientTreatment.Treatment);
            return View(patientTreatment);
        }

        // POST: ARPatientTreatment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            if (id != patientTreatment.PatientTreatmentId)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientTreatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTreatmentExists(patientTreatment.PatientTreatmentId))
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
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // GET: ARPatientTreatment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.patientFirst = Request.Cookies["patientLast"].ToString();
            ViewBag.patientLast = Request.Cookies["patientFirst"].ToString();
            ViewBag.diagnosisName = Request.Cookies["diagnosisName"].ToString();
            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // POST: ARPatientTreatment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientTreatment = await _context.PatientTreatment.FindAsync(id);
            _context.PatientTreatment.Remove(patientTreatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientTreatmentExists(int id)
        {
            return _context.PatientTreatment.Any(e => e.PatientTreatmentId == id);
        }
    }
}
