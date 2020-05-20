using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AcornBox.Application.Features.FileEntries.Commands.CreateFileEntry;
using AcornBox.Application.Features.FileEntries.Commands.UpdateFileEntry;
using AcornBox.Application.Features.FileEntries.Queries;
using AcornBox.Application.Features.FileEntries.Queries.GetFileEntries;
using AcornBox.Application.Features.FileEntries.Queries.GetFileEntryById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcornBox.WebUI.ApiControllers
{
    public class FileEntryController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FileEntryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetFileEntriesQuery()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FileEntryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetFileEntryByIdQuery() { Id = id}));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] CreateFileEntryCommand createFileEntryCmd)
        {
            Guid id = await Mediator.Send(createFileEntryCmd);

            return CreatedAtAction("Get", "FileEntry", new { id }, "todo");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateFileEntryCommand updateFileEntryCmd)
        {            
            await Mediator.Send(updateFileEntryCmd);

            return NoContent();
        }
    }
}