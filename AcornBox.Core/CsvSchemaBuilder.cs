using System;
using System.Collections.Generic;
using System.Text;

namespace AcornBox.Core
{
    public class CsvSchemaBuilder
    {
        private readonly CsvFieldSchemaBuilder[] _fieldSchemaBuilders;

        public CsvSchemaBuilder(string[] headerRecord)
        {
            if (headerRecord == null) throw new ArgumentNullException(nameof(headerRecord), "Cannot be null.");
            if (headerRecord.Length == 0) throw new ArgumentOutOfRangeException(nameof(headerRecord), "Cannot have length zero.");

            _fieldSchemaBuilders = new CsvFieldSchemaBuilder[headerRecord.Length];

            for(int i = 0; i < headerRecord.Length; i++)
            {
                _fieldSchemaBuilders[i] = new CsvFieldSchemaBuilder(headerRecord[i], i);
            }
        }

        public CsvSchema Build()
        {
            CsvSchema schema = new CsvSchema();
            foreach(CsvFieldSchemaBuilder fieldSchemaBuilder in _fieldSchemaBuilders)
            {
                CsvFieldSchema fieldSchema = fieldSchemaBuilder.Build();
                schema.Fields.Add(fieldSchema);
            }

            return schema;
        }

        public void Refine(string[] sampleRecord)
        {
            for(int i = 0; i < _fieldSchemaBuilders.Length; i++)
            {
                _fieldSchemaBuilders[i].Refine(sampleRecord[i]);
            }
        }

        public void Refine(CsvReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader), "Cannot be null.");

            while (reader.Read())
            {
                Refine(reader.Record);
            }
        }
    }
}
