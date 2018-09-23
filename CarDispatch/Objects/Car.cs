using System;
using System.Collections.Generic;
using System.Text;
using CarDispatch.Objects;
public struct Position
{
    public float x;
    public float y;
    public float z;
    public Position(float _x, float _y, float _z)
    {
        this.x = _x;
        this.y = _y;
        this.z = _z;
    }
}
//车的工作状态，workin和workout都是已经有货物了
public enum CarState
{
    Empty=0,WorkIn=1,WorkOut=2,
}
namespace CarDispatch
{
    public class Car
    {
        public Position carPos;
        public EdgeData car_EdgeData; 
        public CarState carState;
        public float speed;//车速
        public Car(Position carPos, EdgeData car_EdgeData, CarState carState, float speed)
        {
            this.carPos = carPos;
            this.car_EdgeData = car_EdgeData;
            this.carState = carState;
            this.speed = speed;
        }
    }
}
