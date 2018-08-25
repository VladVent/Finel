using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using database.DataModel;


namespace Archive1.Models
{
    public class DevView
    {
        public IEnumerable<DevModel> GetDevelopments { get; set; }
        public IEnumerable<PubModel> GetPublishers { get; set; }
        public string Message { get; set; }
    }
}
