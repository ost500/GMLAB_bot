using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Data;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Security.AccessControl;

namespace easygram
{
    class insta_procedure
    {

        protected static ChromeDriver driver;
        private StringBuilder verificationErrors;
        protected string baseURL = "https://www.instagram.com";
        private bool acceptNextAlert = true;
        protected Form1 context;
        protected WebDriverWait wait;

        protected IWebElement myDynamicElement;
        protected sql_connection_manager conn_manager;

        protected Random rnd = new Random();



        public DataTable t;
        public DataRow r;


        //follow 시간 like 시간
        public static DateTime follow_time;
        public static DateTime like_time;
        public static DateTime comment_time;
        public static string save_follow_time;

        //Limit variables
        public static int comments_count;
        public static int likes_count;
        public static int follows_count;
        public static int limit_comments;
        public static int limit_likes;
        public static int limit_follows;

        //Delay variables
        public static double delay_like;
        public static double delay_comment;
        public static double delay_follow;
        public static double delay_unfollow;

        //insta_user variables
        public static string current_user;
        public static string user_agent;
        //public string user_id;


        public insta_procedure(Form1 context, sql_connection_manager conn_manager)
        {


            this.context = context;

            this.conn_manager = conn_manager;

            //랜덤 대기값

            t = context.t;
            r = context.r;

            follow_time = DateTime.Now;
            comment_time = DateTime.Now;



        }

        public string getCurrentPath()
        {
            string currentDirName = Directory.GetCurrentDirectory();
            //     DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(currentDirName).ToString());
            DirectoryInfo directoryInfo = Directory.GetParent(currentDirName);
            string path = directoryInfo.ToString();
            return path;
        }

        public void update_todayCounters(string user_id, string latest_date)
        {
            // latest_date = r["latest_date"].ToString();
            string now_date = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                DateTime dt = DateTime.ParseExact(latest_date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                latest_date = dt.ToString("yyyy-MM-dd");
            }
            catch (Exception) { }

            //if latest_date is not equal to current date then upadte date and set like and comment count to 0
            if (latest_date != now_date)
            {
                conn_manager.update_count_date(user_id, now_date);
            }



        }

        //CHECK  Run Status i.e. start and end time FOR A USER 
        public bool checkRunStatus(string user_id)
        {

            try
            {
                DataRow jobrow = conn_manager.Select_job(user_id);

                int start_time = Int32.Parse(jobrow["hour_between_start"].ToString());
                int end_time = Int32.Parse(jobrow["hour_between_end"].ToString());


                int now_time = Int32.Parse(DateTime.Now.ToString("HH"));

                Thread.Sleep(rnd.Next(3000, 4000));
                //Procedure should not start if now time is not in between start and end time

                if (jobrow == null)
                {
                    context.log(" [이지그램] : No job record  " + user_id);
                    context.log(" [이지그램] :  로그인 http://easygram.kr/ & 딜레이 시간 입력");

                    //context.log("YOU NEED TO ENTER DELAY FOR LIKE , FOLLOW, COMMENT AND UNFOLLOW FOR " + user_id);
                    //context.log("PLEASE context.log IN HERE: http://easygram.kr/ & ADD DELAYS");

                    Thread.Sleep(rnd.Next(3000, 4000));
                    return true;
                }
                else
                {
                    if ((start_time <= now_time) && (now_time <= end_time))
                    {
                        Thread.Sleep(rnd.Next(3000, 4000));
                        return true;

                    }
                    else
                    {
                        context.log("Please check your start or end time"); Thread.Sleep(rnd.Next(3000, 4000)); return false;
                    }


                }
            }
            catch (Exception)
            {
                return true;
            }

        }
        //CHECK STATUS OF JOB FOR A USER 
        public bool checkJobStatus(string user_id)
        {


            DataRow row = conn_manager.Select_job(user_id);
            if (row == null)
            {
                context.log(" [이지그램] : 좋아요, 팔로우, 댓글 딜레이 시간을 입력하세요 " + user_id);
                context.log(" [이지그램] :  로그인 http://easygram.kr/ & 딜레이 시간 입력");

                //context.log("YOU NEED TO ENTER DELAY FOR LIKE , FOLLOW, COMMENT AND UNFOLLOW FOR " + user_id);
                //context.log("PLEASE context.log IN HERE: http://easygram.kr/ & ADD DELAYS");


                return true;
            }
            else
            {
                return true;
            }
        }
        //GET limit_likes,limit__comments and limit__follows from database for current user
        public int getLimitLikes(string user_id)
        {

            DataRow jobrow = conn_manager.Select_job(user_id);
            if (jobrow == null)
            {
                return 1000;
            }
            else
            {

                limit_likes = Int32.Parse(jobrow["limit_likes"].ToString());

                return limit_likes;
            }

        }

        public int getLimitComments(string user_id)
        {

            DataRow jobrow = conn_manager.Select_job(user_id);
            if (jobrow == null)
            {
                return 100;
            }
            else
            {
                limit_comments = Int32.Parse(jobrow["limit_comments"].ToString());


                return limit_comments;
            }
        }

        public int getLimitFollows(string user_id)
        {

            DataRow jobrow = conn_manager.Select_job(user_id);
            if (jobrow == null)
            {
                return 1000;
            }
            else
            {
                limit_follows = Int32.Parse(jobrow["limit_follows"].ToString());

                return limit_follows;

            }
        }

        //GET delay_like,delay_comment,delay_follow and delay_unfollow from database for current user
        public double getLikeDelay(string user_id)
        {

            DataRow jobrow = conn_manager.Select_job(user_id);
            if (jobrow == null)
            {
                return 11;
            }
            else
            {

                delay_like = Int32.Parse(jobrow["delay_like"].ToString());

                return delay_like;
            }

        }

        public double getCommentDealy(string user_id)
        {

            DataRow jobrow = conn_manager.Select_job(user_id);
            if (jobrow == null)
            {
                return 25;
            }
            else
            {
                delay_comment = Int32.Parse(jobrow["delay_comment"].ToString());

                return delay_comment;
            }
        }

