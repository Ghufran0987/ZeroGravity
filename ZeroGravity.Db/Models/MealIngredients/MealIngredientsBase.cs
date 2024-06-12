namespace ZeroGravity.Db.Models.MealIngredients
{
    public abstract class MealIngredientsBase : ModelBase
    {
        public int MealDataId { get; set; }    // Fremdschlüssel auf MealData.Id über Namenskonvention
        public MealData MealData { get; set; } // One-To-Many relationship between MealData and MealIngredientsBase

        // MealData is the principal entity 
        // MealIngredientsBase is the dependent entity



        public int? Amount { get; set; }      // falls später eine Menge mitgespeichert werden soll
    }
}
