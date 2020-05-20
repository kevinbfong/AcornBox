using System;
using System.Collections.Generic;
using System.Text;

namespace AcornBox.Core
{
    public class CsvSchema
    {
        public List<CsvFieldSchema> Fields { get; set; } = new List<CsvFieldSchema>();
    }
}
