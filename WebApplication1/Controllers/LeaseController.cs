using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class LeaseController : Controller
    {
        private readonly IModelService _modelService;

        public LeaseController(IModelService modelService)
        {
            _modelService = modelService ??
                throw new ArgumentNullException(nameof(modelService));
        }

        // GET: LeaseController
        public async Task<IActionResult> Index()
        {
            var index = await _modelService.Index<Lease>();
            foreach (var model in index)
            {
                await ApplyLeaseeName(model);
            }
            return View(index);
        }

        // GET: LeaseController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var model = await _modelService.Show<Lease>((int)id);
            await ApplyLeaseeName(model);
            return View(model);
        }

        // GET: LeaseController/Create
        public async Task<IActionResult> Create()
        {
            await LoadLeaseeLookup();
            var model = await _modelService.Create<Lease>();
            return View(model);
        }

        // POST: LeaseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            var model = new Lease();
            await TryUpdateModelAsync(model);
            var stored = await _modelService.Store(model);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(stored);
            }
        }

        // GET: LeaseController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            await LoadLeaseeLookup();
            if (id == null) return NotFound();
            var model = await _modelService.Edit<Lease>((int)id);
            return View(model);
        }

        // POST: LeaseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, IFormCollection collection)
        {
            if (id == null) return NotFound();
            var model = new Lease();
            await TryUpdateModelAsync(model);
            await _modelService.Update(model);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: LeaseController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var model = await _modelService.Show<Lease>((int)id);
            ApplyLeaseeName(model);
            return View(model);
        }

        // POST: LeaseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection collection)
        {
            if (id == null) return NotFound();
            await _modelService.Destroy<Lease>((int)id);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task LoadLeaseeLookup()
        {
            if (ViewBag.LeaseeLookup != null) return;

            var users = await _modelService.Index<User>();
            IEnumerable<SelectListItem> items = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Fullname
            });
            ViewBag.LeaseeLookup = items;
        }

        private async Task ApplyLeaseeName(Lease model)
        {
            await LoadLeaseeLookup();
            model.LeaseeName = ((IEnumerable<SelectListItem>)
                ViewBag.LeaseeLookup).FirstOrDefault(
                u => u.Value == model.LeaseeId?.ToString())?.Text;
        }

    }
}
