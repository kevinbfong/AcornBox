using AcornBox.Application.Common.Interfaces;
using AcornBox.Domain.Entities;
using Hangfire;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcornBox.Application.Features.FileEntries.Commands.DeleteFileEntry
{
    public class DeleteFileEntryCommand : IRequest
    {
        public Guid? Id { get; set; }

        public class DeleteFileEntryCommandHandler : IRequestHandler<DeleteFileEntryCommand>
        {
            private readonly IBackgroundJobClient _jobClient;
            private readonly IAcornBoxDbContext _dbContext;

            public DeleteFileEntryCommandHandler(IBackgroundJobClient jobClient, IAcornBoxDbContext dbContext)
            {
                _jobClient = jobClient;
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(DeleteFileEntryCommand request, CancellationToken cancellationToken)
            {
                FileEntry fileEntry = await _dbContext.FileEntries.FindAsync(new object[] { request.Id.Value }, cancellationToken);

                if (fileEntry == null)
                {
                    throw new Exception();
                }

                FileInfo fi = new FileInfo(fileEntry.Path);
                string path = "/mnt/share";

                if (fi.Exists && fi.Directory.FullName.Equals(path))
                {
                    fi.Delete();
                }

                _dbContext.FileEntries.Remove(fileEntry);

                await _dbContext.SaveChangesAsync(cancellationToken);

                _jobClient.Delete(fileEntry.GenerateSchemaJobId);

                return Unit.Value;
            }
        }
    }
}
