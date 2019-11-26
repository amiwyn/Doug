using System.Collections.Generic;
using System.Linq;
using Doug.Models;
using Doug.Models.User;
using Doug.Repositories;

namespace Doug.Services
{
    public interface ICraftingService
    {
        DougResponse Craft(List<InventoryItem> items, User user);
    }

    public class CraftingService : ICraftingService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public CraftingService(IRecipeRepository recipeRepository, IItemRepository itemRepository, IInventoryRepository inventoryRepository)
        {
            _recipeRepository = recipeRepository;
            _itemRepository = itemRepository;
            _inventoryRepository = inventoryRepository;
        }

        public DougResponse Craft(List<InventoryItem> items, User user)
        {
            var recipeId = items.OrderBy(item => item.ItemId).Aggregate(string.Empty, (acc, elem) => acc + elem.ItemId);
            var recipe = _recipeRepository.GetRecipeById(recipeId);

            if (recipe == null)
            {
                return new DougResponse(DougMessages.FailedCrafting);
            }

            var result = _itemRepository.GetItem(recipe.Result);

            _inventoryRepository.RemoveItems(user, items.Select(item => item.InventoryPosition));

            _inventoryRepository.AddItem(user, result);

            return new DougResponse(string.Format(DougMessages.SuccessCrafting, result.GetDisplayName()));
        }
    }
}
