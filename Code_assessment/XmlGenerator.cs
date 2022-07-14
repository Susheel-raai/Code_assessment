using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Code_assessment
{
    public class XmlGenerator
    {
        /// <summary>
        /// This method accepts data as an input and returns XML data. The XML nodes are created based on Switch case condition.
        /// The resultant XML data follows column iteration.
        /// </summary>
        /// <param name="csv_dt"> the only parameter that contains the csv data in a tabular format</param>
        /// <returns>Returns an xml formatted data of type XElement</returns>
        /// <exception cref="FormatException"> when input data is not in a correct format</exception>
        public XElement ConvertCsvToXml(DataTable csv_dt)
        {
            XElement customerXml = new XElement("root");

            List<string> headers = new List<string>();
            foreach (DataColumn col in csv_dt.Columns)
            {
                headers.Add(col.ColumnName);
            }

            foreach (var header in headers)
            {
                IEnumerable<string> ColumnData = from columnData in csv_dt.AsEnumerable()
                                                 select columnData.Field<string>(header);
                switch (header)
                {
                    case "Invoice No":
                        ColumnData = ColumnData.Distinct();
                        XElement InvoicesNode = new XElement("Invoices");
                        foreach (var invoiceValue in ColumnData)
                        {
                            var result = from myRow in csv_dt.AsEnumerable()
                                         where myRow.Field<string>("Invoice No") == invoiceValue
                                         select myRow.Field<string>("Total cost");

                            InvoicesNode.Add(new XElement("Invoice", int.Parse(invoiceValue),
                                new XElement("TotalValue", result.Sum(x => decimal.Parse(x)))));
                        }
                        customerXml.Add(InvoicesNode);
                        break;

                    case "Order No":
                        var Orderlines = ColumnData;
                        ColumnData = ColumnData.Distinct();
                        XElement OrdersNode = new XElement("Orders");
                        foreach (var orderValue in ColumnData)
                        {
                            OrdersNode.Add(new XElement("Order", orderValue,
                                new XElement("TotalLines", Orderlines.Count(x => x == orderValue))));
                        }
                        customerXml.Add(OrdersNode);
                        break;

                    case "Customer Name":
                    case "Address 1":
                    case "Address 2":
                    case "City":
                    case "Postcode":
                    case "Country":
                        if (header != "Customer Name")
                        {
                            break;
                        }
                        DataTable customerInfoData = csv_dt.DefaultView.ToTable(true, "Customer Name", "Address 1",
                                                                           "Address 2", "City", "Postcode", "Country");

                        XElement customersInfo = new XElement("Customers",
                          from DataRow row in customerInfoData.Rows
                          let rowData = row
                          select new XElement("CustomerInfo",
                                  new XAttribute("CustomerName", rowData[0]),
                                  new XElement("Address1", rowData[1]),
                                  new XElement("Address2", rowData[2]),
                                  new XElement("City", rowData[3]),
                                  new XElement("Postcode", rowData[4]),
                                  new XElement("Country", rowData[5])
                                  )
                          );
                        customerXml.Add(customersInfo);
                        break;

                    case "Product Code":
                    case "Quantity":
                    case "Unit of Measurement":
                    case "Unit cost":
                    case "Total cost":
                    case "currency":
                        if (header != "Product Code")
                        {
                            break;
                        }
                        DataTable productInfoData = csv_dt.DefaultView.ToTable(true, "Product Code", "Quantity",
                                                                           "Unit of Measurement", "Unit cost", "Total cost", "currency");

                        XElement productInfo = new XElement("Products",
                          from DataRow row in productInfoData.Rows
                          let rowData = row
                          select new XElement("ProductInfo",
                                  new XAttribute("ProductCode", rowData[0]),
                                  new XElement("Quantity", rowData[1]),
                                  new XElement("UnitofMeasurement", rowData[2] == string.Empty ? "Each" : rowData[2]),
                                  new XElement("Unitcost", rowData[3]),
                                  new XElement("Totalcost", rowData[4]),
                                  new XElement("currency", rowData[5] == string.Empty ? "GBP" : rowData[5])
                                  )
                          );
                        customerXml.Add(productInfo);
                        break;

                    default:
                        XElement parentNode = new XElement(ReplaceWord(header.Replace(" ", "")));
                        foreach (var itemValue in ColumnData)
                        {
                            parentNode.Add(new XElement(header.Replace(" ", ""), header == "Unit of Measurement" && itemValue == string.Empty ? "Each" :
                                                            header == "currency" && itemValue == string.Empty ? "GBP" : itemValue));
                        }
                        customerXml.Add(parentNode);
                        break;
                }
            }
            return customerXml;
        }

        /// <summary>
        /// This method accepts a header string as an input and converts them into plural form by appending suffixes. 
        /// </summary>
        /// <param name="header"> header string contains the header name, which is a parent node name in xml tag</param>
        /// <returns> Returns a modified header value of type string</returns>
        public static string ReplaceWord(string header)
        {
            string headerValue;
            if (header.EndsWith("y"))
            {
                header = header.Remove(header.Length - 1);
                headerValue = header + "ies";
            }
            else
            {
                headerValue = header + "s";
            }
            return headerValue;
        }


        /// <summary>
        /// This method takes filepath as an argument and reads the data from specified filepath and converts it 
        /// into a datatable.
        /// </summary>
        /// <param name="filePath"> filepath string contains physical path of the csv file</param>
        /// <returns> Returns a table of type DataTable</returns>
        public DataTable CsvToDataTable(string filePath)
        {
            DataTable csv_dt = new DataTable();

            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF7))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    csv_dt.Columns.Add(header.Trim());
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = csv_dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    csv_dt.Rows.Add(dr);
                }
            }
            return csv_dt;
        }
    }
}
