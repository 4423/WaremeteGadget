using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaremeteGadget
{
    class FgFileReader
    {
        private string filePath;
        CsvContext context;
        CsvFileDescription description;


        public FgFileReader(string filePath)
        {
            this.filePath = filePath;

            context = new CsvContext();
            description = new CsvFileDescription
            {
                SeparatorChar = '\t',
                FirstLineHasColumnNames = false,    //TODO ヘッダの末尾に\tがあるファイルがあるのでtrueにすると落ちる
                EnforceCsvColumnAttribute = true,
                TextEncoding = Encoding.GetEncoding(932)
            };
        }


        /// <summary>
        /// 全ての行を読み込み、ImageDataFile のプロパティにデータを格納します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ImageDataFile> ReadAll()
        {
            return
                from data in context.Read<ImageDataFile>(this.filePath, description).Skip(2)
                select data;
        }


        public IEnumerable<ImageDataFile> GetGroupLayers(string groupLayerId)
        {
            return
                from data in context.Read<ImageDataFile>(this.filePath, description).Skip(2)
                where data.GroupLayerId == groupLayerId
                select data;
        }

    }
}
