using MySql.Data.MySqlClient;
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
        private DataSet request_ds;

        public sql_connection_manager(Form1 context)
        {
            String strConn = "Server=110.35.167.2;Database=easygram;Uid=easygram;Pwd=tU2LHxyyTppHUGvw;";
            conn = new MySqlConnection(strConn);

            conn.Open();
            //MySqlCommand cmd = new MySqlCommand("UPDATE insta_account SET work_number = 0", conn);
            //context.textBox1.Text += cmd.ExecuteNonQuery();

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
                string sql = "SELECT * FROM insta_account WHERE mb_id = '" + context.user + "' AND `status` = 1 AND is_profile = 1 AND posting_number_log > 2 ORDER BY work_number";



                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");

                context.log(ds.Tables.Count.ToString());

                context.textBox1.Text += ds.Tables[0].Rows[0]["no"] + " 여기 확인   \n";
                context.textBox1.Text += ds.Tables[0].Rows[0]["work_number"] + " 여기 확인   \n";
                context.textBox1.Text += ds.Tables[0].Rows[1]["no"] + " 여기 확인   \n";
                context.textBox1.Text += ds.Tables[0].Rows[0]["work_number"] + " 여기 확인   \n";
                context.textBox1.Text += ds.Tables[0].Rows[2]["no"] + " 여기 확인   \n";
                context.textBox1.Text += ds.Tables[0].Rows[0]["work_number"] + " 여기 확인   \n";


                if (ds.Tables.Count > 0)
                {

                    return ds.Tables[0];
                }
                else { return null; }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }

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



        public DataRow check_comments()
        {
            DataSet ds_comment = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                context.textBox1.AppendText(context.user);
                string sql = "SELECT a.mb_id,b.comment FROM insta_comment_my as a, insta_comment as b  WHERE a.mb_id = '" + context.user + "'" +
                             " and a.group_id=b.group_id  ORDER BY b.work_number";
                //   context.log(sql);
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds_comment, "comments");

                if (ds_comment.Tables.Count > 0)
                {
                    // foreach (DataRow r in ds.Tables[0].Rows)
                    // {
                    //    Console.WriteLine(r["ID"]);
                    // context.textBox1.Text += r["ID"].ToString();
                    //}

                    return ds_comment.Tables[0].Rows[0];
                }
                else { return null; }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
                //context.button1.Text = "Failed";
            }

        }

        ////////// Select_comments function closed   //////////


        ////////// Check HAsh_tag function Begins  //////////
        public DataRow check_hashtag()
        {

            ds = new DataSet();

            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                context.textBox1.AppendText(context.user);

                string sql = "SELECT * FROM insta_tag_my WHERE mb_id = '" + context.user + "'";


                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "tags");


                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0];
                }
                else { return null; }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;

            }

        }

        ////////// check_HAsh_tag function closed   //////////



        public DataTable Select_comments()
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                context.textBox1.AppendText(context.user);
                string sql = "SELECT a.mb_id,b.comment FROM insta_comment_my as a, insta_comment as b  WHERE a.mb_id = '" + context.user + "'" +
                             " and a.group_id=b.group_id  ORDER BY b.work_number";

                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "comments");



                if (ds.Tables.Count > 0)
                {
                    //foreach (DataRow r in ds.Tables[0].Rows)
                    //{
                    //    Console.WriteLine(r["ID"]);
                    //    textBox1.Text += r["ID"].ToString();
                    //}

                    return ds.Tables[0];
                }
                else { return null; }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
                //context.button1.Text = "Failed";
            }

        }

        ////////// Select_comments function closed   //////////



        public void Update_worknum()
        {
            MySqlCommand cmd2 = new MySqlCommand("UPDATE insta_account SET work_number = work_number + 1 WHERE no = " + ds.Tables[0].Rows[0]["no"], conn);
            context.textBox1.AppendText("UPDATE insta_account SET work_number = work_number + 1 WHERE no = " + ds.Tables[0].Rows[0]["no"]);
            cmd2.ExecuteNonQuery();

        }

        public void Update_comment_worknum(string comment)
        {
            MySqlCommand cmd3 = new MySqlCommand("UPDATE insta_comment SET work_number = work_number + 1 WHERE comment = '" + comment + "'", conn);
            // context.textBox1.AppendText("UPDATE insta_account SET work_number = work_number + 1 WHERE no = " + ds.Tables[0].Rows[0]["no"]);
            cmd3.ExecuteNonQuery();

        }

        ////////// check_job and select_job function begins   //////////
        public DataRow Select_job(string user_id)
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_job WHERE user_id ='" + user_id + "'";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "job");


                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0];
                }
                else { return null; }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);

                return null;
            }


        }
        ////////// check_job and select_job function closed   //////////

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
                string sql =
                    "SELECT insta_tag.no, insta_tag.tag, insta_tag_my.mb_id " +
                    "FROM insta_tag, insta_tag_my " +
                    "WHERE insta_tag_my.group_id = insta_tag.group_id " +
                    "AND insta_tag_my.mb_id = '" + context.user + "' " +
                    "ORDER BY work_number";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");



                if (ds.Tables.Count > 0)
                {

                    context.textBox1.Text += ds.Tables[0].Rows[0]["no"] + "---------    \n";
                    MySqlCommand cmd2 = new MySqlCommand("UPDATE insta_tag SET work_number = work_number + 1 WHERE no = " + ds.Tables[0].Rows[0]["no"], conn);
                    cmd2.ExecuteNonQuery();


                    //foreach (DataRow r in ds.Tables[0].Rows)
                    //{
                    //    context.textBox1.AppendText(r["tag"].ToString());
                    //}

                    return ds.Tables[0].Rows[0];
                }
                else { return null; }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
                //context.button1.Text = "Failed";
            }

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

                    return ds.Tables[0].Rows[0];
                }
                else { return null; }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
                //context.button1.Text = "Failed";
            }

        }

        public void blocked_update()
        {
            MySqlCommand cmd2 = new MySqlCommand("UPDATE insta_account SET blocked = 1 WHERE no = " + ds.Tables[0].Rows[0]["no"], conn);
            context.textBox1.AppendText("차단 기록");
            cmd2.ExecuteNonQuery();
        }



        public void mysql_refresh()
        {
            try
            {
                String strConn = "Server=110.35.167.2;Database=easygram;Uid=easygram;Pwd=tU2LHxyyTppHUGvw;";
                conn = new MySqlConnection(strConn);

                conn.Open();
            }
            catch (Exception ex)
            {
                context.log("mysql refresh error");
                context.log(ex.StackTrace);
            }

        }

        public DataRow select_request()
        {
            request_ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_request_job " +
                             "WHERE request_follow > done_follow " +
                             "OR request_like > done_like ";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(request_ds, "request");

                if (request_ds.Tables.Count > 0)
                {
                    return request_ds.Tables[0].Rows[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return request_ds.Tables[0].Rows[0];
        }

        public DataRow select_configuration()
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_job " +
                             "WHERE user_id = " + ds.Tables[0].Rows[0]["no"];
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


        //Select user follows 
        public DataTable select_follows(string user_id)

        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기

                string sql = "SELECT * FROM insta_follows WHERE user_id ='" + user_id + "'";

                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "followers");



                if (ds.Tables.Count > 0)
                {

                    return ds.Tables[0];
                }
                else { return null; }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);

                return null;
            }

        }
        //Select Followers Count
        public DataRow select_followers_count(string user_id)
        {
            DataSet ds = new DataSet();
            try
            {


                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_status WHERE user_id ='" + user_id + "' order by created_at DESC";

                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "followers");

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0];
                }
                else { return null; }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }

        }

        //STORE Follow Data
        public void insert_followdata(string current_user, string followed, string follow_time)
        {
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기

                MySqlCommand cmd2 = new MySqlCommand("INSERT INTO `easygram`.`insta_follows` (`no`, `user_id`, `followed_id`, `time`) VALUES(NULL, '" + current_user + "', '" + followed + "', '" + follow_time + "');", conn);

                cmd2.ExecuteNonQuery();
                context.textBox1.AppendText("########## Inserted Follow Data ##########");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }
        //Remove Follow Data
        public void remove_followdata(string current_user, string followed)
        {
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기

                MySqlCommand cmd2 = new MySqlCommand("delete from insta_follows where user_id='" + current_user + "' and followed_id= '" + followed + "'", conn);

                cmd2.ExecuteNonQuery();
                context.textBox1.AppendText("########## Deleted Followed Data ##########");

            }
            catch (Exception e)
            {
                context.log("Delete Query Problem!!!! ");
                Console.WriteLine(e.StackTrace);
            }

        }


        //STORE FOLLOWERS COUNT
        public void insert_followersCount(string current_user, int followers_count, string created_at)
        {
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기


                MySqlCommand cmd2 = new MySqlCommand("INSERT INTO `easygram`.`insta_status` (`no`, `user_id`, `followers`, `created_at`) VALUES(NULL, '" + current_user + "', '" + followers_count + "', '" + created_at + "');", conn);
                // context.log(cmd2.ToString());
                if (cmd2.ExecuteNonQuery() > 0)
                {
                    context.textBox1.AppendText("########## Inserted Followers Count  ##########");
                }
                else { context.log(" Followers Count Not Inserted"); }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }

        public void insert_insta_job()
        {
            context.log("work???????????");
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_account WHERE mb_id='ost5253'";

                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "request");

                context.log("working insert");

                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    context.log("working insert!!");
                    context.log(r["user_id"].ToString());
                    MySqlCommand cmd2 = new MySqlCommand("INSERT INTO `easygram`.`insta_job` (`no`, `mb_id`, `user_id`, `delay_follow`, `delay_like`, `delay_comment`, `delay_unfollow`, `hour_between_start`, `hour_between_end`,`limit_comments`,`limit_follows`,`limit_likes`) VALUES(NULL, 'ost5253', '" + r["user_id"] + "', '3', '11', '11', '3', '0', '0','100','1000','1000');", conn);

                    cmd2.ExecuteNonQuery();

                }


            }
            catch (Exception e)
            {
                context.log(e.StackTrace);
            }

        }

        public void update_follow_done()
        {
            try
            {
                MySqlCommand cmd2 =
                    new MySqlCommand(
                        "UPDATE insta_request_job SET done_follow = done_follow + 1 WHERE no = " +
                        request_ds.Tables[0].Rows[0]["no"], conn);

                cmd2.ExecuteNonQuery();
            }
            catch (Exception)
            {
                context.log("요청 계정 팔로우 업데이트 에러");
            }

        }

        public void update_like_done()
        {
            try
            {
                MySqlCommand cmd2 =
                    new MySqlCommand(
                        "UPDATE insta_request_job SET done_like = done_like + 1 WHERE no = " +
                        request_ds.Tables[0].Rows[0]["no"], conn);

                cmd2.ExecuteNonQuery();
            }
            catch (Exception)
            {
                context.log("요청 계정 좋아요 업데이트 에러");
            }
        }

    }
}
