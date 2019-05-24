using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.LoggingNavigator.Data;

namespace Vector.VLConfig.LoggingNavigator
{
	internal class TimeLineControl : UserControl
	{
		private double mMinimum;

		private double mCurrentMinimum;

		private double mMaximum;

		private double mCurrentMaximum;

		private double mCurrentPosition;

		private List<SpecialEvent> mSpecialEvents = new List<SpecialEvent>();

		private List<MeasurementEvent> mMeasurementEvents = new List<MeasurementEvent>();

		private List<Marker> mMarker = new List<Marker>();

		private List<Trigger> mTriggers = new List<Trigger>();

		private List<VoiceRecordBox> mVoiceRecordBoxes = new List<VoiceRecordBox>();

		private BufferedGraphicsContext mBufferedGraphicsContext;

		private BufferedGraphics mBufferedGraphics;

		private bool mAllowUserToChangePosition = true;

		private bool mAllowUserToZoom = true;

		private bool mAllowUserToScroll = true;

		private bool mAutoScroll = true;

		private bool mShowSpecialEvents = true;

		private bool mShowMeasurementLine = true;

		private bool mShowCurrentPosition = true;

		private bool mShowMarker = true;

		private bool mShowTriggers = true;

		private uint mMinimumSeverityLevelToDisplay;

		private Font mFont = new Font("Tahoma", 8.25f);

		private readonly int mDrawMarginX = 5;

		private readonly int mDrawMarginBottom;

		private int mZoomPixelPos1;

		private int mZoomPixelPos2;

		private readonly double mMinTimeWidth = 5.0;

		private bool mScrollLeft;

		private bool mScrollRight;

		private Timer mScrollTimer = new Timer();

		private readonly int mScrollTimerInterval = 100;

		private ToolTip mToolTip;

		private bool mTooltipActive;

		private Label mTooltipRectLabel;

		private IContainer components;

		[Category("Data")]
		public event EventHandler CurrentPositionChanged;

		[Category("Data"), DefaultValue(0.0)]
		public double Minimum
		{
			get
			{
				return this.mMinimum;
			}
			set
			{
				if (value > this.mMaximum)
				{
					value = this.mMaximum;
				}
				if (this.mMinimum == value)
				{
					return;
				}
				bool flag = this.mCurrentMinimum == this.mMinimum || this.mCurrentMinimum < value || !this.mAllowUserToZoom;
				this.mMinimum = value;
				if (flag)
				{
					this.mCurrentMinimum = this.mMinimum;
					if (this.mCurrentMaximum < this.mCurrentMinimum)
					{
						this.mCurrentMaximum = this.mCurrentMinimum;
					}
				}
				if (this.mCurrentPosition < this.mMinimum)
				{
					this.mCurrentPosition = this.mMinimum;
				}
				this.Redraw(true);
			}
		}

		[Category("Data"), DefaultValue(0.0)]
		public double Maximum
		{
			get
			{
				return this.mMaximum;
			}
			set
			{
				if (value < this.mMinimum)
				{
					value = this.mMinimum;
				}
				if (this.mMaximum == value)
				{
					return;
				}
				bool flag = this.mCurrentMaximum == this.mMaximum || this.mCurrentMaximum > value || !this.mAllowUserToZoom;
				this.mMaximum = value;
				if (flag)
				{
					this.mCurrentMaximum = this.mMaximum;
					if (this.mCurrentMinimum > this.mCurrentMaximum)
					{
						this.mCurrentMinimum = this.mCurrentMaximum;
					}
				}
				if (this.mCurrentPosition > this.mMaximum)
				{
					this.mCurrentPosition = this.mMaximum;
				}
				this.Redraw(true);
			}
		}

		[Category("Data"), DefaultValue(0.0)]
		public double CurrentPosition
		{
			get
			{
				return this.mCurrentPosition;
			}
			set
			{
				if (value < this.mMinimum)
				{
					value = this.mMinimum;
				}
				if (value > this.mMaximum)
				{
					value = this.mMaximum;
				}
				if (this.mCurrentPosition == value)
				{
					return;
				}
				this.mCurrentPosition = value;
				if (this.mAutoScroll)
				{
					double timeWidth = this.TimeWidth;
					if (this.mCurrentPosition < this.mCurrentMinimum)
					{
						this.mCurrentMinimum = this.mCurrentPosition;
						this.mCurrentMaximum = this.mCurrentMinimum + timeWidth;
					}
					else if (this.mCurrentPosition > this.mCurrentMaximum)
					{
						this.mCurrentMaximum = this.mCurrentPosition;
						this.mCurrentMinimum = this.mCurrentMaximum - timeWidth;
					}
				}
				this.Redraw(true);
				this.Fire_CurrentPositionChanged();
			}
		}

		[Category("Behavior"), DefaultValue(true)]
		public bool AllowUserToChangePosition
		{
			get
			{
				return this.mAllowUserToChangePosition;
			}
			set
			{
				this.mAllowUserToChangePosition = value;
			}
		}

