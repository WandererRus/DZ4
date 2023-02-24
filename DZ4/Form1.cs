using System.Drawing;

namespace DZ4
{
    public partial class Form1 : Form
    {
        System.Threading.Timer timer;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TimerCallback callback = new TimerCallback(CallbackDel);
            timer = new System.Threading.Timer(callback,this.Controls,0,1500);

        }

        void CallbackDel(object collection)
        {
            foreach (Control control in (ControlCollection)collection)
            {
                if (control != null && control.GetType() == progressBar1.GetType())
                {
                    ProgressBar bar = (ProgressBar)control;
                    bar.BeginInvoke(new Action(() => {
                        Random random = new Random();
                        bar.Value = random.Next(bar.Minimum, bar.Maximum);
                        Color color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));   
                        bar.BackColor = color;
                        Thread.Sleep(30);
                    }));
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            /*foreach (Control control in this.Controls)
            {
                if (control != null && control.GetType() == progressBar1.GetType())
                {
                    ProgressBar bar = (ProgressBar)control;
                    bar.EndInvoke(null);
                }
            }*/
        }
    }
}