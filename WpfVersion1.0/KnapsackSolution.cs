using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfVersion1._0
{
    [Serializable]
    public class KnapsackSolution
    {
        public KnapsackSolution(TimeSpan time, int finalPrice, int typesOfItems, int finalWeight,
            int capacity, KnapsackAlgorithmInput input, List<int[]> weights, bool bbTrigger)
        {
            Time = time;
            FinalPrice = finalPrice;
            TypesOfItems = typesOfItems;
            Capacity = capacity;
            FinalWeight = finalWeight;
            Input = input;
            if (bbTrigger)
            {
                Weights = Algorithm.DynamicProgrammingAlgorithm(input).Weights;
            }
            else
                Weights = weights;
        }

        public KnapsackSolution(TimeSpan time, int finalPrice, int typesOfItems, int finalWeight,
            int capacity, int maxPrice, KnapsackAlgorithmInput input, List<int[]> weights, bool bbTrigger)
        {
            Time = time;
            FinalPrice = finalPrice;
            TypesOfItems = typesOfItems;
            Capacity = capacity;
            FinalWeight = finalWeight;
            MaxPrice = maxPrice;
            Input = input;
            if (bbTrigger)
            {
                Weights = Algorithm.DynamicProgrammingAlgorithm(input).Weights;
            }
            else
                Weights = weights;
        }

        public KnapsackAlgorithmInput Input { get; }
        public TimeSpan Time { get; }
        public int FinalPrice { get; }
        public int FinalWeight { get; }
        public int Capacity { get; }
        public int TypesOfItems { get; }
        public int MaxPrice { get; set; }
        public List<int[]> Weights { get; set; }

        public override string ToString()
        {
            if (MaxPrice != 0)
            {
                return $"Algorithm running time: {Time}, final price: {FinalPrice} out of {MaxPrice}, final weight: {FinalWeight} / {Capacity}, " +
                    $"we had {TypesOfItems} different items, our accuracy is {FinalPrice / MaxPrice * 100} %";
            }
            string str = $"Algorithm running time: {Time}, final price: {FinalPrice}, final weight: {FinalWeight} / {Capacity}, " +
                    $"we had {TypesOfItems} different items\nItems we take:\n";
            foreach (int[] array in Weights)
            {
                str += $"{array[0]} items with a weight of {array[1]}\n";
            }
            return str;
        }
    }
}

