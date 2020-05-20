using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AcornBox.Core
{
    public class CsvReader : IDisposable
    {
        private CsvHelper.CsvReader _reader;

        public int LineNumber => _reader.Context.RawRow;
        public string Line => _reader.Context.RawRecord;
        public int RecordNumber => _reader.Context.Row;
        public string[] Record => _reader.Context.Record;
        public string[] HeaderRecord => _reader.Context.HeaderRecord;

        public CsvReader(TextReader csv, bool leaveOpen = false)
            : this (csv, new CsvReaderConfiguration(), leaveOpen)
        {
        }

        public CsvReader(TextReader csv, CsvReaderConfiguration config, bool leaveOpen = false)
        {
            _reader = new CsvHelper.CsvReader(csv, config.Culture, leaveOpen);

            _reader.Configuration.Delimiter = config.FieldDelimiterCharacter.ToString();
            _reader.Configuration.DetectColumnCountChanges = true;

            _reader.Configuration.HasHeaderRecord = true;

            _reader.Configuration.IgnoreQuotes = config.IgnoreQuotes;
            _reader.Configuration.Quote = config.QuoteCharacter;
            _reader.Configuration.Escape = config.QuoteEscapeCharacter;

            _reader.Configuration.AllowComments = config.AllowComments;
            _reader.Configuration.Comment = config.CommentCharacter;

            _reader.Read();
            _reader.ReadHeader();
        }

        public bool Read()
        {
            if (_reader.Read())
            {
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}
