using SwColorChanger;

var colorMap = ColorMap.ImportColorMap(out var colorMapName);
var saveName = VehicleFile.AskForSaveName(args);
try
{
   var xmlDocument = VehicleFile.ImportXmlDocument(saveName);
   
   try
   {
      ColorChanger.RecolorXmlDocument(xmlDocument, colorMap);
      VehicleFile.SaveXmlDocument(xmlDocument, saveName + "_rc_" + colorMapName);
      Console.WriteLine("Successfully recolored.");
      WaitForExit();
      return 0;
   }
   catch
   {
      Console.WriteLine("Cannot parse vehicle file.");
      WaitForExit();
      return 1;
   }
}
catch
{
   Console.WriteLine($"Cannot open {saveName} file.");
   WaitForExit();
   return 1;
}

void WaitForExit()
{
   Console.WriteLine("Press any key to exit.");
   Console.ReadKey();
}