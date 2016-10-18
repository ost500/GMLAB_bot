﻿using System;
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

        public int total_user;



        private bool finished_follow = true;
        private bool finished_like = true;

        //로그인 레디, 테더링 레디
        private bool ready_login = false;
        private bool ready_phone = false;

        public bool hash_tag_checked;
        public bool random_user_checked;
        public bool unfollow_checked;

        Main_Manager manager;

        private Thread like_thr;



        public Form1(string user)
        {
            InitializeComponent();

            //conn_manager.like_up();

            this.user = user;

            Thread thr = new Thread(this.form_start);
            thr.Start();

        }

    
        public void form_start()
        {
            manager = new Main_Manager(this);

            try
            {
                t = manager.conn_manager.SelectData();
                r = t.Rows[0];


                foreach (DataRow r in t.Rows)
                {
                    listBox1.Items.Add(r["user_id"]);
                }

                //Check #tag Status ,comment and job status ..IF Ok then Proceed Otherwise Stop

                if (checkCommentStatus() && checkHashTag())
                {
                    //시작 버튼 활성화 시도
                    //get the total users and login
                    total_user = t.Rows.Count;
                    //total_user = 2;
                    

                }
                else
                {
                    MessageBox.Show(" [데이터베이스] 기본 데이터를 입력하세요");
                }
                
                
            }

            //else { MessageBox.Show("먼저 로그인하세요 "); }


            catch { log("No Users Record found!!!"); }

            

        }


        public void log(string logging)
        {
            
                textBox1.AppendText(Environment.NewLine);
                textBox1.AppendText("[" + DateTime.Now.ToLongTimeString() + "]" + logging);
            
        }






        //CHECK STATUS OF HASH TAGS FOR A USER
        public bool checkHashTag()
        {
            DataRow row = manager.conn_manager.check_hashtag();
            if (row == null)
            {
                //log("YOU NEED TO ADD FEW HASH TAGS INTO YOUR ACCOUNT");
                //log("PLEASE LOG IN HERE: http://easygram.kr/   & ADD FEW HASH TAGS");
                log(" [이지그램] 입력할 해시태그를 저장하세요");
                log(" [이지그램] http://easygram.kr/");
                return false;
            }
            else
            {

                return true;
            }
        }
        //CHECK STATUS OF COMMENTS FOR A USER
        public bool checkCommentStatus()
        {
            DataRow row = manager.conn_manager.check_comments();
            if (row == null)
            {
                //log("YOU NEED TO ENTER  COMMENTS");
                //log("PLEASE LOG IN HERE: http://easygram.kr/ & ADD FEW COMMENTS");
                log(" [이지그램] 입력할 댓글을 저장하세요");
                log(" [이지그램] http://easygram.kr/");
                return false;
            }
            else
            {

                return true;
            }

        }

        private void iTalk_Button_21_Click(object sender, EventArgs e)
        {
            like_thr = new Thread(manager.like_proc);
            like_thr.Start();
        }


        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        public void start_button_valid(string LorP)
        {
            if (LorP == "login")
            {

                ready_login = true;
                button1.Enabled = true;
                //  ready_phone = true;//testing only
            }

            if (LorP == "phone")
            {
                ready_phone = true;
                //button2.Enabled = true;
                // ready_login = true;//testing only
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
            try
            {
                Process[] processList = Process.GetProcessesByName("chromedriver");

                if (processList.Length > 0)
                {
                    processList[0].Kill();
                }

            }
            catch (Exception) { }
            try
            {

                IPchange.listener.Close();
            }
            catch (Exception) { }

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            hash_tag_checked = checkBox1.Checked;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            random_user_checked = checkBox2.Checked;

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            unfollow_checked = checkBox3.Checked;
        }

        private void iTalk_RadioButton1_CheckedChanged(object sender)
        {
            manager.mobile_connection();
        }

        private void iTalk_Button_11_Click(object sender, EventArgs e)
        {
            manager.insta_run.quit();
            like_thr.Abort();
        }
    }
}
