using Dapper.Contrib.Extensions;

namespace WebApplication1.Models
{
    public abstract class BaseModel : IModel
    {
        [Computed]
        public int Id { get; set; }
    }
}
