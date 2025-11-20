using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_Now.Helpers
{
    public class ModifyString
    {

        static public string RemoveStringAfterIndex(string input, char x)
        {
           
            int index = input.IndexOf(x);

            if (index >= 0)
            {
                // Substring(startIndex, length): keep characters from index 0 up to and including the character
                input = input.Substring(0, index );
            }

           return input;

        }
    }
}
