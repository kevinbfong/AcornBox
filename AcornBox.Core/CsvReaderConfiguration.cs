using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AcornBox.Core
{
    public class CsvReaderConfiguration
    {
        public CultureInfo Culture { get; set; } = System.Threading.Thread.CurrentThread.CurrentCulture;
        public char FieldDelimiterCharacter { get; set; } = ',';
        public bool IgnoreQuotes { get; set; } = false;
        public char QuoteCharacter { get; set; } = '"';
        public char QuoteEscapeCharacter { get; set; } = '"';
        public bool AllowComments { get; set; } = true;
        public char CommentCharacter { get; set; } = '#';

    }
}
