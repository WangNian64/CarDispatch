using System;
using System.Collections.Generic;
using System.Text;

namespace CarDispatch.Objects
{
    public class TimeSegment
    {
        //时间段的种类
        public enum TimeSegType
        {
            ReachLoad=0, Load=1, ReachUnload=2, Unload=3
        }
        public float length;//时间段长度
        public TimeSegType type;//时间段种类
        public TimeSegment(float length, TimeSegType type)
        {
            this.length = length;
            this.type = type;
        }
    }
}
