using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaremeteGadget
{
    class SizeData
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Label;
        }
    }
}
