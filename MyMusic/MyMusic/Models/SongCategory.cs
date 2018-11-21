using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.Models
{
    public class SongCategory
    {
        public string SongId { get; set; }
        public string CategoryId { get; set; }
        public Song Song { get; set; }
        public Category Category { get; set; } 
    }
}