		[Category("Behavior"), DefaultValue(true)]
		public bool AllowUserToZoom
		{
			get
			{
				return this.mAllowUserToZoom;
			}
			set
			{
				if (this.mAllowUserToZoom == value)
				{
					return;
				}
				this.mAllowUserToZoom = value;
				if (!this.mAllowUserToZoom)
				{
					this.mCurrentMinimum = this.mMinimum;
					this.mCurrentMaximum = this.mMaximum;
					this.Redraw(true);
				}
			}
		}

		[Category("Behavior"), DefaultValue(true)]
		public bool AllowUserToScroll
		{
			get
			{
				return this.mAllowUserToScroll;
			}
			set
			{
				this.mAllowUserToScroll = value;
				this.mScrollTimer.Enabled = this.mAllowUserToScroll;
			}
		}

		[Category("Behavior"), DefaultValue(true)]
		public override bool AutoScroll
		{
			get
			{
				return this.mAutoScroll;
			}
			set
			{
				this.mAutoScroll = value;
				if (this.mAutoScroll)
				{
					this.CurrentPosition = this.mCurrentPosition;
				}
			}
		}

		[Category("Appearance"), DefaultValue(true)]
		public bool ShowSpecialEvents
		{
			get
			{
				return this.mShowSpecialEvents;
			}
			set
			{
				if (this.mShowSpecialEvents == value)
				{
					return;
				}
				this.mShowSpecialEvents = value;
				this.Redraw(false);
			}
		}

		[Category("Appearance"), DefaultValue(true)]
		public bool ShowMeasurementLine
		{
			get
			{
				return this.mShowMeasurementLine;
			}
			set
			{
				if (this.mShowMeasurementLine == value)
				{
					return;
				}
				this.mShowMeasurementLine = value;
				this.Redraw(true);
			}
		}

		[Category("Appearance"), DefaultValue(true)]
		public bool ShowMarker
		{
			get
			{
				return this.mShowMarker;
			}
			set
			{
				if (this.mShowMarker == value)
				{
					return;
				}
				this.mShowMarker = value;
				this.Redraw(false);
			}
		}

		[Category("Appearance"), DefaultValue(true)]
		public bool ShowTriggers
		{
			get
			{
				return this.mShowTriggers;
			}
			set
			{
				if (this.mShowTriggers == value)
				{
					return;
				}
				this.mShowTriggers = value;
				this.Redraw(false);
			}
		}

		[Category("Appearance"), DefaultValue(true)]
		public bool ShowCurrentPosition
		{
			get
			{
				return this.mShowCurrentPosition;
			}
			set
			{
				if (this.mShowCurrentPosition == value)
				{
					return;
				}
				this.mShowCurrentPosition = value;
				this.Redraw(true);
			}
		}

		[Category("Appearance"), DefaultValue(0)]
		public uint MinimumSeverityLevelToDisplay
		{
			get
			{
				return this.mMinimumSeverityLevelToDisplay;
			}
			set
			{
				if (value > 5u)
				{
					value = 5u;
				}
				if (this.mMinimumSeverityLevelToDisplay == value)
				{
					return;
				}
				this.mMinimumSeverityLevelToDisplay = value;
				this.Redraw(true);
			}
		}

		private double CurrentMinimum
		{
			get
			{
				return this.mCurrentMinimum;
			}
			set
			{
				if (value > this.mCurrentMaximum)
				{
					value = this.mCurrentMaximum;
				}
				if (value < this.mMinimum)
				{
					value = this.mMinimum;
				}
				if (this.mCurrentMinimum == value)
				{
					return;
				}
				this.mCurrentMinimum = value;
				this.Redraw(true);
			}
		}

		private double CurrentMaximum
		{
			get
			{
				return this.mCurrentMaximum;
			}
			set
			{
				if (value < this.mCurrentMinimum)
				{
					value = this.mCurrentMinimum;
				}
				if (value > this.mMaximum)
				{
					value = this.mMaximum;
				}
				if (this.mCurrentMaximum == value)
				{
					return;
				}
				this.mCurrentMaximum = value;
				this.Redraw(true);
			}
		}

		private int TimeLineLeft
		{
			get
			{
				return this.mDrawMarginX;
			}
		}

		private int TimeLineRight
		{
			get
			{
				return base.Size.Width - this.mDrawMarginX - 1;
			}
		}

		private int TimeLineWidth
		{
			get
			{
				return this.TimeLineRight - this.TimeLineLeft;
			}
		}

		private int TimeLineY
		{
			get
			{
				return base.Size.Height - this.mDrawMarginBottom - 20;
			}
		}

		private double TimeWidth
		{
			get
			{
				return this.mCurrentMaximum - this.mCurrentMinimum;
			}
		}

