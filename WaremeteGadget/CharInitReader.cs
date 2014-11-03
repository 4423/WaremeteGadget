using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaremeteGadget
{
    class CharInitReader
    {
        private string filePath;
        CsvContext context;
        CsvFileDescription description;

        public CharInitReader(string filePath)
        {
            this.filePath = filePath;

            context = new CsvContext();
            description = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                TextEncoding = Encoding.GetEncoding(932)
            };
        }


        public IEnumerable<CharDataFile> ReadAll()
        {
            return
                from data in context.Read<CharDataFile>(filePath, description).Skip(1)
                select data;
        }
    }
}
