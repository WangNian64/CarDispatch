using System;
using System.Collections;
using CarDispatch.Objects;
using System.Collections.Generic;
namespace CarDispatch
{
    class Program
    {
        #region 计算匹配相关
        //得到全部的匹配组合
        public static void getMatchList(ref List<Dictionary<Car, Task>> matchList, List<Car> cars, List<Task> tasks, int begin, int end)
        {
            if (begin == end)
            {
                //添加一个匹配
                Dictionary<Car, Task> match = new Dictionary<Car, Task>();
                for (int i = 0; i < cars.Count; i++)
                {
                    match.Add(cars[i], tasks[i]);
                }
                matchList.Add(match);
            }
            for (int i = begin; i < end; i++)
            {
                if (IsSwap(tasks, begin, i))
                {
                    Swap(tasks, begin, i);
                    getMatchList(ref matchList, cars, tasks, begin + 1, end);
                    Swap(tasks, begin, i);
                }
            }
        }
        //判断是否重复,重复的话要交换
        public static bool IsSwap(List<Task> tasks, int nBegin, int nEnd)
        {
            for (int i = nBegin; i < nEnd; i++)
                if (tasks[i] == tasks[nEnd])
                    return false;
            return true;
        }
        //交换数组中指定元素
        static void Swap(List<Task> tasks, int x, int y)
        {
            Task t = tasks[x];
            tasks[x] = tasks[y];
            tasks[y] = t;
        }
        #endregion

        //给小车分配任务
        public static void DistributeTasks(Dictionary<Car, Task> match)
        {
            foreach (KeyValuePair<Car, Task> kvp in match)
            {
                Car car = kvp.Key;
                Task task = kvp.Value;
                
            }
        }
        static void Main(string[] args)
        {
            //初始化轨道
            TrailerGraph trailer = new TrailerGraph().createTrailer();

            //初始化小车(按照车的前后顺序)
            float carSpeed = 1f;
            List<Car> cars = new List<Car>();
            Car car1 = new Car(new Position(0, 15, 0), new EdgeData(), CarState.Empty, carSpeed);
            Car car2 = new Car(new Position(-5, 10, 0), new EdgeData(), CarState.Empty, carSpeed);
            cars.Add(car1);
            cars.Add(car2);

            //添加任务队列
            List<Task> taskList = new List<Task>();
            Task task1 = new Task(new Position(5, 5, 0), new EdgeData(2, 3, LineType.Straight, 10),
                                  new Position(-5, 5, 0), new EdgeData(6, 1, LineType.Straight, 10));
            Task task2 = new Task(new Position(5, -5, 0), new EdgeData(3, 4, LineType.Straight, 10),
                                  new Position(-5, -5, 0), new EdgeData(5, 6, LineType.Straight, 10));
            taskList.Add(task1);
            taskList.Add(task2);

            //得到所有的匹配组合matchList
            List<Dictionary<Car, Task>> matchList = new List<Dictionary<Car, Task>>();
            getMatchList(ref matchList, cars, taskList, 0, cars.Count);
            #region
            //计算不同排列下的任务实现总时间，每次只优化任务队列的前2个任务
            //如果小车已经在运送了
            //只实现一帧的计算
            //分为任务和小车
            //先得到当前的任务队列(先假设只处理前2个任务)
            //假设同时只有2辆车（车有位置等信息）
            //计算不同排列下的任务实现总时间
            //假设只能顺时针运动
            //难点：计算路径长度
            //还要考虑最短路径（虽然只有一个运动方向）、、、
            //得出最优方案
            //根据最优方案调整小车的运动
            #endregion
            //计算每个组合的总时间
            //并求出其中最短时间的组合
            float[] times = new float[cars.Count];
            int index = 0;
            float minTime = 10000000f;//最短时间
            Dictionary<Car, Task> minMatch = matchList[0];//最短时间对应的匹配
            foreach (Dictionary<Car, Task> match in matchList)
            { 
                times[index] = trailer.calcuMultiTaskTime(match);
                if(times[index] < minTime)
                {
                    minTime = times[index];
                    minMatch = match;
                }
                index++;
            }
            //给各个小车分发任务
            DistributeTasks(minMatch);
            Console.ReadKey();
        }
    }
}