		public TimeLineControl()
		{
			this.InitializeComponent();
			this.mDrawMarginBottom = (int)Math.Ceiling((double)this.MeasureString(base.CreateGraphics(), "Ij").Height);
			base.Paint += new PaintEventHandler(this.OnPaint);
			base.Resize += new EventHandler(this.OnResize);
			base.MouseDown += new MouseEventHandler(this.OnMouseDown);
			base.MouseMove += new MouseEventHandler(this.OnMouseMove);
			base.MouseUp += new MouseEventHandler(this.OnMouseUp);
			this.mBufferedGraphicsContext = BufferedGraphicsManager.Current;
			this.mBufferedGraphicsContext.MaximumBuffer = new Size(base.Width + 1, base.Height + 1);
			this.mBufferedGraphics = this.mBufferedGraphicsContext.Allocate(base.CreateGraphics(), new Rectangle(0, 0, base.Width, base.Height));
			this.DoubleBuffered = true;
			this.mToolTip = new ToolTip();
			this.mToolTip.IsBalloon = true;
			this.mTooltipRectLabel = new Label();
			this.mTooltipRectLabel.BackColor = Color.Transparent;
			this.mTooltipRectLabel.Text = string.Empty;
			this.Clear();
			this.mScrollTimer.Interval = this.mScrollTimerInterval;
			this.mScrollTimer.Tick += new EventHandler(this.ScrollTimer_Tick);
			this.mScrollTimer.Enabled = true;
		}

		public void AddReplaceMeasurementEvent(double timestampBegin, double timestampEnd, MeasurementType type)
		{
			MeasurementEvent item = new MeasurementEvent(timestampBegin, timestampEnd, type);
			this.mMeasurementEvents.Add(item);
			this.mMeasurementEvents.Sort(new Comparison<MeasurementEvent>(TimeLineControl.SortMeasurementEventsByTimeAscending));
		}

		public void AddReplaceMeasurementEventWithoutRedraw(double timestampBegin, double timestampEnd, MeasurementType type)
		{
			MeasurementEvent item = new MeasurementEvent(timestampBegin, timestampEnd, type);
			this.mMeasurementEvents.Add(item);
		}

		public void MeasurementEventManualSort()
		{
			this.mMeasurementEvents.Sort(new Comparison<MeasurementEvent>(TimeLineControl.SortMeasurementEventsByTimeAscending));
		}

		public void ManualRedraw()
		{
			this.Redraw(true);
		}

		public void AddMarker(Marker newVal)
		{
			if (this.MarkerExists(newVal))
			{
				return;
			}
			this.mMarker.Add(newVal);
			this.mMarker.Sort(new Comparison<Marker>(TimeLineControl.SortEntriesByTimeAscending));
		}

		public void AddMarkerList(IList<Marker> list)
		{
			HashSet<Marker> hashSet = new HashSet<Marker>();
			foreach (Marker current in list)
			{
				if (!hashSet.Contains(current))
				{
					hashSet.Add(current);
					this.mMarker.Add(current);
				}
			}
			this.mMarker.Sort(new Comparison<Marker>(TimeLineControl.SortEntriesByTimeAscending));
		}

		public void SetVoiceRecordList(IList<VoiceRecord> list)
		{
			this.StopAllSounds();
			this.mVoiceRecordBoxes.Clear();
			foreach (VoiceRecord current in list)
			{
				VoiceRecordBox voiceRecordBox = new VoiceRecordBox(this, current);
				this.mToolTip.SetToolTip(voiceRecordBox, voiceRecordBox.Tooltip);
				this.mVoiceRecordBoxes.Add(voiceRecordBox);
			}
		}

		public void AddTriggerList(IList<Trigger> list)
		{
			HashSet<Trigger> hashSet = new HashSet<Trigger>();
			foreach (Trigger current in list)
			{
				if (!hashSet.Contains(current))
				{
					hashSet.Add(current);
					this.mTriggers.Add(current);
				}
			}
			this.mTriggers.Sort(new Comparison<Trigger>(TimeLineControl.SortEntriesByTimeAscending));
		}

		public void Clear()
		{
			this.Minimum = 0.0;
			this.CurrentMinimum = 0.0;
			this.Maximum = 0.0;
			this.CurrentMaximum = 0.0;
			this.StopAllSounds();
			this.mVoiceRecordBoxes.Clear();
			this.mSpecialEvents.Clear();
			this.mMeasurementEvents.Clear();
			this.mMarker.Clear();
			this.mTriggers.Clear();
			base.Controls.Clear();
			this.AddReplaceMeasurementEvent((double)this.TimeLineLeft, (double)this.TimeLineRight, MeasurementType.None);
		}

		public void StopAllSounds()
		{
			foreach (VoiceRecordBox current in this.mVoiceRecordBoxes)
			{
				current.Stop();
			}
		}

		private void Fire_CurrentPositionChanged()
		{
			if (this.CurrentPositionChanged != null)
			{
				this.CurrentPositionChanged(this, new EventArgs());
			}
		}

