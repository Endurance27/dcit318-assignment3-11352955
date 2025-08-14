using System;
using System.Collections.Generic;

// Marker interface
public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}

// Product classes
public class ElectronicItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public string Brand { get; }
    public int WarrantyMonths { get; }

    public ElectronicItem(int id, string name, int qty, string brand, int warranty)
    {
        Id = id; Name = name; Quantity = qty; Brand = brand; WarrantyMonths = warranty;
    }

    public override string ToString() => $"{Name} ({Brand}), Qty: {Quantity}, Warranty: {WarrantyMonths} months";
}

public class GroceryItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; }

    public GroceryItem(int id, string name, int qty, DateTime expiry)
    {
        Id = id; Name = name; Quantity = qty; ExpiryDate = expiry;
    }

    public override string ToString() => $"{Name}, Qty: {Quantity}, Expires: {ExpiryDate.ToShortDateString()}";
}

// Custom exceptions
public class DuplicateItemException : Exception
{
    public DuplicateItemException(string msg) : base(msg) { }
}
public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string msg) : base(msg) { }
}
public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string msg) : base(msg) { }
}

// Generic repository
public class InventoryRepository<T> where T : IInventoryItem
{
    private Dictionary<int, T> _items = new();

    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
            throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
        _items[item.Id] = item;
    }

    public T GetItemById(int id)
    {
        if (!_items.ContainsKey(id))
            throw new ItemNotFoundException($"Item with ID {id} not found.");
        return _items[id];
    }

    public void RemoveItem(int id)
    {
        if (!_items.Remove(id))
            throw new ItemNotFoundException($"Cannot remove item with ID {id} - not found.");
    }

    public List<T> GetAllItems() => new List<T>(_items.Values);

    public void UpdateQuantity(int id, int newQty)
    {
        if (newQty < 0)
            throw new InvalidQuantityException("Quantity cannot be negative.");
        var item = GetItemById(id);
        item.Quantity = newQty;
    }
}

// Warehouse manager
public class WareHouseManager
{
    private InventoryRepository<ElectronicItem> _electronics = new();
    private InventoryRepository<GroceryItem> _groceries = new();

    public void SeedData()
    {
        _electronics.AddItem(new ElectronicItem(1, "Laptop", 5, "Dell", 24));
        _electronics.AddItem(new ElectronicItem(2, "Phone", 10, "Samsung", 12));
        _groceries.AddItem(new GroceryItem(1, "Rice", 50, DateTime.Now.AddMonths(6)));
        _groceries.AddItem(new GroceryItem(2, "Milk", 30, DateTime.Now.AddMonths(1)));
    }

    public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
    {
        foreach (var item in repo.GetAllItems())
            Console.WriteLine(item);
    }

    public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
    {
        try
        {
            var item = repo.GetItemById(id);
            repo.UpdateQuantity(id, item.Quantity + quantity);
            Console.WriteLine($"Stock updated: {item.Name}, New Qty: {item.Quantity}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
    {
        try
        {
            repo.RemoveItem(id);
            Console.WriteLine($"Item with ID {id} removed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void TestExceptions()
    {
        try
        {
            _electronics.AddItem(new ElectronicItem(1, "Duplicate Laptop", 3, "HP", 12));
        }
        catch (DuplicateItemException ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }

        try
        {
            _groceries.RemoveItem(999);
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }

        try
        {
            _electronics.UpdateQuantity(2, -5);
        }
        catch (InvalidQuantityException ex)
        {
            Console.WriteLine($"Caught: {ex.Message}");
        }
    }

    public void Run()
    {
        SeedData();
        Console.WriteLine("Grocery Items:");
        PrintAllItems(_groceries);
        Console.WriteLine("\nElectronic Items:");
        PrintAllItems(_electronics);
        TestExceptions();
    }
}

public class Program3
{
    public static void Main()
    {
        var manager = new WareHouseManager();
        manager.Run();
    }
}
