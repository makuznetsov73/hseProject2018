using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfVersion1._0
{
    [Serializable]
    public class KnapsackSolutions
    {
        public List<KnapsackSolution> Solutions { get; }
        public TimeSpan Time { get; }

        public KnapsackSolutions(List<KnapsackSolution> solutions, TimeSpan time)
        {
            Solutions = solutions;
            Time = time;
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < Solutions.Count; i++)
            {
                res += (Solutions[i] + "\n");
            }
            res += $"Our final time: {Time}";
            return res;
        }
    }
}
