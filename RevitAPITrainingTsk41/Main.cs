using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingTsk41
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;//Обращение к Revit
            UIDocument uidoc = uiapp.ActiveUIDocument;//Обращение к интерфейсу текущ элемента
            Document doc = uidoc.Document; //Обращение к документу

            string wallnfo = string.Empty;

            var walls = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType()
                .Cast<Wall>()
                .ToList();

            

            double wallVal = 0;

            foreach (Wall wall in walls)
            {
                //string wallName = wall.get_Parameter(BuiltInParameter.SYMBOL_NAME_PARAM).AsString();
                //string wallVal = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsString();
                //Parameter nameParameter = wall.get_Parameter(BuiltInParameter.SYMBOL_NAME_PARAM); 
                //string wallName = nameParameter.AsString();
                Parameter valParameter = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                if (valParameter.StorageType == StorageType.Double)
                {
                    wallVal = UnitUtils.ConvertFromInternalUnits(valParameter.AsDouble(), UnitTypeId.CubicMeters);
                }

                wallnfo += $"{wall.Name}\t{wallVal}{Environment.NewLine}";
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string txtPath = Path.Combine(desktopPath, "wallInfo.txt");


            File.WriteAllText(txtPath, wallnfo);
            //string desctopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //string cvsPath = Path.Combine(desctopPath, "wallnfo.csv");


            //File.WriteAllText(cvsPath, wallnfo);

            return Result.Succeeded;
        }
    }
}
