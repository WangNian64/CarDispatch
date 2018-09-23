using System;
using System.Collections.Generic;
using System.Text;
using CarDispatch.Objects;
namespace CarDispatch.Tools
{
    public class Tool
    {
        //计算图中两点间的最短路径
        public static float Djstl(TrailerGraph g, int s, int e)
        {
            float shortLength = 0;
            int[] label = new int[g.numVertex]; //标记是否找过
            for (int i = 0; i < label.Length; i++)
            {
                label[i] = -1;
            }
            label[0] = 1;
            Console.Write(s.ToString() + " ");
            while (s != e)
            {
                int start = s;
                float low = 10000;
                int k = s;
                for (int j = 0; j < g.numVertex; j++)
                {
                    if (label[j] == -1 && low > g.adjMatrix[s, j].weight && g.adjMatrix[s, j].weight != 0)
                    {
                        low = g.adjMatrix[s, j].weight;
                        k = j;
                    }
                }
                label[s] = 1;
                s = k;
                int end = s;
                shortLength += g.adjMatrix[start, end].weight;
            }
            return shortLength;
        }
    }
}
