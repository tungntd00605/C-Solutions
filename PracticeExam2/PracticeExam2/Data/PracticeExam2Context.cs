using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PracticeExam2.Models
{
    public class PracticeExam2Context : DbContext
    {
        public PracticeExam2Context (DbContextOptions<PracticeExam2Context> options)
            : base(options)
        {
        }

        public DbSet<PracticeExam2.Models.Customer> Customer { get; set; }
    }
}
