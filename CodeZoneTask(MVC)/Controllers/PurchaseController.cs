using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeZoneTask_MVC_.Models;
using CodeZoneTask_MVC_.ViewModels;
using System.Linq;

namespace CodeZoneTask_MVC_.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly CodeZoneEntities _context;

        public PurchaseController(CodeZoneEntities context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Transaction(int? storeId, int? itemId)
        {
            var transactionVm = new StockIncreaseViewModel
            {
                Stores = _context.Stores.ToList(),
                Items = _context.Items.ToList()
            };

            if (storeId.HasValue && itemId.HasValue)
            {
                var storeItem = _context.StoreItems
                    .FirstOrDefault(si => si.StoreId == storeId.Value && si.ItemId == itemId.Value);

                if (storeItem != null)
                {
                    transactionVm.StoreId = storeId.Value;
                    transactionVm.ItemId = itemId.Value;
                    transactionVm.Quantity = storeItem.Quantity;
                }
            }

            return View(transactionVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transaction(StockIncreaseViewModel transactionVm)
        {
            if (ModelState.IsValid)
            {
                var storeItem = _context.StoreItems
                    .FirstOrDefault(si => si.StoreId == transactionVm.StoreId && si.ItemId == transactionVm.ItemId);

                if (storeItem != null)
                {
                    storeItem.Quantity += transactionVm.Quantity;
                }
                else
                {
                    _context.StoreItems.Add(new StoreItem { StoreId = transactionVm.StoreId.Value, ItemId = transactionVm.ItemId.Value, Quantity = transactionVm.Quantity });
                }
                _context.SaveChanges();

                return RedirectToAction("Transaction", new { storeId = transactionVm.StoreId, itemId = transactionVm.ItemId });
            }

            transactionVm.Stores = _context.Stores.ToList();
            transactionVm.Items = _context.Items.ToList();
            return View(transactionVm);
        }

        public IActionResult Balance(int storeId, int itemId)
        {
            var storeItem = _context.StoreItems
                .FirstOrDefault(si => si.StoreId == storeId && si.ItemId == itemId);

            if (storeItem != null)
            {
                return Ok(storeItem.Quantity); // Return current balance (quantity) if found
            }

            return Ok(0); // Return 0 if no row found for the store and item
        }
    }
}
