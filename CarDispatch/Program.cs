using System;
using System.Collections;
using CarDispatch.Objects;
namespace CarDispatch
{
    class Program
    {
        //得到最优序列
        public static void getBestOrder(Queue taskQueue, ArrayList cars)
        {
            //简化问题，假设轨道上只有2个车,同时最多2个任务
            ArrayList times = new ArrayList();//存放所有的时间
            //计算所有的排列组合
            //车和需求要绑定，且有先后顺序
            //如车1-任务1，然后车2-任务2
            //总共有4种情况
            foreach(Task t in taskQueue)
            {
                
            }
        }
        static void Main(string[] args)
        {
            float carSpeed = 1f;
            ArrayList cars = new ArrayList();
            Car car1 = new Car(new Position(0, 15, 0), new EdgeData(), CarState.Empty, carSpeed);
            Car car2 = new Car(new Position(-5, 10, 0), new EdgeData(), CarState.Empty, carSpeed);
            cars.Add(car1);
            cars.Add(car2);

            Queue taskQueue = new Queue();
            Task task1 = new Task(new Position(5, 5, 0), new EdgeData(2, 3, LineType.Straight, 10), 
                                  new Position(-5, 5, 0), new EdgeData(6, 1, LineType.Straight, 10));
            Task task2 = new Task(new Position(5, -5, 0), new EdgeData(3, 4, LineType.Straight, 10),
                                  new Position(-5, -5, 0), new EdgeData(5, 6, LineType.Straight, 10));
            taskQueue.Enqueue(task1);
            taskQueue.Enqueue(task2);


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
            Console.ReadKey();
        }
    }
}
