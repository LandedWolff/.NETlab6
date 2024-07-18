using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;
using Lab6.Models.Prediction;

namespace Lab6.Pages.Predictions
{
    public class DeleteModel : PageModel
    {
        private readonly Lab6.Data.PredictionDataContext _context;

        public DeleteModel(Lab6.Data.PredictionDataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Prediction Prediction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Predictions == null)
            {
                return NotFound();
            }

            var prediction = await _context.Predictions.FirstOrDefaultAsync(m => m.PredictionId == id);

            if (prediction == null)
            {
                return NotFound();
            }
            else
            {
                Prediction = prediction;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Predictions == null)
            {
                return NotFound();
            }
            var prediction = await _context.Predictions.FindAsync(id);

            if (prediction != null)
            {
                Prediction = prediction;
                _context.Predictions.Remove(Prediction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
