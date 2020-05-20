using AcornBox.Application.Common.Interfaces;
using AcornBox.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcornBox.Application.Features.FileEntries.Queries.GetFileEntryById
{
    public class GetFileEntryByIdQuery : IRequest<FileEntryDto>
    {
        public Guid? Id { get; set; }

        public class GetFileEntryByIdQueryHandler : IRequestHandler<GetFileEntryByIdQuery, FileEntryDto>
        {
            private readonly IAcornBoxDbContext _dbContext;

            public GetFileEntryByIdQueryHandler(IAcornBoxDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<FileEntryDto> Handle(GetFileEntryByIdQuery request, CancellationToken cancellationToken)
            {
                FileEntry fileEntry = await _dbContext.FileEntries.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

                if (fileEntry == null)
                {
                    throw new Exception("");
                }

                FileEntryDto fileEntryDto = FileEntryDto.Project(fileEntry);

                return fileEntryDto;
            }
        }
    }
}
