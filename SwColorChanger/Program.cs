using SwColorChanger;

var colorMap = ColorMap.ImportColorMap();
try
{
   var saveName = VehicleFile.AskForSaveName(args);
   var xmlDocument = VehicleFile.ImportXmlDocument(saveName);
   
   try
   {
      ColorChanger.RecolorXmlDocument(xmlDocument, colorMap);
      VehicleFile.SaveXmlDocument(xmlDocument, saveName + "_rc");
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
   Console.WriteLine($"Cannot open {args[0]} file.");
   WaitForExit();
   return 1;
}

void WaitForExit()
{
   Console.WriteLine("Press any key to exit.");
   Console.ReadKey();
}