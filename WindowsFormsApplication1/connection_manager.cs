using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class sql_connection_manager
    {
        public MySqlConnection conn;
        Form1 context;

        public sql_connection_manager(Form1 context)
        {
            String strConn = "Server=110.35.167.2;Database=easygram;Uid=easygram;Pwd=tU2LHxyyTppHUGvw;";
            conn = new MySqlConnection(strConn);

            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE insta_account SET work_number = 0", conn);
            context.textBox1.Text += cmd.ExecuteNonQuery();

            this.context = context;
        }

        public DataTable SelectData(int index)
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_account ORDER BY work_number";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");


                context.textBox1.Text += ds.Tables[0].Rows[0]["no"]+"    \n";

                
                MySqlCommand cmd2 = new MySqlCommand("UPDATE insta_account SET work_number = work_number + 1 WHERE no = " + ds.Tables[0].Rows[0]["no"], conn);
                cmd2.ExecuteNonQuery();
                
                
                


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


        public DataTable Select_RandomUser()
        {
            DataSet ds = new DataSet();
            try
            {

                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_account ORDER BY work_number";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");


                context.textBox1.Text += ds.Tables[0].Rows[0]["no"] + "    \n";


                MySqlCommand cmd2 = new MySqlCommand("UPDATE insta_account SET work_number = work_number + 1 WHERE no = " + ds.Tables[0].Rows[0]["no"], conn);
                cmd2.ExecuteNonQuery();





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
                string sql = "SELECT * FROM insta_tag ORDER BY work_number";
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
    }
}
