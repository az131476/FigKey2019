using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace PC.ProductLine.Flow.NodeLib
{
   public static class ONode
    {
        /// <summary>
        /// 绘制普通活动的样式
        /// </summary>
       public static Lassalle.Flow.Node Get_Rectangle_Style()
        {
            Lassalle.Flow.Node node1 = new Lassalle.Flow.Node();
            node1.DrawColor = System.Drawing.Color.Blue;
            node1.Shape = new Lassalle.Flow.Shape(Lassalle.Flow.ShapeStyle.Rectangle, Lassalle.Flow.ShapeOrientation.so_0);
            node1.Trimming = StringTrimming.None;
            return node1;
        }
    
        /// <summary>
        /// 绘制菱形
        /// </summary>
       public static Lassalle.Flow.Node Get_Losange_Style()
        {

           
            Lassalle.Flow.Node node1 = new Lassalle.Flow.Node();

           node1.DrawColor = System.Drawing.Color.Blue;
           node1.Shape = new Lassalle.Flow.Shape(Lassalle.Flow.ShapeStyle.Losange, Lassalle.Flow.ShapeOrientation.so_270);
           return node1;
        }
       
    }
}
