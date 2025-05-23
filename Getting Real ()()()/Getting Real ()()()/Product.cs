using System;

namespace Getting_Real_______
{
    public class Products
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public double Price { get; set; }
        public char Location { get; set; }
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
        public string Prescription { get; set; }

        // Fuld konstruktør
        public Products(string name, int id, double price, char location, int currentStock, int minimumStock, string prescription)
        {
            try
            {
                Name = name;
                Id = id;
                Price = price;
                Location = location;
                CurrentStock = currentStock;
                MinimumStock = minimumStock;
                Prescription = prescription;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl i konstruktøren: {ex.Message}");
            }
        }

        // Laver tekstlinje til objekt
        public static Products FromString(string productString)
        {
            try
            {
                string[] parts = productString.Split('|');

                if (parts.Length != 7)
                    throw new FormatException("Forkert antal felter i linjen.");

                string name = parts[0];

                if (!int.TryParse(parts[1], out int id))
                    throw new FormatException($"Ugyldigt ID: {parts[1]}");

                if (!double.TryParse(parts[2], out double price))
                    throw new FormatException($"Ugyldig pris: {parts[2]}");

                if (string.IsNullOrEmpty(parts[3]) || parts[3].Length != 1)
                    throw new FormatException($"Ugyldig placering: {parts[3]}");

                char location = parts[3][0];

                if (!int.TryParse(parts[4], out int currentStock))
                    throw new FormatException($"Ugyldig beholdning: {parts[4]}");

                if (!int.TryParse(parts[5], out int minimumStock))
                    throw new FormatException($"Ugyldigt minimumsbeholdning: {parts[5]}");

                string prescription = parts[6];

                return new Products(name, id, price, location, currentStock, minimumStock, prescription);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved parsing af produktstreng: {ex.Message}");
                return null; // Kan behandles som "fejlbehæftet" af kaldende metode
            }
        }

        // Gemmes som tekstlinje
        public override string ToString()
        {
            try
            {
                return $"{Name}|{Id}|{Price}|{Location}|{CurrentStock}|{MinimumStock}|{Prescription}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl i ToString(): {ex.Message}");
                return string.Empty;
            }
        }
    }
}
