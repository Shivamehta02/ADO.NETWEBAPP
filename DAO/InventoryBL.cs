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
		public void AddInventory(Inventory inventory)
		{
			string connectionString = Configuration["connectionStrings:DefaultConnection"];
			SqlConnection connection = new SqlConnection(connectionString);
			string query = "select * from Inventory";
			SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
			SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
			DataTable dataTable = new DataTable();
			dataAdapter.Fill(dataTable);

			DataRow dataRow = dataTable.NewRow();
			dataRow[1] = inventory.Name;
			dataRow[2] = inventory.Price;
			dataRow[3] = inventory.Quantity;
			dataTable.Rows.Add(dataRow);
			dataAdapter.UpdateCommand = commandBuilder.GetInsertCommand();
			dataAdapter.Update(dataTable);

		}

		public void EditInventory(int id, Inventory inventory)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Inventory> GetInventories()
		{
			string connectionString = Configuration["connectionStrings:DefaultConnection"];
			List<Inventory> inventories = new List<Inventory>();   // since we need to show multiple values

			SqlConnection connection = new SqlConnection(connectionString);
			string query = "select* from Inventory";
			SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
			DataTable dataTable = new DataTable();
			dataAdapter.Fill(dataTable);
            foreach (DataRow datarow in dataTable.Rows)       //we are converting because these are of object type 
            {
				Inventory inventory = new Inventory(); 
				inventory.Id = Convert.ToInt32(datarow[0]);
				inventory.Name = datarow[1].ToString();
				inventory.Price = decimal.Parse(datarow[2].ToString());
				inventory.Quantity = int.Parse(datarow[3].ToString());
				inventories.Add(inventory);
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
			throw new NotImplementedException();
		}
	}
}
