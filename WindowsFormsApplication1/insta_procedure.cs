using OpenQA.Selenium;
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

        private ChromeDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;
        Form1 context;
        WebDriverWait wait;
        IWebElement myDynamicElement;
        int sleep_t;

        sql_connection_manager conn_manager;

        Random rnd = new Random();


        DataTable t;
        DataRow r;


        public insta_procedure(Form1 context, sql_connection_manager conn_manager)
        {
            this.context = context;

            this.conn_manager = conn_manager;

            //랜덤 대기값




        }

        public void log(string logging)
        {
            context.textBox1.AppendText(Environment.NewLine);
            context.textBox1.AppendText(logging);
        }

        public void start(int idx)
        {

            t = conn_manager.SelectData(4);
            r = t.Rows[0];

            foreach (DataRow r in t.Rows)
            {
                
                context.listBox1.Items.Add(r["user_id"]);
                
            }


            String path = "D:\\chrome_cache\\" + r["user_id"].ToString();


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




        public void login()
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
                }

                return;
            }





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

        public void hash_tag_search()
        {

            go_to_there("/explore/tags/" + conn_manager.Select_tag()["tag"]);
            //log("해쉬태그 검색");
            Thread.Sleep(rnd.Next(1000, 3000));
        }

        public void random_user()
        {
            //log("유저 검색");
            go_to_there("/yunakim/");
            Thread.Sleep(rnd.Next(1000, 3000));
        }


        public void like_loop()
        {

            int i = 0;




            if (IsElementPresent(By.Id("pImage_" + i)))
            {
                IWebElement img_element = driver.FindElement(By.Id("pImage_" + i));
                img_element = img_element.FindElement(By.XPath(".."));
                img_element = img_element.FindElement(By.XPath(".."));
                img_element = img_element.FindElement(By.XPath(".."));

                img_element.Click();

                DateTime currentTime = DateTime.Now;
                DateTime future = currentTime.AddSeconds(5);
                while (future > currentTime)
                {
                    currentTime = DateTime.Now;


                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    myDynamicElement = wait.Until(d => d.FindElement(By.LinkText("다음")));
                    //"다음"이 나올 때 까지 기다림

                    if (IsElementPresent(By.CssSelector("span._soakw.coreSpriteHeartOpen")))
                    //"좋아요"가 클릭 돼 있지 않을 때
                    {
                        driver.FindElement(By.CssSelector("span._soakw.coreSpriteHeartOpen")).Click();
                        //log("좋아요를 눌렀습니다");
                        //"좋아요" 클릭! 
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    else
                    //"좋아요"가 클릭 돼 있을 때
                    {
                        Thread.Sleep(rnd.Next(1000, 3000));
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
                        //log("다음 게시물로 넘어갑니다");
                    }
                }

                Thread.Sleep(rnd.Next(1000, 3000));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._3eajp")));

                driver.FindElement(By.CssSelector("button._3eajp")).Click();

            }
        }

        public void like_follow_loop()
        {

            int i = 0;




            if (IsElementPresent(By.Id("pImage_" + i)))
            {
                IWebElement img_element = driver.FindElement(By.Id("pImage_" + i));
                img_element = img_element.FindElement(By.XPath(".."));
                img_element = img_element.FindElement(By.XPath(".."));
                img_element = img_element.FindElement(By.XPath(".."));

                img_element.Click();

                int k = 0;

                DateTime currentTime = DateTime.Now;
                DateTime future = currentTime.AddSeconds(3);
                while (future > currentTime)
                {
                    currentTime = DateTime.Now;



                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    myDynamicElement = wait.Until(d => d.FindElement(By.LinkText("다음")));
                    //"다음"이 나올 때 까지 기다림

                    if (IsElementPresent(By.CssSelector("span._soakw.coreSpriteHeartOpen")))
                    //"좋아요"가 클릭 돼 있지 않을 때
                    {
                        driver.FindElement(By.CssSelector("span._soakw.coreSpriteHeartOpen")).Click();

                        //"좋아요" 클릭! 
                        //log("좋아요를 눌렀습니다");
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }
                    else
                    //"좋아요"가 클릭 돼 있을 때
                    {
                        Thread.Sleep(rnd.Next(1000, 3000));
                    }



                    //팔로우
                    if (IsElementPresent(By.XPath("//button")))
                    {
                        try
                        {
                            //팔로우를 찾아서 있으면 진행 없으면 에러
                            Assert.AreEqual("팔로우", driver.FindElement(By.XPath("//button")).Text);

                            driver.FindElement(By.XPath("//button")).Click();
                            Thread.Sleep(rnd.Next(1000, 3000));
                        }
                        catch (Exception e) { }
                        //log("팔로우 했습니다");
                    }

                    if (!IsElementPresent(By.LinkText("다음")))
                    //"다음"이 없을 때
                    {
                        Thread.Sleep(rnd.Next(1000, 3000));
                        break;
                    }
                    else
                    //"다음"이 있을 때
                    {
                        driver.FindElement(By.LinkText("다음")).Click();
                        //log("다음 게시물로 넘어갑니다");
                    }

                }


                Thread.Sleep(rnd.Next(1000, 3000));

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
