using AcornBox.Application.Common.Interfaces;
using AcornBox.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcornBox.Application.Features.FileEntries.Commands.SetFileEntryJobId
{
    public class SetFileEntryJobIdCommand : IRequest
    {
        public Guid? Id { get; set; }
        public string JobId { get; set; }

        public class SetFileEntryJobIdCommandHandler : IRequestHandler<SetFileEntryJobIdCommand>
        {
            private readonly IAcornBoxDbContext _dbContext;

            public SetFileEntryJobIdCommandHandler(IAcornBoxDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(SetFileEntryJobIdCommand request, CancellationToken cancellationToken)
            {
                FileEntry fileEntry = await _dbContext.FileEntries.FindAsync(new object[] { request.Id.Value }, cancellationToken);

                if (fileEntry == null)
                {
                    throw new Exception();
                }

                fileEntry.GenerateSchemaJobId = request.JobId;

                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
