using System;
using System.Drawing;
using System.Windows.Forms;

namespace com.lover.common
{
	public class UiUtils
	{
		private static UiUtils _instance = new UiUtils();

		private int _lastX = -1;

		private int _lastY = -1;

		private bool _dragging;

		public static UiUtils getInstance()
		{
			return UiUtils._instance;
		}

		private UiUtils()
		{
		}

		public void registerMovableForm(Form win)
		{
			win.MouseDown += new MouseEventHandler(this.win_MouseDown);
			win.MouseMove += new MouseEventHandler(this.win_MouseMove);
			win.MouseUp += new MouseEventHandler(this.win_MouseUp);
            foreach (Control control in win.Controls)
            {
                control.MouseDown += new MouseEventHandler(this.win_MouseDown);
                control.MouseMove += new MouseEventHandler(this.win_MouseMove);
                control.MouseUp += new MouseEventHandler(this.win_MouseUp);
            }
		}

		private void win_MouseUp(object sender, MouseEventArgs e)
		{
			this._dragging = false;
		}

		private void win_MouseMove(object sender, MouseEventArgs e)
		{
            if (this._dragging)
			{
				Rectangle r = new Rectangle(e.X, e.Y, 1, 1);
				bool flag2 = sender is Form;
				Form form;
				Rectangle rectangle;
				if (flag2)
				{
					form = (sender as Form);
					rectangle = form.RectangleToScreen(r);
				}
				else
				{
					Control control = sender as Control;
					rectangle = control.RectangleToScreen(r);
					form = (control.Parent as Form);
					bool flag3 = form == null;
					if (flag3)
					{
						return;
					}
				}
				int x = form.Location.X + (rectangle.X - this._lastX);
				int y = form.Location.Y + (rectangle.Y - this._lastY);
				this._lastX = rectangle.X;
				this._lastY = rectangle.Y;
				form.SetDesktopLocation(x, y);
			}
		}

		private void win_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Rectangle r = new Rectangle(e.X, e.Y, 1, 1);
				Rectangle rectangle;
				if (sender is Form)
				{
                    rectangle = ((Form)sender).RectangleToScreen(r);
				}
				else
				{
					Control control = sender as Control;
                    rectangle = ((Control)sender).RectangleToScreen(r);
                    if (((Control)sender).Parent is Form)
					{
						return;
					}
				}
				this._lastX = rectangle.X;
				this._lastY = rectangle.Y;
				this._dragging = true;
			}
		}

		public void info(string msg)
		{
			MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		public void error(string msg)
		{
			MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}

		public DialogResult confirm(string msg)
		{
			return MessageBox.Show(msg, "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		}
	}
}
