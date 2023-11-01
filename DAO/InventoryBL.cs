using ADOCOREWEBAPP.Models;
using System.Data;
using System.Data.SqlClient;

namespace ADOCOREWEBAPP.DAO
{
	public class InventoryBL : IInventoryInterface
	{
		public IConfiguration Configuration { get;  }
		public InventoryBL(IConfiguration configuration) 
		{
			Configuration = configuration;
		}
		//public void AddInventory(Inventory inventory)
		//{
		//	string connectionString = Configuration["connectionStrings:DefaultConnection"];
		//	SqlConnection connection = new SqlConnection(connectionString);
		//	string query = "select * from Inventory";
		//	SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
		//	SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
		//	DataTable dataTable = new DataTable();
		//	dataAdapter.Fill(dataTable);

		//	DataRow dataRow = dataTable.NewRow();
		//	dataRow[1] = inventory.Name;
		//	dataRow[2] = inventory.Price;
		//	dataRow[3] = inventory.Quantity;
		//	dataTable.Rows.Add(dataRow);
		//	dataAdapter.UpdateCommand = commandBuilder.GetInsertCommand();
		//	dataAdapter.Update(dataTable);

		//}

		public void AddInventory(Inventory inventory)   //using stored procedure
		{
			string connectionString = Configuration["connectionStrings:DefaultConnection"];
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var query = "InsertInventory";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					// Add parameters to the stored procedure
					command.Parameters.Add(new SqlParameter("@Name", inventory.Name));
					command.Parameters.Add(new SqlParameter("@Price", inventory.Price));
					command.Parameters.Add(new SqlParameter("@Quantity", inventory.Quantity));
					command.Parameters.Add(new SqlParameter("@AddedOn", inventory.AddedOn));

					command.ExecuteNonQuery();
				}
			}

		}

		public void EditInventory(int id, Inventory inventory)
		{
			string connectionString = Configuration["connectionStrings:DefaultConnection"];
			SqlConnection connection = new SqlConnection(connectionString);

			string query = "select * from Inventory where Id = @Id";
			SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
			dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);

			SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

			DataTable dataTable = new DataTable();
			dataAdapter.Fill(dataTable);

			if (dataTable.Rows.Count == 1)
			{
				DataRow dataRow = dataTable.Rows[0];
				dataRow[1] = inventory.Name;
				dataRow[2] = inventory.Price;
				dataRow[3] = inventory.Quantity;

				dataAdapter.Update(dataTable);
			}
		}

		//public IEnumerable<Inventory> GetInventories()
		//{
		//	string connectionString = Configuration["connectionStrings:DefaultConnection"];
		//	List<Inventory> inventories = new List<Inventory>();   // since we need to show multiple values

		//	SqlConnection connection = new SqlConnection(connectionString);
		//	string query = "select* from Inventory";
		//	SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
		//	DataTable dataTable = new DataTable();
		//	dataAdapter.Fill(dataTable);
		//          foreach (DataRow datarow in dataTable.Rows)       //we are converting because these are of object type 
		//          {
		//		Inventory inventory = new Inventory(); 
		//		inventory.Id = Convert.ToInt32(datarow[0]);
		//		inventory.Name = datarow[1].ToString();
		//		inventory.Price = decimal.Parse(datarow[2].ToString());
		//		inventory.Quantity = int.Parse(datarow[3].ToString());
		//		inventories.Add(inventory);
		//          }

		//          return inventories;

		//}

		public IEnumerable<Inventory> GetInventories()// using stored procedure
		{
			string connectionString = Configuration["connectionStrings:DefaultConnection"];
			List<Inventory> inventories = new List<Inventory>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand("inventories", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					DataSet ds = new DataSet();
					SqlDataAdapter adapter = new SqlDataAdapter(command);
					adapter.Fill(ds);
					connection.Close();

					if(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
					{
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
							var dr = ds.Tables[0].Rows[i];
							var obj = new Inventory
							{
								Id = Convert.ToInt32(dr["Id"]),
								Name = dr["Name"].ToString(),
								Price = Convert.ToDecimal(dr["Price"]),
								Quantity = Convert.ToInt32(dr["Quantity"]),
								AddedOn = Convert.ToDateTime(dr["AddedOn"])
							};
						inventories.Add(obj);
						}
                    }
					//using (SqlDataReader reader = command.ExecuteReader())
					//{
					//	while (reader.Read())
					//	{
					//		Inventory inventory = new Inventory();
					//		inventory.Id = reader.GetInt32(0);
					//		inventory.Name = reader.GetString(1);
					//		inventory.Price = reader.GetDecimal(2);
					//		inventory.Quantity = reader.GetInt32(3);
					//		inventories.Add(inventory);
					//	}
					//}
				}
			}

			return inventories;
		}
		
		public Inventory GetInventory(int id)
		{
			Inventory inventory = new Inventory();

			string connectionString = Configuration["connectionStrings:DefaultConnection"];
			SqlConnection connection = new SqlConnection(connectionString);
			string query = "select* from Inventory";
			SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
			DataTable dataTable = new DataTable();
			dataAdapter.Fill(dataTable);

			foreach (DataRow datarow in dataTable.Rows)
			{
                if (Convert.ToInt32(datarow[0])==id)
                {
					inventory.Id = Convert.ToInt32(datarow[0]);
					inventory.Name = datarow[1].ToString();
					inventory.Price = decimal.Parse(datarow[2].ToString());
					inventory.Quantity = int.Parse(datarow[3].ToString());
					inventory.AddedOn = Convert.ToDateTime(datarow[4].ToString());
				}

			}
				return inventory;
		}

		public void RemoveInventory(int id, Inventory inventory)
		{
			string connectionString = Configuration["connectionStrings:DefaultConnection"];
			SqlConnection connection = new SqlConnection(connectionString);

			string query = "select * from Inventory where Id = @Id";
			SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
			dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);

			SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

			DataTable dataTable = new DataTable();
			dataAdapter.Fill(dataTable);

			if (dataTable.Rows.Count == 1)
			{
				DataRow dataRow = dataTable.Rows[0];
				dataRow.Delete();

				dataAdapter.Update(dataTable);
			}
		}
	}
}
