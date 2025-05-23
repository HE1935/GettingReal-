namespace Getting_Real_______
{
    internal class Program
    {
        static void Main(string[] args)
        {
            iProductRepository repository = new InventoryRepository();
            List<Products> products = InventoryRepository.LoadProducts();

            ShowMenu(repository, products);
        }

        public static void ShowMenu(iProductRepository repository, List<Products> inventory)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Inventory System ---");
                Console.WriteLine("1: Add Product");
                Console.WriteLine("2: Update Product");
                Console.WriteLine("3: Delete Product");
                Console.WriteLine("4: Search list");
                Console.WriteLine("5: Print Inventory list");
                Console.WriteLine("6: Print minimum stock list");
                Console.WriteLine("7: Checkout product");
                Console.WriteLine("0: Close program");
                Console.Write("Enter choice: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        {
                            MenuTitle("Add Product");
                            Products newProduct = repository.addproducts(inventory);
                            inventory.Add(newProduct);
                            InventoryRepository.SaveProducts(inventory);
                            PressAnyKey();
                        }
                        break;

                    case "2":
                        MenuTitle("Update Product");
                        repository.Updateproduct(inventory);
                        PressAnyKey();
                        break;

                    case "3":
                        MenuTitle("Delete Product");
                        repository.Deleteproduct(inventory);
                        PressAnyKey();
                        break;

                    case "4":
                        MenuTitle("Search list");
                        SearchProductDetails(inventory);
                        PressAnyKey();
                        break;

                    case "5":
                        MenuTitle("Print Inventory List");
                        foreach (var item in inventory)
                        {
                            Console.WriteLine(item);
                        }
                        PressAnyKey();
                        break;

                    case "6":
                        MenuTitle("Print Minimum Stock List");
                        foreach (var item in inventory.Where(p => p.CurrentStock < p.MinimumStock))
                        {
                            Console.WriteLine(item);
                        }
                        PressAnyKey();
                        break;

                    case "7":
                        MenuTitle("Checkout");
                        repository.Checkout(inventory);
                        break;


                    case "0":
                        Console.WriteLine("Ending program");
                        return;

                    default:
                        Console.WriteLine("Invalid choice");
                        PressAnyKey();
                        break;
                }
            }
        }

        public static void MenuTitle(string title)
        {
            Console.Clear();
            Console.WriteLine($"--- {title} ---\n");
        }

        public static void PressAnyKey()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public static void SearchProductDetails(List<Products> inventory)
        {
            Console.Clear();
            Console.WriteLine("--- Search Product Details ---");
            Console.Write("Enter product name or ID (or type '0' to return to main menu): ");
            string input = Console.ReadLine();

            if (input == "0")
                return;

            int.TryParse(input, out int parsedId);

            Products foundProduct = inventory.FirstOrDefault(p =>
                p.Name.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                p.Id == parsedId);


            if (foundProduct != null)
            {
                Console.WriteLine("\nProduct found:");
                Console.WriteLine($"Name:           {foundProduct.Name}");
                Console.WriteLine($"ID:             {foundProduct.Id}");
                Console.WriteLine($"Price:          {foundProduct.Price} DKK");
                Console.WriteLine($"Location:       {foundProduct.Location}");
                Console.WriteLine($"Current Stock:  {foundProduct.CurrentStock}");
                Console.WriteLine($"Minimum Stock:  {foundProduct.MinimumStock}");
                Console.WriteLine($"Prescription:   {foundProduct.Prescription}");
            }
            else
            {
                Console.WriteLine("\nProduct not found.");
            }


        }
    }
}
