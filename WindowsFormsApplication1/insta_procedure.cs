﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Data;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;

namespace WindowsFormsApplication1
{
    class insta_procedure
    {

        private static ChromeDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;
        Form1 context;
        WebDriverWait wait;
        IWebElement myDynamicElement;
        int sleep_t;

        protected sql_connection_manager conn_manager;

        protected Random rnd = new Random();


        DataTable t;
        DataRow r;

        //follow 시간 like 시간
        static public DateTime follow_time;
        static public DateTime like_time;



        public insta_procedure(Form1 context, sql_connection_manager conn_manager)
        {
            this.context = context;

            this.conn_manager = conn_manager;

            //랜덤 대기값

            t = context.t;
            r = context.r;

            follow_time = DateTime.Now;

        }

        public bool like_time_gap()
        {
            DateTime now = DateTime.Now;
            log(now.ToString());
            log(like_time.ToString());
            if (now.Subtract(like_time).TotalSeconds > 11)
            {
                like_time = DateTime.Now;
                return true;
            }
            else
            {
                //Thread.Sleep(11000);
                //like_time = DateTime.Now;
                return false;
            }
        }

        public bool follow_time_gap()
        {
            DateTime now = DateTime.Now;
            log("-----------------");
            log(now.ToString());
            log(follow_time.ToString());
            log(now.Subtract(follow_time).TotalMinutes.ToString() + "follow_time_gap");
            
            if (now.Subtract(follow_time).TotalMinutes > 3)
            {
                follow_time = DateTime.Now;
                return true;
            }
            else
            {
                //Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                return false;
            }
        }



        public void log(string logging)
        {
            context.textBox1.AppendText(Environment.NewLine);
            context.textBox1.AppendText("[" + DateTime.Now.ToString() + "]" + logging);
        }

        public void start()
        {


            t = conn_manager.SelectData(4);
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
            log(r["user_id"].ToString());
            string a = r["user_id"].ToString();
            string path = "D:\\chrome_cache\\" + a.Trim();

            ChromeOptions co = new ChromeOptions();

            co.AddArguments("user-data-dir=" + path);

            var driverService = ChromeDriverService.CreateDefaultService("C:\\Program Files (x86)\\Google\\Chrome\\Application");
            driverService.HideCommandPromptWindow = true;
            //driverService.Port = my_port;
            //co.BinaryLocation = "C:\\Program Files (x86)\\Google\\Chrome\\Application";


            driver = new ChromeDriver(driverService, co);
            //driver = new ChromeDriver("C:\\Program Files (x86)\\Google\\Chrome\\Application");



            baseURL = "https://www.instagram.com";
            verificationErrors = new StringBuilder();

            log("시작했습니다");

            driver.Navigate().GoToUrl(baseURL + "/");
            log("메인으로 갔습니다");


        }

        public bool block_check()
        {

            try
            {
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
                    //안나오면 정상 가동
                    log("계정 인증이 없다");
                    return false;
                }

                log("계정 인증 있다");
                //DB 기록
                conn_manager.blocked_update();
                return true;
            }

            log("로그인 버튼이 있다");
            login();
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
            //log("로그인 성공");
        }

        public void go_to_there(string where_to)
        {
            driver.Navigate().GoToUrl(baseURL + where_to);
            //log(where_to + "(으)로 이동");
        }



        public void random_user()
        {
            DataTable random_user_table = conn_manager.Select_RandomUser(2);

            go_to_there("/" + random_user_table.Rows[0]["user_id"]);
            Thread.Sleep(rnd.Next(1000, 3000));
            //like_follow_loop();

            //log("유저 검색");

        }

        public void hash_tag_search()
        {
            go_to_there("/explore/tags/" + conn_manager.Select_tag()["tag"]);
            //log("해쉬태그 검색");
            Thread.Sleep(rnd.Next(1000, 3000));
        }