		private void OnPaint(object sender, PaintEventArgs e)
		{
			this.mBufferedGraphics.Render(e.Graphics);
		}

		private void OnResize(object sender, EventArgs e)
		{
			base.SuspendLayout();
			if (base.Height != 67)
			{
				base.Height = 67;
			}
			if (base.Width < 67)
			{
				base.Width = 67;
			}
			if (base.Parent != null && base.Parent.Width < base.Width)
			{
				base.Width = base.Parent.Width - 15;
			}
			if (this.mBufferedGraphics != null)
			{
				this.mBufferedGraphics.Dispose();
				this.mBufferedGraphics = null;
			}
			this.mBufferedGraphics = this.mBufferedGraphicsContext.Allocate(base.CreateGraphics(), new Rectangle(0, 0, base.Width, base.Height));
			this.Redraw(true);
			base.ResumeLayout(true);
		}

		private void OnMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (this.mAllowUserToChangePosition && this.mMaximum > this.mMinimum)
				{
					this.CurrentPosition = this.TimePosFromPixelPos(e.X);
					this.Cursor = Cursors.SizeWE;
				}
				else
				{
					this.Cursor = Cursors.No;
				}
			}
			if (e.Button == MouseButtons.Right)
			{
				if (this.mAllowUserToZoom && this.mMaximum - this.mMinimum > this.mMinTimeWidth)
				{
					this.mZoomPixelPos1 = ((e.X < this.TimeLineLeft) ? this.TimeLineLeft : ((e.X > this.TimeLineRight) ? this.TimeLineRight : e.X));
					this.mZoomPixelPos1 = this.PixelPosFromTimePos(this.TimePosFromPixelPos(this.mZoomPixelPos1));
					this.mZoomPixelPos2 = this.mZoomPixelPos1;
					this.Cursor = Cursors.Cross;
					this.Redraw(false);
					return;
				}
				this.Cursor = Cursors.No;
			}
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left || (e.Button & MouseButtons.Right) == MouseButtons.Right)
			{
				this.mScrollLeft = (e.X < this.TimeLineLeft);
				this.mScrollRight = (e.X > this.TimeLineRight);
			}
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left && this.mAllowUserToChangePosition)
			{
				this.CurrentPosition = this.TimePosFromPixelPos(e.X);
			}
			if ((e.Button & MouseButtons.Right) == MouseButtons.Right && this.mAllowUserToZoom)
			{
				this.mZoomPixelPos2 = ((e.X < this.TimeLineLeft) ? this.TimeLineLeft : ((e.X > this.TimeLineRight) ? this.TimeLineRight : e.X));
				this.mZoomPixelPos2 = this.PixelPosFromTimePos(this.TimePosFromPixelPos(this.mZoomPixelPos2));
				this.Redraw(false);
			}
			bool flag = false;
			IEnumerable<Entry> enumerable = new List<Entry>();
			if (this.mShowTriggers)
			{
				enumerable = enumerable.Concat(this.mTriggers);
			}
			if (this.mShowMarker)
			{
				enumerable = enumerable.Concat(this.mMarker);
			}
			foreach (Entry current in enumerable.Reverse<Entry>())
			{
				int num = this.PixelPosFromTimePos(current.TimeSpec);
				if (num > this.TimeLineLeft && num < this.TimeLineRight)
				{
					Rectangle arg_18B_0 = current.TooltipRect;
					if (current.TooltipRect.Contains(e.Location))
					{
						string toolTip = this.mToolTip.GetToolTip(this.mTooltipRectLabel);
						if (!this.mTooltipActive || toolTip != current.Tooltip)
						{
							this.mTooltipActive = true;
							this.mTooltipRectLabel.Width = current.TooltipRect.Width;
							this.mTooltipRectLabel.Height = current.TooltipRect.Height;
							this.mTooltipRectLabel.Location = current.TooltipRect.Location;
							base.Controls.Add(this.mTooltipRectLabel);
							this.mToolTip.SetToolTip(this.mTooltipRectLabel, current.Tooltip);
						}
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.mToolTip.Hide(this);
				this.mTooltipActive = false;
				base.Controls.Remove(this.mTooltipRectLabel);
			}
		}

		private void OnMouseUp(object sender, MouseEventArgs e)
		{
			this.Cursor = Cursors.Default;
			this.mScrollLeft = false;
			this.mScrollRight = false;
			if (e.Button == MouseButtons.Right && this.mAllowUserToZoom && this.mZoomPixelPos1 > 0 && this.mZoomPixelPos2 > 0)
			{
				if (this.mZoomPixelPos1 == this.mZoomPixelPos2)
				{
					this.mCurrentMinimum = this.mMinimum;
					this.mCurrentMaximum = this.mMaximum;
				}
				else
				{
					double val = this.TimePosFromPixelPos(this.mZoomPixelPos1);
					double val2 = this.TimePosFromPixelPos(this.mZoomPixelPos2);
					double num = Math.Min(val, val2);
					double num2 = Math.Max(val, val2);
					if (num2 - num < this.mMinTimeWidth)
					{
						double num3 = (num2 + num) / 2.0;
						num = num3 - this.mMinTimeWidth / 2.0;
						num2 = num3 + this.mMinTimeWidth / 2.0;
						if (num < this.mMinimum)
						{
							num = this.mMinimum;
							num2 = Math.Min(this.mMinimum + this.mMinTimeWidth, this.mMaximum);
						}
						if (num2 > this.mMaximum)
						{
							num2 = this.mMaximum;
							num = Math.Max(this.mMaximum - this.mMinTimeWidth, this.mMinimum);
						}
					}
					this.mCurrentMinimum = num;
					this.mCurrentMaximum = num2;
					if (this.mCurrentPosition < this.mCurrentMinimum)
					{
						this.mCurrentPosition = this.mCurrentMinimum;
					}
					if (this.mCurrentPosition > this.mCurrentMaximum)
					{
						this.mCurrentPosition = this.mCurrentMaximum;
					}
				}
				this.mZoomPixelPos1 = 0;
				this.mZoomPixelPos2 = 0;
				this.Redraw(true);
			}
		}

		private void ScrollTimer_Tick(object sender, EventArgs e)
		{
			double num = this.mCurrentMaximum - this.mCurrentMinimum;
			double num2 = num / 10.0;
			if (this.mScrollLeft && this.mCurrentMinimum > this.mMinimum)
			{
				if (this.mCurrentMinimum - num2 < this.mMinimum)
				{
					this.mCurrentMinimum = this.mMinimum;
					this.mCurrentMaximum = this.mMinimum + num;
				}
				else
				{
					this.mCurrentMinimum -= num2;
					this.mCurrentMaximum -= num2;
				}
				this.CurrentPosition = this.mCurrentMinimum;
				return;
			}
			if (this.mScrollRight && this.mCurrentMaximum < this.mMaximum)
			{
				if (this.mCurrentMaximum + num2 > this.mMaximum)
				{
					this.mCurrentMinimum = this.mMaximum - num;
					this.mCurrentMaximum = this.mMaximum;
				}
				else
				{
					this.mCurrentMinimum += num2;
					this.mCurrentMaximum += num2;
				}
				this.CurrentPosition = this.mCurrentMaximum;
			}
		}

		private static int SortMeasurementEventsByTimeAscending(MeasurementEvent x, MeasurementEvent y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null && y != null)
			{
				return -1;
			}
			if (x != null && y == null)
			{
				return 1;
			}
			if (x.Type == MeasurementType.Permanent && y.Type == MeasurementType.Triggered)
			{
				return -1;
			}
			if (x.Type == MeasurementType.Triggered && y.Type == MeasurementType.Permanent)
			{
				return 1;
			}
			return x.TimestampBegin.CompareTo(y.TimestampBegin);
		}

		private static int SortEntriesByTimeAscending(Entry x, Entry y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null && y != null)
			{
				return -1;
			}
			if (x != null && y == null)
			{
				return 1;
			}
			return x.TimeSpec.CompareTo(y.TimeSpec);
		}

		private void Redraw(bool redrawIcons)
		{
			this.mBufferedGraphics.Graphics.Clear(this.BackColor);
			this.Draw_TimeLine(this.mBufferedGraphics.Graphics);
			this.Draw_ZoomActiveHint(this.mBufferedGraphics.Graphics);
			if (this.mShowSpecialEvents)
			{
				this.Draw_SpecialEvents(this.mBufferedGraphics.Graphics);
			}
			if (this.mShowMeasurementLine)
			{
				this.Draw_MeasurementLine(this.mBufferedGraphics.Graphics);
			}
			if (this.mShowTriggers)
			{
				this.Draw_Triggers(this.mBufferedGraphics.Graphics);
			}
			if (this.mShowMarker)
			{
				this.Draw_Marker(this.mBufferedGraphics.Graphics);
			}
			if (this.mShowCurrentPosition)
			{
				this.Draw_CurrentPosition(this.mBufferedGraphics.Graphics);
			}
			if (redrawIcons)
			{
				base.Controls.Clear();
			}
			this.Draw_VoiceRecords(this.mBufferedGraphics.Graphics, redrawIcons);
			this.Draw_ZoomRectangle(this.mBufferedGraphics.Graphics);
			this.Refresh();
		}

		private void Draw_TimeLine(Graphics graphics)
		{
			graphics.DrawLine(new Pen(Color.Black), new Point(this.TimeLineLeft, this.TimeLineY), new Point(this.TimeLineRight, this.TimeLineY));
			graphics.DrawLine(new Pen(Color.Black), new Point(this.TimeLineLeft, this.TimeLineY + 19), new Point(this.TimeLineLeft, this.TimeLineY - 5));
			string s = this.TimeLabel(this.CurrentMinimum);
			SizeF labelSize = this.MeasureString(graphics, s);
			float x = this.LabelPosX(graphics, labelSize, base.ClientRectangle.Left, this.TimeLineLeft, base.ClientRectangle.Right);
			float y = (float)base.ClientRectangle.Bottom - labelSize.Height;
			graphics.DrawString(s, this.mFont, new SolidBrush(Color.Black), new PointF(x, y));
			double timePos = this.TimePosFromPixelPos(this.TimeLineLeft + this.TimeLineWidth / 2);
			int num = this.PixelPosFromTimePos(timePos);
			graphics.DrawLine(new Pen(Color.Black), new Point(num, this.TimeLineY + 19), new Point(num, this.TimeLineY + 1));
			s = this.TimeLabel(timePos);
			labelSize = this.MeasureString(graphics, s);
			x = this.LabelPosX(graphics, labelSize, base.ClientRectangle.Left, num, base.ClientRectangle.Right);
			y = (float)base.ClientRectangle.Bottom - labelSize.Height;
			graphics.DrawString(s, this.mFont, new SolidBrush(Color.Black), new PointF(x, y));
			graphics.DrawLine(new Pen(Color.Black), new Point(this.TimeLineRight, this.TimeLineY + 19), new Point(this.TimeLineRight, this.TimeLineY - 5));
			s = this.TimeLabel(this.CurrentMaximum);
			labelSize = this.MeasureString(graphics, s);
			x = this.LabelPosX(graphics, labelSize, base.ClientRectangle.Left, this.TimeLineRight, base.ClientRectangle.Right);
			y = (float)base.ClientRectangle.Bottom - labelSize.Height;
			graphics.DrawString(s, this.mFont, new SolidBrush(Color.Black), new PointF(x, y));
		}

		private void Draw_ZoomActiveHint(Graphics graphics)
		{
			if (this.mCurrentMinimum == this.mMinimum && this.mCurrentMaximum == this.mMaximum)
			{
				return;
			}
			string s = "Z";
			SizeF labelSize = this.MeasureString(graphics, s);
			float x = this.LabelPosX(graphics, labelSize, base.ClientRectangle.Left, this.TimeLineLeft, base.ClientRectangle.Right) + 1f;
			float y = (float)(this.TimeLineY - 3) - labelSize.Height;
			graphics.DrawString(s, this.mFont, new SolidBrush(Color.Blue), new PointF(x, y));
			x = this.LabelPosX(graphics, labelSize, base.ClientRectangle.Left, this.TimeLineRight, base.ClientRectangle.Right) + 1f;
			graphics.DrawString(s, this.mFont, new SolidBrush(Color.Blue), new PointF(x, y));
		}

		private void Draw_SpecialEvents(Graphics graphics)
		{
			for (int i = 0; i <= 5; i++)
			{
				if ((long)i >= (long)((ulong)this.mMinimumSeverityLevelToDisplay))
				{
					foreach (SpecialEvent current in this.mSpecialEvents)
					{
						if ((ulong)current.Severity == (ulong)((long)i))
						{
							int num = this.PixelPosFromTimePos(current.Timestamp);
							if (num > this.TimeLineLeft && num < this.TimeLineRight)
							{
								Color color = Color.Red;
								if (i == 0)
								{
									color = Color.Silver;
								}
								else if (i == 1)
								{
									color = Color.Black;
								}
								else if (i == 2)
								{
									color = Color.Blue;
								}
								else if (i == 3)
								{
									color = Color.Green;
								}
								else if (i == 4)
								{
									color = Color.Yellow;
								}
								Pen pen = new Pen(color);
								Brush brush = new SolidBrush(color);
								graphics.DrawLine(pen, new Point(num, this.TimeLineY - 1), new Point(num, this.TimeLineY - 5));
								SizeF labelSize = this.MeasureString(graphics, current.Label);
								float x = this.LabelPosX(graphics, labelSize, this.TimeLineLeft, num, this.TimeLineRight);
								float y = (float)(this.TimeLineY - 3) - labelSize.Height;
								graphics.DrawString(current.Label, this.mFont, brush, new PointF(x, y));
							}
						}
					}
				}
			}
		}

		private void Draw_Marker(Graphics graphics)
		{
			Color black = Color.Black;
			Pen pen = new Pen(black);
			foreach (Marker current in this.mMarker)
			{
				int num = this.PixelPosFromTimePos(current.Timestamp);
				if (num > this.TimeLineLeft && num < this.TimeLineRight)
				{
					graphics.DrawLine(pen, new Point(num, this.TimeLineY - 1), new Point(num, this.TimeLineY - 5));
					this.Draw_Entry_Label(graphics, current, num);
				}
			}
		}

		private void Draw_Triggers(Graphics graphics)
		{
			Color black = Color.Black;
			Pen pen = new Pen(black);
			foreach (Trigger current in this.mTriggers)
			{
				int num = this.PixelPosFromTimePos(current.TimeSpec);
				if (num > this.TimeLineLeft && num < this.TimeLineRight)
				{
					graphics.DrawLine(pen, new Point(num, this.TimeLineY - 1), new Point(num, this.TimeLineY - 5));
					this.Draw_Entry_Label(graphics, current, num);
				}
			}
		}

		private void Draw_VoiceRecords(Graphics graphics, bool redrawIcons)
		{
			foreach (VoiceRecordBox current in this.mVoiceRecordBoxes)
			{
				int num = this.PixelPosFromTimePos(current.VoiceRecord.TimeSpec);
				if (num > this.TimeLineLeft && num < this.TimeLineRight)
				{
					Color black = Color.Black;
					Pen pen = new Pen(black);
					graphics.DrawLine(pen, new Point(num, this.TimeLineY + 1), new Point(num, this.TimeLineY + 4));
					if (redrawIcons)
					{
						float num2 = this.LabelPosX(graphics, new SizeF(17f, 15f), this.TimeLineLeft, num, this.TimeLineRight);
						base.Controls.Add(current);
						current.Location = new Point((int)num2 + 3, this.TimeLineY + 5);
					}
				}
			}
		}

		private void Draw_Entry_Label(Graphics graphics, Entry tmpEvent, int pixelPos)
		{
			Brush brush = new SolidBrush(Color.Black);
			SizeF labelSize = this.MeasureString(graphics, tmpEvent.Label);
			float num = this.LabelPosX(graphics, labelSize, this.TimeLineLeft, pixelPos, this.TimeLineRight);
			float num2 = (float)(this.TimeLineY - 3) - labelSize.Height;
			tmpEvent.TooltipRect = new Rectangle((int)num, (int)num2, (int)labelSize.Width, (int)labelSize.Height);
			graphics.FillRectangle(new SolidBrush(SystemColors.Control), tmpEvent.TooltipRect);
			graphics.DrawString(tmpEvent.Label, this.mFont, brush, new PointF(num, num2));
		}

		private void Draw_MeasurementLine(Graphics graphics)
		{
			int alpha = 160;
			if (this.mCurrentMinimum == this.mCurrentMaximum)
			{
				return;
			}
			for (int i = 0; i < this.mMeasurementEvents.Count; i++)
			{
				MeasurementEvent measurementEvent = this.mMeasurementEvents[i];
				int num = this.PixelPosFromTimePos(measurementEvent.TimestampBegin);
				if (num <= this.TimeLineLeft)
				{
					num = this.TimeLineLeft + 1;
				}
				int num2 = this.PixelPosFromTimePos(measurementEvent.TimestampEnd);
				if (num2 >= this.TimeLineRight)
				{
					num2 = this.TimeLineRight - 1;
				}
				Pen pen;
				Pen pen2;
				Pen pen3;
				if (measurementEvent.Type == MeasurementType.Triggered)
				{
					pen = new Pen(Color.FromArgb(alpha, Color.LightGreen));
					pen2 = new Pen(Color.FromArgb(alpha, Color.MediumSeaGreen));
					pen3 = new Pen(Color.FromArgb(alpha, Color.DarkGreen));
				}
				else if (measurementEvent.Type == MeasurementType.Permanent)
				{
					pen = new Pen(Color.FromArgb(alpha, Color.LightYellow));
					pen2 = new Pen(Color.FromArgb(alpha, Color.Yellow));
					pen3 = new Pen(Color.FromArgb(alpha, Color.Gold));
				}
				else if (measurementEvent.Type == MeasurementType.CANoe)
				{
					pen = new Pen(Color.FromArgb(alpha, Color.LightPink));
					pen2 = new Pen(Color.FromArgb(alpha, Color.Red));
					pen3 = new Pen(Color.FromArgb(alpha, Color.DarkRed));
				}
				else
				{
					pen = new Pen(Color.LightGray);
					pen2 = new Pen(Color.Silver);
					pen3 = new Pen(Color.Gray);
				}
				graphics.DrawLine(pen, new Point(num, this.TimeLineY - 2), new Point(num2, this.TimeLineY - 2));
				graphics.DrawLine(pen2, new Point(num, this.TimeLineY - 1), new Point(num2, this.TimeLineY - 1));
				graphics.DrawLine(pen2, new Point(num, this.TimeLineY), new Point(num2, this.TimeLineY));
				graphics.DrawLine(pen3, new Point(num, this.TimeLineY + 1), new Point(num2, this.TimeLineY + 1));
			}
		}

		private void Draw_CurrentPosition(Graphics graphics)
		{
			if (this.CurrentPosition < this.mCurrentMinimum || this.CurrentPosition > this.mCurrentMaximum || this.mCurrentMinimum == this.mCurrentMaximum)
			{
				return;
			}
			int num = this.PixelPosFromTimePos(this.CurrentPosition);
			string s = this.TimeLabel(this.CurrentPosition);
			SizeF labelSize = this.MeasureString(graphics, s);
			float x = this.LabelPosX(graphics, labelSize, base.ClientRectangle.Left, num, base.ClientRectangle.Right);
			float y = (float)base.ClientRectangle.Top;
			graphics.DrawString(s, this.mFont, new SolidBrush(Color.Blue), new PointF(x, y));
			graphics.DrawLine(new Pen(Color.Blue), new Point(num, this.TimeLineY + 4), new Point(num, (int)Math.Floor((double)labelSize.Height)));
		}

		private void Draw_ZoomRectangle(Graphics graphics)
		{
			if (!this.mAllowUserToZoom || this.mZoomPixelPos1 == 0 || this.mZoomPixelPos2 == 0)
			{
				return;
			}
			int num = Math.Min(this.mZoomPixelPos1, this.mZoomPixelPos2);
			int num2 = Math.Max(this.mZoomPixelPos1, this.mZoomPixelPos2);
			int centerPos = (num + num2) / 2;
			string s = "zoom";
			if (num != num2)
			{
				s = this.TimeLabel(this.TimePosFromPixelPos(num)) + " - " + this.TimeLabel(this.TimePosFromPixelPos(num2));
			}
			SizeF labelSize = this.MeasureString(graphics, s);
			float num3 = (float)base.ClientRectangle.Top;
			float num4 = this.LabelPosX(graphics, labelSize, base.ClientRectangle.Left, centerPos, base.ClientRectangle.Right);
			graphics.FillRectangle(new SolidBrush(this.BackColor), new Rectangle((int)num4 - 10, (int)num3, (int)labelSize.Width + 20, (int)labelSize.Height));
			graphics.DrawString(s, this.mFont, new SolidBrush(Color.Blue), new PointF(num4, num3));
			int num5 = (int)Math.Floor((double)labelSize.Height);
			int num6 = this.TimeLineY + 20;
			int num7 = num2 - num;
			int height = num6 - num5;
			if (num7 > 0)
			{
				graphics.DrawRectangle(new Pen(Color.Blue), new Rectangle(num, num5, num7, height));
			}
		}

		private bool SpecialEventExists(SpecialEvent searchValue)
		{
			foreach (SpecialEvent current in this.mSpecialEvents)
			{
				if (current.Timestamp == searchValue.Timestamp && current.Label == searchValue.Label)
				{
					return true;
				}
			}
			return false;
		}

		private MeasurementEvent GetMeasurementEvent(double timestamp)
		{
			foreach (MeasurementEvent current in this.mMeasurementEvents)
			{
				if (current.TimestampBegin == timestamp)
				{
					return current;
				}
			}
			return null;
		}

		private bool MarkerExists(Marker searchValue)
		{
			foreach (Marker current in this.mMarker)
			{
				if (current.Timestamp == searchValue.Timestamp && current.Label == searchValue.Label)
				{
					return true;
				}
			}
			return false;
		}

		private bool TriggerExists(Trigger searchValue)
		{
			foreach (Trigger current in this.mTriggers)
			{
				if (current.TimeSpec == searchValue.TimeSpec && current.Label == searchValue.Label)
				{
					return true;
				}
			}
			return false;
		}

		private int PixelPosFromTimePos(double timePos)
		{
			if (timePos <= this.CurrentMinimum)
			{
				return this.TimeLineLeft;
			}
			if (timePos >= this.CurrentMaximum)
			{
				return this.TimeLineRight;
			}
			return (int)Math.Floor((timePos - this.CurrentMinimum) * (double)this.TimeLineWidth / this.TimeWidth + (double)this.TimeLineLeft);
		}

		private double TimePosFromPixelPos(int pixelPos)
		{
			if (pixelPos <= this.TimeLineLeft)
			{
				return this.CurrentMinimum;
			}
			if (pixelPos >= this.TimeLineRight)
			{
				return this.CurrentMaximum;
			}
			return Math.Round(this.TimeWidth * (double)(pixelPos - this.TimeLineLeft) / (double)this.TimeLineWidth + this.CurrentMinimum, 1);
		}

		private float LabelPosX(Graphics graphics, SizeF labelSize, int leftBorder, int centerPos, int rightBorder)
		{
			float num = (float)centerPos - labelSize.Width / 2f;
			if (num < (float)leftBorder)
			{
				num = (float)leftBorder;
			}
			if (num + labelSize.Width > (float)(rightBorder + 2))
			{
				num = (float)rightBorder - labelSize.Width + 2f;
			}
			return num;
		}

		private string TimeLabel(double timePos)
		{
			TimeSpec timeSpec = new TimeSpec((ulong)timePos);
			return timeSpec.DateTime.ToShortDateString() + " " + timeSpec.DateTime.ToShortTimeString();
		}

		private SizeF MeasureString(Graphics g, string s)
		{
			return g.MeasureString(s, this.mFont);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleMode = AutoScaleMode.Inherit;
			this.DoubleBuffered = true;
			base.Name = "TimeLineControl";
			base.Size = new Size(350, 65);
			base.ResumeLayout(false);
		}
	}
}
