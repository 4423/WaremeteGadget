using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaremeteGadget
{
    class ImageDataFile
    {
        [CsvColumn(Name = "#layer_type", FieldIndex = 1)]
        public string LayerType { get; set; }
        [CsvColumn(Name = "name", FieldIndex = 2)]
        public string Name { get; set; }
        [CsvColumn(Name = "left", FieldIndex = 3)]
        public string Left { get; set; }
        [CsvColumn(Name = "top", FieldIndex = 4)]
        public string Top { get; set; }
        [CsvColumn(Name = "width", FieldIndex = 5)]
        public string Width { get; set; }
        [CsvColumn(Name = "height", FieldIndex = 6)]
        public string Height { get; set; }
        [CsvColumn(Name = "type", FieldIndex = 7, CanBeNull = true)]
        public string Type { get; set; }
        [CsvColumn(Name = "opacity", FieldIndex = 8, CanBeNull = true)]
        public string Opacity { get; set; }
        [CsvColumn(Name = "visible", FieldIndex = 9, CanBeNull = true)]
        public string Visible { get; set; }
        [CsvColumn(Name = "layer_id", FieldIndex = 10, CanBeNull = true)]
        public string LayerId { get; set; }
        [CsvColumn(Name = "group_layer_id", FieldIndex = 11, CanBeNull = true)]
        public string GroupLayerId { get; set; }
        [CsvColumn(Name = "base", FieldIndex = 12, CanBeNull = true)]
        public string Base { get; set; }
        [CsvColumn(Name = "images", FieldIndex = 13, CanBeNull = true)]
        public string Images { get; set; }
        [CsvColumn(Name = "", FieldIndex = 14, CanBeNull = true)]
        public string Unknown { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
