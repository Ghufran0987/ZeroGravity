using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Proxies.MealIngredientsProxy;

namespace ZeroGravity.Mobile.Contract.Helper
{
    public static class MealsDataIngredientsHelper
    {
        public static MealDataProxy GatherIngredients(MealDataProxy meal, bool hasIngredientGrains, bool hasIngredientVegetables, bool hasIngredientFruits, bool hasIngredientDairy, bool hasIngredientProtein)
        {
            meal.Ingredients.Clear();
            if (hasIngredientGrains)
            {
                meal.Ingredients.Add(new GrainsProxy());
            }
            if (hasIngredientVegetables)
            {
                meal.Ingredients.Add(new VegetablesProxy());
            }
            if (hasIngredientFruits)
            {
                meal.Ingredients.Add(new FruitsProxy());
            }
            if (hasIngredientDairy)
            {
                meal.Ingredients.Add(new DairyProxy());
            }
            if (hasIngredientProtein)
            {
                meal.Ingredients.Add(new ProteinProxy());
            }

            return meal;
        }
    }
}
