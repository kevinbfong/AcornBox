using AcornBox.Application.Common.Interfaces;
using AcornBox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcornBox.Persistence
{
    public class AcornBoxDbContext : DbContext, IAcornBoxDbContext
    {
        public DbSet<FileEntry> FileEntries { get; set; }

        public AcornBoxDbContext(DbContextOptions<AcornBoxDbContext> opts)
            : base(opts)
        {
        }
    }
}
