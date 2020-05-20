using AcornBox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcornBox.Application.Common.Interfaces
{
    public interface IAcornBoxDbContext
    {
        DbSet<FileEntry> FileEntries { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
