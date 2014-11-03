using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaremeteGadget
{
    class SelectedItems
    {
        public string LayerInfoFilePath { get; set; }
        public SizeData Size { get; set; }
        public CharDataFile CharacterInfo { get; set; }
        public ImageDataFile Dress { get; set; }
        public bool IsWink { get; set; }
        public IEnumerable<ImageDataFile> Eyes { get; set; }
        public bool IsLipSync { get; set; }
        public IEnumerable<ImageDataFile> Mouths { get; set; }
        public BlushItems BlushLevel { get; set; }
    }
}
