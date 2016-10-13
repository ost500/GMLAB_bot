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


namespace WindowsFormsApplication1
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


        DataTable t;
        DataRow r;

        //follow 시간 like 시간
        public static DateTime follow_time;
        public static DateTime like_time;
        public static DateTime comment_time;
        public static string save_follow_time;


        public static int delay_like;
        public static int delay_comment;
        public static int delay_follow;
        public static int delay_unfollow;
        public static string current_user;

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
            DirectoryInfo directoryInfo = Directory.GetParent(Directory.GetParent(currentDirName).ToString());
            string path = directoryInfo.ToString();
            return path;
        }


        //CHECK STATUS OF JOB FOR A USER 
        public bool checkJobStatus(string user_id)
        {


            DataRow row = conn_manager.Select_job(user_id);
            if (row == null)
            {
                log("YOU NEED TO ENTER DELAY FOR LIKE , FOLLOW, COMMENT AND UNFOLLOW FOR " + user_id);
                log("PLEASE LOG IN HERE: http://easygram.kr/ & ADD DELAYS");

                return false;
            }
            else
            {
                return true;
            }
        }


        //GET delay_like,delay_comment,delay_follow and delay_unfollow from database for current user
        public int getLikeDelay(string user_id)
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

        public int getCommentDealy(string user_id)
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

        public int getFollowDelay(string user_id)
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

        public int getUnfollowDelay(string user_id)
        {
            DataRow jobrow = conn_manager.Select_job(user_id);
            if (jobrow == null)
            {
                return 3;
            }
            else
            {
                delay_unfollow = Int32.Parse(jobrow["delay_unfollow"].ToString());

                return delay_unfollow;
            }
        }


        //SET TIME DELAY TO LIKE PICTURES
        public bool like_time_gap(int delay_like)
        {
            //log("LIKE DELAY:"+like.ToString());
            DateTime now = DateTime.Now;
            log(now.ToString());
            log(like_time.ToString());
            log(now.Subtract(follow_time).TotalSeconds.ToString() + "Like_time_gap");
            if (now.Subtract(like_time).TotalSeconds > delay_like)
            {
                like_time = DateTime.Now;
                return true;
            }
            else
            {
                //Thread.Sleep(11000);
                //like_time = DateTime.Now;
                log("no like");
                return false;
            }
        }

        //SET TIME DELAY TO FOLLOW OTHERS
        public bool follow_time_gap(int delay_follow)
        {
            DateTime now = DateTime.Now;
            //  log("FOLLOW DELAY:" + follow.ToString());
            log("-----------------");
            log(now.ToString());
            log(follow_time.ToString());
            log(now.Subtract(follow_time).TotalMinutes.ToString() + "follow_time_gap");

            if (now.Subtract(follow_time).TotalMinutes > delay_follow)
            {
                follow_time = DateTime.Now;
                return true;
            }
            else
            {
                //Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                log("no follow");
                return false;
            }
        }

        //SET TIME DELAY TO COMMENT ON PICTURES
        public bool comment_time_gap(int delay_comment)
        {
            DateTime now = DateTime.Now;
            //  log("COMMENT DELAY:" + comment.ToString());
            log(now.ToString());
            log(comment_time.ToString());

            log(now.Subtract(comment_time).TotalSeconds.ToString() + "comment_time_gap");

            if (now.Subtract(comment_time).TotalSeconds > delay_comment)

            {
                comment_time = DateTime.Now;
                return true;
            }
            else
            {
                log("no work");
                //Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                return false;
            }
        }


        public void saveFollowData()
        {
            // Check for exact CSS Selector and save Follow data

            if (IsElementPresent(By.CssSelector("a._4zhc5")))
            {

                //find the above followed user
                string followed = driver.FindElement(By.CssSelector("a._4zhc5")).Text;

                //set time
                save_follow_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //insert followed id into database
                conn_manager.insert_followdata(current_user, followed, save_follow_time);
            }
            else if (IsElementPresent(By.CssSelector("h1._i572c")))
            {
                //find the above followed user
                string followed = driver.FindElement(By.CssSelector("h1._i572c")).Text;

                //set time
                save_follow_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //insert followed id into database
                conn_manager.insert_followdata(current_user, followed, save_follow_time);
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
                log("Language Changed after clicking on Profile");

            }
            catch (Exception exc)
            {
                log("Profile is already in korean");
            }

        }



        public void log(string logging)
        {
            context.textBox1.AppendText(Environment.NewLine);
            context.textBox1.AppendText("[" + DateTime.Now.ToString() + "]" + logging);

        }

        public void start()
        {



            t = conn_manager.SelectData();
            if (t == null) { throw new NullReferenceException(); }

            r = t.Rows[0];
            log("row[0]" + t.Rows[0]["user_id"]);

            //work_number 1 더하기
            conn_manager.Update_worknum();


            context.listBox1.Items.Clear();

            foreach (DataRow r in t.Rows)
            {
                context.listBox1.Items.Add(r["user_id"]);
            }


            //String path = "D:\\chrome_cache\\" + r["user_id"].ToString();

            current_user = r["user_id"].ToString();

            log("  USER:" + current_user + " \n");

            delay_like = getLikeDelay(current_user);
            delay_comment = getCommentDealy(current_user);
            delay_follow = getFollowDelay(current_user);
            delay_unfollow = getUnfollowDelay(current_user);


            //Get current directory path and set chrome cache path
            string currentDir = getCurrentPath();
            string path = currentDir + "\\chrome_cache\\" + current_user.Trim();

            ChromeOptions co = new ChromeOptions();

            co.AddArguments("user-data-dir=" + path);

            var driverService = ChromeDriverService.CreateDefaultService("C:\\Program Files (x86)\\Google\\Chrome\\Application");
            driverService.HideCommandPromptWindow = true;
            //driverService.Port = my_port;
            //co.BinaryLocation = "C:\\Program Files (x86)\\Google\\Chrome\\Application";


            driver = new ChromeDriver(driverService, co);
            //driver = new ChromeDriver("C:\\Program Files (x86)\\Google\\Chrome\\Application");



            verificationErrors = new StringBuilder();

            log("시작했습니다");

            driver.Navigate().GoToUrl(baseURL + "/");
            log("메인으로 갔습니다");

        }

        public void language_check()
        {

            try
            {


                //check for English Log in
                Assert.AreEqual("Log in", driver.FindElement(By.CssSelector("a._fcn8k")).Text);

                //if matched then change language to Korean otherwise its already korean
                new SelectElement(driver.FindElement(By.CssSelector("select._nif11"))).SelectByText("Korean");
                log("Language Changed");
            }
            catch (Exception e)
            {
                context.log("It's already Korean");
            }


        }


        public bool block_check()
        {

            try
            {

                //Check job status ..IF Ok then Proceed Otherwise Stop

                if (!checkJobStatus(current_user)) {  return true; }

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
                    log("에러가 났습니다");
                }
                catch (Exception e)
                {

                    //Change Language to Korean If Profile is in English
                    changeLanguageOnProfile();

                    //안나오면 정상 가동
                    log("계정 인증이 없다");
                    return false;
                }

                log("계정 인증 있다");
                //DB 기록
                conn_manager.blocked_update();
                Thread.Sleep(rnd.Next(15000, 20000));//to verify
                return true;
            }

            log("로그인 버튼이 있다");
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
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            for (int i = 0; i <= 150; i += 10)
            {
                js.ExecuteScript("window.scrollTo(" + i + ", " + (i + 10) + ");");
                Thread.Sleep(rnd.Next(100, 300));
            }
            for (int i = 150; i <= 0; i -= 10)
            {
                js.ExecuteScript("window.scrollTo(" + i + ", " + (i - 10) + ");");
                Thread.Sleep(rnd.Next(100, 300));
            }


            Thread.Sleep(rnd.Next(1000, 3000));

            driver.FindElement(By.CssSelector("input._9x5sw._qy55y")).Clear();
            Thread.Sleep(rnd.Next(1000, 3000));
            driver.FindElement(By.CssSelector("input._9x5sw._qy55y")).SendKeys(where_to);
            Thread.Sleep(rnd.Next(1000, 3000));
            driver.FindElement(By.XPath("//div[2]/div/a/div/div[2]")).Click();

            log(where_to + "(으)로 이동");
        }





        public void like_loop(int follow_count, int like_count = 1000)
        {


            int i = 0;
            Thread.Sleep(rnd.Next(1000, 3000));


            //팔로우
            if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")))
            {
                log("111팔로우");
                try
                {
                    //팔로우를 찾아서 있으면 진행 없으면 에러
                    Assert.AreEqual("팔로우",
                        driver.FindElement(
                                By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                            .Text);
                    if (follow_time_gap(delay_follow))
                    {

                        driver.FindElement(
                                By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                            .Click();
                        //Save Follow data
                        saveFollowData();

                        log("팔로우 했습니다1");
                    }


                    Thread.Sleep(rnd.Next(1000, 3000));
                }
                catch (Exception e) { log("팔로우를 못찾았습니다1"); }

            }
            //다른모양의 팔로우 
            else if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")))
            {
                log("222팔로우");
                try
                {
                    //팔로우를 찾아서 있으면 진행 없으면 에러
                    Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Text);

                    if (follow_time_gap(delay_follow))
                    {

                        driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Click();

                        //Save Follow data
                        saveFollowData();

                        log("팔로우 했습니다2");
                    }

                    Thread.Sleep(rnd.Next(1000, 3000));
                }
                catch (Exception e) { }
            }



            if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/div/div/div/a/div")))
            {
               
                IWebElement img_element = driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/div/div/div/a/div"));

                img_element.Click();

                int k = 0;

                DateTime currentTime = DateTime.Now;
                DateTime future = currentTime.AddMinutes(6);
                while (follow_count != 0)
                {
                    currentTime = DateTime.Now;

                    Thread.Sleep(rnd.Next(1000, 3000));


                    try
                    {
                        //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                        //myDynamicElement = wait.Until(d => d.FindElement(By.LinkText("다음")));
                        //"다음"이 나올 때 까지 기다림
                        //driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(500));
                        while (!driver.ExecuteScript("return document.readyState").ToString().Equals("complete"))
                        {
                            //페이지가 다 로딩 될 때 까지 기다린다
                        }

                    }
                    catch (Exception e)
                    {
                        log("errorrrrrrrrrr!!!");
                        Thread.Sleep(rnd.Next(1000, 3000));

                        break;
                    }



                    //if (follow_time_gap(delay_follow))
                    //{
                    //팔로우
                    if (IsElementPresent(By.XPath("//header/span/button")))
                    {
                        try
                        {
                            //팔로우를 찾아서 있으면 진행 없으면 에러
                            Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//header/span/button")).Text);
                            driver.FindElement(By.XPath("//header/span/button")).Click();


                            //Save Follow data
                            saveFollowData();


                            follow_count--;



                            ////팔로우하면 댓글 자동
                            //t = conn_manager.Select_comments();

                            //string comment = t.Rows[0]["comment"].ToString();

                            ////팔로우를 찾아서 있으면 진행 없으면 에러
                            //driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(comment);
                            //driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(Keys.Enter);
                            ////update worknumber  of comment
                            //conn_manager.Update_comment_worknum(comment);


                            Thread.Sleep(rnd.Next(1000, 3000));
                        }
                        catch (Exception e)
                        {
                            break;
                        }
                        log("팔로우 했습니다");
                    }

                    //}


                    //if (IsElementPresent(By.CssSelector("span._soakw.coreSpriteHeartOpen")))
                    ////"좋아요"가 클릭 돼 있지 않을 때
                    //{
                    //    if (like_time_gap(delay_like))
                    //    {
                    //        driver.FindElement(By.CssSelector("span._soakw.coreSpriteHeartOpen")).Click();
                    //    }


                    //    //"좋아요" 클릭! 
                    //    //log("좋아요를 눌렀습니다");
                    //    Thread.Sleep(rnd.Next(1000, 3000));
                    //}
                    //else
                    ////"좋아요"가 클릭 돼 있을 때
                    //{

                    //}

                    ////comment portion
                    //if (IsElementPresent(By.CssSelector("input._7uiwk._qy55y")))
                    //{
                    //    if (comment_time_gap(delay_comment))
                    //    {

                    //        t = conn_manager.Select_comments();

                    //        string comment = t.Rows[0]["comment"].ToString();

                    //        //팔로우를 찾아서 있으면 진행 없으면 에러
                    //        driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(comment);
                    //        driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(Keys.Enter);
                    //        //update worknumber  of comment
                    //        conn_manager.Update_comment_worknum(comment);

                    //    }
                    //    Thread.Sleep(rnd.Next(1000, 3000));
                    //    Thread.Sleep(rnd.Next(1000, 3000));
                    //}


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
                                if (!follow_time_gap(delay_follow))
                                {
                                    DateTime now = DateTime.Now;
                                    Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                                }

                                //팔로우 
                            }
                            catch (Exception ex)
                            {
                                log("이미 팔로우 했습니다");
                            }
                            break;

                        }


                    }
                    else
                    {

                        //"다음"이 있을 때
                        driver.FindElement(By.LinkText("다음")).Click();


                        log("다음 게시물로 넘어갑니다");
                    }

                }
                try
                {
                    // 닫기
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._3eajp")));


                    driver.FindElement(By.CssSelector("button._3eajp")).Click();
                }
                catch (Exception e)
                {
                    //닫기가 없으면 그냥 패스~
                }


            }




            log("기다렸다가 팔로우를 할 시간이에요");

            //랜덤 유저검색 후 팔로우
            if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")))
            {

                log("333팔로우");
                try
                {
                    //팔로우를 찾아서 있으면 진행 없으면 에러
                    Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")).Text);
                    if (follow_time_gap(delay_follow))
                    {
                        driver.FindElement(
                                By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                            .Click();

                        //Save Follow data
                        saveFollowData();



                        log("팔로우 했습니다3 --이곳입니다");
                        log(follow_time.ToString() + "팔로우한 시각3");
                    }
                    else
                    {


                        //대기 타다가 팔로우
                        DateTime now = DateTime.Now;
                        log(now.ToString() + "현시각");
                        log(follow_time.ToString() + "팔로우한 시각");
                        log(((int)(now.Subtract(follow_time).TotalSeconds) * 1000).ToString() + "여기에러인가요");
                        Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                        driver.FindElement(
                                By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                            .Click();

                        //Save Follow data
                        saveFollowData();

                        log("기다렸다가 팔로우 했습니다3 --이곳입니다");

                    }

                    Thread.Sleep(rnd.Next(1000, 3000));
                }
                catch (Exception e) { log(e.StackTrace); }

            }
            //다른모양의 팔로우 
            else if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")))
            {
                log("444팔로우");
                try
                {
                    //팔로우를 찾아서 있으면 진행 없으면 에러
                    Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Text);

                    if (follow_time_gap(delay_follow))
                    {
                        driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Click();

                        //Save Follow data
                        saveFollowData();

                        log("팔로우 했습니다4 --이곳입니다");
                    }
                    else
                    {

                        //대기 타다가 팔로우
                        DateTime now = DateTime.Now;
                        Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                        driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Click();

                        //Save Follow data
                        saveFollowData();

                        log("기다렸다가 팔로우 했습니다4 --이곳입니다");
                    }
                    Thread.Sleep(rnd.Next(1000, 3000));
                }
                catch (Exception e) { }
            }

            follow_time = DateTime.Now;


        }

        //UNFOLLOW PROCEDURE
        public void unfollow()
        {

            bool unfollow_flag = false;
            string web_followers;
            string follower_username;
            string following_username;
            IWebElement Box;
            int DivHeight;
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            int scroll;

            try
            {
                if (IsElementPresent(By.LinkText("프로필")))
                {
                    //Click on profile
                    driver.FindElement(By.LinkText("프로필")).Click();

                    Thread.Sleep(rnd.Next(1000, 3000));
                    try
                    {

                        web_followers = driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/ul/li[2]/a/span")).Text;
                        //span[@id='react-root']/section/main/article/header/div[2]/ul/li[2]/a/span         

                        Thread.Sleep(rnd.Next(1000, 3000));
                        int followers_count = Int32.Parse(web_followers, NumberStyles.Integer | NumberStyles.AllowThousands, new CultureInfo("en-US"));

                        //get  followers from db
                        DataRow row = conn_manager.select_followers_count(current_user);

                        if (row != null)
                        {

                            //subtract db followers from website followers and add 15 to result
                            int followers_count_new = followers_count - Int32.Parse(row["followers"].ToString());
                            log("Check_COUNT: " + followers_count_new.ToString());
                            // followers_count_new = followers_count_new + 25;
                            if (followers_count > 0) { followers_count_new = followers_count_new + 25; } else { followers_count_new = 25; }

                            log("WEB_COUNT: " + followers_count.ToString());

                            log(" POSSIBILITY_COUNT: " + followers_count_new);

                            //get all followed users of current user
                            t = conn_manager.select_follows(current_user);


                            if (t != null)
                            {
                                //For each  followed users match with website's followers list
                                foreach (DataRow r in t.Rows)
                                {

                                    //Set the intial scroll Delay
                                    scroll = 5;


                                    //get the user followed by me and time as well
                                    string followedby_me = r["followed_id"].ToString();
                                    log("User Followed by Me: " + followedby_me + " \n");

                                    DateTime time_whenfollowed = (DateTime)r["time"];
                                    DateTime now = DateTime.Now;
                                    double duration = now.Subtract(time_whenfollowed).TotalHours;

                                    log(" Duration =" + now.Subtract(time_whenfollowed).TotalHours.ToString() + " \n");

                                    //Click on followers to get all list
                                    driver.FindElement(By.XPath("//li[2]/a")).Click();
                                    //GET the BOX height
                                    Box = driver.FindElement(By.ClassName("_4gt3b"));
                                    DivHeight = Box.Size.Height;
                                    //Add Delay
                                    Thread.Sleep(rnd.Next(1000, 3000));

                                    if (duration > 72)
                                    {
                                        for (int i = 1; i < followers_count_new; i++)
                                        {
                                            try
                                            {


                                                //get the user from website follower list
                                                follower_username = driver.FindElement(By.XPath("//li[" + i + "]/div/div/div/div/a")).Text;


                                                if (i == scroll)
                                                {

                                                    // Scroll inside web element vertically (e.g. 100 pixel)

                                                    DivHeight = DivHeight + scroll * 10;
                                                    js.ExecuteScript("arguments[0].scrollTop = arguments[1];", driver.FindElement(By.ClassName("_4gt3b")), DivHeight);
                                                    Thread.Sleep(rnd.Next(1000, 2000));
                                                    scroll = scroll + 3;
                                                    log("Scroll" + scroll);

                                                }

                                                //if user is not in the list
                                                if (followedby_me != follower_username)
                                                {

                                                    unfollow_flag = true;
                                                }
                                                else
                                                {
                                                    unfollow_flag = false;
                                                    log(" MATCHED AND FOLLOWING ME"); Thread.Sleep(rnd.Next(1000, 3000));
                                                    break;
                                                }//End of if-else
                                            }
                                            catch { log("Wrong Followers Index: " + i); break; }
                                        }//end of for loop



                                    }
                                    else { log("Duration is not greater than 72 hours. Check next followed_id"); }

                                    ////////////////////RESET PART BEGINS////////////////////////////////////////////////

                                    //Reset the scroll delay to initial value
                                    scroll = 5;

                                    //Reset scroll to Top of list

                                    for (int j = followers_count_new; j >= 0; j -= 20)
                                    {

                                        // Scroll inside web element vertically (e.g. 100 pixel)
                                        js.ExecuteScript("arguments[0].scrollTop = arguments[1];", driver.FindElement(By.ClassName("_4gt3b")), j);
                                        Thread.Sleep(rnd.Next(1000, 1000));

                                    }
                                    /////////////////////////////RESET PART END ////////////////////////////////////////


                                    /////// Unfollow if flag is true [its not found in the follower's list] ////////////
                                    if (unfollow_flag == true)
                                    {

                                        try
                                        {

                                            // 닫기
                                            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                                            myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._3eajp")));

                                            //Close followers page
                                            driver.FindElement(By.CssSelector("button._3eajp")).Click();
                                            log("Opening following.....");
                                            //Just Sleep
                                            Thread.Sleep(rnd.Next(1000, 3000));

                                            //Click on following to get all list
                                            driver.FindElement(By.XPath("//li[3]/a")).Click();
                                            //Find the Height
                                            Box = driver.FindElement(By.ClassName("_4gt3b"));
                                            DivHeight = Box.Size.Height;
                                            log("Opened");
                                            Thread.Sleep(rnd.Next(1000, 3000));

                                            for (int i = 1; i < followers_count_new; i++)
                                            {
                                                try
                                                {

                                                    //get the  user from website following list
                                                    following_username = driver.FindElement(By.XPath("//li[" + i + "]/div/div/div/div/a")).Text;

                                                    if (i == scroll)
                                                    {
                                                        // Scroll inside web element vertically (e.g. 100 pixel)

                                                        DivHeight = DivHeight + scroll * 10;
                                                        js.ExecuteScript("arguments[0].scrollTop = arguments[1];", driver.FindElement(By.ClassName("_4gt3b")), DivHeight);
                                                        Thread.Sleep(rnd.Next(1000, 2000));
                                                        scroll = scroll + 3;
                                                        log("scroll:" + scroll.ToString());
                                                    }

                                                    //if he or she is our following list
                                                    if (followedby_me == following_username)
                                                    {
                                                        try
                                                        {

                                                            //then unfollow him or her 
                                                            driver.FindElement(By.XPath("//li[" + i + "]/div/div/span/button")).Click();
                                                            //Delete from db
                                                            conn_manager.remove_followdata(current_user, followedby_me);
                                                            log("<<<<<<<<<<<<<<< UNFOLLOWED>>>>>>>>>>>>>>>");
                                                            Thread.Sleep(rnd.Next(1000, 3000));

                                                            //Wait
                                                            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                                                            myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._3eajp")));

                                                            for (int j = followers_count_new; j >= 0; j -= 20)
                                                            {
                                                                // Scroll inside web element vertically (e.g. 100 pixel)
                                                                js.ExecuteScript("arguments[0].scrollTop = arguments[1];", driver.FindElement(By.ClassName("_4gt3b")), j);
                                                                Thread.Sleep(rnd.Next(1000, 1000));
                                                                ///////////////////// Unfollow if flag is true [its not found in the follower's list]   /////////////////////////
                                                            }

                                                            //Close following page
                                                            driver.FindElement(By.CssSelector("button._3eajp")).Click();

                                                            break;
                                                        }
                                                        catch { log("Not able to find Following button"); }
                                                    }
                                                    else
                                                    { //log("Not Found in Following List"); Thread.Sleep(rnd.Next(1000, 1000)); 
                                                    }
                                                    //End of If-ELSE

                                                }
                                                catch { log("Wrong Following Index: " + i); Thread.Sleep(rnd.Next(1000, 1000)); break; }

                                            }//End of For loop

                                            for (int j = followers_count_new; j >= 0; j -= 20)
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
                                        catch (Exception e)
                                        {
                                            //닫기가 없으면 그냥 패스~
                                            log("UnFollow Error:May be unable to click on Following ");
                                        }
                                    }//End of if [Flag Check]

                                }//End of foreach loop
                            }
                            else
                            {
                                log("Not following Anyone");
                            }
                        }
                        else
                        {
                            log("Will not Unfollow as Followers count is not available now");
                        }//End of if-else

                    }
                    catch { log("Not able to access Follwers Count from website"); }
                }//end of if(IsElementPresent)
            }
            catch (Exception) { log("Profile not in Korean"); }


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
                    {

                        string followers_num = driver.FindElement(By.CssSelector("a._s53mj >span._bkw5z")).Text;

                        Thread.Sleep(rnd.Next(1000, 3000));
                        int followers_count = Int32.Parse(followers_num, NumberStyles.Integer | NumberStyles.AllowThousands, new CultureInfo("en-US"));

                        string created_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        conn_manager.insert_followersCount(current_user, followers_count, created_at);
                        //Just Sleep
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    catch { log("Followers Word not Found!!"); }

                }
            }
            catch (Exception) { log("Profile not found!!"); }



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
            //log("로그아웃 성공");
        }

        public void quit()
        {
            driver.Quit();
        }





        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
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
