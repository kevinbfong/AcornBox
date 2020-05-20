using AcornBox.Application.Common.Interfaces;
using AcornBox.Application.Features.FileEntries.Queries.GetFileEntryById;
using AcornBox.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcornBox.Application.Features.FileEntries.Commands.UpdateFileEntry
{
    public class UpdateFileEntryCommand : IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public long Length { get; set; }
        public string Path { get; set; }
        public string Schema { get; set; }
        public string GenerateSchemaJobId { get; set; }
        public string GenerateSchemaWorker { get; set; }

        public class UpdateFileEntryCommandHandler : IRequestHandler<UpdateFileEntryCommand>
        {
            private readonly IAcornBoxDbContext _dbContext;

            public UpdateFileEntryCommandHandler(IAcornBoxDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(UpdateFileEntryCommand request, CancellationToken cancellationToken)
            {
                FileEntry fileEntry = await _dbContext.FileEntries.FindAsync(new object[] { request.Id.Value }, cancellationToken);

                if (fileEntry == null)
                {
                    throw new Exception();
                }

                fileEntry.Name = request.Name;
                fileEntry.Length = request.Length;
                fileEntry.Path = request.Path;
                fileEntry.Schema = request.Schema;
                fileEntry.GenerateSchemaJobId = request.GenerateSchemaJobId;
                fileEntry.GenerateSchemaWorker = request.GenerateSchemaWorker;

                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
