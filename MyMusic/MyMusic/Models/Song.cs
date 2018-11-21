using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.Models
{
    public class Song
    {        
        public string Id { get; set; }    
        public string Name { get; set; }
        public string Singer { get; set; }
        public string Author { get; set; }
        public List<SongCategory> SongCategory { get; set; }
    }
}
