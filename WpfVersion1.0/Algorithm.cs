using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfVersion1._0
{
    public static class Algorithm
    {
        public delegate KnapsackSolution AlgorithmDelegate(KnapsackAlgorithmInput test);
        static double eParam = 0.2;
        public static int index = 0;

        public static double EParam
        {
            get
            {
                return eParam;
            }
            set
            {
                eParam = value;
            }
        }

        public static KnapsackSolution GreedyAlgorithm(KnapsackAlgorithmInput test)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            int[] priceWeightRatio = new int[test.TypesOfItems];
            int[] pricesCopy = (int[])test.Prices.Clone();
            int[] weightsCopy = (int[])test.Weights.Clone();

            for (int i = 0; i < test.TypesOfItems; i++)
            {
                priceWeightRatio[i] = test.Prices[i] / test.Weights[i];
            }
            Array.Sort((int[])priceWeightRatio.Clone(), pricesCopy);
            Array.Sort((int[])priceWeightRatio.Clone(), weightsCopy);
            //Array.Sort(priceWeightRatio);

            List<int[]> list = new List<int[]>();

            int totalWeight = 0;
            int totalPrice = 0;
            int itemIndex = test.TypesOfItems - 1;
            while (totalWeight < test.Capacity && itemIndex >= 0)
            {
                int index1 = 0;
                while (totalWeight + weightsCopy[itemIndex] < test.Capacity)
                {                    
                    totalWeight += weightsCopy[itemIndex];
                    totalPrice += pricesCopy[itemIndex];
                    if (totalWeight + weightsCopy[itemIndex] >= test.Capacity)
                    {
                        list.Add(new int[] { index1 + 1, weightsCopy[itemIndex]});
                    }
                    index1++;
                }
                itemIndex--;
            }

            startTime.Stop();
            return new KnapsackSolution(startTime.Elapsed, totalPrice, test.TypesOfItems, totalWeight,
                test.Capacity, test.MaxPrice, test, list, false);
        }

        public static KnapsackSolution DynamicProgrammingAlgorithm(KnapsackAlgorithmInput test)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            List<int[]>[] weightsArray = new List<int[]>[test.Capacity + 1];
            weightsArray[0] = new List<int[]>();
            int[] knapsack = new int[test.Capacity + 1];
            knapsack[0] = 0;
            int[] totalWeights = new int[test.Capacity + 1];
            totalWeights[0] = 0;

            for (int i = 1; i <= test.Capacity; i++)
            {
                int max = knapsack[i - 1];
                int weight = totalWeights[i - 1];
                List<int[]> maxList = new List<int[]>(weightsArray[i - 1]);
                for (int j = 0; j < test.TypesOfItems; j++)
                {
                    int x = i - test.Weights[j];
                    List<int[]> duplicateXList;
                    if (x >= 0)
                    {
                        duplicateXList = new List<int[]>(weightsArray[x]);
                        if ((knapsack[x] + test.Prices[j]) > max)
                        {
                            max = knapsack[x] + test.Prices[j];
                            weight = totalWeights[x] + test.Weights[j];
                            duplicateXList.Add(new int[2] { 1, test.Weights[j] });
                            maxList = new List<int[]>(duplicateXList);
                        }
                    }
                    weightsArray[i] = new List<int[]>(maxList);
                    knapsack[i] = max;
                    totalWeights[i] = weight;
                }
            }
            
            for (int i = 0; i < weightsArray[test.Capacity].Count; i++)
            {
                int number = weightsArray[test.Capacity][i][1];
                for (int j = i + 1; j < weightsArray[test.Capacity].Count; j++)
                {
                    if (weightsArray[test.Capacity][j][1] == weightsArray[test.Capacity][i][1])
                    {
                        weightsArray[test.Capacity][i][0]++;
                        weightsArray[test.Capacity].RemoveAt(j);
                        j--;
                    }

                }
            }

            startTime.Stop();
            return new KnapsackSolution(startTime.Elapsed, knapsack[test.Capacity], test.TypesOfItems,
                totalWeights[test.Capacity], test.Capacity, test, weightsArray[test.Capacity], false);
        }

        public static KnapsackSolution DynamicProgrammingAlgorithmFptas(KnapsackAlgorithmInput test, double K)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            double[] knapsack = new double[test.Capacity + 1];
            knapsack[0] = 0;
            int[] totalWeights = new int[test.Capacity + 1];
            totalWeights[0] = 0;

            for (int i = 1; i <= test.Capacity; i++)
            {
                double max = knapsack[i - 1];
                int weight = totalWeights[i - 1];
                for (int j = 0; j < test.TypesOfItems; j++)
                {
                    int x = i - test.Weights[j];
                    if (x >= 0 && (knapsack[x] + test.Prices[j]) > max)
                    {
                        max = (knapsack[x] + test.Prices[j]) * K;
                        weight = totalWeights[x] + test.Weights[j];
                    }
                    knapsack[i] = max;
                    totalWeights[i] = weight;
                }
            }

            startTime.Stop();
            return new KnapsackSolution(startTime.Elapsed, (int)knapsack[test.Capacity], test.TypesOfItems,
                totalWeights[test.Capacity], test.Capacity, test.MaxPrice, test, new List<int[]>(), false);
        }

        public static KnapsackSolution BranchBoundAlgorithm(KnapsackAlgorithmInput test)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            int[] priceWeightRatio = new int[test.TypesOfItems];
            int[] pricesCopy = (int[])test.Prices.Clone();
            int[] weightsCopy = (int[])test.Weights.Clone();

            for (int i = 0; i < test.TypesOfItems; i++)
            {
                priceWeightRatio[i] = test.Prices[i] / test.Weights[i];
            }
            Array.Sort((int[])priceWeightRatio.Clone(), pricesCopy);
            Array.Sort((int[])priceWeightRatio.Clone(), weightsCopy);

            int totalPrice = 0;
            int totalWeight = 0;

            Array.Reverse(pricesCopy);
            Array.Reverse(weightsCopy);

            BranchesBound(pricesCopy, weightsCopy, test.Capacity, 0, totalPrice, totalWeight,
                ref totalPrice, ref totalWeight, startTime);

            startTime.Stop();

            return new KnapsackSolution(startTime.Elapsed, totalPrice, test.TypesOfItems, totalWeight,
                test.Capacity, test.MaxPrice, test, new List<int[]>(), true);
        }//*/
            
        public static KnapsackSolution FptasAlgorithm(KnapsackAlgorithmInput test)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();

            int[] weightsCopy = (int[])test.Weights.Clone();
            int[] pricesCopy1 = (int[])test.Prices.Clone();
            int[] pricesCopy2 = (int[])test.Prices.Clone();
            Array.Sort(pricesCopy1);

            double K = pricesCopy1[pricesCopy1.Length - 1] * EParam / test.TypesOfItems;

            for (int i = 0; i < test.TypesOfItems; i++)
            {
                pricesCopy2[i] = (int)(pricesCopy2[i] / K);
            }

            KnapsackAlgorithmInput newInput = new KnapsackAlgorithmInput(test.TypesOfItems, pricesCopy2,
                test.Weights, test.Capacity, test.PriceBorder, test.WeightBorder);

            KnapsackSolution sol = DynamicProgrammingAlgorithm(newInput);

            startTime.Stop();
            return new KnapsackSolution(startTime.Elapsed, (int)(sol.FinalPrice * K), test.TypesOfItems, sol.FinalWeight,
                test.Capacity, test.MaxPrice, test, sol.Weights, false);
        }

        public static void BranchesBound(int[] prices, int[] weights, int capacity,
            int step, int totalPriceBranch, int totalWeightBranch, ref int totalPrice,
            ref int totalWeight, System.Diagnostics.Stopwatch time)
        {
            int weightIndex = 0;
            int newTotalWeightBranch;
            int newTotalPriceBranch;
            //Console.WriteLine(totalPrice + "  3  " + index);
            if (step < prices.Length && totalWeightBranch + weights[step] <= capacity)
            {
                while (totalWeightBranch + weights[step] * weightIndex <= capacity)
                {  
                    newTotalWeightBranch = totalWeightBranch + weightIndex * weights[step];
                    newTotalPriceBranch = totalPriceBranch + weightIndex * prices[step];
                    //Console.WriteLine(totalPriceBranch + "   " + step);
                    int nextStep = step + 1;
                    index += 1;
                    BranchesBound(prices, weights, capacity,
                        nextStep, newTotalPriceBranch, newTotalWeightBranch,
                        ref totalPrice, ref totalWeight, time);
                    weightIndex++;
                }
            }
            else
            {
                if (totalPriceBranch > totalPrice)
                {
                    totalWeight = totalWeightBranch;
                    totalPrice = totalPriceBranch;
                }
            }
        }

        public static KnapsackSolutions BigTest(List<KnapsackAlgorithmInput>tests, AlgorithmDelegate Algorithm)
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();
            TimeSpan[] timeArray = new TimeSpan[tests.Count];
            List<KnapsackSolution> solutionPack = new List<KnapsackSolution>();

            foreach(KnapsackAlgorithmInput test in tests)
            {
                solutionPack.Add(Algorithm(test));
            }
            startTime.Stop();
            return new KnapsackSolutions(solutionPack, startTime.Elapsed);
        }//*/
    }
}
