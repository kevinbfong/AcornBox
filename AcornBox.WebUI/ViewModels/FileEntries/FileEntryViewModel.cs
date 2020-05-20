using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcornBox.WebUI.ViewModels.FileEntries
{
    public class FileEntryViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public long Length { get; set; }
        public string Path { get; set; }
        public string Schema { get; set; }
        public string JobId { get; set; }
        public string WorkerName { get; set; }
    }
}
