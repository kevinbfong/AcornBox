using System;
using System.Collections.Generic;
using System.Text;

namespace AcornBox.Domain.Entities
{
    public class FileEntry
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public long Length { get; set; }
        public string Path { get; set; }
        public string Schema { get; set; }
        public string GenerateSchemaJobId { get; set; }
        public string GenerateSchemaWorker { get; set; }
    }
}
