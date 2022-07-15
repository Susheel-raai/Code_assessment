using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.Threading.Tasks;

namespace Code_assessment
{
    /// <summary>
    /// This method accepts data as an input and returns XML data. The XML nodes are created based on Switch case condition.
    /// The resultant XML data follows column iteration.
    /// </summary>
    /// <param name="csv_dt"> the only parameter that contains the csv data in a tabular format</param>
    /// <returns>Returns an xml formatted data of type XElement</returns>
    /// <exception cref="FormatException"> when input data is not in a correct format</exception>
    public class XmlCreator
    {
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
                    case "Order No":
                    case "Order Line No":
                    case "Total cost":
                    case "Quantity":

                        if (header != "Invoice No")
                        {
                            break;
                        }


                        ColumnData = ColumnData.Distinct();

                        XElement parentInvoiceNode = new XElement("Invoices");
                        foreach (var invoiceValue in ColumnData)
                        {
                            var InvoicesData = from myrow in csv_dt.AsEnumerable()
                                               where myrow.Field<string>("Invoice No") == invoiceValue
                                               select new
                                               {
                                                   order = myrow.Field<string>("Order No"),
                                                   totalvalue = myrow.Field<string>("Total cost"),
                                                   orderLine = myrow.Field<string>("Order Line No"),
                                                   productCode = myrow.Field<string>("Product Code"),
                                                   quantity = myrow.Field<string>("Quantity"),
                                               };

                            var invoiceNode = new XElement("Invoice",
                                             new XAttribute("InvoiceNo", int.Parse(invoiceValue)),
                                             new XElement("TotalValue", InvoicesData.Select(x => x.totalvalue).Sum(k => decimal.Parse(k))));

                            var orderParentNode = new XElement("Orders");

                            foreach (var order in InvoicesData.Select(x => x.order).Distinct())
                            {
                                var orderData = InvoicesData.Select(x => x.order);

                                var orderLines = InvoicesData.Where(x => x.order == order);

                                var orderNode = new XElement("Order",
                                               new XAttribute("OrderNo", order),
                                               new XElement("TotalLines", orderData.Count(x => x == order)));

                                var orderLineNode = new XElement("OrderLines");

                                foreach (var row in orderLines)
                                {
                                    orderLineNode.Add(new XElement("OrderLine",
                                        new XAttribute("OrderLineNo", row.orderLine),
                                        new XElement("ProductCode", row.productCode),
                                        new XElement("Quantity", row.quantity),
                                        new XElement("TotalCost", row.totalvalue)));
                                }
                                orderNode.Add(orderLineNode);
                                orderParentNode.Add(orderNode);
                            }
                            invoiceNode.Add(orderParentNode);
                            parentInvoiceNode.Add(invoiceNode);
                        }
                        customerXml.Add(parentInvoiceNode);
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
                        DataTable customerInfoData = csv_dt.DefaultView.ToTable(true, "Customer Name", "Invoice No", "Address 1",
                                                                           "Address 2", "City", "Postcode", "Country");

                        XElement customersInfo = new XElement("Customers",
                          from DataRow row in customerInfoData.Rows
                          let rowData = row
                          select new XElement("Customer",
                                  new XAttribute("CustomerName", rowData[0]),
                                  new XElement("InvoiceNo", Convert.ToInt32(rowData[1])),
                                  new XElement("Address1", rowData[2]),
                                  new XElement("Address2", rowData[3]),
                                  new XElement("City", rowData[4]),
                                  new XElement("Postcode", rowData[5]),
                                  new XElement("Country", rowData[6])
                                  )
                          );
                        customerXml.Add(customersInfo);
                        break;

                    case "Product Code":
                    case "Unit of Measurement":
                    case "Unit cost":
                    case "currency":
                        if (header != "Product Code")
                        {
                            break;
                        }
                        DataTable productInfoData = csv_dt.DefaultView.ToTable(true, "Product Code",
                                                        "Unit of Measurement", "Unit cost", "currency");

                        XElement productInfo = new XElement("Products",
                          from DataRow row in productInfoData.Rows
                          let rowData = row
                          select new XElement("Product",
                                  new XAttribute("ProductCode", rowData[0]),
                                  new XElement("UnitofMeasurement", rowData[1] == string.Empty ? "Each" : rowData[1]),
                                  new XElement("UnitCost", rowData[2]),
                                  new XElement("Currency", rowData[3] == string.Empty ? "GBP" : rowData[3])
                                  )
                          );
                        customerXml.Add(productInfo);
                        break;

                    default:

                        XElement parentNode = new XElement(XmlGenerator.ReplaceWord(header.Replace(" ", "")));
                        foreach (var itemValue in ColumnData)
                        {
                            parentNode.Add(new XElement(header.Replace(" ", ""), itemValue));
                        }
                        customerXml.Add(parentNode);
                        break;
                }
            }
            return customerXml;
        }
    }
}
