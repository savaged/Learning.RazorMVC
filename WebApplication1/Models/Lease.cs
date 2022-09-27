namespace WebApplication1.Models
{
    /// <summary>
    /// CREATE TABLE "Lease" (
    ///     "Id"    INTEGER NOT NULL,
    ///     "Start" TEXT,
    ///     "End"   TEXT,
    ///     PRIMARY KEY("Id" AUTOINCREMENT)
    /// );
    /// </summary>
    public class Lease : BaseModel
    {
        public Lease()
        {
            Start = DateTime.Now;
            End = ((DateTime)Start).AddDays(28);
        }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? LeaseeId { get; set; }
    }
}
