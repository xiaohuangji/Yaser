using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LoginSdoDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(html))
            {
                MessageBox.Show("请点击获得验证码");
                return;
            }
            string postData =
                //"__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=" + viewstate.Replace("+","%2B").Replace ("=","%3D").Replace ("/","%2F") +"&Txt_UserName=50691526&Txt_PassWord=0406&txtCheckCode" + textBox3.Text + "&__EVENTVALIDATION="+validation .Replace ("+","%2B").Replace ("=","%3D").Replace ("/","%2F")+"&Cmd_ok.x=25&Cmd_ok.y=13";
            "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE1MjA1NTIwNzcPZBYCAgEPZBYEAgEPZBYEAgkPDxYCHgRUZXh0BRTnm67liY3lnKjnur%2FvvJo1M%2BS6umRkAgsPDxYCHwAFFeacgOWkmuWcqOe6v%2B%2B8mjEwMOS6umRkAg8PDxYCHgdWaXNpYmxlaGRkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBQZDbWRfb2tFxWUKaw1vPibuXBM3fDY6A11e4A%3D%3D&Txt_UserName=50691526&Txt_PassWord=0406&txtCheckCode=" + textBox3.Text + "&__EVENTVALIDATION=%2FwEWBQLV24qbCQKXtJ3hBAKS5MT8AgKY2YWXBgKkkoy5CW%2BwVQDlPlv29Nr0HE4cmWmIN%2F5f&Cmd_ok.x=38&Cmd_ok.y=8";
            string header;
            string returnHtml = Http.GetHtml("http://yueche.woiche.com", postData, aspcookie, out header); //login
          
            if (hasdenglu(returnHtml))
            {
                this.label4.Text = "已登录";
                this.label2.Text = DateTime.Now.ToString();
                this.timer1.Enabled = true;
                this.button2.Enabled = false;
                if (hasche(returnHtml))
                {
                    for (int i=0; i < 3; i++)
                    {
                        this.Location = new Point(this.Location .X +5,this.Location .Y -5);
                        this.Location = new Point(this.Location.X -5, this.Location.Y+5 );
                        Console.Beep();
                    }
                    DialogResult result = MessageBox.Show("有车了有车了", "返回值 确定 取消", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                    if (result == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start("C:\\Program Files\\360\\360se3\\360SE.exe", "http://yueche.woiche.com/NetBpkFrom.aspx");
                    }
                }
            }
            else
            {
                MessageBox.Show("登录失败，请重新获取验证码登录！");
            }
        }
        //private string Getvalidation(string html)
        //{
        //    string str = html.Substring(html.IndexOf("id=\"__EVENTVALIDATION\""));
        //    str = str.Substring(str.IndexOf("value")+7);
        //    return str.Substring(0, str.IndexOf("\""));
        //}

        //private string Getviewstate(string html)
        //{
        //    string str = html.Substring(html.IndexOf("id=\"__VIEWSTATE\""));
        //    str = str.Substring(str.IndexOf("value") + 7);
        //    return str.Substring(0, str.IndexOf("\""));
        //}

        private bool hasdenglu(string html)
        {
            if (html.Contains("<title>NetBpkFrom</title>"))
                return true;
            else
                return false;
        }

        private bool hasche(string html)
        {
            if (html.Contains("images/mode2.jpg"))
                return true;
            else
                return false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        string aspcookie = "";
        private string html = "";
        private void button3_Click(object sender, EventArgs e)
        {

            html = Http.GetHtml("http://yueche.woiche.com", out aspcookie);//获得Cookie中的SessionID
            aspcookie = aspcookie.Split(';')[0];//这句话可用可不用
            string header = "";
            byte[] b = { };
            Image img = new Bitmap(
            Http.GetStreamByBytes("http://yueche.woiche.com", "http://yueche.woiche.com/CheckCode.aspx", b,
                                  aspcookie, out header));//获得验证码图片
            this.pictureBox1.Image = img;
        }

      

        private void timer1_Tick(object sender, EventArgs e)
        {
            string header = "";
            string returnHtml = Http.GetHtml("http://yueche.woiche.com/NetBpkFrom.aspx", aspcookie, out header, "http://yueche.woiche.com/"); //login

            if (hasdenglu(returnHtml))
            {
                this.label2.Text = DateTime.Now.ToString();
                if (hasche(returnHtml))
                {
                   
                    for (int i = 0; i < 3; i++)
                    {
                        this.Location = new Point(this.Location.X + 5, this.Location.Y - 5);
                        this.Location = new Point(this.Location.X - 5, this.Location.Y + 5);
                        Console.Beep();

                    }
                   DialogResult result =MessageBox.Show("有车了有车了", "返回值 确定 取消",MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                   if (result == DialogResult.OK)
                   {
                       System.Diagnostics.Process.Start("C:\\Program Files\\360\\360se3\\360SE.exe", "http://yueche.woiche.com/NetBpkFrom.aspx");
                   }
                
                }
            }
            else
            {
                MessageBox.Show("刷新页面失败，请重新获取验证码登录！");
            
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int shuaxin=Convert.ToInt32(textBox1.Text);
                this.timer1.Enabled = false;
                this.timer1.Interval = shuaxin * 1000;
                this.timer1.Enabled = true;
            }
            catch
            {
                MessageBox.Show("请输入正确的数值！");
            }
        }
    }
}