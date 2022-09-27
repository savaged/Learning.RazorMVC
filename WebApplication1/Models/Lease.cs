using Dapper.Contrib.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Start { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? End { get; set; }

        public int? LeaseeId { get; set; }

        [Computed]
        [DisplayName("Leasee")]
        public string? LeaseeName { get; set; }
    }
}
