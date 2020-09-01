using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medicinas.Models;

namespace Medicinas.Controllers
{
    public class MedicamentosController : Controller
    {
        private readonly farmaciaContext _context;

        public MedicamentosController(farmaciaContext context)
        {
            _context = context;
        }

        // GET: Medicamentos
        public async Task<IActionResult> Index()
        {
            var farmaciaContext = _context.Medicamentos.Include(m => m.IdUsuarioNavigation).OrderBy(m => m.Caducidad);
            return View(await farmaciaContext.ToListAsync());
        }

        // GET: Medicamentos/Create
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Medicamentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Presentacion,ContenidoActual,FrecuenciaDeTomaHr,CantidadDeToma,Caducidad,IdUsuario")] Medicamentos medicamentos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicamentos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUsuario"] = new SelectList(_context.AspNetUsers, "Id", "Id", medicamentos.IdUsuario);
            return View(medicamentos);
        }

        // GET: Medicamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicamentos = await _context.Medicamentos.FindAsync(id);
            if (medicamentos == null)
            {
                return NotFound();
            }
            ViewData["IdUsuario"] = new SelectList(_context.AspNetUsers, "Id", "Id", medicamentos.IdUsuario);
            return View(medicamentos);
        }

        // POST: Medicamentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Presentacion,ContenidoActual,FrecuenciaDeTomaHr,CantidadDeToma,Caducidad,IdUsuario")] Medicamentos medicamentos)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);

            medicamentos.ContenidoActual = medicamento.ContenidoActual - medicamentos.CantidadDeToma;
            if(medicamentos.ContenidoActual < 0)
            {
                medicamento.ContenidoActual = 0;
            }

            medicamentos = medicamento;

            if (id != medicamentos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicamentos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentosExists(medicamentos.Id))
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
            ViewData["IdUsuario"] = new SelectList(_context.AspNetUsers, "Id", "Id", medicamentos.IdUsuario);
            return View(medicamentos);
        }

        private bool MedicamentosExists(int id)
        {
            return _context.Medicamentos.Any(e => e.Id == id);
        }
    }
}
