using Code_assessment;
using System.Data;
using System.Xml.Linq;

namespace Code_assessmentTests
{
    public class CustomerTest
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
        public void ReplaceWordExpectedOutputNotNullTest()
        {
            string header = "Currency";

            Assert.NotNull(XmlGenerator.ReplaceWord(header));
        }

        [Fact]
        public void ReplaceWordExpectedOutputTest1()
        {
            string header = "Currency";
            var expectedResult = "Currencies";

            Assert.Equal(expectedResult, XmlGenerator.ReplaceWord(header));
        }

        [Fact]
        public void ReplaceWordExpectedOutputTest2()
        {
            string header = "Invoice";
            var expectedResult = "Invoices";

            Assert.Equal(expectedResult, XmlGenerator.ReplaceWord(header));
        }

        [Fact]
        public void CsvToDataTableNotNullTest()
        {
            string filePath = @"D:\Practice\Code_assessment\Code_assessmentTests\test.csv";
            var actualResult = new XmlGenerator();

            Assert.NotNull(actualResult.CsvToDataTable(filePath));
        }

        [Fact]
        public void CsvToDataTableCountTest()
        {
            string filePath = @"D:\Practice\Code_assessment\Code_assessmentTests\test.csv";

            var actualResult = new XmlGenerator();

            var dt = CustomerTest.DataTableForTest();

            var actualtb = actualResult.CsvToDataTable(filePath);
            Assert.True(dt.Rows.Count == actualtb.Rows.Count);
        }

        [Fact]
        public void CsvToDataTableExpectedOutputTest()
        {
            string filePath = @"D:\Practice\Code_assessment\Code_assessmentTests\test.csv";

            var actualResult = new XmlGenerator();

            var dt = CustomerTest.DataTableForTest();

            var actualtb = actualResult.CsvToDataTable(filePath);
            Assert.Equal(dt.Rows[0][0], actualtb.Rows[0][0]);
        }

        [Fact]
        public void ConvertCsvToXmlNotNullTest()
        {
            var dt = CustomerTest.DataTableForTest();

            var actualResult = new XmlGenerator();

            Assert.NotNull(actualResult.ConvertCsvToXml(dt));
        }

        [Fact]
        public void ConvertCsvToXmlExpectedOutputTest()
        {

            var dt = CustomerTest.DataTableForTest();
            var actualResult = new XmlGenerator();

            XElement root = new XElement("root",
                new XElement("test1s",
                new XElement("test1","1"),
                new XElement("test1","passed")),
                new XElement("test2s",
                new XElement("test2","2"),
                new XElement("test2","fail")));

            Assert.True(XNode.DeepEquals(root, actualResult.ConvertCsvToXml(dt)));
        }
    }
}