using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.DatabaseModel.Models;

namespace StackUnderflow.EF
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {

        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Reply> Replies { get; set; }
        public DbSet<QuestionModel> QuestionModel { get; set; }

    }
}