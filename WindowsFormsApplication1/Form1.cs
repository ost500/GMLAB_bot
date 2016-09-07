using MySql.Data.MySqlClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        sql_connection_manager conn_manager;

        int i = 0;

        public Form1()
        {
            InitializeComponent();

            conn_manager = new sql_connection_manager(this);

            DataRow r = conn_manager.version_control();

            textBox1.Text = r["LatestVersion"].ToString();

            conn_manager.like_up();

        }

        public void like_proc()
        {
            try
            {

                insta_procedure insta_run = new insta_procedure(this, conn_manager);

                //시작
                

                while (true)
                {

                    insta_run.start(i++);

                    //로그인
                    insta_run.login();
                    //1. 해시태그 검색
                    insta_run.hash_tag_search();

                    
                    //좋아요 + 팔로우 루프
                    insta_run.like_follow_loop();

                    

                    //2. 등록된 유저 검색
                    insta_run.random_user();
                    //좋아요 루프
                    insta_run.like_loop();

                    //insta_run.logout();

                    //3. 요청 유저
                    //팔로우, 좋아요

                    insta_run.quit();


                }


            }
            catch (Exception ex)
            {
                textBox1.Text = ex.StackTrace;
            }
        }




        private void button1_Click(object sender, EventArgs e)
        {

            Thread like_thr = new Thread(like_proc);
            like_thr.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["mb_id"] = textBox2.Text;
                values["mb_password"] = textBox3.Text;
                values["easygram"] = "K";

                var response = client.UploadValues("http://www.easygram.kr/manager/bbs/login_check.php", values);

                var responseString = Encoding.Default.GetString(response);
                
                if(responseString == "clear")
                {
                    MessageBox.Show("success");
                    textBox1.AppendText("로그인 성공");
                }
                else
                {
                    MessageBox.Show("계정 정보를 다시 확인 하세요");
                    textBox1.AppendText("로그인 실패");
                }
            }
        }
    }
}
