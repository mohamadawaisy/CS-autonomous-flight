using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arduinoUI
{
    public partial class Form1 : Form
    {

        SerialPort _serialPort = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);
        int hight, target, correction,flag=0;
        Thread readThread;
        public Form1()
        {
            InitializeComponent();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (flag == 1)
            {
                flag = 0;
                _serialPort.Close();

                readThread.Abort();
                // readThread.Join();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (flag == 0)
            {
                
                pictureBox1.Location= new Point(pictureBox1.Location.X, pictureBox1.Location.Y); 
                flag = 1;
                _serialPort.Open();

              //  byte[] b = BitConverter.GetBytes(Convert.ToInt32(textBox1.Text));
               // serialPort1.Write(b, 0, 4);

                readThread = new Thread(Read);
                readThread.Start();
                // readThread.Join();
            }
        }
        
        public void Read()
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            while (true)
            {
                try
                {
                    byte[] b = BitConverter.GetBytes(Convert.ToInt32(textBox1.Text));
                   
                    
                    string message = _serialPort.ReadLine();
                    char[] arr = message.ToCharArray();
                    AppendTextBox(message.ToString());
                    if (arr.Length==11)
                    {
                            string[] s = message.Split(delimiterChars);
                        if (s.Length >= 3)
                        {
                            hight = Convert.ToInt32(s[0]);
                            target = Convert.ToInt32(s[1]);
                            correction = Convert.ToInt32(s[2]);
                            AppendTextBox1(hight.ToString());
                            AppendTextBox2(target.ToString());
                            AppendTextBox3(correction.ToString());
                            
                            Appendimg(hight);
                        }
                    }
                }
                catch (TimeoutException) { }
            }
        }
        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            textBox2.Text += value;
        }
        public void AppendTextBox1(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox1), new object[] { value });
                return;
            }
            textBox3.Text = value;
        }
        public void AppendTextBox2(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox2), new object[] { value });
                return;
            }
            textBox4.Text = value;
        }
        public void AppendTextBox3(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox3), new object[] { value });
                return;
            }
            textBox5.Text = value;
        }
        public void Appendimg(int value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int>(Appendimg), new object[] { value });
                return;
            }
            pictureBox1.Location = new Point(pictureBox1.Location.X, 600-hight*3);
        }


    }
}
