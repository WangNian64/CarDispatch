using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using CarDispatch.Tools;
//轨道结构，用有向图表示
namespace CarDispatch.Objects
{

    public enum LineType
    {
        Straight=0, Curve=1
    }
    //装货还是卸货
    public enum TaskType
    {
        Load=0, UnLoad=1
    }
    public struct EdgeData
    {
        public int vertexNum1;
        public int vertexNum2;
        public LineType lineType;
        public float weight;
        public EdgeData(int v1, int v2, LineType lt, float w)
        {
            vertexNum1 = v1;
            vertexNum2 = v2;
            lineType = lt;
            weight = w;
        }
    }
    //结点
    public class Vertex
    {
        public string data;//顶点数据
        public Position vertexPos;//顶点的坐标
        public bool IsVisited;
        public Vertex(string _data, Position _vertexPos)
        {
            this.data = _data;
            this.vertexPos = _vertexPos;
        }
    }
    public class TrailerGraph
    {
        //TrailerGraph有关属性
        private float lineLength = 10.0f;//直线段长度
        private float curveRadius = 5.0f;//半径
        public float Infinity = 100000f;
        //图中所能包含的点上限
        private const int MAX_NUMBER = 100;
        //顶点数组
        public Vertex[] vertexes;
        //邻接矩阵
        public EdgeData[,] adjMatrix;
        //统计当前图中有几个点
        public int numVertex = 0;
        //初始化num个点的图
        public TrailerGraph(int num = MAX_NUMBER)
        {
            //初始化邻接矩阵和顶点数组
            adjMatrix = new EdgeData[num, num];
            vertexes = new Vertex[num];
            //将代表邻接矩阵的表全初始化为0
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    adjMatrix[i, j].lineType = LineType.Straight;
                    adjMatrix[i, j].vertexNum1 = i;
                    adjMatrix[i, j].vertexNum2 = j;
                    adjMatrix[i, j].weight = Infinity;//初始化weight为最大值
                }
            }
        }
        //向图中添加节点
        public void AddVertex(String v, Position pos)
        {
            vertexes[numVertex] = new Vertex(v, pos);
            numVertex++; 
        }
        //向图中添加有向边,weight就是两点之间的距离
        public void AddEdge(int vertex1, int vertex2, LineType lineType, float weight)
        {
            adjMatrix[vertex1, vertex2].vertexNum1 = vertex1;
            adjMatrix[vertex1, vertex2].vertexNum2 = vertex2;
            adjMatrix[vertex1, vertex2].lineType = lineType;
            adjMatrix[vertex1, vertex2].weight = weight;

        }
        //显示点
        public void DisplayVert(int vertexPosition)
        {
            Console.WriteLine(vertexes[vertexPosition] + " ");
        }
        //生成一个轨道图的对象
        public TrailerGraph createTrailer(int numVertex)
        {
            TrailerGraph trailerGraph = new TrailerGraph(numVertex);
            float curveLength = MathF.PI * curveRadius;
            //数据
            EdgeData[] edgeDatas = {
                new EdgeData(1, 2, LineType.Curve, curveLength),
                new EdgeData(2, 3, LineType.Straight, lineLength),
                new EdgeData(3, 4, LineType.Straight, lineLength),
                new EdgeData(3, 6, LineType.Curve, curveLength),
                new EdgeData(4, 5, LineType.Curve, curveLength),
                new EdgeData(5, 6, LineType.Straight, lineLength),
                new EdgeData(6, 1, LineType.Straight, lineLength)
            };
            //只需要x、y
            Vertex[] vertexes = {
                new Vertex(null, new Position(-5, 10, 0)),
                new Vertex(null, new Position(5, 10, 0)),
                new Vertex(null, new Position(5, 0, 0)),
                new Vertex(null, new Position(5, -10, 0)),
                new Vertex(null, new Position(-5, -10, 0)),
                new Vertex(null, new Position(-5, 0, 0))
            };
            //给点赋值
            for (int i = 0; i < numVertex; i++)
            {
                trailerGraph.vertexes[i] = new Vertex(vertexes[i].data, vertexes[i].vertexPos);
            }
            //给边赋值
            for (int i = 0; i < numVertex; i++)
            {
                trailerGraph.AddEdge(numVertex, numVertex, LineType.Straight, 0);
            }
            for (int i = 0; i < edgeDatas.Length; i++)
            {
                trailerGraph.AddEdge(edgeDatas[i].vertexNum1, edgeDatas[i].vertexNum2, edgeDatas[i].lineType, edgeDatas[i].weight);
            }
            return trailerGraph;
        }
        //计算车到任务点的距离(这里只包括取货或者放货)
        public float getShortLength(Car car, Task task, TaskType taskType)
        {
            float shortLength = 0.0f;
            int startNum = car.car_EdgeData.vertexNum2;
            int endNum = 0;
            if (taskType == TaskType.Load)
            {
                endNum = task.loadGoods_EdgeData.vertexNum1;
            }
            else
            {
                endNum = task.unloadGoods_EdgeData.vertexNum1;
            }
            //使用djstra算法
            shortLength += CarDispatch.Tools.Tool.Djstl(this, startNum, endNum);
            //计算剩余的长度（包括小车到所在线段终点的距离+货物到达所在线段起点的距离）
            float lineLength1, lineLength2;
            lineLength1 = lineLength2 = 0.0f;
            Position carPos = car.carPos;//车的位置
            Position carVertexPos = this.vertexes[startNum].vertexPos;//车所在曲线的终点位置
            //求lineLength1
            if (car.car_EdgeData.lineType == LineType.Curve)
            {
                float distance = MathF.Sqrt(MathF.Pow(carPos.x - carVertexPos.x, 2)
                    + MathF.Pow(carPos.y - carVertexPos.y, 2));
                //注意ACos返回的是弧度值
                float degree = MathF.Acos(distance * 0.5f / curveRadius) * 2;//两点在圆弧上的圆心角度
                lineLength1 = degree * curveRadius;//弧长 = 半径 * 弧度
            }
            else
            {
                lineLength1 = MathF.Sqrt(MathF.Pow(carPos.x - carVertexPos.x, 2)
                    + MathF.Pow(carPos.y - carVertexPos.y, 2));
            }
            Position taskPos = task.loadGoodsPos;
            Position taskVertexPos = this.vertexes[endNum].vertexPos;//取货位置所在曲线起点的位置
            //求lineLength2
            if (task.loadGoods_EdgeData.lineType == LineType.Curve)
            {
                float distance = MathF.Sqrt(MathF.Pow(taskPos.x - taskVertexPos.x, 2)
                    + MathF.Pow(taskPos.y - taskVertexPos.y, 2));
                //
                float degree = MathF.Acos(distance * 0.5f / curveRadius) * 2;
                lineLength2 = degree * curveRadius;
            }
            //总长度=lineLength1 + 中间路径 + lineLength2;
            shortLength += lineLength1 + lineLength2;
            return shortLength;
        }
        //计算一个某车-某任务每一段的时间长度(假定小车匀速运动)
        public float calcuTaskTime(Car car, Task task)
        {
            //取货中小车的运动时间
            float reachLoadTime = 0.0f;
            if (getShortLength(car, task, TaskType.Load) != 0)
            {
                reachLoadTime = getShortLength(car, task, TaskType.Load) / car.speed;
            }
            //小车装货时间
            float loadTime = task.loadTime;
            //放货中小车的运动时间
            float reachUnloadTime = 0.0f;
            if (getShortLength(car, task, TaskType.UnLoad) != 0)
            {
                reachUnloadTime = getShortLength(car, task, TaskType.Load) / car.speed;
            }
            //小车卸货时间
            float unloadTime = task.unloadTime;
            float taskTime = reachLoadTime + loadTime + reachUnloadTime + unloadTime;
            return taskTime;
        }
        
        //计算多任务、多小车下的总时间(已经配对)
        //问题：现在要在一帧里要计算未来多帧内 多个小车处理多个任务的总时间
        //因为小车在不断运动，所以多帧之后小车的位置会发生很大改变，这样你就需要在一帧内获得小车多帧后的位置
        //解决：使用近似处理，因为运货速度很快，而装货速度慢，假设装货结束车2就在车1后面停止(有安全距离)
        //即车1任务结束后，设置车2的位置为车1任务的终点

        //问题：车1和车2的总时间计算
        //车1在运动时，车2也能运动，而车1停下时，车2可能和车1相撞，
        //车2中间会因为车1停下 要等待，总时间 要综合两车的时间，算出最大值
        //车1的总时间里没有等待其他车的时间
        //车2在车1停下装货/卸货时需要等待，在车1运动时不需要停止

        //现在的问题：类似打印机和CPU处理多进程的问题，当两车都在运动时，时间不会增加
        //当车1停下时，车2也要停下，车1运动后车2也能运动

        
        //解决：先计算两辆车每段的时间，车1的所有阶段都正常
        //问题：小车有先后关系，这个会改变吗？可能，因为中间路径的存在
        //顺序是1-1,2-2和2-2,1-1有什么区别
        //后面车的时间一定>前车，故总时间一定大于前车
        //关键是后车的路径要最短

        //2车都在取货/卸货时互不影响
        ///////////////
        //终极问题：多小车同时工作的最大时间计算
        public float calcuMultiTaskTime(List<Dictionary<Car, Task>> car_task_list)
        {
            //先假设至多2个任务、2个车
            //先计算1-1,2-2

            return 0.0f;
        }

    }
    ////车2的时间段
    //public float calcuMultiTaskTime(List<Dictionary<Car, Task>> carTaskdict_list)
    //{
    //    float totalTaskTime = 0.0f;
    //    for (int i = 0; i < carTaskdict_list.Count; i++)
    //    {
    //        totalTaskTime += carTaskdict_list[i]
    //    }
    //}

}
