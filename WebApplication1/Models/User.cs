namespace WebApplication1.Models
{
    /// <summary>
    /// CREATE TABLE "User" (
    ///     "Id"	INTEGER,
    ///     "Fullname"	TEXT,
    ///     PRIMARY KEY("Id" AUTOINCREMENT)
    /// );
    /// </summary>
    public class User : BaseModel
    {
        public string? Fullname { get; set; }
    }
}
