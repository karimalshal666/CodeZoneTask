// PurchaseController.cs
using AutoMapper;
using CodeZoneTask_MVC_.Interfaces;
using CodeZoneTask_MVC_.Models;
using CodeZoneTask_MVC_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                    _mapper.Map(storeItem, transactionVm); 
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
                        var newStoreItem = _mapper.Map<StoreItem>(transactionVm);
                        await _storeItemRepository.AddAsync(newStoreItem);
                    }
                }

                var redirectParams = _mapper.Map<StockIncreaseViewModel>(transactionVm);
                return RedirectToAction("Transaction", redirectParams);
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
