using ADOCOREWEBAPP.DAO;
using ADOCOREWEBAPP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ADOCOREWEBAPP.Controllers
{
	public class InventoryController : Controller
	{
		IInventoryInterface inventoryInterface;
		public InventoryController(IConfiguration configuration) 
		{
			inventoryInterface = new InventoryBL(configuration);
		}

		public IActionResult Index()
		{
			return View(inventoryInterface.GetInventories());
		}

		public IActionResult Create() 
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Inventory inventory)
		{
			inventoryInterface.AddInventory(inventory);
			return RedirectToAction("Index");
		}

		public IActionResult Details(int id)
		{
			return View(inventoryInterface.GetInventory(id));
		}
	}
}
