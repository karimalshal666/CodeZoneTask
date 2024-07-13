using CodeZoneTask_MVC_.Models;
using CodeZoneTask_MVC_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CodeZoneTask_MVC_.Controllers
{
    public class StoreController : Controller
    {
        private readonly CodeZoneEntities _context;

        public StoreController(CodeZoneEntities context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetAllStores(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var stores = await _context.Stores
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new StoreViewModel { Id = s.Id, Name = s.Name })
                .ToListAsync();

            ViewBag.Page = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(await _context.Stores.CountAsync() / (double)pageSize);

            return View(stores);
        }

        [HttpGet]
        public IActionResult AddStore()
        {
            var viewModel = new StoreViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStore(StoreViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if store name already exists
                var existingStore = _context.Stores.FirstOrDefault(s => s.Name == viewModel.Name);
                if (existingStore != null)
                {
                    ModelState.AddModelError("Name", "Store name already exists.");
                    return View(viewModel);
                }

                var newStore = new Store { Name = viewModel.Name };
                _context.Stores.Add(newStore);
                _context.SaveChanges();

                return RedirectToAction(nameof(GetAllStores));
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            var viewModel = new StoreViewModel { Id = store.Id, Name = store.Name };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStore(StoreViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if the new store name already exists
                var existingStore = await _context.Stores
                    .FirstOrDefaultAsync(s => s.Name == viewModel.Name && s.Id != viewModel.Id);

                if (existingStore != null)
                {
                    ModelState.AddModelError("Name", "Store name already exists.");
                    return View(viewModel);
                }

                var store = await _context.Stores.FindAsync(viewModel.Id);
                if (store == null)
                {
                    return NotFound();
                }

                store.Name = viewModel.Name;
                _context.Update(store);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(GetAllStores));
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            var viewModel = new StoreViewModel { Id = store.Id, Name = store.Name };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store != null)
            {
                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(GetAllStores));
        }

    }
}