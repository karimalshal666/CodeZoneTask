using CodeZoneTask_MVC_.Interfaces;
using CodeZoneTask_MVC_.Models;
using CodeZoneTask_MVC_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeZoneTask_MVC_.Controllers
{
    public class StoreController : Controller
    {
        private readonly IRepository<Store> _storeRepository;

        public StoreController(IRepository<Store> storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<IActionResult> GetAllStores(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var stores = await _storeRepository.GetAllAsync();

            var storesViewModel = stores
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new StoreViewModel { Id = s.Id, Name = s.Name })
                .ToList();

            ViewBag.Page = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(stores.Count / (double)pageSize);

            return View(storesViewModel);
        }

        [HttpGet]
        public IActionResult AddStore()
        {
            var viewModel = new StoreViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStore(StoreViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if store name already exists
                var stores = await _storeRepository.GetAllAsync();
                var existingStore = stores.FirstOrDefault(s => s.Name == viewModel.Name);

                if (existingStore != null)
                {
                    ModelState.AddModelError("Name", "Store name already exists.");
                    return View(viewModel);
                }

                var newStore = new Store { Name = viewModel.Name };
                await _storeRepository.AddAsync(newStore);

                return RedirectToAction(nameof(GetAllStores));
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditStore(int id)
        {
            var store = await _storeRepository.GetByIdAsync(id);
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
                var stores = await _storeRepository.GetAllAsync();
                var existingStore = stores.FirstOrDefault(s => s.Name == viewModel.Name && s.Id != viewModel.Id);

                if (existingStore != null)
                {
                    ModelState.AddModelError("Name", "Store name already exists.");
                    return View(viewModel);
                }

                var store = await _storeRepository.GetByIdAsync(viewModel.Id);
                if (store == null)
                {
                    return NotFound();
                }

                store.Name = viewModel.Name;
                await _storeRepository.UpdateAsync(store);

                return RedirectToAction(nameof(GetAllStores));
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var store = await _storeRepository.GetByIdAsync(id);
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
            var store = await _storeRepository.GetByIdAsync(id);
            if (store != null)
            {
                await _storeRepository.DeleteAsync(store);
            }
            return RedirectToAction(nameof(GetAllStores));
        }
    }
}
