using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using System.Linq.Expressions;

namespace Revit_Now.RevitCommands.Commands
{
    [TransactionAttribute(TransactionMode.Manual)]
    #region
    //This Revit command allows the user to select a linked element and create a section box around it 
    #endregion
    public class CreateViews : IExternalCommand
    {
        public static UIDocument uidoc { get; set; }
        public static Document doc { get; set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uidoc = commandData.Application.ActiveUIDocument;
            doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList <Element> levelList = collector.WhereElementIsNotElementType().OfClass(typeof(Level)).ToElements();
            StringBuilder sb = new StringBuilder();
            
            ViewFamilyType viewType = new FilteredElementCollector(doc)
            .OfClass(typeof(ViewFamilyType))
            .Cast<ViewFamilyType>().FirstOrDefault(x => x.ViewFamily == ViewFamily.FloorPlan);

            //.Cast<ViewFamilyType>().FirstOrDefault(x => x.ViewFamily == ViewFamily.FloorPlan && x.Name == "f");

            //sb.Append(viewType.Name.ToString()); 


            foreach (Element el in levelList)
            {
                sb.Append(el.Name.ToString());
                sb.Append(Environment.NewLine);

                Level level = el as Level;
                using (Transaction t = new Transaction(doc, "Create Floor Plan"))
                {
                    t.Start();
                    ViewPlan viewPlan = ViewPlan.Create(doc, viewType.Id, level.Id);
                    viewPlan.Name = "For Architecture - " + level.Name;
                    t.Commit();
                }
                
            }
            TaskDialog.Show("Level Name", sb.ToString());

            return Result.Succeeded;     
        }
    }
}
