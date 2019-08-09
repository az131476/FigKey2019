using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace PC.ProductLine.Flow.Math
{
    public static class OMath
    {
        public static RectangleF getRectangeFBy2Pointf(PointF p1,PointF p2)
        {

           
            SizeF pSize=new SizeF();
            if (p2.Y > p1.Y)
                pSize.Height=p2.Y - p1.Y;//大减小再除以2
            else
                pSize.Height = p1.Y - p2.Y;//大减小再除以2

            if (p2.X > p1.X)
                pSize.Width = p2.X - p1.X;//大减小再除以2
            else
                pSize.Width = p1.X - p2.X;//大减小再除以2

            PointF pCenter = new PointF();

            pCenter.X = p2.X + pSize.Width;
            pCenter.Y = p2.Y +pSize.Height;
            return new RectangleF(pCenter,pSize);
        
        }
    }
}
