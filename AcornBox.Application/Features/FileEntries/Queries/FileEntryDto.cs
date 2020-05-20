using System;
using System.Collections.Generic;
using System.Text;

namespace AcornBox.Application.Features.FileEntries.Queries
{
    public class FileEntryDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public long Length { get; set; }
        public string Path { get; set; }
        public string Schema { get; set; }
        public string GenerateSchemaJobId { get; set; }
        public string GenerateSchemaWorker { get; set; }

        public static FileEntryDto Map(Domain.Entities.FileEntry fileEntryEntity)
        {
            return new FileEntryDto()
            {
                Id = fileEntryEntity.Id,
                Name = fileEntryEntity.Name,
                Length = fileEntryEntity.Length,
                Path = fileEntryEntity.Path,
                Schema = fileEntryEntity.Schema,
            };
        }

        public static Func<Domain.Entities.FileEntry, FileEntryDto> Project = (fileEntryEntity) => {

            return new FileEntryDto()
            {
                Id = fileEntryEntity.Id,
                Name = fileEntryEntity.Name,
                Length = fileEntryEntity.Length,
                Path = fileEntryEntity.Path,
                Schema = fileEntryEntity.Schema,
                GenerateSchemaJobId = fileEntryEntity.GenerateSchemaJobId,
                GenerateSchemaWorker = fileEntryEntity.GenerateSchemaWorker
            };
        };
    }
}
