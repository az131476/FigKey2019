using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Drawing;

namespace Vector.VLConfig.GUI
{
	internal class GridUtil
	{
		public static void DrawImageTextCell(RowCellCustomDrawEventArgs e, Bitmap image)
		{
			Rectangle bounds = e.Bounds;
			bounds.Inflate(1, 1);
			GridRowCellState state = ((GridCellInfo)e.Cell).State;
			if ((state & GridRowCellState.Selected) == GridRowCellState.Selected)
			{
				e.Graphics.FillRectangle(e.Appearance.GetBackBrush(e.Cache), bounds);
			}
			if ((state & GridRowCellState.FocusedCell) == GridRowCellState.FocusedCell)
			{
				e.Cache.Paint.DrawFocusRectangle(e.Graphics, bounds, e.Appearance.ForeColor, e.Appearance.BackColor);
			}
			Point location = e.Bounds.Location;
			location.Y++;
			e.Graphics.DrawImage(image, location);
			Rectangle bounds2 = e.Bounds;
			bounds2.Width -= 20;
			bounds2.X += 20;
			e.Appearance.DrawString(e.Cache, e.DisplayText, bounds2);
			e.Handled = true;
		}
	}
}
