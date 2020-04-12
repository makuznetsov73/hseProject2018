using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfVersion1._0
{
    [Serializable]
    public class KnapsackAlgorithmInputs
    {
        public List<KnapsackAlgorithmInput> Inputs { get; set; }
        public KnapsackAlgorithmInputs(List<KnapsackAlgorithmInput> inputs)
        {
            Inputs = inputs;
        }
        public KnapsackAlgorithmInputs(KnapsackAlgorithmInputs inputs)
        {
            Inputs = new List<KnapsackAlgorithmInput>();
            foreach (KnapsackAlgorithmInput input in inputs.Inputs)
            {
                Inputs.Add(new KnapsackAlgorithmInput(input));
            }
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < Inputs.Count; i++)
            {
                res += $"Input number {i}:\n";
                string prices = "Our prices: ";
                foreach (int price in Inputs[i].Prices)
                {
                    prices += (price.ToString() + " ");
                }
                string weights = "Our weights: ";
                foreach (int weight in Inputs[i].Weights)
                {
                    weights += (weight.ToString() + " ");
                }

                res += $"Capacity of our knapsack: {Inputs[i].Capacity} \nNumber of different items: {Inputs[i].TypesOfItems}" +
                    $"\n{weights}\n{prices}\n\n";
            }
            return res;
        }
    }
}