        public double getFollowDelay(string user_id)
        {

            DataRow jobrow = conn_manager.Select_job(user_id);
            if (jobrow == null)
            {
                return 3;
            }
            else
            {
                delay_follow = Int32.Parse(jobrow["delay_follow"].ToString());

                return delay_follow;

            }
        }

        public double getUnfollowDelay(string user_id)
        {
            DataRow jobrow = conn_manager.Select_job(user_id);
            if (jobrow == null)
            {
                return 72;
            }
            else
            {
                delay_unfollow = Int32.Parse(jobrow["delay_unfollow"].ToString());

                return delay_unfollow;
            }
        }


        //SET TIME DELAY TO LIKE PICTURES
        public bool like_time_gap(double delay_like)
        {

            //context.log("LIKE DELAY:"+like.ToString());
            DateTime now = DateTime.Now;
            //context.log(now.ToString());
            //context.log(like_time.ToString());
            //context.log(now.Subtract(follow_time).TotalSeconds.ToString() + "Like_time_gap");
            string remaining_like_time = now.Subtract(like_time).TotalSeconds.ToString("##.###");
            if (now.Subtract(like_time).TotalSeconds > delay_like)
            {
                like_time = DateTime.Now;
                return true;
            }
            else
            {
                //Thread.Sleep(11000);
                //like_time = DateTime.Now;

                //context.log("no like");

                context.log(" [인스타 루프] 좋아요 : 딜레이 간격을 위해 휴식합니다");
                context.log(" [인스타 루프] 좋아요 : " + remaining_like_time + "[" + delay_like + "] 초 간격");
                return false;
            }
        }

        //SET TIME DELAY TO FOLLOW OTHERS
        public bool follow_time_gap(double delay_follow)
        {
            DateTime now = DateTime.Now;
            //context.log("-----------------");
            //context.log(now.ToString());
            //context.log(follow_time.ToString());
            //context.log(now.Subtract(follow_time).TotalMinutes.ToString() + "follow_time_gap");
            string remaining_follow_time = now.Subtract(follow_time).TotalMinutes.ToString("##.###");
            if (now.Subtract(follow_time).TotalMinutes > delay_follow)
            {
                follow_time = DateTime.Now;
                return true;
            }
            else
            {
                //Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                //context.log("no follow");

                context.log(" [인스타 루프] 팔로우 : 딜레이 간격을 위해 휴식합니다");
                context.log(" [인스타 루프] 팔로우 : " + remaining_follow_time + "[" + delay_follow + "] 분 간격");
                return false;
            }
        }

        //SET TIME DELAY TO COMMENT ON PICTURES
        public bool comment_time_gap(double delay_comment)
        {
            DateTime now = DateTime.Now;

            //  context.log("COMMENT DELAY:" + comment.ToString());
            //context.log(now.ToString());
            //context.log(comment_time.ToString());

            //context.log(now.Subtract(comment_time).TotalSeconds.ToString() + "comment_time_gap");

            string remaining_comment_time = now.Subtract(comment_time).TotalSeconds.ToString("##.###");
            if (now.Subtract(comment_time).TotalSeconds > delay_comment)

            {
                comment_time = DateTime.Now;
                return true;
            }
            else
            {
                //context.context.log("no work");
                //Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                context.log(" [인스타 루프] 댓글 : 딜레이 간격을 위해 휴식합니다");
                // context.log(" [인스타 루프] 댓글 : " + delay_comment.ToString("##.###") + "초 간격");
                context.log(" [인스타 루프] 댓글 : " + remaining_comment_time + "[" + delay_comment + "] 초 간격");
                return false;
            }
        }


        public void saveFollowData(string followed)
        {
            // Check for exact CSS Selector and save Follow data

            /*  if (IsElementPresent(By.CssSelector("a._4zhc5")))
              {

                  //find the above followed user
                  string followed = driver.FindElement(By.CssSelector("a._4zhc5")).Text;
                  */
            //set time
            save_follow_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                DataTable follow_data = conn_manager.select_specific_follow(current_user, followed);
                //insert followed id into database
                if (follow_data.Rows.Count > 0)
                {
                    context.log(current_user + " is already following " + followed);
                }
                else
                {
                    conn_manager.insert_followdata(current_user, followed, save_follow_time);
                }
            }
            catch { context.log("Error in the select_specfic_follow Query"); }
            /* }
             else if (IsElementPresent(By.CssSelector("h1._i572c")))
             {
                 //find the above followed user
                 string followed = driver.FindElement(By.CssSelector("h1._i572c")).Text;

                 //set time
                 save_follow_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                 try
                 {
                     DataTable follow_data = conn_manager.select_specific_follow(current_user, followed);
                     //insert followed id into database
                     if (follow_data.Rows.Count > 0)
                     {
                         context.log(current_user + " is already following" + followed);
                     }
                     else
                     {
                         conn_manager.insert_followdata(current_user, followed, save_follow_time);
                     }
                 }
                 catch { context.log("Error in the select_specfic_follow Query"); }
             }*/

        }
        public void saveLikesCount()
        {
            //set time

            context.log("L: "+likes_count.ToString());
            conn_manager.update_likescount(current_user, likes_count);
            Thread.Sleep(rnd.Next(2000, 3000));
        }

        public void saveCommentsCount()
        {
            //set time
          
            conn_manager.update_commentscount(current_user, comments_count);
            Thread.Sleep(rnd.Next(2000, 3000));
        }
        public void saveFollowsCount()
        {
            //set time
            conn_manager.update_followscount(current_user, follows_count);
            Thread.Sleep(rnd.Next(2000, 3000));
        }

