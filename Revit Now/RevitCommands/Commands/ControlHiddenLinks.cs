using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_Now.RevitCommands.Commands
{
    public class ControlHiddenLinks
    {
        static public void HideLinkedElementsInView(IList<Element> linkList, View view,string linkName)
        {
           
            foreach (Autodesk.Revit.DB.Element link in linkList)
            {   if (link.Name.ToString().Equals(linkName)) {
                    continue;


                }
                else
                {
                    view.HideElements(new List<Autodesk.Revit.DB.ElementId> { link.Id });
                }

            }
                    
        }

    }
}
