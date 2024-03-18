using CsvHelper.Configuration;
using CsvHelper;
using System.Formats.Asn1;
using System.Globalization;
using Microsoft.AspNetCore.Authentication;

namespace AksiaAssessment2024.Helpers
{
    public class CsvParser
    {
        public IEnumerable<T> Parse<T>(Stream csvStream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<T>();
                return records.ToList();
            }
        }
    }
}
