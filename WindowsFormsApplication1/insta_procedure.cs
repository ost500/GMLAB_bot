using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Data;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;



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
        int sleep_t;

        protected sql_connection_manager conn_manager;

        protected Random rnd = new Random();


        DataTable t;
        DataRow r;

        //follow 시간 like 시간
        public static DateTime follow_time;
        public static DateTime like_time;
        public static DateTime comment_time;
        public static string save_follow_time;

        private int like_time_sec = 11;
        private int follow_time_min = 3;
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

            //int time;

            //if ((time = (int) conn_manager.select_configuration()["delay_like"]) != null)
            //{
            //    like_time_sec = time;
            //}
            //if ((time = (int)conn_manager.select_configuration()["delay_follow"]) != null)
            //{
            //    follow_time_min = time;
            //}

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
                //return false;
                return true;
            }
            else
            {
                log("continue");
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
                return 1;
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
                //return 3;
                return 1;
            }
            else
            {
                delay_follow = Int32.Parse(jobrow["delay_follow"].ToString());

                //return delay_follow;
                return 1;
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
                log("FFFRRRRRR: " + followed);
                //set time
                save_follow_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //insert followed id into database
                conn_manager.insert_followdata(current_user, followed, save_follow_time);
            }
            else if (IsElementPresent(By.CssSelector("h1._i572c")))
            {
                //find the above followed user
                string followed = driver.FindElement(By.CssSelector("h1._i572c")).Text;
                log("FFFFFFFFFFFFFFF: " + followed);
                //set time
                save_follow_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //insert followed id into database
                conn_manager.insert_followdata(current_user, followed, save_follow_time);
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
            r = t.Rows[0];
            log("row[0]" + t.Rows[0]["user_id"]);
            //log("row[1]" + t.Rows[1]);



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

            log(delay_like.ToString());
            log(delay_comment.ToString());
            log(delay_follow.ToString());
            log(delay_unfollow.ToString());
            //Get current directory path and set chrome cache path
            string currentDir = getCurrentPath();
            string path = currentDir + "\\chrome_cache\\" + current_user.Trim();

            // log(path);
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

        public bool block_check()
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
                context.log("It's Koreean");
            }


            try
            {

                //Check #tag Status ,comment and job status ..IF Ok then Proceed Otherwise Stop

                //  if (!checkHashTag()) { log("STOP"); return true; }
                //  if (!checkCommentStatus()) { log("STOP"); return true; }
                if (!checkJobStatus(current_user)) { log("STOPPED!!"); return true; }

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
                    try
                    {

                        //check for Profile Keyword 
                        Assert.AreEqual("Profile", driver.FindElement(By.LinkText("Profile")).Text);
                        //if Profile Word exist [means lanuage is English] then click on the Profile 
                        driver.FindElement(By.LinkText("Profile")).Click();
                        //Now Change the language to Korean
                        new SelectElement(driver.FindElement(By.CssSelector("select._nif11"))).SelectByText("Korean");
                        log("Language Changed after login");
                    }
                    catch (Exception exe)
                    {
                        log("korean");
                        //  return false;
                    }


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



            //driver.Navigate().GoToUrl(baseURL + where_to);
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
                context.log("FOUND FIRST PICTURE##################");
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





            try
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
                catch (Exception e) { }


                driver.FindElement(By.CssSelector("button._3eajp")).Click();
            }
            catch (Exception e)
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
                catch (Exception ex) { }
            }

            follow_time = DateTime.Now;


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
