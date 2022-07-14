using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.Threading.Tasks;
using Code_assessment;

namespace Code_assessmentTests
{
    public class NewCustomerTests
    {
        /// <summary>
        /// The method below creates a sample DataTable statically to avoid redundant code for every test case.
        /// </summary>
        /// <returns> The method returns a DataTable </returns>
        public static DataTable DataTableForTest()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("test1");
            dt.Columns.Add("test2");

            DataRow dr1 = dt.NewRow();
            dr1["test1"] = "1";
            dr1["test2"] = "2";
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2["test1"] = "passed";
            dr2["test2"] = "fail";
            dt.Rows.Add(dr2);

            return dt;
        }

        [Fact]
        public void ConvertCsvToXmlNotNullTest()
        {
            var dt = CustomerTest.DataTableForTest();

            var actualResult = new XmlCreator();

            Assert.NotNull(actualResult.ConvertCsvToXml(dt));
        }

        [Fact]
        public void ConvertCsvToXmlExpectedOutputTest()
        {

            var dt = CustomerTest.DataTableForTest();
            var actualResult = new XmlCreator();

            XElement root = new XElement("root",
                new XElement("test1s",
                new XElement("test1", "1"),
                new XElement("test1", "passed")),
                new XElement("test2s",
                new XElement("test2", "2"),
                new XElement("test2", "fail")));

            Assert.True(XNode.DeepEquals(root, actualResult.ConvertCsvToXml(dt)));
        }
    }

}
