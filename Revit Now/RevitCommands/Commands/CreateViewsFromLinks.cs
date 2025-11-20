using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Revit_Now.Helpers;

namespace Revit_Now.RevitCommands.Commands
{
    [TransactionAttribute(TransactionMode.Manual)]
    #region
    //This Revit command allows the user to select a linked element and create a section box around it 
    #endregion

    public class CreateViewsFromLinks : IExternalCommand
    {
        public static UIDocument uidoc { get; set; }
        public static Document doc { get; set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uidoc = commandData.Application.ActiveUIDocument;
            doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);

            IList<Element> LinksList = collector.WhereElementIsNotElementType().OfClass(typeof(RevitLinkInstance)).ToElements();

            ViewFamilyType viewFamilyType = new FilteredElementCollector(doc)
                         .OfClass(typeof(ViewFamilyType))
                         .Cast<ViewFamilyType>()
                         .FirstOrDefault(x => x.ViewFamily == ViewFamily.ThreeDimensional);

  
            ElementId viewFamilyTypeId = viewFamilyType.Id;

            StringBuilder sb = new StringBuilder();
            foreach (Element link in LinksList) {

                using (Transaction t = new Transaction(doc, "Create View"))
                {
                    t.Start();
                    // Now use it to create the 3D view
                    View3D view3d = View3D.CreateIsometric(doc, viewFamilyTypeId);
                   
                    view3d.Name = ModifyString.RemoveStringAfterIndex(link.Name.ToString(), ':');
                    t.Commit();
                    sb.Append(link.Name.ToString());
                    sb.Append(Environment.NewLine);
                }
               

            }
            TaskDialog.Show("Links in Model", sb.ToString());

            return Result.Succeeded;
        }
    }
}