        public void changeLanguageOnLogin()
        {

            try
            {


                //check for English Log in
                Assert.AreEqual("Log in", driver.FindElement(By.CssSelector("a._fcn8k")).Text);

                //if matched then change language to Korean otherwise its already korean
                new SelectElement(driver.FindElement(By.CssSelector("select._nif11"))).SelectByText("Korean");
                context.log("Login Language Changed");
            }
            catch (Exception e)
            {

                // context.log("Login is already Korean");

            }


        }


        public void changeLanguageOnProfile()
        {
            try
            {

                //check for Profile Keyword 
                Assert.AreEqual("Profile", driver.FindElement(By.LinkText("Profile")).Text);
                //if Profile Word exist [means lanuage is English] then click on the Profile 
                driver.FindElement(By.LinkText("Profile")).Click();
                //Now Change the language to Korean
                new SelectElement(driver.FindElement(By.CssSelector("select._nif11"))).SelectByText("Korean");
                context.log("Language Changed after clicking on Profile");

            }
            catch (Exception exc)
            {

                //context.log("Profile is already in korean");

            }

        }
        public void update_user_agent()
        {
            try
            {
                //Select agent from insta_user_agent
                DataTable agent_tbl = conn_manager.Select_agent();

                if (agent_tbl != null)
                {

                    //get the random  user agent
                    int agent_rows_num = agent_tbl.Rows.Count;
                    int range = rnd.Next(1, agent_rows_num);


                    DataRow agent_row = agent_tbl.Rows[range];
                    string agent = agent_row["agent"].ToString();
                    //Update the user agent
                    conn_manager.Update_user_agent(current_user, agent);
                    //set new user agent
                    user_agent = agent_row["agent"].ToString();

                }
                else
                {
                    context.log("No user agents are there");
                }
            }
            catch { context.log("User agent not updated"); }
        }



        //public void context.log(string logging)
        //{
        //    context.textBox1.AppendText(Environment.NewLine);
        //    context.textBox1.AppendText("[" + DateTime.Now.ToString() + "]" + logging);

        //}

        public void start()
        {

            t = conn_manager.SelectData();
            if (t == null) { throw new NullReferenceException(); }

            r = t.Rows[0];

            //work_number 1 더하기
            conn_manager.Update_worknum();


            context.listBox1.Items.Clear();

            foreach (DataRow r in t.Rows)
            {
                context.listBox1.Items.Add(r["user_id"].ToString());
            }

            //Select the current item in the list
            context.listBox1.Focus();
            context.listBox1.SetSelected(0, true);

            //String path = "D:\\chrome_cache\\" + r["user_id"].ToString();

            current_user = r["user_id"].ToString();


            //context.context.log(" USER:" + current_user + " \n");
            context.log(" [이지그램] 로그인 유저 :  " + current_user + " \n");

            //update the like,comment and follow counters
            update_todayCounters(r["user_id"].ToString(), r["latest_date"].ToString());

            //check for the start and end timimgs
            if (!checkRunStatus(current_user))
            {
                context.log("Thank You!!!!");
                //Thread.Sleep(rnd.Next(4000, 5000));
                return;
            }


            //get all limits
            limit_likes = getLimitLikes(current_user);
            limit_comments = getLimitComments(current_user);
            limit_follows = getLimitFollows(current_user);

            //get today's counts
            likes_count = Int32.Parse(r["likes_count"].ToString());
            comments_count = Int32.Parse(r["comments_count"].ToString());
            follows_count = Int32.Parse(r["follows_count"].ToString());

            //get all delays
            delay_like = getLikeDelay(current_user);
            delay_comment = getCommentDealy(current_user);
            delay_follow = getFollowDelay(current_user);
            delay_unfollow = getUnfollowDelay(current_user);


            //Get current directory path and set charome cache path
            string currentDir = getCurrentPath();
            // context.log(currentDir);
            string path = currentDir + "\\Release\\chrome_cache\\" + current_user.Trim();
            //   string path = "c:\\chrome_cache\\" + current_user.Trim();

            /*  DirectoryInfo dirInfo = new DirectoryInfo(@"c:\chrome_cache");

              DirectorySecurity dirSecurity = dirInfo.GetAccessControl();

              dirSecurity.AddAccessRule(new FileSystemAccessRule
                  (Enviroment.Username,
                  FileSystemRights.ReadData, AccessControlType.Allow));

              dirInfo.SetAccessControl(dirSecurity);*/

            ChromeOptions co = new ChromeOptions();

            co.AddArguments("user-data-dir=" + path);



            /*
            //get the Browser User Agent For current user
            user_agent = r["user_agent"].ToString();


            if (user_agent == "")
            {
                update_user_agent();

            }
            //Set the Browser User Agent For current user
            co.AddArguments("--user-agent=" + user_agent);
            */



            var options = new ChromeOptions();


            var driverService = ChromeDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
            driverService.HideCommandPromptWindow = true;

            //driverService.Port = my_port;
            //co.BinaryLocation = "C:\\Program Files (x86)\\Google\\Chrome\\Application";


            //driver = new ChromeDriver(co);
            driver = new ChromeDriver(driverService, co);
            //driver = new ChromeDriver("C:\\Program Files (x86)\\Google\\Chrome\\Application");



            verificationErrors = new StringBuilder();

            context.log(" [인스타 루프] : 시작했습니다");

            driver.Navigate().GoToUrl(baseURL + "/");
            context.log(" [인스타 루프] : 메인으로 갔습니다");

        }


        public bool block_check()
        {


            try
            {

                //Check job status ..IF Ok then Proceed Otherwise Stop

                if (!checkJobStatus(current_user)) { return true; }

                //로그인 버튼이 안뜨고 에러가 난다면 로그인이 돼 있다는 얘기

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                myDynamicElement = wait.Until(d => d.FindElement(By.LinkText("로그인")));



            }
            catch (Exception ex)
            {
                try
                {

                    Assert.AreEqual("계정 인증", driver.FindElement(By.CssSelector("h2")).Text);
                    //계정 인증 메세지가 나오면 막힌 계정임
                    //Error
                    context.log(" [인스타 루프] : 에러가 있습니다");
                }
                catch (Exception e)
                {

                    //Change Language to Korean If Profile is in English
                    changeLanguageOnProfile();

                    //안나오면 정상 가동
                    //no verify your account
                    //context.log("계정 인증이 없다");


                    return false;
                }
                //verify your account
                context.log(" [인스타 루프] : 계정 인증 존재");
                //DB 기록
                conn_manager.blocked_update();
                Thread.Sleep(rnd.Next(15000, 20000));//to verify
                return true;
            }
            //login button exists
            context.log(" [인스타 루프] : 로그인 버튼 존재");
            login(); //instagram login

            return false;
        }


