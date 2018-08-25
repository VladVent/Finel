using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using database.DataModel;

namespace Archive1.Models
{
    public class LGModels
    {
        
        [Required]
        public string GameName { get; set; }
        public LGModels(string _courseName)
        {
     
            GameName = _courseName;
        }
    }
}
