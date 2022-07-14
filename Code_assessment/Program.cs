using Code_assessment;



/* storing file path in a filePath Variable of type string.
 * storing file information in fileInfo based on file path.
 * try{
 *      checking whether a file exists in a given path and the given file is csv file, based on existance and extension in "if" condition.
 *      if {
 *              calling a method "CsvToDataTable(filepath)" with filepath as a parameter, returns a datatable.
 *              calling a method "ConvertCsvToXml(datatable)" with datatable as a parameter, returns xml data.
 *              the xml file gets created with the same name as the csv file.
 *          }
 *      else{
 *          this block will execute when the given condition in not satisfied.
 *          }
 *     }
 * Catch (Exception e)
 *      {
 *              if any runtime exception occurs this block will throw an error message.
 *              streamWriter creates a text file with same name as csv file that contains log time and error message.
 *      }
 */


string filePath = @"D:\Practice\Code_assessment\candidate_test_sample.csv";                   
var fileInfo = new FileInfo(filePath);                                                       

try
{
    if (fileInfo.Exists && fileInfo.Extension==".csv")                                        
    {                  
        
        XmlGenerator generator = new XmlGenerator();                                         
        var csv_dt = generator.CsvToDataTable(@fileInfo.FullName);

        var xml = generator.ConvertCsvToXml(csv_dt);
        System.IO.File.WriteAllText(@fileInfo.FullName.Replace(".csv", ".xml"),
                                   xml.ToString());

        XmlCreator creator = new XmlCreator();
        var xmlNew = creator.ConvertCsvToXml(csv_dt);
        System.IO.File.WriteAllText(@fileInfo.FullName.Replace(".csv", "New.xml"),
                                    xmlNew.ToString());

        Console.WriteLine("Successfully Converted \nPress any key to close");
        Console.ReadKey();
    }
    else
    {
        Console.WriteLine("File not found in a given path");                                  
    }
}
catch (Exception ex)
{                                                                                               
    Console.WriteLine("Error in Converting File to XML");                                       
    using (StreamWriter sw = File.CreateText(@fileInfo.FullName.Replace(".csv", "logs.txt")))   
    {
        sw.WriteLine(DateTime.Now + ": " + ex.Message);
    }
}
