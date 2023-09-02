using ADOCOREWEBAPP.Models;

namespace ADOCOREWEBAPP.DAO
{
	public interface IInventoryInterface
	{
		public IEnumerable<Inventory> GetInventories();
		public void  AddInventory(Inventory inventory);
		public Inventory GetInventory(int id);
		public void RemoveInventory(int id, Inventory inventory);
		public void EditInventory(int id, Inventory inventory);

	}
}
