using System;
using System.Collections.Generic;
using System.Text;

namespace AcornBox.Core
{
    public class CsvFieldSchemaBuilder
    {
        private readonly string _fieldName;
        private readonly int _fieldNumber;
        private bool _fieldNullable;
        private DateType _fieldDataType;

        public CsvFieldSchemaBuilder(string fieldName, int fieldNumber)
        {
            if (fieldName == null) throw new ArgumentNullException(
                nameof(fieldName), "Cannot be null.");
            
            if (string.IsNullOrWhiteSpace(fieldName)) throw new ArgumentOutOfRangeException(
                nameof(fieldName), "Cannot be null, empty, or whitespace.");
            
            if (fieldNumber < 0) throw new ArgumentOutOfRangeException(
                nameof(fieldNumber), "Cannot be less than zero.");

            _fieldName = fieldName;
            _fieldNumber = fieldNumber;
            _fieldDataType = DateType.Unknown;
            _fieldNullable = false;
        }

        public CsvFieldSchema Build()
        {
            return new CsvFieldSchema()
            {
                Name = _fieldName,
                Number = _fieldNumber,
                DataType = _fieldDataType.ToString(),
                Nullable = _fieldNullable
            };
        }

        public void Refine(string sampleField)
        {
            if (string.IsNullOrEmpty(sampleField))
            {
                _fieldNullable = true;
            }
            else if (_fieldDataType != DateType.String)
            {
                if ((_fieldDataType == DateType.Unknown || _fieldDataType == DateType.Int64) &&
                    long.TryParse(sampleField, out long _))
                {
                    _fieldDataType = DateType.Int64;
                }
                else if ((_fieldDataType == DateType.Unknown || _fieldDataType == DateType.Decimal || 
                    _fieldDataType == DateType.Int64) && decimal.TryParse(sampleField, out decimal _))
                {
                    // todo: double check condition, on second glance it doesn't work look like it'll work.
                    _fieldDataType = DateType.Decimal;
                }
                else if ((_fieldDataType == DateType.Unknown || _fieldDataType == DateType.DateTimeOffset) &&
                    DateTimeOffset.TryParse(sampleField, out DateTimeOffset _))
                {
                    _fieldDataType = DateType.DateTimeOffset;
                }
                else if ((_fieldDataType == DateType.Unknown || _fieldDataType == DateType.Boolean) &&
                    bool.TryParse(sampleField, out bool _))
                {
                    _fieldDataType = DateType.Boolean;
                }
                else
                {
                    _fieldDataType = DateType.String;
                }
            }
        }
    }
}
