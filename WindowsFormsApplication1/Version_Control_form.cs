using MySql.Data.MySqlClient;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading;
using System.Windows.Forms;

namespace easygram
{
    public partial class Version_Control_form : Form
    {
        public Version_Control_form()
        {
            InitializeComponent();

            new Thread(background_proc).Start();
        }

        public static void RemoveFileSecurity(string fileName, string account,
            FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Remove the FileSystemAccessRule from the security settings.
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);

        }

        protected void background_proc()
        {
            String strConn = "Server=110.35.167.2;Database=easygram;Uid=easygram;Pwd=tU2LHxyyTppHUGvw;Allow Zero Datetime=true";
            MySqlConnection conn = new MySqlConnection(strConn);
            conn.Open();
            string sql = "SELECT * FROM easygram_VersionControl";
            MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            adpt.Fill(ds, "members");

            if (ds.Tables[0].Rows[0]["LatestVersion"].ToString() != Assembly.GetExecutingAssembly().GetName().Version.ToString())
            {
                MessageBox.Show("update");
                System.IO.FileInfo file = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

                //RemoveFileSecurity(file.Name, @"DomainName\AccountName", FileSystemRights.ReadData, AccessControlType.Allow);

                System.IO.File.Delete(file.DirectoryName + "\\" + "old" + "\\" + file.Name.Replace(file.Extension, "") + "-1" + file.Extension);

                System.IO.File.Move(file.FullName, file.DirectoryName + "\\" + "old" + "\\" + file.Name.Replace(file.Extension, "") + "-1" + file.Extension);

                using (var sftp = new SftpClient("gmlab.kr", 22, "www_user", "qwqw12"))
                {
                    sftp.Connect();
                    List<SftpFile> fileList = sftp.ListDirectory("./easygram/easygram_update/").ToList();


                    foreach (var ftpfile in fileList)
                    {


                        if (ftpfile.Name != "." && ftpfile.Name != "..")
                        {

                            using (var filea = File.OpenWrite(ftpfile.Name))
                            {
                                sftp.DownloadFile(ftpfile.FullName, filea);
                            }
                        }

                    }

                    sftp.Disconnect();
                }
                conn.Close();

                Process.Start(file.FullName);

                Application.Exit();
            }
            else
            {
                this.Close();
                Application.Run(new Login_Form());
            }
        }

    }
}
