using System.Drawing;
using System.Timers;

namespace DZ4
{
    public partial class Form1 : Form
    {
        //System.Threading.Timer timer; //task1
        System.Threading.Timer[] timer; //task2
        List<ResultRow> results = new List<ResultRow>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //TimerCallback callback = new TimerCallback(CallbackDel);  // task1
            //timer = new System.Threading.Timer(callback,this.Controls,0,1500); // task1
        }

        void CallbackDel(object collection)
        {
            /* foreach (Control control in (ControlCollection)collection)  //task1
             {
                 if (control != null && control.GetType() == progressBar1.GetType())
                 {
                     ProgressBar bar = (ProgressBar)control;
                     bar.BeginInvoke(new Action(() => {
                         Random random = new Random();
                         bar.Value = random.Next(bar.Minimum, bar.Maximum);
                         Thread.Sleep(30);
                     }));
                 }
             }*/
             foreach (Control control in (ControlCollection)collection)  //task2
             {
                 if (control != null && control.GetType() == progressBar1.GetType())
                 {
                     ProgressBar bar = (ProgressBar)control;
                     bar.BeginInvoke(new Action(() => {
                         Random random = new Random();
                         int step = random.Next(0, 5);
                         if (bar.Value + step > bar.Maximum)
                         { 
                             bar.Value = bar.Maximum;
                             results.Add(new ResultRow(bar.Name,TimeSpan.Zero));
                         }
                         else
                             bar.Value += step;
                     }));
                 }
             }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            /*foreach (Control control in this.Controls)  //task1
            {
                if (control != null && control.GetType() == progressBar1.GetType())
                {
                    ProgressBar bar = (ProgressBar)control;
                    bar.EndInvoke(null);
                }
            }*/
        }
    }

    class MyTimer
    {
        System.Threading.Timer timer;
        ProgressBar _progress;
        public MyTimer(ProgressBar progress)
        {
            _progress = progress;
        }

        void startTimer()
        {
            TimerCallback callback = new TimerCallback(this.CallbackDel);  
            timer = new System.Threading.Timer(callback, _progress, 0, 500); 
        }
        void deleteTimer()
        {
            timer.Dispose();
        }
        void CallbackDel(object obj)
        {
            ProgressBar bar = (ProgressBar)obj;
            bar.BeginInvoke(new Action(() => {
                Random random = new Random();
                int step = random.Next(0, 5);
                if (bar.Value + step > bar.Maximum)
                {
                    bar.Value = bar.Maximum;
                    results.Add(new ResultRow(bar.Name, TimeSpan.Zero));
                }
                else
                    bar.Value += step;
            }));                
            
        }
    }

    class ResultRow 
    {
        string BarName;
        TimeSpan span;
        
        
        public ResultRow(string name, TimeSpan time)
        {
            BarName = name;
            span = time;
        }
        public override string ToString()
        {
            return BarName + " " + span.ToString();
        }
    }
}