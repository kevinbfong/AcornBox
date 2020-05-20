using AcornBox.Application.Common.Interfaces;
using AcornBox.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcornBox.Application.Features.FileEntries.Commands.CreateFileEntry
{
    public class CreateFileEntryCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public long Length { get; set; }
        public string Path { get; set; }
        public string GenerateSchemaJobId { get; set; }

        public class CreateFileEntryCommandHandler : IRequestHandler<CreateFileEntryCommand, Guid>
        {
            private readonly IAcornBoxDbContext _dbContext;

            public CreateFileEntryCommandHandler(IAcornBoxDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Guid> Handle(CreateFileEntryCommand request, CancellationToken cancellationToken)
            {
                FileEntry fileEntry = new FileEntry()
                {
                    Name = request.Name,
                    Length = request.Length,
                    Path = request.Path,
                    GenerateSchemaJobId = request.GenerateSchemaJobId,
                };

                await _dbContext.FileEntries.AddAsync(fileEntry);
                await _dbContext.SaveChangesAsync(cancellationToken);

                Guid id = fileEntry.Id.Value; // todo

                return id;
            }
        }
    }
}
