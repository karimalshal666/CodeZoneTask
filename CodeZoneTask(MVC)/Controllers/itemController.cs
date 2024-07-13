using Microsoft.AspNetCore.Mvc;
using CodeZoneTask_MVC_.Models;
using Microsoft.EntityFrameworkCore;
using CodeZoneTask_MVC_.ViewModels;


namespace CodeZoneTask_MVC_.Controllers
{
    public class itemController : Controller
    {
        private readonly CodeZoneEntities _context;

        public itemController(CodeZoneEntities context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetAllItems(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var items = await _context.Items
                .OrderBy(i => i.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new ItemViewModel { Id = i.Id, Name = i.Name })
                .ToListAsync();

            ViewBag.Page = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(await _context.Items.CountAsync() / (double)pageSize);

            return View(items);
        }

        [HttpGet]
        public IActionResult AddItem()
        {
            var viewModel = new ItemViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItem(ItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if item name already exists
                var existingItem = _context.Items.FirstOrDefault(i => i.Name == viewModel.Name);
                if (existingItem != null)
                {
                    ModelState.AddModelError("Name", "Item name already exists.");
                    return View(viewModel);
                }

                var newItem = new Item { Name = viewModel.Name };
                _context.Items.Add(newItem);
                _context.SaveChanges();

                return RedirectToAction(nameof(GetAllItems));
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var viewModel = new ItemViewModel { Id = item.Id, Name = item.Name };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(ItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if the new item name already exists
                var existingItem = await _context.Items
                    .FirstOrDefaultAsync(i => i.Name == viewModel.Name && i.Id != viewModel.Id);

                if (existingItem != null)
                {
                    ModelState.AddModelError("Name", "Item name already exists.");
                    return View(viewModel);
                }

                var item = await _context.Items.FindAsync(viewModel.Id);
                if (item == null)
                {
                    return NotFound();
                }

                item.Name = viewModel.Name;
                _context.Update(item);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(GetAllItems));
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var viewModel = new ItemViewModel { Id = item.Id, Name = item.Name };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(GetAllItems));
        }

    }
}
