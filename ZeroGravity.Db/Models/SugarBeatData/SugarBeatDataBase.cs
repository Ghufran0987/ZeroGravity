namespace ZeroGravity.Db.Models.SugarBeatData
{
    public abstract class SugarBeatDataBase : ModelBase
    {
        public int GlucoseDataId { get; set; }    // Fremdschlüssel auf GlucoseData.Id über Namenskonvention
        public GlucoseData GlucoseData { get; set; } // One-To-Many relationship between GlucoseData and SugarBeatData

        // GlucoseData is the principal entity 
        // SugarBeatData is the dependent entity

        public double? Amount { get; set; }
    }
}
