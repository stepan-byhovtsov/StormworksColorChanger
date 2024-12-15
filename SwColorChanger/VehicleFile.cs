using System.Xml;

namespace SwColorChanger;

public static class VehicleFile
{
   public static string GeneratePath(string fileName)
   {
      return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
         @"Stormworks\data\vehicles", fileName + ".xml");
   }

   public static string AskForSaveName(string[] args)
   {
      if (args.Length == 0)
      {
         Console.Write("Please, specify vehicle to replace colors: ");
         return Console.ReadLine()!;
      }

      return args[0];
   }
   
   public static XmlDocument ImportXmlDocument(string name)
   {
      var path = GeneratePath(name);
      using var file = File.Open(path, FileMode.Open);
      var xmlDocument = new XmlDocument();
      xmlDocument.Load(file);
      return xmlDocument;
   }
   
   public static void SaveXmlDocument(XmlDocument xmlDocument, string name)
   {
      var outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Stormworks\data\vehicles", name + ".xml");
      xmlDocument.Save(outputPath);
   }
}