using Data.Entities.Base;

namespace Data.Entities;

public class User : BaseEntity<int>
{
    public User()
    {
        Documents = new HashSet<Document>();
    }

    public required string Name { get; set; }
    public required string Password { get; set; }
    public ICollection<Document> Documents { get; set; }
}