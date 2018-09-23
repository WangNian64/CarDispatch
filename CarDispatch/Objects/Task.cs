using System;
using System.Collections.Generic;
using System.Text;
using CarDispatch.Objects;
namespace CarDispatch
{
    public class Task
    {
        public Position loadGoodsPos;//取货位置
        public EdgeData loadGoods_EdgeData;//取货点所在的边
        public Position unloadGoodsPos;//放货位置
        public EdgeData unloadGoods_EdgeData;//放货点所在的边
        public float loadTime = 1.0f;//装货时间
        public float unloadTime = 1.0f;//放货时间
        public Task(Position loadGoodsPos, EdgeData loadGoods_EdgeData,
                Position unloadGoodsPos, EdgeData unloadGoods_EdgeData,
                float loadTime = 1.0f, float unloadTime = 1.0f)
        {
            this.loadGoodsPos = loadGoodsPos;
            this.loadGoods_EdgeData = loadGoods_EdgeData;
            this.unloadGoodsPos = unloadGoodsPos;
            this.unloadGoods_EdgeData = unloadGoods_EdgeData;
            this.unloadTime = unloadTime;
            this.loadTime = loadTime;
        }
    }
}
