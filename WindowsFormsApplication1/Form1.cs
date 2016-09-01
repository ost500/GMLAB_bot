using MySql.Data.MySqlClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();

        }

        public void like_proc()
        {
            insta_procedure insta_run = new insta_procedure(this);

            insta_run.start();
            insta_run.login();
            insta_run.go_to_there("/yunakim/");

            //insta_run.follow_account();
            insta_run.like_loop();
            //insta_run.logout();
        }

        


        private void button1_Click(object sender, EventArgs e)
        {
            Thread like_thr = new Thread(like_proc);
            like_thr.Start();

        }




    }
}
