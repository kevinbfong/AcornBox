using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using AcornBox.Application.Features.FileEntries.Commands.DeleteFileEntry;
using AcornBox.Application.Features.FileEntries.Commands.UploadFile;
using AcornBox.Application.Features.FileEntries.Queries;
using AcornBox.Application.Features.FileEntries.Queries.GetFileEntries;
using AcornBox.Application.Features.FileEntries.Queries.GetFileEntryById;
using AcornBox.WebUI.ViewModels.FileEntries;

using MediatR;

namespace AcornBox.WebUI.Controllers
{
    public class FileEntriesController : Controller
    {
        private readonly ILogger<FileEntriesController> _logger;
        private readonly IMediator _mediator;

        public FileEntriesController(ILogger<FileEntriesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        private static readonly Func<FileEntryDto, FileEntryViewModel> MapDtoToVm = (fileEntryDto) => new FileEntryViewModel()
        {
            Id = fileEntryDto.Id,
            Name = fileEntryDto.Name,
            Length = fileEntryDto.Length,
            Path = fileEntryDto.Path,
            Schema = fileEntryDto.Schema,
            JobId = fileEntryDto.GenerateSchemaJobId,
            WorkerName = fileEntryDto.GenerateSchemaWorker,
        };

        [HttpGet]
        public async Task<IActionResult> Index()
        {           
            IEnumerable<FileEntryDto> fileEntryDtos = await  _mediator.Send(new GetFileEntriesQuery());

            IEnumerable<FileEntryViewModel> fileEntryVms = fileEntryDtos
                .Select(MapDtoToVm)
                .AsEnumerable();

            return View(fileEntryVms);
        }        

        [HttpGet]
        public async Task<IActionResult> Schema([FromRoute] Guid id)
        {
            FileEntryDto fileEntryDto = await _mediator.Send(new GetFileEntryByIdQuery() { Id = id });

            if (fileEntryDto == null)
            {
                return NotFound();
            }

            FileEntryViewModel fileEntryVm = MapDtoToVm(fileEntryDto);

            return View(fileEntryVm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            FileEntryDto fileEntryDto = await _mediator.Send(new GetFileEntryByIdQuery() { Id = id });

            if (fileEntryDto == null)
            {
                return NotFound();
            }

            FileEntryViewModel fileEntryVm = MapDtoToVm(fileEntryDto);

            return View(fileEntryVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid id, int? dud)
        {
            await _mediator.Send(new DeleteFileEntryCommand() { Id = id });
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]IFormFile file)
        {
            string fileName = file.FileName;
            long fileLength = file.Length;

            Stream stream = file.OpenReadStream();
            

            Guid id = await _mediator.Send(
                new UploadFileCommand()
                {
                    Name = fileName,
                    Length = fileLength,
                    File = stream
                });

            return RedirectToAction("Index");
        }
    }
}