        public void like_loop(int follow_count)
        {


            int i = 0;


            
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
                    if (follow_time_gap())
                    {
                        driver.FindElement(
                                By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                            .Click();
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

                    if (follow_time_gap())
                    {
                        driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Click();
                        log("팔로우 했습니다2");
                    }

                    Thread.Sleep(rnd.Next(1000, 3000));
                }
                catch (Exception e) { }
            }



            if (IsElementPresent(By.Id("pImage_" + i)))
            {
                IWebElement img_element = driver.FindElement(By.Id("pImage_" + i));
                img_element = img_element.FindElement(By.XPath(".."));
                img_element = img_element.FindElement(By.XPath(".."));
                img_element = img_element.FindElement(By.XPath(".."));

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



                    if (follow_time_gap())
                    {
                        //팔로우
                        if (IsElementPresent(By.XPath("//button")))
                        {
                            try
                            {
                                //팔로우를 찾아서 있으면 진행 없으면 에러
                                Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//button")).Text);
                                driver.FindElement(By.XPath("//button")).Click();
                                follow_count--;
                                Thread.Sleep(rnd.Next(1000, 3000));
                            }
                            catch (Exception e) { }
                            log("팔로우 했습니다");
                        }

                    }




                    if (IsElementPresent(By.CssSelector("span._soakw.coreSpriteHeartOpen")))
                    //"좋아요"가 클릭 돼 있지 않을 때
                    {
                        if (like_time_gap())
                        {
                            driver.FindElement(By.CssSelector("span._soakw.coreSpriteHeartOpen")).Click();
                        }


                        //"좋아요" 클릭! 
                        //log("좋아요를 눌렀습니다");
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    else
                    //"좋아요"가 클릭 돼 있을 때
                    {

                    }







                    if (!IsElementPresent(By.LinkText("다음")))
                    //"다음"이 없을 때
                    {
                        //다음이 없고 follow는 안했을 때 대기
                        if (!follow_time_gap())
                        {
                            DateTime now = DateTime.Now;
                            Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                        }
                        break;
                    }
                    else
                    //"다음"이 있을 때
                    {
                        driver.FindElement(By.LinkText("다음")).Click();

                        log("다음 게시물로 넘어갑니다");
                    }



                }
                
                // 닫기
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._3eajp")));

                driver.FindElement(By.CssSelector("button._3eajp")).Click();

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
                    if (follow_time_gap())
                    {
                        driver.FindElement(
                                By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                            .Click();
                        log("팔로우 했습니다3 --이곳입니다");
                        log(follow_time.ToString() + "팔로우한 시각3");
                    }
                    else
                    {
                        
                        //대기 타다가 팔로우
                        DateTime now = DateTime.Now;
                        log(now.ToString()+"현시각");
                        log(follow_time.ToString()+"팔로우한 시각");
                        log(((int)(now.Subtract(follow_time).TotalSeconds) * 1000).ToString()+"여기에러인가요");
                        Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                        driver.FindElement(
                                By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                            .Click();
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

                    if (follow_time_gap())
                    {
                        driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Click();
                        log("팔로우 했습니다4 --이곳입니다");
                    }
                    else
                    {
                        //대기 타다가 팔로우
                        DateTime now = DateTime.Now;
                        Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                        driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Click();
                        log("기다렸다가 팔로우 했습니다4 --이곳입니다");
                    }
                    Thread.Sleep(rnd.Next(1000, 3000));
                }
                catch (Exception e) { }
            }

            follow_time = DateTime.Now;
            
        }



        public void follow_loop()
        {

            int i = 0;


            //팔로우
            if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")))
            {
                try
                {
                    //팔로우를 찾아서 있으면 진행 없으면 에러
                    Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")).Text);
                    follow_time_gap();
                    driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")).Click();
                    Thread.Sleep(rnd.Next(1000, 3000));
                }
                catch (Exception e) { }
                log("팔로우 했습니다");
            }
            else if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")))
            {
                try
                {
                    //팔로우를 찾아서 있으면 진행 없으면 에러
                    Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Text);
                    follow_time_gap();
                    driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Click();
                    Thread.Sleep(rnd.Next(1000, 3000));
                }
                catch (Exception e) { }
            }




            if (IsElementPresent(By.Id("pImage_" + i)))
            {
                IWebElement img_element = driver.FindElement(By.Id("pImage_" + i));
                img_element = img_element.FindElement(By.XPath(".."));
                img_element = img_element.FindElement(By.XPath(".."));
                img_element = img_element.FindElement(By.XPath(".."));

                img_element.Click();

                int k = 0;

                DateTime currentTime = DateTime.Now;
                DateTime future = currentTime.AddSeconds(10);
                while (future > currentTime)
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






                    //팔로우
                    if (IsElementPresent(By.XPath("//button")))
                    {
                        try
                        {
                            //팔로우를 찾아서 있으면 진행 없으면 에러
                            Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//button")).Text);
                            follow_time_gap();
                            driver.FindElement(By.XPath("//button")).Click();
                            Thread.Sleep(rnd.Next(1000, 3000));
                        }
                        catch (Exception e) { }
                        log("팔로우 했습니다");
                    }





                    if (!IsElementPresent(By.LinkText("다음")))
                    //"다음"이 없을 때
                    {
                        break;
                    }
                    else
                    //"다음"이 있을 때
                    {
                        driver.FindElement(By.LinkText("다음")).Click();

                        log("다음 게시물로 넘어갑니다");
                    }



                }


                //팔로우
                if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")))
                {
                    try
                    {
                        //팔로우를 찾아서 있으면 진행 없으면 에러
                        Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button")).Text);
                        if (follow_time_gap())
                        {
                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                                .Click();
                            log("팔로우 했습니다");
                        }
                        else
                        {
                            DateTime now = DateTime.Now;
                            Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                        }

                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    catch (Exception e) { }
                    
                }

                //다른모양의 팔로우 
                else if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")))
                {
                    try
                    {
                        //팔로우를 찾아서 있으면 진행 없으면 에러
                        Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Text);

                        if (follow_time_gap())
                        {
                            driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Click();
                            log("팔로우 했습니다");
                        }
                        else
                        {
                            DateTime now = DateTime.Now;
                            Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                        }
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    catch (Exception e) { }
                }




                // 닫기
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._3eajp")));

                driver.FindElement(By.CssSelector("button._3eajp")).Click();

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

        private bool IsElementPresent(By by)
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
