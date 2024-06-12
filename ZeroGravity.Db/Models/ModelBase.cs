namespace ZeroGravity.Db.Models
{
    public class ModelBase
    {
        public int Id { get; set; }

        public ModelBase()
        {
        }

        public ModelBase(int Id)
        {
            this.Id = Id;
        }
    }
}