        public void login()
        {

            driver.FindElement(By.LinkText("로그인")).Click();
            driver.FindElement(By.Name("username")).Clear();
            driver.FindElement(By.Name("username")).SendKeys(r["user_id"].ToString());
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys(r["user_pass"].ToString());

            Thread.Sleep(rnd.Next(1000, 3000));
            driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/div[2]/div/div/form/span/button")).Click();


            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("input._9x5sw._qy55y")));

        }

        public void go_to_there(string where_to)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                for (int i = 0; i <= 150; i += 20)
                {
                    js.ExecuteScript("window.scrollTo(" + i + ", " + (i + 20) + ");");
                    Thread.Sleep(rnd.Next(100, 300));
                }
                for (int i = 150; i >= 0; i -= 20)
                {
                    js.ExecuteScript("window.scrollTo(" + i + ", " + (i - 20) + ");");
                    Thread.Sleep(rnd.Next(100, 300));
                }
            }
            catch (Exception)
            {

            }


            Thread.Sleep(rnd.Next(1000, 3000));

            driver.FindElement(By.CssSelector("input._9x5sw._qy55y")).Clear();
            Thread.Sleep(rnd.Next(1000, 3000));
            driver.FindElement(By.CssSelector("input._9x5sw._qy55y")).SendKeys(where_to);
            Thread.Sleep(rnd.Next(1000, 3000));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            myDynamicElement = wait.Until(d => d.FindElement(By.XPath("//div[2]/div/a/div/div[2]")));

            driver.FindElement(By.XPath("//div[2]/div/a/div/div[2]")).Click();

            context.log(" [인스타 루프] : " + where_to + "(으)로 이동");
        }





        public void like_loop(int follow_count, int like_count = 1000)
        {


            int i = 0;
            Thread.Sleep(rnd.Next(1000, 3000));

            if (follows_count < limit_follows)
            {
                //팔로우
                //If there is a follow button on first page, try to press
                if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")))
                {
                    try
                    {
                        //팔로우를 찾아서 있으면 진행 없으면 에러
                        Assert.AreEqual("팔로우",
                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                                .Text);
                        if (follow_time_gap(delay_follow))
                        {
                            string followed = "";
                            if (IsElementPresent(By.CssSelector("a._4zhc5")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("a._4zhc5")).Text;
                            }
                            else if (IsElementPresent(By.CssSelector("h1._i572c")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("h1._i572c")).Text;
                            }

                            driver.FindElement(
                            By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")).Click();
                            //Save Follow data
                            saveFollowData(followed);
                            //Set Follows Count for today
                            follows_count = follows_count + 1;
                            //clicked follow button
                            context.log(" [인스타 루프] : 팔로우 했습니다");
                        }


                        Thread.Sleep(rnd.Next(1000, 3000));
                    }

                    catch (Exception e) { /*context.log("팔로우를 못찾았습니다1");*/ }


                }
                //다른모양의 팔로우 
                else if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")))
                {
                    try
                    {
                        //팔로우를 찾아서 있으면 진행 없으면 에러
                        Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Text);

                        if (follow_time_gap(delay_follow))
                        {

                            string followed = "";
                            if (IsElementPresent(By.CssSelector("a._4zhc5")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("a._4zhc5")).Text;
                            }
                            else if (IsElementPresent(By.CssSelector("h1._i572c")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("h1._i572c")).Text;
                            }

                            driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Click();

                            //Save Follow data
                            saveFollowData(followed);
                            //Set Follows Count for today
                            follows_count = follows_count + 1;
                            context.log(" [인스타 루프] 팔로우 했습니다");
                        }

                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    catch (Exception e) { }
                }
            }
            else { context.log("Follow Limit reached"); }

            //finding first picture
            if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/div/div/div/a/div")))
            {

                IWebElement img_element = driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/div/div/div/a/div"));

                img_element.Click();



                DateTime currentTime = DateTime.Now;
                DateTime future = currentTime.AddMinutes(6);
                while (follow_count != 0)
                {
                    currentTime = DateTime.Now;

                    Thread.Sleep(rnd.Next(1000, 3000));


                    /*  try
                      {
                          //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                          //myDynamicElement = wait.Until(d => d.FindElement(By.LinkText("다음")));
                          //"다음"이 나올 때 까지 기다림
                          //driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(500));
                          while (!driver.ExecuteScript("return document.readyState").ToString().Equals("complete"))
                          {
                              //페이지가 다 로딩 될 때 까지 기다린다
                              //waiting the page ready
                          }

                      }
                      catch (Exception e)
                      {
                          //failed to load the page
                          context.log(" [인스타 루프] 페이지 로딩에 실패했습니다");

                          Thread.Sleep(rnd.Next(1000, 3000));

                          break;
                      }*/

                    if (likes_count < limit_likes)
                    {

                        //LIKE portion
                        if (IsElementPresent(By.CssSelector("span._soakw.coreSpriteHeartOpen")))
                        //"좋아요"가 클릭 돼 있지 않을 때
                        {
                            if (like_time_gap(delay_like))
                            {
                                driver.FindElement(By.CssSelector("span._soakw.coreSpriteHeartOpen")).Click();
                                //Set Likes Count for today
                                likes_count = likes_count + 1;
                            }


                            //"좋아요" 클릭! 
                            context.log(" [인스타 루프] : 좋아요를 눌렀습니다");
                            Thread.Sleep(rnd.Next(1000, 3000));
                        }


                    }
                    else { context.log("Like Limit reached"); }

                    if (follows_count < limit_follows)
                    {

                        if (follow_time_gap(delay_follow))
                        {
                            //follow delay
                            //팔로우
                            if (IsElementPresent(By.XPath("//header/span/button")))
                            {
                                try
                                {
                                    //팔로우를 찾아서 있으면 진행 없으면 에러
                                    Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//header/span/button")).Text);

                                    string followed = "";
                                    if (IsElementPresent(By.CssSelector("a._4zhc5")))
                                    {
                                        //get the name of user to be followed
                                        followed = driver.FindElement(By.CssSelector("a._4zhc5")).Text;
                                    }
                                    else if (IsElementPresent(By.CssSelector("h1._i572c")))
                                    {
                                        //get the name of user to be followed
                                        followed = driver.FindElement(By.CssSelector("h1._i572c")).Text;
                                    }

                                    driver.FindElement(By.XPath("//header/span/button")).Click();


                                    //Save Follow data
                                    saveFollowData(followed);
                                    //Set Follows Count for today
                                    follows_count = follows_count + 1;
                                    context.log(" [인스타 루프] : 팔로우 했습니다");

                                    follow_count--;

                                    Thread.Sleep(rnd.Next(1000, 3000));

                                    if (comments_count < limit_comments)
                                    {
                                        //팔로우하면 댓글 자동
                                        t = conn_manager.Select_comments();

                                        string comment = t.Rows[0]["comment"].ToString();

                                        //팔로우를 찾아서 있으면 진행 없으면 에러
                                        driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(comment);
                                        Thread.Sleep(rnd.Next(1000, 2000));
                                        driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(Keys.Enter);
                                        //update worknumber  of comment
                                        conn_manager.Update_comment_worknum(comment);
                                        //Set Comments Count for today
                                        comments_count = comments_count + 1;
                                    }
                                    Thread.Sleep(rnd.Next(1000, 3000));
                                }
                                catch (Exception e)
                                {

                                }
                                break;

                            }

                        }
                    }
                    else { context.log("Follow Limit reached"); }

                    //comment portion

                    if (comments_count < limit_comments)
                    {

                        if (IsElementPresent(By.CssSelector("input._7uiwk._qy55y")))
                        {

                            if (comment_time_gap(delay_comment))
                            {

                                t = conn_manager.Select_comments();

                                string comment = t.Rows[0]["comment"].ToString();

                                //팔로우를 찾아서 있으면 진행 없으면 에러
                                driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(comment);
                                Thread.Sleep(rnd.Next(1000, 2000));
                                driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(Keys.Enter);
                                //update worknumber  of comment
                                conn_manager.Update_comment_worknum(comment);
                                //Set Comments Count for today
                                comments_count = comments_count + 1;

                                context.log(" [인스타 루프] : 댓글을 입력했습니다");

                            }
                            Thread.Sleep(rnd.Next(1000, 3000));

                        }

                    }
                    else { context.log("comment Limit reached"); }

                    //If there is no next
                    if (!IsElementPresent(By.LinkText("다음"))) //"다음"이 없을 때
                    {

                        //다음이 없고 follow는 안했을 때 대기
                        if (!follow_time_gap(delay_follow))
                        {

                            try
                            {
                                //check already follow 
                                //If it is true, break
                                Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//button")).Text);
                                //다음이 없고 follow는 안했을 때 대기
                                //If it is no next and didn't follow yet, we wait for follow gap
                                if (!follow_time_gap(delay_follow))
                                {
                                    DateTime now = DateTime.Now;
                                    Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                                }

                                //팔로우 
                            }
                            catch (Exception ex)
                            {
                                //If there is no follow button, we already follow him
                                context.log(" [인스타 루프] : 이미 팔로우 했습니다");
                            }
                            break;

                        }


                    }
                    else
                    {
                        //If there is next button, click and next
                        //"다음"이 있을 때
                        driver.FindElement(By.LinkText("다음")).Click();


                        context.log(" [인스타 루프] : 다음 게시물로 넘어갑니다");
                    }

                }
                // END OF WHILE


                try
                {
                    // 닫기
                    // close
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._3eajp")));


                    driver.FindElement(By.CssSelector("button._3eajp")).Click();
                }
                catch (Exception e)
                {
                    //닫기가 없으면 그냥 패스~
                }


            }




            if (follows_count < limit_follows)
            {
                // we passed the follow button because of follow time gap above, So we have to wait and follow
                //랜덤 유저검색 후 팔로우
                if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")))
                {


                    try
                    {
                        //팔로우를 찾아서 있으면 진행 없으면 에러
                        Assert.AreEqual("팔로우",
                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                                .Text);
                        if (follow_time_gap(delay_follow))
                        {

                            string followed = "";
                            if (IsElementPresent(By.CssSelector("a._4zhc5")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("a._4zhc5")).Text;
                            }
                            else if (IsElementPresent(By.CssSelector("h1._i572c")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("h1._i572c")).Text;
                            }

                            //If follow time gap was done
                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                                .Click();

                            //Save Follow data
                            saveFollowData(followed);
                            //Set Follows Count for today
                            follows_count = follows_count + 1;

                            //follow
                            context.log(" [인스타 루프] : 팔로우 했습니다");
                        }
                        else
                        {
                            // otherwise we have to wait again

                            // caculate appropriate waiting time
                            //대기 타다가 팔로우
                            DateTime now = DateTime.Now;
                            //context.log(now.ToString() + "현시각");
                            //context.log(follow_time.ToString() + "팔로우한 시각");
                            //context.log(((int)(now.Subtract(follow_time).TotalSeconds) * 1000).ToString() + "여기에러인가요");
                            Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));

                            string followed = "";
                            if (IsElementPresent(By.CssSelector("a._4zhc5")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("a._4zhc5")).Text;
                            }
                            else if (IsElementPresent(By.CssSelector("h1._i572c")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("h1._i572c")).Text;
                            }

                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                                .Click();

                            //Save Follow data
                            saveFollowData(followed);
                            //Set Follows Count for today
                            follows_count = follows_count + 1;

                            context.log(" [인스타 루프] : 팔로우 했습니다");

                        }

                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    catch (Exception e)
                    {
                        //failed to follow after waiting
                        context.log(" [인스타 루프] : 팔로우에 실패했습니다");
                    }

                }
                //다른모양의 팔로우 
                // different figure of follow button
                else if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")))
                {

                    try
                    {
                        //팔로우를 찾아서 있으면 진행 없으면 에러
                        Assert.AreEqual("팔로우",
                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button"))
                                .Text);

                        if (follow_time_gap(delay_follow))
                        {

                            string followed = "";
                            if (IsElementPresent(By.CssSelector("a._4zhc5")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("a._4zhc5")).Text;
                            }
                            else if (IsElementPresent(By.CssSelector("h1._i572c")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("h1._i572c")).Text;
                            }

                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button"))
                                .Click();

                            //Save Follow data
                            saveFollowData(followed);
                            //Set Follows Count for today
                            follows_count = follows_count + 1;

                            context.log(" [인스타 루프] : 팔로우 했습니다");
                        }
                        else
                        {

                            //대기 타다가 팔로우
                            DateTime now = DateTime.Now;
                            Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));


                            string followed = "";
                            if (IsElementPresent(By.CssSelector("a._4zhc5")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("a._4zhc5")).Text;
                            }
                            else if (IsElementPresent(By.CssSelector("h1._i572c")))
                            {
                                //get the name of user to be followed
                                followed = driver.FindElement(By.CssSelector("h1._i572c")).Text;
                            }

                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button"))
                                .Click();

                            //Save Follow data
                            saveFollowData(followed);
                            //Set Follows Count for today
                            follows_count = follows_count + 1;

                            context.log(" [인스타 루프] : 팔로우 했습니다");
                        }
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    catch (Exception e)
                    {
                        //failed to follow after waiting
                        context.log(" [인스타 루프] : 팔로우에 실패했습니다");
                    }
                }

                follow_time = DateTime.Now;
            }
            else { context.log("Follow Limit reached"); }
        }

        //UNFOLLOW PROCEDURE
        public void unfollow()
        {

            bool unfollow_flag = false;
            bool alreday_exist = false;
            string web_follower_count;
            string follower_username;
            string following_username;
            List<string> followed_list = new List<string>();
            List<string> not_unfollow_list = new List<string>();
            IWebElement Box;
            int DivHeight;
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            int scroll;
            int scroll_up_speed;


            try
            {
                if (IsElementPresent(By.LinkText("프로필")))
                {
                    Thread.Sleep(rnd.Next(1000, 3000));
                    //Click on profile
                    driver.FindElement(By.LinkText("프로필")).Click();

                    Thread.Sleep(rnd.Next(1000, 3000));
                    try
                    {

                        web_follower_count = driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/ul/li[2]/a/span")).Text;
                        //span[@id='react-root']/section/main/article/header/div[2]/ul/li[2]/a/span         

                        Thread.Sleep(rnd.Next(1000, 3000));
                        int web_followers = Int32.Parse(web_follower_count, NumberStyles.Integer | NumberStyles.AllowThousands, new CultureInfo("en-US"));

                        //get  followers from db
                        DataTable tbl = conn_manager.select_follows(current_user);

                        if (tbl.Rows.Count > 0)
                        {

                            //get all followed users of current user
                            DataRow row = conn_manager.select_followers_count(current_user);

                            int db_followers = Int32.Parse(row["followers"].ToString());
                            // context.log("GDGDGDGGDGDGDG " + row["followers"].ToString());
                            if (db_followers > 0)
                            {

                                //subtract db followers from website followers and add 15 to result
                                int followers_count_new = web_followers - db_followers;
                                // context.log("Check_COUNT: " + followers_count_new.ToString());
                                // followers_count_new = followers_count_new + 25;
                                if (web_followers > 0)
                                {
                                    followers_count_new = followers_count_new + 40;
                                    scroll_up_speed = 15;
                                }
                                else
                                {
                                    followers_count_new = 30;
                                    scroll_up_speed = 5;
                                }
                                //context.log("WEB_COUNT: " + followers_count.ToString());

                                context.log(" [언팔로우] : " + followers_count_new + " 개의 팔로우 계정을 검사합니다");
                                // context.log(" POSSIBILITY_COUNT: " + followers_count_new);
                                //Initialize scroll
                                scroll = 5;
                                Thread.Sleep(rnd.Next(1000, 3000));
                                //Click on followers to get all list
                                driver.FindElement(By.XPath("//li[2]/a")).Click();
                                //GET the BOX height
                                Box = driver.FindElement(By.ClassName("_4gt3b"));
                                DivHeight = Box.Size.Height;
                                //Add Delay
                                Thread.Sleep(rnd.Next(1000, 3000));

                                //Duplicate db followed data to followed list
                                foreach (DataRow r in tbl.Rows)
                                {
                                    followed_list.Add(r["followed_id"].ToString());
                                }

                                context.log(" [언팔로우] : Searching in Followers list.............. ");
                                for (int i = 1; i <= followers_count_new; i++)
                                {

                                    try
                                    {
                                        //Set the intial scroll Delay

                                        if (i == scroll)
                                        {

                                            // Scroll inside web element vertically (e.g. 100 pixel)

                                            DivHeight = DivHeight + scroll * 10;
                                            js.ExecuteScript("arguments[0].scrollTop = arguments[1];",
                                            driver.FindElement(By.ClassName("_4gt3b")), DivHeight);
                                            Thread.Sleep(rnd.Next(1000, 2000));
                                            scroll = scroll + 3;

                                        }

                                        //get the user from website follower list
                                        follower_username = driver.FindElement(By.XPath("//li[" + i + "]/div/div/div/div/a")).Text;
                                        //For each  followed users match with website's followers list
                                        // context.log(" [언팔로우] : " + i);

                                        foreach (DataRow r in tbl.Rows)
                                        {


                                            //get the user followed by me and time as well
                                            string followedby_me = r["followed_id"].ToString();
                                            DateTime time_whenfollowed = Convert.ToDateTime(r["time"].ToString());
                                            DateTime now = DateTime.Now;
                                            double duration = now.Subtract(time_whenfollowed).TotalHours;
                                            //   context.log(" Duration =" + now.Subtract(time_whenfollowed).TotalHours.ToString() + " \n");                                                               
                                            if (duration > delay_unfollow)
                                            {
                                                //Check if user is already in the not_unfollow list or not[because he may be already following me]
                                                if (not_unfollow_list.Count > 0)
                                                {
                                                    foreach (string not_unfollow_user in not_unfollow_list)
                                                    {
                                                        if (followedby_me != not_unfollow_user)
                                                        {
                                                            alreday_exist = false;

                                                        }
                                                        else { alreday_exist = true; break; }
                                                    }
                                                }
                                                //If not in the list then add other wise no need to add
                                                if (alreday_exist == false)
                                                {

                                                    //if user is not in the list
                                                    if (followedby_me == follower_username)
                                                    {

                                                        //Remove from followed list
                                                        followed_list.Remove(followedby_me);
                                                        //Add to not unfollow list
                                                        not_unfollow_list.Add(followedby_me);

                                                        // context.log(" MATCHED AND FOLLOWING ME");
                                                        context.log("[언팔로우] : 맞팔을 확인했습니다");
                                                        Thread.Sleep(rnd.Next(1000, 2000));
                                                    }

                                                }//End of already exist
                                            }
                                            else
                                            {
                                                //Check if user is already in the not_unfollow list or not
                                                if (not_unfollow_list.Count > 0)
                                                {
                                                    foreach (string not_unfollow_user in not_unfollow_list)
                                                    {
                                                        if (followedby_me != not_unfollow_user)
                                                        {
                                                            alreday_exist = false;


                                                        }
                                                        else { alreday_exist = true; break; }
                                                    }

                                                }
                                                //If not inb the list then add other wise no need to add
                                                if (alreday_exist == false)
                                                {
                                                    //Remove from followed list
                                                    followed_list.Remove(followedby_me);
                                                    //Add to not unfollow list
                                                    not_unfollow_list.Add(followedby_me);

                                                }
                                            }
                                        } //end of for each loop
                                    }
                                    catch
                                    {
                                        context.log("[언팔로우] : 가능한 팔로워 인덱스를 넘었습니다 " + i);
                                        break;
                                        //context.log("Wrong Followers Index: " + i); break;
                                    }


                                }//End of for

                                ////////////////////RESET PART BEGINS////////////////////////////////////////////////



                                //Reset scroll to Top of list

                                for (int j = followers_count_new; j >= 0; j -= scroll_up_speed)
                                {

                                    // Scroll inside web element vertically (e.g. 100 pixel)
                                    js.ExecuteScript("arguments[0].scrollTop = arguments[1];",
                                        driver.FindElement(By.ClassName("_4gt3b")), j);
                                    Thread.Sleep(rnd.Next(1000, 1000));

                                }

                                // 닫기
                                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                                myDynamicElement =
                                    wait.Until(d => d.FindElement(By.CssSelector("button._3eajp")));

                                //Close followers page
                                driver.FindElement(By.CssSelector("button._3eajp")).Click();

                                //Just Sleep
                                Thread.Sleep(rnd.Next(1000, 3000));

                                //Print  NOT UNFOLLOW LIST
                                context.log("########### TO BE UNFOLLOW LIST: ###### ");
                                foreach (string followed_by_me in followed_list)
                                {

                                    context.log(followed_by_me);
                                    Thread.Sleep(1000);
                                }
                                context.log("##################################### ");

                                //Reset the scroll delay to initial value
                                scroll = 5;

                                /////////////////////////////RESET PART END ////////////////////////////////////////

                                //context.log("Opening following.....");
                                Thread.Sleep(rnd.Next(1000, 3000));
                                //Click on following to get all list
                                driver.FindElement(By.XPath("//li[3]/a")).Click();
                                //Find the Height
                                Box = driver.FindElement(By.ClassName("_4gt3b"));
                                DivHeight = Box.Size.Height;
                                // context.log("Opened");
                                context.log("[언팔로우] : 팔로잉 리스트를 엽니다");

                                context.log("Unfollow is Runing............");

                                //  if (not_unfollow_list.Count != tbl.Rows.Count) //donot unfollow anyone
                                if (followed_list.Count > 0)
                                {
                                    for (int i = 1; i <= followers_count_new; i++)
                                    {

                                        try
                                        {
                                            //get the  user from website following list
                                            Thread.Sleep(rnd.Next(1000, 3000));
                                            following_username = driver.FindElement(By.XPath("//li[" + i + "]/div/div/div/div/a")).Text;
                                            //  context.log("Following "+ i + following_username);

                                            if (i == scroll)
                                            {
                                                // Scroll inside web element vertically (e.g. 100 pixel)
                                                context.log("Scroll: " + scroll.ToString());
                                                DivHeight = DivHeight + scroll * 10;
                                                js.ExecuteScript("arguments[0].scrollTop = arguments[1];",
                                                driver.FindElement(By.ClassName("_4gt3b")), DivHeight);
                                                Thread.Sleep(rnd.Next(1000, 2000));
                                                scroll = scroll + 3;
                                            }
                                            // context.log("ROW COUNT DURING UNFOLLOW : " + followed_list.Count.ToString());

                                            // if (tbl.Rows.Count > 0)
                                            if (followed_list.Count > 0)
                                            {

                                                //For each  followed users match with website's followers list

                                                foreach (string followedby_me in followed_list)
                                                {

                                                    //get the user followed by me 
                                                    //if he or she is our following list
                                                    if (followedby_me == following_username)
                                                    {
                                                        try
                                                        {
                                                            Thread.Sleep(rnd.Next(2000, 3000));
                                                            //then unfollow him or her 
                                                            driver.FindElement(By.XPath("//li[" + i + "]/div/div/span/button")).Click();
                                                            //Remove from followed list
                                                            followed_list.Remove(followedby_me);
                                                            //Delete from db
                                                            conn_manager.remove_followdata(current_user, followedby_me);

                                                            //context.log("<<<<<<<<<<<<<<< UNFOLLOWED>>>>>>>>>>>>>>>");
                                                            context.log("[언팔로우] : 언팔로우 했습니다");
                                                            Thread.Sleep(rnd.Next(2000, 3000));
                                                            break;
                                                        }
                                                        catch
                                                        {
                                                            //context.log("Not able to find Following button");
                                                            context.log("[언팔로우] : 팔로인 버튼을 찾지 못했습니다");
                                                        }
                                                    }
                                                    else
                                                    {

                                                        if (i == followers_count_new) //if we are at last and user is not matched in the following list
                                                        {
                                                            context.log("[언팔로우] : " + followedby_me + " NOT MATCHED IN THE LIST and DELETED ");
                                                            //Remove from followed list
                                                            followed_list.Remove(followedby_me);
                                                            //Delete from db
                                                            conn_manager.remove_followdata(current_user, followedby_me);

                                                        }

                                                    } //End of If

                                                    // } //End of if Unfollow
                                                } //End of For each loop
                                            }
                                            else { break; }//End of if-else check count
                                        }
                                        catch
                                        {
                                            //context.log("Wrong Following Index: " + i);
                                            context.log("[언팔로우] : 가능한 인덱스를 넘었습니다 : " + i);
                                            Thread.Sleep(rnd.Next(1000, 1000));
                                            break;
                                        }


                                    }//End of for loop

                                    // failed to unfollow and scroll up
                                    for (int j = followers_count_new; j >= 0; j -= scroll_up_speed)
                                    {
                                        // Scroll inside web element vertically (e.g. 100 pixel)
                                        js.ExecuteScript("arguments[0].scrollTop = arguments[1];", driver.FindElement(By.ClassName("_4gt3b")), j);
                                        Thread.Sleep(rnd.Next(1000, 1000));
                                        ///////////////////// Unfollow if flag is true [its not found in the follower's list]   /////////////////////////
                                    }
                                    //Wait
                                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                                    myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._3eajp")));
                                    //Close following page
                                    driver.FindElement(By.CssSelector("button._3eajp")).Click();
                                }
                                else { context.log("No One to unfollow"); }
                            }
                            else
                            {
                                // context.log("Will not Unfollow as Followers count is not available in DB");
                                context.log("[언팔로우] : 그동안 아무도 팔로우 하지 않았습니다");

                            }
                        }
                        else
                        {

                            //  context.log("Not following Anyone");
                            context.log("[언팔로우] : 팔로우가 0 입니다");
                        } //End of if-else

                    }
                    catch
                    {
                        //  context.log("Not able to access Follwers Count from website");
                        context.log("[언팔로우] : 팔로워가 0 입니다");
                    }
                } //end of if(IsElementPresent)
            }
            catch (Exception)
            {
                //context.log("Profile not in Korean");
                context.log("[언팔로우] : 실패했습니다");
            }


        } //End of Unfollow Procedure



        //STORE FOLLOWERS PROCEDURE
        public void store_followers()
        {
            //Change Language to Korean If Profile is in English
            changeLanguageOnProfile();
            //Just Sleep
            Thread.Sleep(rnd.Next(1000, 3000));

            try
            {

                if (IsElementPresent(By.LinkText("프로필")))
                {

                    driver.FindElement(By.LinkText("프로필")).Click();
                    Thread.Sleep(rnd.Next(1000, 3000));
                    try
                    {//li[2]/span/span
                     // store count of followers
                        string followers_num = "0";

                        if (IsElementPresent(By.XPath("//li[2]/span/span")))
                        {
                            followers_num = driver.FindElement(By.XPath("//li[2]/a/span")).Text;
                        }
                        else if (IsElementPresent(By.XPath("//li[2]/a/span")))
                        {

                            followers_num = driver.FindElement(By.XPath("//li[2]/a/span")).Text;
                        }


                        Thread.Sleep(rnd.Next(1000, 3000));
                        int followers_count = Int32.Parse(followers_num, NumberStyles.Integer | NumberStyles.AllowThousands, new CultureInfo("en-US"));

                        string created_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        //update the followers count in insta_follows
                        conn_manager.Update_followers(current_user, followers_count);

                        //inser the followers count in insta_status
                        conn_manager.insert_followersCount(current_user, followers_count, created_at);
                        //Just Sleep
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    catch
                    {
                        context.log("[언팔로우] : 팔로워를 찾지 못했습니다");
                        //context.log("Followers Word not Found!!");
                    }

                }
            }
            catch (Exception)
            {
                context.log("[언팔로우] : 프로필을 찾지 못했습니다");
                //context.log("Profile not found!!");
            }



        }


        public void logout()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            myDynamicElement = wait.Until(d => d.FindElement(By.LinkText("프로필")));

            driver.FindElement(By.LinkText("프로필")).Click();

            Thread.Sleep(rnd.Next(1000, 3000));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            myDynamicElement = wait.Until(d => d.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/div/button")));

            driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/div/button")).Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._4y3e3")));

            driver.FindElement(By.CssSelector("button._4y3e3")).Click();
            //context.log("로그아웃 성공");
        }

        public void quit()
        {
            //driver.Quit();

            TeardownTest();
        }





        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                Thread.Sleep(3000);
                try
                {
                    driver.Quit();
                }
                catch (Exception)
                {

                }

            }

        }

        protected bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }




    }
}
