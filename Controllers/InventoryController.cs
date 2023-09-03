using ADOCOREWEBAPP.DAO;
using ADOCOREWEBAPP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ADOCOREWEBAPP.Controllers;

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
		if(id== 0)
		{
			return NotFound("no id found");
		}
            else
            {
                
            var inventory = inventoryInterface.GetInventory(id);
		return View(inventory);
            }
	}
	public IActionResult Edit(int id)
	{
		if (id == 0)
		{
			return NotFound("No id found");
		}

		var inventory = inventoryInterface.GetInventory(id);

		if (inventory == null)
		{
			return NotFound("Inventory item not found");
		}

		return View(inventory);
	}

	[HttpPost]
	public IActionResult Edit(int id, Inventory updatedInventory)
	{
		if (id == 0)
		{
			return NotFound("No id found");
		}

		var existingInventory = inventoryInterface.GetInventory(id);

		if (existingInventory == null)
		{
			return NotFound("Inventory item not found");
		}

		// Update the properties of the existingInventory object with the new data
		existingInventory.Name = updatedInventory.Name;
		existingInventory.Price = updatedInventory.Price;
		existingInventory.Quantity = updatedInventory.Quantity;

		// Call the method to update the inventory item in your data store
		inventoryInterface.EditInventory(id, existingInventory);

		return RedirectToAction("Index");
	}


	public IActionResult Remove(int id, Inventory request)
	{
		if (id == 0)
		{
			return NotFound("No id found");
		}

		var inventory = inventoryInterface.GetInventory(id);

		if (inventory == null)
		{
			return NotFound("Inventory item not found");
		}

		// Call the method to remove the inventory item from your data store
		inventoryInterface.RemoveInventory(id, inventory);

		return RedirectToAction("Index");
	}

}
