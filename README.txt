1. Project Description

- A C# Console application that monitors a directory for csv files, reads them in and then outputs a file in the XML format.

	1. The solution reads a csv file and converts the CSV data into a XML file.

	2. The format of the output XML file is the expected format given in the task description.

	3. The technology used to implement the task is C# console application in .NET Core 6.

      4. The functionalities are tested through xUnit testing tool.

	5. In future, this solution can be enhanced by giving it an ability to read multiple CSV files in a directory and convert them into expected XML format. But this again has some 

         challenges which I will mention later in this document.
	

2. How to run the project?

- The solution of this task is sent in a zip format. This includes:

		a.  All the packages required to run the solution in your environment.

		b.  An Input CSV file.

		c.  An Output XML file.

		d.  Log File - This file logs the errors that occurs in runtime.

		e.  A ReadMe File - This file includes the project description, observations, Assumptions, and Challenges.

- Alternatively you can also clone the project from the below github link:
	
		https://github.com/Susheel-raai/Code_assessment.git


3. How to Use the Project?

- To run the solution successfully, you will need to override the CSV file path with the current filepath in the "Program.cs" file.

	string filePath = @"D:\Practice\Code_assessment\candidate_test_sample.csv";

- By overriding the above filepath with a sample filepath, a XMl file with the same name would be created within the same directory. Additionally, a log file will also be created in the 

  same directory to track the runtime errors.


Observations:

- From the CSV data (candidate_test_sample.csv) I have interpreted the following:

	1. There is a direct relationship between invoice No's and customer info. That is, an invoice number is generated for every customer which is unique.

	2. The order lines represent the number of items in a particular order.

	3. The ProductCode is unique and unit of measurement, unit cost, currency depends on the ProductCode.

	4. The total cost is the resultant product of the unit cost and quantity. (totalCost = UnitCost * Quantity). So there is a hidden relationship between ProductCode and TotalCost.


Assumptions:

- Considering my observations, I have made the following assumptions.

	1. Within each 'Invoice' node there is a 'Total Value' node that shows the total value (Total Cost) of all of the orders within the invoice.

	2. Within each 'Order' node there is a 'Total Lines' node representing the total number of items within an order at an instance.

	3. I have assumed customer name, Address 1, Address 2, City, Postcode, and Country are related to customer info. Therefore, I have created a parent node as customerInfo and within 	   
         this node I have these attributes as child nodes.

	4. Similarly, ProductCodes, Unit of Measurement, unit cost, Total Cost, and Currency are related to ProductInfo. Therefore, I have created a parent node as ProductInfo and within 

	   this node I have these attributes as child nodes.

	5. If in future, a new column gets added to the existing CSV Files that does not contain any relationship with other columns, a XML element is added to the existing root element with 
	
	   its header name as a XML element name.


Challenges:

- After considering the observations and assumptions, I followed a static approach to convert the CSV file data to XML File.

- In future, if another CSV file is added to the same directory that does not have same column names, the solution will create dynamic XML tags without any relationship between the 

  columns. 