using System.Linq;
using Doug.Models;

namespace Doug.Repositories
{
    public interface IRecipeRepository
    {
        Recipe GetRecipeById(string recipeId);
    }

    public class RecipeRepository : IRecipeRepository
    {
        private readonly DougContext _db;

        public RecipeRepository(DougContext db)
        {
            _db = db;
        }

        public Recipe GetRecipeById(string recipeId)
        {
            return _db.Recipes.SingleOrDefault(recipe => recipe.Id == recipeId);
        }
    }
}
