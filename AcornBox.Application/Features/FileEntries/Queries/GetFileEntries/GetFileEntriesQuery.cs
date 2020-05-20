using AcornBox.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcornBox.Application.Features.FileEntries.Queries.GetFileEntries
{
    public class GetFileEntriesQuery : IRequest<IEnumerable<FileEntryDto>>
    {

        public class GetFileEntriesQueryHandler : IRequestHandler<GetFileEntriesQuery, IEnumerable<FileEntryDto>>
        {
            private readonly IAcornBoxDbContext _dbContext;

            public GetFileEntriesQueryHandler(IAcornBoxDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<IEnumerable<FileEntryDto>> Handle(GetFileEntriesQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<FileEntryDto> fileEntries = _dbContext.FileEntries
                    .Select(FileEntryDto.Project)
                    .AsEnumerable();

                return Task.FromResult(fileEntries);
            }
        }
    }
}
