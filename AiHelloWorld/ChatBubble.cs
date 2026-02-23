using System;
using System.Drawing;
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
            lblText.Font = new Font("Segoe UI", 10F);
            lblText.Padding = new Padding(10, 8, 10, 8);
            lblText.Margin = new Padding(4);
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
                    this.BackColor = Color.White;
                    lblText.ForeColor = Color.FromArgb(28, 28, 30);
                }
                else
                {
                    this.Dock = DockStyle.Right;
                    this.BackColor = Color.FromArgb(0, 122, 255);
                    lblText.ForeColor = Color.White;
                }
            }
        }

        public void SetAlignment(BubbleAlign align)
        {
            if (align == BubbleAlign.Left)
            {
                this.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                this.BackColor = Color.White;
                this.Margin = new Padding(8, 4, 80, 4);
                lblText.ForeColor = Color.FromArgb(28, 28, 30);
            }
            else
            {
                this.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                this.BackColor = Color.FromArgb(0, 122, 255);
                this.Margin = new Padding(80, 4, 8, 4);
                lblText.ForeColor = Color.White;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.Width < 2 || this.Height < 2)
                return;

            int radius = 16;
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
