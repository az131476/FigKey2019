using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BXHSerialPort
{
    class LinegetXgetY
    {
        float a, b;
        public LinegetXgetY(float x1,float y1, float x2, float y2)
        {
            a = (y2 - y1) / (x2 - x1);
            b = y1 - a * x1;
        }
        public float getX(float y)
        {
            return (y - b) / a;
        }
        public float getY(float x)
        {
            return a * x + b;
        }
    }
}
