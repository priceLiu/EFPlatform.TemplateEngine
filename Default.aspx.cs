using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EFPlatform.TemplateEngine;

public partial class _Default : Page
{
	private string outputPath;
	private string categoryFileName;
	private string productFileName;
	private static DbProviderFactory dbFactory;
	private DbConnection connection;

	protected void Page_Load(object sender, EventArgs e)
	{
		outputPath = Server.MapPath("./");
		categoryFileName = string.Format(@"{0}\Template\Category.html", outputPath);
		productFileName = string.Format(@"{0}\Template\Product.html", outputPath);
		string currentConnection = ConfigurationManager.AppSettings["Connection"];
		ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[currentConnection];
		this.GetConnection(css);
	}

	private void GenerateCategory()
	{
		string template = Helper.ReadTextFile(categoryFileName);
		Generator gen = new Generator(template);
		gen.ParseTemplate();

        //用起始标志，标志的4个区间
		Region rgnTitle = gen.GetRegion("Title");
		Region rgnCategory = gen.GetRegion("Category");
		Region rgnProducts = gen.GetRegion("Products");
		Region rgnNavigator = gen.GetRegion("Navigator");

		if(rgnTitle == null || rgnCategory == null || rgnProducts == null || rgnNavigator == null)
		{
			Response.Write("Missing region.");
			return;
		}

		int categoryId;
		string outputFileName;
		DataView dvCategory = this.GetCategoryTable().DefaultView;
		Pager pgrCategory = new Pager(1, dvCategory.Count);

		for(int i = 0; i < pgrCategory.PageCount; i++)
		{
			rgnTitle.DataSource = (string)dvCategory[i]["CategoryName"];        //Use a string as data source
			rgnCategory.DataSource = dvCategory[i];        //Use a DataRowView object as data source

            categoryId = (int)dvCategory[i]["CategoryID"];
            rgnProducts.DataSource = this.GetProductTable(categoryId);        //Use a DataTable object as data souce
			
            //set pager
            pgrCategory.CurrentPage = i + 1;
			rgnNavigator.DataSource = pgrCategory;        //Use a Pager object as data source
			
            outputFileName = string.Format(@"{0}\Html\Category{1}.html", outputPath, categoryId);
			Helper.WriteTextFile(outputFileName, gen.Generate());
		}
	}

	private void GenerateProduct()
	{
		string template = Helper.ReadTextFile(productFileName);
		Generator gen = new Generator(template);
		gen.ParseTemplate();
		Region rgnTitle = gen.GetRegion("Title");
		Region rgnProduct = gen.GetRegion("Product");
		Region rgnNavigator = gen.GetRegion("Navigator");

		if(rgnTitle == null || rgnProduct == null || rgnNavigator == null)
		{
			Response.Write("Missing region.");
			return;
		}

		string outputFileName;
		List<Product> productList = this.GetProductList();
		Pager pgrProduct = new Pager(1, productList.Count);

		for(int i = 0; i < pgrProduct.PageCount; i++)
		{
			rgnTitle.DataSource = productList[i].CategoryName;        //Use a string as data source
			rgnProduct.DataSource = productList[i];        //Use a Product object as data source
			pgrProduct.CurrentPage = i + 1;
			rgnNavigator.DataSource = pgrProduct;        //Use a Pager object as data source
			outputFileName = string.Format(@"{0}\Html\Product{1}.html", outputPath, productList[i].ProductID);
			Helper.WriteTextFile(outputFileName, gen.Generate());
		}
	}

	#region DataSourcePreparing
	private void GetConnection(ConnectionStringSettings css)
	{
		if(dbFactory == null)
		{
			dbFactory = DbProviderFactories.GetFactory(css.ProviderName);
		}

		this.connection = dbFactory.CreateConnection();
		this.connection.ConnectionString = css.ConnectionString;
	}

	private DataTable GetCategoryTable()
	{
		string commandText = "SELECT CategoryID, CategoryName, Description FROM Categories";
		DbDataAdapter da = dbFactory.CreateDataAdapter();
		da.SelectCommand = dbFactory.CreateCommand();
		da.SelectCommand.Connection = this.connection;
		da.SelectCommand.CommandText = commandText;
		DataTable dt = new DataTable();
		this.connection.Open();
		da.Fill(dt);
		this.connection.Close();
		return dt;
	}

	private DataTable GetProductTable(int categoryId)
	{
		string commandText = string.Format("SELECT * FROM Products WHERE CategoryID = {0}", categoryId);
		DbDataAdapter da = dbFactory.CreateDataAdapter();
		da.SelectCommand = dbFactory.CreateCommand();
		da.SelectCommand.Connection = this.connection;
		da.SelectCommand.CommandText = commandText;
		DataTable dt = new DataTable();
		this.connection.Open();
		da.Fill(dt);
		this.connection.Close();
		return dt;
	}

	private List<Product> GetProductList()
	{
		string commandText = "SELECT p.*, c.CategoryName, s.CompanyName FROM (Products AS p INNER JOIN Categories AS c ON p.CategoryID = c.CategoryID) INNER JOIN Suppliers AS s ON p.SupplierID = s.SupplierID ORDER BY p.ProductID";
		DbCommand command = this.connection.CreateCommand();
		command.CommandText = commandText;
		List<Product> productList = new List<Product>();
		Product product;
		this.connection.Open();

		using(DbDataReader dr = command.ExecuteReader())
		{
			while(dr.Read())
			{
				product = new Product();
				Helper.FillModel(product, dr);
				productList.Add(product);
			}
		}

		this.connection.Close();
		return productList;
	}

	private class Product
	{
		private int productID;

		public int ProductID
		{
			get { return productID; }
			set { productID = value; }
		}

		private string productName;

		public string ProductName
		{
			get { return productName; }
			set { productName = value; }
		}

		private string companyName;

		public string CompanyName
		{
			get { return companyName; }
			set { companyName = value; }
		}

		private int categoryID;

		public int CategoryID
		{
			get { return categoryID; }
			set { categoryID = value; }
		}

		private string categoryName;

		public string CategoryName
		{
			get { return categoryName; }
			set { categoryName = value; }
		}

		private string quantityPerUnit;

		public string QuantityPerUnit
		{
			get { return quantityPerUnit; }
			set { quantityPerUnit = value; }
		}

		private decimal unitPrice;

		public decimal UnitPrice
		{
			get { return unitPrice; }
			set { unitPrice = value; }
		}

		private int unitsInStock;

		public int UnitsInStock
		{
			get { return unitsInStock; }
			set { unitsInStock = value; }
		}

		private int unitsOnOrder;

		public int UnitsOnOrder
		{
			get { return unitsOnOrder; }
			set { unitsOnOrder = value; }
		}

		private int reorderLevel;

		public int ReorderLevel
		{
			get { return reorderLevel; }
			set { reorderLevel = value; }
		}

	}
	#endregion

	protected void Button1_Click(object sender, EventArgs e)
	{
		this.GenerateCategory();
	}

	protected void Button2_Click(object sender, EventArgs e)
	{
		this.GenerateProduct();
	}
}
