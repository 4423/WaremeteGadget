using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;

namespace WaremeteGadget
{
    class CharDataFile
    {
        [CsvColumn(Name = "# キャラクタ名", FieldIndex = 1)]
        public string CharName { get; set; }
        [CsvColumn(Name = "ポーズ名", FieldIndex = 2)]
        public string PoseName { get; set; }
        [CsvColumn(Name = "立ち絵ポーズの基本ファイル名", FieldIndex = 3)]
        public string PoseFile { get; set; }
        [CsvColumn(Name = "xoffet", FieldIndex = 4)]
        public int Offset_X { get; set; }
        [CsvColumn(Name = "yoffset", FieldIndex = 5)]
        public int Offset_Y { get; set; }
        [CsvColumn(Name = "facezoom", FieldIndex = 6)]
        public int FaceZoom { get; set; }
        [CsvColumn(Name = "facexoff", FieldIndex = 7)]
        public int FaceOff_X { get; set; }
        [CsvColumn(Name = "faceyoff", FieldIndex = 8)]
        public int FaceOff_Y { get; set; }
        [CsvColumn(Name = "emorev", FieldIndex = 9)]
        public int Emorev { get; set; }
    }

}
