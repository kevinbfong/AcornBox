using AcornBox.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Flurl.Http;
using System.IO;
using AcornBox.Core;
using Microsoft.Extensions.Configuration;

namespace AcornBox.Worker
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
    }


    public class GenerateSchema : IGenerateSchema
    {
        private readonly IConfiguration _config;

        public GenerateSchema(IConfiguration config)
        {
            _config = config;
            _endpoint = _config["AcornBoxWebUI"];
        }


        // private readonly string _endpoint = "http://localhost:50548";
        private readonly string _endpoint;// = "http://acornbox.webui";




        void IGenerateSchema.GenerateSchema(Guid id)
        {
            System.Threading.Thread.Sleep(60 * 1000);


            string resourceUrl = _endpoint + $"/api/FileEntry/{id}";

            FileEntryDto fileEntry;

            try
            {
                fileEntry = resourceUrl.GetJsonAsync<FileEntryDto>()
                    .GetAwaiter()
                    .GetResult();
            }
            catch(FlurlHttpException ex)
            {
                throw;
            }


            using FileStream fs = new FileStream(fileEntry.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader sr = new StreamReader(fs, Encoding.Default, true);
            using CsvReader csvReader = new CsvReader(sr);
            CsvSchemaBuilder csvSchemaBuilder = new CsvSchemaBuilder(csvReader.HeaderRecord);
            csvSchemaBuilder.Refine(csvReader);

            CsvSchema csvSchema = csvSchemaBuilder.Build();

            string csvSchemaStr = Newtonsoft.Json.JsonConvert.SerializeObject(csvSchema, Newtonsoft.Json.Formatting.Indented);

            fileEntry.Schema = csvSchemaStr;

            fileEntry.GenerateSchemaWorker = Environment.MachineName;

            try
            {
                resourceUrl.PutJsonAsync(fileEntry).GetAwaiter().GetResult();
            }
            catch(FlurlHttpException ex)
            {
                throw;
            }
        }
    }
}
