using System.Drawing;
using System.Timers;

namespace DZ4
{
    public partial class Form1 : Form
    {
        //System.Threading.Timer timer; //task1
        List<ResultRow> results = new List<ResultRow>();
        List<MyTimer> timers = new List<MyTimer>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //TimerCallback callback = new TimerCallback(CallbackDel);  // task1
            //timer = new System.Threading.Timer(callback,this.Controls,0,1500); // task1
            foreach (Control control in this.Controls)
            {
                if (control != null && control.GetType() == progressBar1.GetType())
                {
                    ProgressBar bar = (ProgressBar)control;
                    timers.Add(new MyTimer(bar));
                }
            }

            foreach (MyTimer timer in timers)
            {
                timer.startTimer();
            }

            Thread thread = new Thread(
                new ThreadStart(
                        delegate()
                        {
                            bool notend = true;
                            int counter = 0;
                            while (notend)
                            {
                                foreach (MyTimer timer in timers)
                                {
                                    if (!timer.isRunning)
                                    {
                                        counter++;
                                    }
                                    if (counter == 5)
                                    {
                                        notend = false;
                                    }
                                    else
                                    {
                                        counter = 0;
                                    }
                                }
                                Thread.Sleep(200);
                            }
                            foreach (MyTimer timer in timers)
                            {
                                results.Add(timer.getResultTimers());
                            }
                        }
                    )
                );
            thread.Start();

            
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

        private void button1_Click(object sender, EventArgs e)
        {
            string res = "";
            foreach(ResultRow result in results) 
            {
                res += result.ToString() + "\n";
            }
            MessageBox.Show(res);
        }
    }

    class MyTimer
    {
        public bool isRunning = false;
        System.Threading.Timer timer;
        ProgressBar _progress;
        DateTime time;
        TimeSpan span = TimeSpan.Zero;
        public MyTimer(ProgressBar progress)
        {           
            _progress = progress;
        }

        public void startTimer()
        {
            isRunning = true;
            time = DateTime.Now;
            TimerCallback callback = new TimerCallback(this.CallbackDel);  
            timer = new System.Threading.Timer(callback, _progress, 0, 500); 
        }
        void deleteTimer()
        {
            span = DateTime.Now - time;
            isRunning = false;
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
                    deleteTimer();
                }
                else
                    bar.Value += step;
            }));                
            
        }
        public ResultRow getResultTimers()
        {
            return new ResultRow(_progress.Name, span);
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