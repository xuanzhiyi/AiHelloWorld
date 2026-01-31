using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AiHelloWorld
{
    public partial class ChatBubble : UserControl
    {
        public ChatBubble()
        {
            InitializeComponent();

			this.AutoSize = true;

			lblText.AutoSize = true;
			lblText.Padding = new Padding(7);
			lblText.Margin = new Padding(5);
			lblText.MaximumSize = new Size(420, 0);

		}

		public string MessageText
		{
			get => lblText.Text;
			set => lblText.Text = value;
		}
		public enum BubbleAlign
		{
			Left,
			Right
		}

		public BubbleAlign Align
		{
			set
			{
				if (value == BubbleAlign.Left)
				{
					this.Dock = DockStyle.Left;
					this.BackColor = Color.LightGray;
				}
				else
				{
					this.Dock = DockStyle.Right;
					this.BackColor = Color.LightBlue;
				}
			}
		}

		public void SetAlignment(BubbleAlign align)
		{
			if (align == BubbleAlign.Left)
			{
				this.Anchor = AnchorStyles.Left | AnchorStyles.Top;

				this.BackColor = Color.LightGray;
				this.Margin = new Padding(5, 5, 100, 5); // push left
			}
			else
			{
				this.Anchor = AnchorStyles.Right | AnchorStyles.Top;

				this.BackColor = Color.LightBlue;
				this.Margin = new Padding(100, 5, 5, 5); // push right
			}

		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			int radius = 20; // corner roundness
			var path = new System.Drawing.Drawing2D.GraphicsPath();

			path.StartFigure();
			path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
			path.AddArc(new Rectangle(this.Width - radius, 0, radius, radius), 270, 90);
			path.AddArc(new Rectangle(this.Width - radius, this.Height - radius, radius, radius), 0, 90);
			path.AddArc(new Rectangle(0, this.Height - radius, radius, radius), 90, 90);
			path.CloseFigure();

			this.Region = new Region(path);
		}


	}
}
