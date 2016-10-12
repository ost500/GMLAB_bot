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
        

        int i = 0;

        public string user;

        public DataRow r;
        public DataTable t;

        

        

        private bool finished_follow = true;
        private bool finished_like = true;

        //로그인 레디, 테더링 레디
        private bool ready_login = false;
        private bool ready_phone = false;

        Main_Manager manager;



        public Form1()
        {
            InitializeComponent();
            
            //conn_manager.like_up();

            
            

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            manager = new Main_Manager(this);
        }


        public void log(string logging)
        {


            Invoke(new MethodInvoker(delegate ()
            {

                textBox1.AppendText(Environment.NewLine);
            textBox1.AppendText("[" + DateTime.Now.ToLongTimeString() + "]" + logging);


            }));

        }






        //CHECK STATUS OF HASH TAGS FOR A USER
        public bool checkHashTag()
        {
            DataRow row = manager.conn_manager.check_hashtag();
            if (row == null)
            {
                log("YOU NEED TO ADD FEW HASH TAGS INTO YOUR ACCOUNT");
                log("PLEASE LOG IN HERE: http://easygram.kr/   & ADD FEW HASH TAGS");
                return false;
            }
            else
            {
                log("continue");
                return true;
            }
        }
        //CHECK STATUS OF COMMENTS FOR A USER
        public bool checkCommentStatus()
        {
            DataRow row = manager.conn_manager.check_comments();
            if (row == null)
            {
                log("YOU NEED TO ENTER  COMMENTS");
                log("PLEASE LOG IN HERE: http://easygram.kr/ & ADD FEW COMMENTS");
                return false;
            }
            else
            {
                log("continue");
                return true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Thread like_thr = new Thread(manager.like_proc);
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



                    t = manager.conn_manager.SelectData();
                    r = t.Rows[0];

                    foreach (DataRow r in t.Rows)
                    {
                        listBox1.Items.Add(r["user_id"]);
                    }

                    //Check #tag Status ,comment and job status ..IF Ok then Proceed Otherwise Stop

                    if (checkCommentStatus() && checkHashTag())
                    { //시작 버튼 활성화 시도
                        start_button_valid("login");
                    }
                    else { MessageBox.Show("Some Initial Data Required "); }

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
                ready_phone = true;//testing only
            }

            if (LorP == "phone")
            {
                ready_phone = true;
                ready_login = true;//testing only
            }

            // log("LN "+ready_login.ToString()); log("PH "+ready_phone.ToString());

            if (ready_login && ready_phone)
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected_account = listBox1.SelectedItem.ToString();
            account_label.Text = selected_account;


            try
            {
                DataRow dr = manager.conn_manager.Select_job(selected_account);
                if (dr == null)
                {
                    limit_comment.Text = "None";
                    limit_follow.Text = "None";
                    limit_like.Text = "None";
                    limit_unfollow.Text = "None";


                    delay_follow.Text = "None";
                    delay_like.Text = "None";
                    delay_unfollow.Text = "None";
                    delay_comment.Text = "None";

                    time_start.Text = "None";
                    time_finish.Text = "None";

                }
                else
                {
                    limit_comment.Text = dr["limit_comments"].ToString();
                    limit_follow.Text = dr["limit_follows"].ToString();
                    limit_like.Text = dr["limit_likes"].ToString();
                    //limit_unfollow.Text = dr["unfollows"].ToString();


                    delay_follow.Text = dr["delay_follow"].ToString();
                    delay_like.Text = dr["delay_like"].ToString();
                    delay_unfollow.Text = dr["delay_unfollow"].ToString();
                    delay_comment.Text = dr["delay_comment"].ToString();

                    time_start.Text = dr["hour_between_start"].ToString();
                    time_finish.Text = dr["hour_between_end"].ToString();
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("저장된 설정이 없습니다");
            }




        }


        private void button4_Click(object sender, EventArgs e)
        {
            log(manager.conn_manager.select_request()["mb_id"].ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            log(manager.conn_manager.Select_content()["content"].ToString());
            t = manager.conn_manager.SelectData();
            r = t.Rows[0];

            foreach (DataRow r in t.Rows)
            {
                log(r["user_id"].ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            log("is it working?");
            manager.conn_manager.insert_insta_job();
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

   

        private void button7_Click(object sender, EventArgs e)
        {
            if (Main_Manager.ipchanger.socket_check())
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
            new Thread(Main_Manager.ipchanger.send_change).Start();
        }
    }
}
