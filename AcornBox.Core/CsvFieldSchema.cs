using System;
using System.Collections.Generic;
using System.Text;

namespace AcornBox.Core
{
    public class CsvFieldSchema
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string DataType { get; set; }
        public bool Nullable { get; set; }
    }
}
