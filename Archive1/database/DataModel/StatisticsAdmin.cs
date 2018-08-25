using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace database.DataModel
{
    public  class StatisticsAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public StatisticsAdmin()
        {

        }
        public StatisticsAdmin(string _N)
        {
            Name = _N;
        }
    }
}
