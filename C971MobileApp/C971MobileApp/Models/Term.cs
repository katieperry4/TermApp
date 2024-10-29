using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971MobileApp.Models
{
    [Table("term")]
    public class Term
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int TermId {  get; set; }

        [Column("termName")]
        public string TermName { get; set; }

        [Column("termStart")]
        public DateTime TermStart { get; set; }

        [Column("termEnd")]
        public DateTime TermEnd { get; set; }

    }
}
