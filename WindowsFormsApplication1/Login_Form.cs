using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
            
        }

       
        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox2.UseSystemPasswordChar = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["mb_id"] = textBox1.Text;
                values["mb_password"] = textBox2.Text;
                values["easygram"] = "K";

                var response = client.UploadValues("http://www.easygram.kr/manager/bbs/login_check.php", values);

                var responseString = Encoding.Default.GetString(response);

                if (responseString == "clear")
                {
                    MessageBox.Show(" [이지그램] : 로그인 성공");
                    string user = values["mb_id"];


                    this.Hide();

                    Form1 mainForm1 = new Form1(user);
                    mainForm1.Show();


                }
                else
                {

                    MessageBox.Show(" [이지그램] 로그인 실패");
                }
            }
        }
    }
}
