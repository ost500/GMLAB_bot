using System;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public sql_connection_manager conn_manager;

        int i = 0;

        public string user;

        public DataRow r;
        public DataTable t;
        
        private IPchange ipchanger;

        private bool finished_follow = true;
        private bool finished_like = true;

        //로그인 레디, 테더링 레디
        private bool ready_login = false;
        private bool ready_phone = false;

        //Thread
        private Thread like_thr;

        public Form1()
        {
            InitializeComponent();

            conn_manager = new sql_connection_manager(this);

            DataRow r = conn_manager.version_control();

            textBox1.Text = "EASYGRAM Version. " + r["LatestVersion"].ToString() + "\n";

            conn_manager.like_up();


            ipchanger = new IPchange(this, conn_manager);

            insta_procedure.follow_time = DateTime.Now;
            insta_procedure.like_time = DateTime.Now;


            //모바일 연결



        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            like_thr = new Thread(ipchanger.StartListening);
            like_thr.Start();
        }


        public void log(string logging)
        {
            
                Invoke(new MethodInvoker(delegate () {

                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText("[" + DateTime.Now.ToLongTimeString() + "]" + logging);

                }));
               
          }

        public void like_proc()
        {
            while (true)
            {
                try
                {

                    insta_procedure insta_run = new insta_procedure(this, conn_manager);

                    //시작



                    insta_run.start();


                    if (insta_run.block_check())
                    {
                        //막힌 계정이면 끄고 루프 탈출
                        insta_run.quit();
                        return;
                    }
                    //막힌 계정이 아니면 로그인


                    ////1.해시태그 검색
                    insta_run.hash_tag_search();


                    ////좋아요 루프
                    insta_run.like_loop(1);



                    ////2. 등록된 유저 검색
                    insta_run.random_user();

                    insta_run.like_loop(1);


                    //insta_run.logout();

                    //3. 요청 유저
                    //팔로우, 좋아요
                    insta_run.require();

                    insta_run.like_loop(insta_run.require_follow_count(), insta_run.require_like_count());


                    insta_run.quit();


                    new Thread(ipchanger.send_change).Start();
                    Thread.Sleep(5000);


                }

                catch (Exception ex)
                {

                    Invoke(new MethodInvoker(delegate ()
                    {
                        log(ex.StackTrace);
                        //  textBox1.Text ="ERROR";
                    }));
                }

            }

        }

        //public void follow_proc()
        //{

        //    try
        //    {
        //        insta_procedure insta_run2 = new insta_procedure(this, conn_manager);

        //        //시작



        //        insta_run2.start();


        //        if (insta_run2.block_check())
        //        {
        //            //막힌 계정이면 끄고 루프 탈출
        //            insta_run2.quit();
        //            return;
        //        }
        //        //막힌 계정이 아니면 로그인


        //        //1.해시태그 검색
        //        insta_run2.hash_tag_search();


        //        //팔로우 루프
        //        insta_run2.follow_loop();



        //        //2. 등록된 유저 검색
        //        insta_run2.random_user();

        //        insta_run2.follow_loop();


        //        //insta_run.logout();

        //        //3. 요청 유저
        //        //팔로우, 좋아요

        //        insta_run2.quit();

        //        finished_follow = true;
        //        check_finish_proc();


        //    }
        //    catch (Exception ex)
        //    {
        //        textBox1.Text = ex.StackTrace;
        //    }


        //}




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

                if (responseString == "clear")
                {
                    MessageBox.Show("success");
                    textBox1.AppendText("로그인 성공");
                    user = values["mb_id"];

                    //시작 버튼 활성화 시도
                    start_button_valid("login");

                    t = conn_manager.SelectData();
                    r = t.Rows[0];

                    foreach (DataRow r in t.Rows)
                    {
                        listBox1.Items.Add(r["user_id"]);
                    }
                }
                else
                {
                    MessageBox.Show("계정 정보를 다시 확인 하세요");
                    textBox1.AppendText("로그인 실패");
                }
            }
        }

        public void start_button_valid(string LorP)
        {
            if (LorP == "login")
            {
                ready_login = true;
            }
            else if (LorP == "phone")
            {
                ready_phone = true;
            }

            if (ready_login && ready_phone)
            {
                button1.Enabled = true;
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected_account = listBox1.SelectedItem.ToString();
            account_label.Text = selected_account;


            try
            {
                DataRow dr = conn_manager.Select_job(selected_account);
                limit_comment.Text = dr["comments"].ToString();
                limit_follow.Text = dr["follows"].ToString();
                limit_like.Text = dr["likes"].ToString();
                limit_unfollow.Text = dr["unfollows"].ToString();


                delay_follow.Text = dr["delay_follow"].ToString();
                delay_like.Text = dr["delay_like"].ToString();
                delay_unfollow.Text = dr["delay_unfollow"].ToString();
                delay_comment.Text = dr["delay_comment"].ToString();

                time_start.Text = dr["hour_between_start"].ToString();
                time_finish.Text = dr["hour_between_end"].ToString();
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("저장된 설정이 없습니다");
            }

            


        }


        private void button4_Click(object sender, EventArgs e)
        {
            log(conn_manager.select_request()["mb_id"].ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            log(conn_manager.Select_content()["content"].ToString());
            t = conn_manager.SelectData();
            r = t.Rows[0];

            foreach (DataRow r in t.Rows)
            {
                log(r["user_id"].ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            conn_manager.insert_insta_job();
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process[] processList = Process.GetProcessesByName("chromedriver");

            if (processList.Length > 0)
            {
                processList[0].Kill();
            }

            IPchange.listener.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ipchanger.refresh_connection();
            like_thr.Join();
            like_thr = new Thread(ipchanger.StartListening);
            like_thr.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (ipchanger.socket_check())
            {
                log("socket connected");
            }
            else
            {
                log("socket not connected");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new Thread(ipchanger.send_change).Start();
        }
    }
}
