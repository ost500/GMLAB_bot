﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class sql_connection_manager
    {
        public MySqlConnection conn;
        public Form1 context;
        public DataSet ds;

        public sql_connection_manager(Form1 context)
        {
            String strConn = "Server=110.35.167.2;Database=easygram;Uid=easygram;Pwd=tU2LHxyyTppHUGvw;";
            conn = new MySqlConnection(strConn);

            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE insta_account SET work_number = 0", conn);
            context.textBox1.Text += cmd.ExecuteNonQuery();

            this.context = context;

            ds = new DataSet();
        }

        public DataTable SelectData()
        {
            ds = new DataSet();
            try
            {


                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                context.textBox1.AppendText(context.user);
                context.log(context.user + " <= contect user");
                string sql = "SELECT * FROM insta_account WHERE mb_id = '" + context.user + "' ORDER BY work_number";

                

                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");



                context.textBox1.Text += ds.Tables[0].Rows[0]["no"] + " 여기 확인   \n";
                context.textBox1.Text += ds.Tables[0].Rows[0]["work_number"] + " 여기 확인   \n";
                context.textBox1.Text += ds.Tables[0].Rows[1]["no"] + " 여기 확인   \n";
                context.textBox1.Text += ds.Tables[0].Rows[0]["work_number"] + " 여기 확인   \n";
                context.textBox1.Text += ds.Tables[0].Rows[2]["no"] + " 여기 확인   \n";
                context.textBox1.Text += ds.Tables[0].Rows[0]["work_number"] + " 여기 확인   \n";


                if (ds.Tables.Count > 0)
                {
                    //foreach (DataRow r in ds.Tables[0].Rows)
                    //{
                    //    Console.WriteLine(r["ID"]);
                    //    textBox1.Text += r["ID"].ToString();
                    //}
                    return ds.Tables[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);

            }
            return ds.Tables[0];
        }

        public DataTable Select_user()
        {
            ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                context.textBox1.AppendText(context.user);
                string sql = "SELECT * FROM insta_account WHERE mb_id = '" + context.user + "' ORDER BY work_number";

                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");




                context.textBox1.Text += ds.Tables[0].Rows[0]["no"] + "    \n";




                if (ds.Tables.Count > 0)
                {
                    //foreach (DataRow r in ds.Tables[0].Rows)
                    //{
                    //    Console.WriteLine(r["ID"]);
                    //    textBox1.Text += r["ID"].ToString();
                    //}
                    return ds.Tables[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                //context.button1.Text = "Failed";
            }
            return ds.Tables[0];
        }


        public DataTable Select_comments()
        {
            ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                context.textBox1.AppendText(context.user);
                string sql = "SELECT * FROM insta_comment WHERE mb_id = '" + context.user + "' ORDER BY work_number";

                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "comments");




                context.textBox1.Text += ds.Tables[0].Rows[0]["no"] + "    \n";




                if (ds.Tables.Count > 0)
                {
                    //foreach (DataRow r in ds.Tables[0].Rows)
                    //{
                    //    Console.WriteLine(r["ID"]);
                    //    textBox1.Text += r["ID"].ToString();
                    //}
                    return ds.Tables[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                //context.button1.Text = "Failed";
            }
            return ds.Tables[0];
        }

        ////////// Select_comments function closed   //////////



        public void Update_worknum()
        {
            MySqlCommand cmd2 = new MySqlCommand("UPDATE insta_account SET work_number = work_number + 1 WHERE no = " + ds.Tables[0].Rows[0]["no"], conn);
            context.textBox1.AppendText("UPDATE insta_account SET work_number = work_number + 1 WHERE no = " + ds.Tables[0].Rows[0]["no"]);
            cmd2.ExecuteNonQuery();

        }

        public DataTable Select_RandomUser(int num_random_user)
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_account ORDER BY work_number_log";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");


                context.textBox1.Text += ds.Tables[0].Rows[0]["no"] + "    \n";

                for (int i = 0; i < num_random_user; i++)
                {
                    MySqlCommand cmd2 = new MySqlCommand("UPDATE insta_account SET work_number_log = work_number_log + 1 WHERE no = " + ds.Tables[0].Rows[i]["no"], conn);
                    cmd2.ExecuteNonQuery();
                }






                if (ds.Tables.Count > 0)
                {
                    //foreach (DataRow r in ds.Tables[0].Rows)
                    //{
                    //    Console.WriteLine(r["ID"]);
                    //    textBox1.Text += r["ID"].ToString();
                    //}
                    return ds.Tables[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                //context.button1.Text = "Failed";
            }
            return ds.Tables[0];
        }


        public DataRow Select_tag()
        {
            context.textBox1.AppendText("clicked");
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_tag WHERE mb_id = '" + context.user + "' ORDER BY work_number";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");

                context.textBox1.Text += ds.Tables[0].Rows[0]["no"] + "---------    \n";
                MySqlCommand cmd2 = new MySqlCommand("UPDATE insta_tag SET work_number = work_number + 1 WHERE no = " + ds.Tables[0].Rows[0]["no"], conn);
                cmd2.ExecuteNonQuery();


                if (ds.Tables.Count > 0)
                {
                    //foreach (DataRow r in ds.Tables[0].Rows)
                    //{
                    //    context.textBox1.AppendText(r["tag"].ToString());
                    //}
                    return ds.Tables[0].Rows[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                //context.button1.Text = "Failed";
            }
            return ds.Tables[0].Rows[0];
        }


        public DataRow Select_content()
        {
            
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_contents WHERE state = '0' AND file !=  '' ORDER BY no limit 1";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");

             
                if (ds.Tables.Count > 0)
                {
                    
                    //foreach (DataRow r in ds.Tables[0].Rows)
                    //{
                    //    context.textBox1.AppendText(r["tag"].ToString());
                    //}
                    return ds.Tables[0].Rows[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                //context.button1.Text = "Failed";
            }
            return ds.Tables[0].Rows[0];
        }


        public void like_up()
        {
            DataSet ds = new DataSet();
            string sql = "UPDATE insta_account SET others_like = others_like + 1 WHERE no = 3";
            MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
            adpt.Fill(ds, "members");
        }

        public DataRow version_control()
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM easygram_VersionControl";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");
                if (ds.Tables.Count > 0)
                {
                    //foreach (DataRow r in ds.Tables[0].Rows)
                    //{
                    //    Console.WriteLine(r["ID"]);
                    //    textBox1.Text += r["ID"].ToString();
                    //}
                    return ds.Tables[0].Rows[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                //context.button1.Text = "Failed";
            }
            return ds.Tables[0].Rows[0];
        }

        public void blocked_update()
        {
            MySqlCommand cmd2 = new MySqlCommand("UPDATE insta_account SET blocked = 1 WHERE no = " + ds.Tables[0].Rows[0]["no"], conn);
            context.textBox1.AppendText("차단 기록");
            cmd2.ExecuteNonQuery();
        }

        public DataRow select_job(string user_id)
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_job WHERE account_id = (SELECT no FROM insta_account WHERE user_id = \"" + user_id + "\")";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return ds.Tables[0].Rows[0];
        }

        public void mysql_refresh()
        {
            String strConn = "Server=110.35.167.2;Database=easygram;Uid=easygram;Pwd=tU2LHxyyTppHUGvw;";
            conn = new MySqlConnection(strConn);

            conn.Open();
        }

        public DataRow select_request()
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_request_job " +
                             "WHERE request_follow > done_follow " +
                             "OR request_like > done_like ";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "request");

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return ds.Tables[0].Rows[0];
        }

        public DataRow select_configuration()
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_job " +
                             "WHERE account_id = " + ds.Tables[0].Rows[0]["no"];
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "request");

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return null;
        }

        public void insert_insta_job()
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_account ";
                             
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "request");


                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    context.log(r["user_id"].ToString());
                    MySqlCommand cmd2 = new MySqlCommand("INSERT INTO `easygram`.`insta_job` (`no`, `mb_id`, `account_id`, `delay_follow`, `delay_like`, `delay_comment`, `delay_unfollow`, `hour_between_start`, `hour_between_end`) VALUES(NULL, 'admin', '"+ r["user_id"] + "', '3', '11', '11', '3', '0', '0');", conn);
                    context.textBox1.AppendText("차단 기록");
                    cmd2.ExecuteNonQuery();

                }

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            
        }

    }
}
