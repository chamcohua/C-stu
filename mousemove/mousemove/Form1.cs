using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mousemove
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        struct aPoint//曲线上的一个点
        {
            public int time;
            public int yPos;
        };
        List<aPoint> mousePosCurve = new List<aPoint>();//曲线

        int t0 = 0;//存放零时刻毫秒数
        int t1 = 0;//存放当前时刻毫秒数

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (t0 == 0) t0 = System.Environment.TickCount;//t0存放0时刻的毫秒数
                if (System.Environment.TickCount - t1 > 10)//避免数据点过密
                {
                    t1 = System.Environment.TickCount;//获取系统启动后经过的毫秒数，int

                    aPoint a;//临时定义一个数据点
                    a.time = t1 - t0;//计算数据的X坐标，t0为零时刻的毫秒数
                    a.yPos = panel1.Height - e.Y;//计算数据的Y坐标，鼠标向上运动为正方向
                    mousePosCurve.Add(a);//曲线添加一个点
                    chart1.Series[0].Points.AddXY(a.time, a.yPos);//绘图，可视化
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                chart1.Series[0].Points.Clear();
                mousePosCurve.Clear();
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                string strline;
                string[] columns;
                while ((strline = sr.ReadLine()) != null) //一次读入一行
                {
                    columns = strline.Split(','); //按逗号分割成多个列
                    aPoint a;
                    a.time = Convert.ToInt32(columns[0]); //字符串转化为整数
                    a.yPos = Convert.ToInt32(columns[1]); //字符串转化为整数
                    mousePosCurve.Add(a); //曲线中添加一个值
                    chart1.Series[0].Points.AddXY(a.time, a.yPos); //可视化
                }
                sr.Close();


            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, System.Text.Encoding.ASCII);
                int n = mousePosCurve.Count();
                for (int i = 0; i < n; i += 1)
                {
                    sw.Write(mousePosCurve[i].time.ToString() + ","
                                              + mousePosCurve[i].yPos.ToString() + "\r\n");
                }
                sw.Close();

            }

        }
    }
}
