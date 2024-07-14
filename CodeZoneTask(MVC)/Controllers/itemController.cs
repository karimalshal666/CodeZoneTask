using AutoMapper;
using CodeZoneTask_MVC_.Interfaces;
using CodeZoneTask_MVC_.Models;
using CodeZoneTask_MVC_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeZoneTask_MVC_.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemController(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAllItems(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var items = await _itemRepository.GetAllAsync();

            var itemsViewModel = items
                .OrderBy(i => i.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(i => _mapper.Map<ItemViewModel>(i))
                .ToList();

            var totalItemsCount = items.Count; 
            var totalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);

            ViewBag.Page = pageNumber;
            ViewBag.TotalPages = totalPages;

            return View(itemsViewModel);
        }

        [HttpGet]
        public IActionResult AddItem()
        {
            var viewModel = new ItemViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(ItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Trim whitespace from item name
                viewModel.Name = viewModel.Name?.Trim();

                if (string.IsNullOrWhiteSpace(viewModel.Name))
                {
                    ModelState.AddModelError("Name", "Item name cannot be empty.");
                    return View(viewModel);
                }

                // Check if item name already exists (after trimming whitespace)
                var existingItem = await _itemRepository.GetByNameAsync(viewModel.Name);
                if (existingItem != null)
                {
                    ModelState.AddModelError("Name", "Item name already exists.");
                    return View(viewModel);
                }

                var newItem = _mapper.Map<Item>(viewModel);
                await _itemRepository.AddAsync(newItem);

                return RedirectToAction(nameof(GetAllItems));
            }

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> EditItem(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<ItemViewModel>(item);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(ItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Trim whitespace from item name
                viewModel.Name = viewModel.Name?.Trim();

                if (string.IsNullOrWhiteSpace(viewModel.Name))
                {
                    ModelState.AddModelError("Name", "Item name cannot be empty.");
                    return View(viewModel);
                }

                // Check if the new item name already exists (after trimming whitespace)
                var existingItem = await _itemRepository.GetByNameAsync(viewModel.Name);

                // Ensure we don't find the same item when only trimming whitespace
                if (existingItem != null && existingItem.Id != viewModel.Id)
                {
                    ModelState.AddModelError("Name", "Item name already exists.");
                    return View(viewModel);
                }

                var itemToUpdate = await _itemRepository.GetByIdAsync(viewModel.Id);
                if (itemToUpdate == null)
                {
                    return NotFound();
                }

                _mapper.Map(viewModel, itemToUpdate);

                await _itemRepository.UpdateAsync(itemToUpdate);

                return RedirectToAction(nameof(GetAllItems));
            }

            return View(viewModel);
        }




        [HttpGet]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<ItemViewModel>(item);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item != null)
            {
                await _itemRepository.DeleteAsync(item);
            }
            return RedirectToAction(nameof(GetAllItems));
        }
    }
}
