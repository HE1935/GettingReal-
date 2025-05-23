using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Getting_Real_______
{
    internal class InventoryRepository : iProductRepository
    {
        private static string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data");

        public static void SetDataFolder(string folderPath)
        {
            try
            {
                dataFolder = folderPath;
                Directory.CreateDirectory(dataFolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved oprettelse af datamappe: {ex.Message}");
            }
        }

        public static void ResetDataFolder()
        {
            try
            {
                dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");
                Directory.CreateDirectory(dataFolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved nulstilling af datamappe: {ex.Message}");
            }
        }

        public static void SaveProducts(List<Products> products)
        {
            try
            {
                string path = Path.Combine(dataFolder, "inventory.txt");
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using StreamWriter writer = new StreamWriter(path);
                foreach (var product in products)
                {
                    writer.WriteLine(product.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved gemning af produkter: {ex.Message}");
            }
        }

        public static List<Products> LoadProducts()
        {
            List<Products> result = new();
            try
            {
                string path = Path.Combine(dataFolder, "inventory.txt");
                if (!File.Exists(path)) return result;

                foreach (var line in File.ReadAllLines(path))
                {
                    try
                    {
                        var product = Products.FromString(line);
                        if (product != null)
                            result.Add(product);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ugyldig linje springes over: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved indlæsning af produkter: {ex.Message}");
            }

            return result;
        }

        private int GenerateRandomId(List<Products> existingProducts)
        {
            try
            {
                Random random = new Random();
                int newId;
                do
                {
                    newId = random.Next(1000, 9999);
                } while (existingProducts.Any(p => p.Id == newId));
                return newId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved generering af ID: {ex.Message}");
                return -1;
            }
        }

        Products iProductRepository.addproducts(List<Products> products)
        {
            try
            {
                int id = GenerateRandomId(LoadProducts());
                Console.WriteLine($"Generated ID: {id}");

                Console.Write("Name? ");
                string name = Console.ReadLine();

                double price;
                while (true)
                {
                    Console.Write("Price? ");
                    if (double.TryParse(Console.ReadLine(), out price)) break;
                    Console.WriteLine("Invalid number. Try again.");
                }

                char location;
                while (true)
                {
                    Console.Write("Location (single character)? ");
                    if (char.TryParse(Console.ReadLine(), out location)) break;
                    Console.WriteLine("Invalid input. Try again.");
                }

                int currentStock;
                while (true)
                {
                    Console.Write("Current stock? ");
                    if (int.TryParse(Console.ReadLine(), out currentStock)) break;
                    Console.WriteLine("Invalid number. Try again.");
                }

                int minStock;
                while (true)
                {
                    Console.Write("Minimum stock? ");
                    if (int.TryParse(Console.ReadLine(), out minStock)) break;
                    Console.WriteLine("Invalid number. Try again.");
                }

                Console.Write("Prescription needed? ");
                string prescription = Console.ReadLine();

                return new Products(name, id, price, location, currentStock, minStock, prescription);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved oprettelse af produkt: {ex.Message}");
                return null;
            }
        }

        void iProductRepository.Deleteproduct(List<Products> products)
        {
            try
            {
                Console.Write("Enter the name or ID to delete: ");
                string input = Console.ReadLine();

                Products productToDelete = products.FirstOrDefault(p =>
                    p.Name.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                    (int.TryParse(input, out int id) && p.Id == id));

                if (productToDelete != null)
                {
                    products.Remove(productToDelete);
                    SaveProducts(products);
                    Console.WriteLine("Product deleted.");
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved sletning: {ex.Message}");
            }
        }

        Products iProductRepository.Updateproduct(List<Products> products)
        {
            try
            {
                Console.Write("Enter the name or ID of the product to update: ");
                string input = Console.ReadLine();

                int.TryParse(input, out int idInput);
                Products productToUpdate = products.FirstOrDefault(p =>
                    p.Name.Equals(input, StringComparison.OrdinalIgnoreCase) || p.Id == idInput);

                if (productToUpdate != null)
                {
                    Console.WriteLine("Leave input empty to keep current value.");

                    Console.Write($"New price ({productToUpdate.Price}): ");
                    string priceInput = Console.ReadLine();
                    if (double.TryParse(priceInput, out double newPrice))
                        productToUpdate.Price = newPrice;

                    Console.Write($"New location ({productToUpdate.Location}): ");
                    string locInput = Console.ReadLine();
                    if (char.TryParse(locInput, out char newLoc))
                        productToUpdate.Location = newLoc;

                    Console.Write($"New minimum stock ({productToUpdate.MinimumStock}): ");
                    string minInput = Console.ReadLine();
                    if (int.TryParse(minInput, out int newMin))
                        productToUpdate.MinimumStock = newMin;

                    Console.Write($"New current stock ({productToUpdate.CurrentStock}): ");
                    string currInput = Console.ReadLine();
                    if (int.TryParse(currInput, out int newCurr))
                        productToUpdate.CurrentStock = newCurr;

                    SaveProducts(products);
                    Console.WriteLine("Product updated.");
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }

                return productToUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved opdatering: {ex.Message}");
                return null;
            }
        }

        public void Checkout(List<Products> inventory)
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("--- Checkout ---");
                    Console.Write("Enter product name or ID (or type '0' to exit): ");
                    string input = Console.ReadLine();
                    if (input == "0") break;

                    int.TryParse(input, out int idInput);
                    Products product = inventory.FirstOrDefault(p =>
                        p.Name.Equals(input, StringComparison.OrdinalIgnoreCase) || p.Id == idInput);

                    if (product != null)
                    {
                        Console.WriteLine($"Found: {product.Name}, Stock: {product.CurrentStock}");
                        Console.Write("How many to check out? ");
                        string qtyInput = Console.ReadLine();

                        if (int.TryParse(qtyInput, out int qty))
                        {
                            if (qty <= 0)
                            {
                                Console.WriteLine("Quantity must be greater than 0.");
                            }
                            else if (qty > product.CurrentStock)
                            {
                                Console.WriteLine("Not enough stock.");
                            }
                            else
                            {
                                product.CurrentStock -= qty;
                                SaveProducts(inventory);
                                Console.WriteLine($"{qty} items checked out.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid quantity.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Product not found.");
                    }

                    Program.PressAnyKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl i checkout: {ex.Message}");
            }
        }

        public static void SearchProductDetails(List<Products> inventory)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Search Product ---");
                Console.Write("Enter product name or ID (or '0' to cancel): ");
                string input = Console.ReadLine();
                if (input == "0") return;

                int.TryParse(input, out int idInput);
                Products product = inventory.FirstOrDefault(p =>
                    p.Name.Equals(input, StringComparison.OrdinalIgnoreCase) || p.Id == idInput);

                if (product != null)
                {
                    Console.WriteLine($"\nName:           {product.Name}");
                    Console.WriteLine($"ID:             {product.Id}");
                    Console.WriteLine($"Price:          {product.Price} DKK");
                    Console.WriteLine($"Location:       {product.Location}");
                    Console.WriteLine($"Current Stock:  {product.CurrentStock}");
                    Console.WriteLine($"Minimum Stock:  {product.MinimumStock}");
                    Console.WriteLine($"Prescription:   {product.Prescription}");
                }
                else
                {
                    Console.WriteLine("Product not found.");
                }

                Program.PressAnyKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved søgning: {ex.Message}");
            }
        }
    }
}
