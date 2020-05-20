using AcornBox.Common;
using Hangfire;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcornBox.Application.Features.FileEntries.Commands.UploadFile
{
    public class UploadFileCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public long Length { get; set; }
        public Stream File { get; set; }

        public class UploadFileCommandHander : IRequestHandler<UploadFileCommand, Guid>
        {
            private readonly IBackgroundJobClient _jobClient;
            private readonly IMediator _mediator;
            

            public UploadFileCommandHander(IBackgroundJobClient jobClient, IMediator mediator)
            {
                _jobClient = jobClient;
                _mediator = mediator;
            }

            public async Task<Guid> Handle(UploadFileCommand request, CancellationToken cancellationToken)
            {
                string path = "/mnt/share";

                string filePath = Path.Combine(path, request.Name);

                using FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
                fs.SetLength(request.Length);
                fs.Seek(0, SeekOrigin.Begin);
                await request.File.CopyToAsync(fs);

                await fs.FlushAsync();
                fs.Close();
                fs.Dispose();

                Guid id = await _mediator.Send(new CreateFileEntry.CreateFileEntryCommand()
                {
                    Name = request.Name,
                    Length = request.Length,
                    Path = filePath,
                });

                string jobId = _jobClient.Enqueue<IGenerateSchema>(x => x.GenerateSchema(id));                

                await _mediator.Send(new SetFileEntryJobId.SetFileEntryJobIdCommand()
                { 
                    Id = id,
                    JobId = jobId
                });

                return id;
            }
        }
    }
}
