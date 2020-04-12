using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfVersion1._0
{
    [Serializable]
    public class KnapsackAlgorithmInput
    {
        static public Random rand = new Random();

        public KnapsackAlgorithmInput(KnapsackAlgorithmInput input)
        {
            TypesOfItems = input.TypesOfItems;
            Prices = input.Prices;
            Weights = input.Weights;
            Capacity = input.Capacity;
            WeightBorder = input.WeightBorder;
            PriceBorder = input.PriceBorder;
            MaxPrice = input.MaxPrice;
        }

        public KnapsackAlgorithmInput()
        {
            TypesOfItems = 5;
            Prices = new int[TypesOfItems];
            Weights = new int[TypesOfItems];
            Capacity = 5;

        }

        public KnapsackAlgorithmInput(int typesOfItems, int[] prices, int[] weights,
            int capacity, int priceBorder, int weightBorder)
        {
            TypesOfItems = typesOfItems;
            Prices = prices;
            Weights = weights;
            Capacity = capacity;
            WeightBorder = weightBorder;
            PriceBorder = priceBorder;
            MaxPrice = 0;
        }

        public KnapsackAlgorithmInput(int typesOfItems, int[] prices, int[] weights,
            int capacity, int maxPrice)
        {
            TypesOfItems = typesOfItems;
            Prices = prices;
            Weights = weights;
            Capacity = capacity;
            MaxPrice = maxPrice;
        }



        public static KnapsackAlgorithmInput GenerateFullRandomInput()
        {
            int priceBorder = rand.Next(1, 100000);
            int types = rand.Next(1, 100);
            int capacity = rand.Next(1, 100000);
            int weightBorder = rand.Next(1, capacity);

            int[] prices = new int[types];
            for (int i = 0; i < types; i++)
            {
                prices[i] = rand.Next(1, priceBorder);
            }

            int[] weights = new int[types];
            for (int i = 0; i < types; i++)
            {
                weights[i] = rand.Next(1, weightBorder);
            }

            KnapsackAlgorithmInput test = new KnapsackAlgorithmInput(types, prices, weights, capacity, priceBorder, weightBorder);
            return test;
        }

        public static KnapsackAlgorithmInput GenerateInput(int priceBorder, int weightBorder, int types)
        {
            int[] prices = new int[types];
            int capacity = rand.Next(weightBorder, 100000);

            for (int i = 0; i < types; i++)
            {
                prices[i] = rand.Next(priceBorder / 100, priceBorder);
            }

            int[] weights = new int[types];
            for (int i = 0; i < types; i++)
            {
                weights[i] = rand.Next(weightBorder / 100, weightBorder);
            }

            prices[rand.Next(0, types)] = priceBorder;
            weights[rand.Next(0, types)] = weightBorder;

            KnapsackAlgorithmInput test = new KnapsackAlgorithmInput(types, prices, weights, capacity, priceBorder, weightBorder);
            return test;
        }

        public static KnapsackAlgorithmInput GenerateInputCapacity(int capacity)
        {
            int priceBorder = rand.Next(1, 100000);
            int types = rand.Next(1, 100);
            int weightBorder = rand.Next(1, capacity);

            int[] prices = new int[types];
            for (int i = 0; i < types; i++)
            {
                prices[i] = rand.Next(1, priceBorder);
            }

            int[] weights = new int[types];
            for (int i = 0; i < types; i++)
            {
                weights[i] = rand.Next(1, weightBorder);
            }

            prices[rand.Next(0, types)] = priceBorder;
            weights[rand.Next(0, types)] = weightBorder;

            KnapsackAlgorithmInput test = new KnapsackAlgorithmInput(types, prices, weights, capacity, priceBorder, weightBorder);
            return test;
        }

        public int TypesOfItems { get; private set; }
        public int[] Prices { get; private set; }
        public int[] Weights { get; private set; }
        public int Capacity { get; private set; }
        public int WeightBorder { get; private set; }
        public int PriceBorder { get; private set; }
        public int MaxPrice { get; private set; }

        public static void InputToFile(string filePath, KnapsackAlgorithmInput input, bool append)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, append))
            {
                file.Write($"n{input.TypesOfItems}");
                file.Write($"c{input.Capacity}");
                file.Write("b");

                foreach (int weight in input.Weights)
                {
                    file.Write($"w{weight}");
                }
                file.Write("b");
                foreach (int price in input.Prices)
                {
                    file.Write($"p{price}");
                }
                file.Write("b");

                KnapsackSolution sol = Algorithm.DynamicProgrammingAlgorithm(input);

                file.WriteLine($"res{sol.FinalPrice}");
            }
        }

        public static void InputToFile(string filePath, KnapsackAlgorithmInput[] inputs, bool append)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, append))
            {
                foreach (KnapsackAlgorithmInput input in inputs)
                {
                    file.Write($"n{input.TypesOfItems}");
                    file.Write($"c{input.Capacity}");
                    file.Write("b");
                    foreach (int weight in input.Weights)
                    {
                        file.Write($"w{weight}");
                    }
                    file.Write("b");
                    foreach (int price in input.Prices)
                    {
                        file.Write($"p{price}");
                    }
                    file.Write("b");

                    KnapsackSolution sol = Algorithm.DynamicProgrammingAlgorithm(input);

                    file.WriteLine($"res{sol.FinalPrice}");
                }
            }
        }

        public static KnapsackAlgorithmInput[] InputFromFile(string filePath)
        {
            string[] separator = { "b", "c", "n", "res" };
            List<string[]> separatedLines = new List<string[]>();
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);

            string line;
            while ((line = file.ReadLine()) != null)
            {
                separatedLines.Add(line.Split(separator, StringSplitOptions.RemoveEmptyEntries));
            }

            KnapsackAlgorithmInput[] inputs = new KnapsackAlgorithmInput[separatedLines.Count];

            for (int i = 0; i < separatedLines.Count; i++)
            {
                int[] weights = separatedLines[i][2].Split('w').
                    Where(x => !string.IsNullOrWhiteSpace(x)).
                    Select(x => int.Parse(x)).ToArray();
                int[] prices = separatedLines[i][3].Split('p').
                    Where(x => !string.IsNullOrWhiteSpace(x)).
                    Select(x => int.Parse(x)).ToArray();
                int capacity;
                int.TryParse(separatedLines[i][1], out capacity);
                int typesOfItems;
                int.TryParse((separatedLines[i][0].Split(new char[] { 'n' })[0]), out typesOfItems);
                int maxPrice;
                int.TryParse(separatedLines[i][4], out maxPrice);
                inputs[i] = new KnapsackAlgorithmInput(typesOfItems, prices, weights, capacity, maxPrice);
            }

            return inputs;

        }

        public override string ToString()
        {
            string prices = "Our prices: ";
            foreach (int price in Prices)
            {
                prices += (price.ToString() + " ");
            }
            string weights = "Our weights: ";
            foreach (int weight in Weights)
            {
                weights += (weight.ToString() + " ");
            }

            return $"Capacity of our knapsack: {Capacity} \nNumber of different items: {TypesOfItems}" +
                $"\n{weights}\n{prices}\n\n";
        }
    }
}
