using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WaremeteGadget
{
    enum BlushLevel
    {
        None,
        Low,
        High
    }

    class BlushItems
    {
        public BlushLevel Level { get; set; }

        public override string ToString()
        {
            return Enum.GetName(typeof(BlushLevel), Level);
        }

        public string OriginalIdentifier()
        {
            switch (Level)
            {
                case BlushLevel.None: return "([^Ａ-Ｚ])$";
                case BlushLevel.Low: return "(_Ｌ)";
                case BlushLevel.High: return "(_Ｈ)";
            }

            throw new NullReferenceException();
        }


        public bool IsMatch(string name)
        {
            Regex regex = new Regex(@".*" + OriginalIdentifier());
            return regex.IsMatch(name);
        }
    }
}
