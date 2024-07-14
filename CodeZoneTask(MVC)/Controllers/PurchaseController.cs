using AutoMapper;
using CodeZoneTask_MVC_.Interfaces;
using CodeZoneTask_MVC_.Models;
using CodeZoneTask_MVC_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CodeZoneTask_MVC_.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IStoreItemRepository _storeItemRepository;
        private readonly IMapper _mapper;

        public PurchaseController(IStoreRepository storeRepository, IItemRepository itemRepository, IStoreItemRepository storeItemRepository, IMapper mapper)
        {
            _storeRepository = storeRepository;
            _itemRepository = itemRepository;
            _storeItemRepository = storeItemRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Transaction(int? storeId, int? itemId, string transactionType)
        {
            var transactionVm = new StockIncreaseViewModel
            {
                Stores = await _storeRepository.GetAllAsync(),
                TransactionType = transactionType
            };

            var items = await _itemRepository.GetAllAsync();
            transactionVm.Items = _mapper.Map<List<ItemViewModel>>(items); 

            if (storeId.HasValue && itemId.HasValue)
            {
                var storeItem = await _storeItemRepository.GetByStoreAndItemAsync(storeId.Value, itemId.Value);

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
        public async Task<IActionResult> Transaction(StockIncreaseViewModel transactionVm)
        {
            if (ModelState.IsValid)
            {
                var storeItem = await _storeItemRepository.GetByStoreAndItemAsync(transactionVm.StoreId.Value, transactionVm.ItemId.Value);

                if (storeItem != null)
                {
                    if (transactionVm.TransactionType == "purchase")
                    {
                        storeItem.Quantity += transactionVm.Quantity;
                    }
                    else if (transactionVm.TransactionType == "sell")
                    {
                        // Check if selling quantity exceeds current balance
                        if (transactionVm.Quantity > storeItem.Quantity)
                        {
                            ModelState.AddModelError("Quantity", "Selling quantity cannot exceed current balance.");
                            transactionVm.Stores = await _storeRepository.GetAllAsync();
                            transactionVm.Items = _mapper.Map<List<ItemViewModel>>(await _itemRepository.GetAllAsync());
                            return View(transactionVm);
                        }

                        storeItem.Quantity -= transactionVm.Quantity;
                    }

                    await _storeItemRepository.UpdateAsync(storeItem);
                }
                else
                {
                    if (transactionVm.TransactionType == "purchase")
                    {
                        await _storeItemRepository.AddAsync(new StoreItem { StoreId = transactionVm.StoreId.Value, ItemId = transactionVm.ItemId.Value, Quantity = transactionVm.Quantity });
                    }
                }

                return RedirectToAction("Transaction", new { storeId = transactionVm.StoreId, itemId = transactionVm.ItemId, transactionType = transactionVm.TransactionType });
            }

            transactionVm.Stores = await _storeRepository.GetAllAsync();
            transactionVm.Items = _mapper.Map<List<ItemViewModel>>(await _itemRepository.GetAllAsync());
            return View(transactionVm);
        }

        public async Task<IActionResult> Balance(int storeId, int itemId)
        {
            var storeItem = await _storeItemRepository.GetByStoreAndItemAsync(storeId, itemId);

            if (storeItem != null)
            {
                return Ok(storeItem.Quantity);
            }

            return Ok(0);
        }
    }
}
