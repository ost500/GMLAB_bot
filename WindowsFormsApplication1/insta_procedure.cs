using MySql.Data.MySqlClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class insta_procedure
    {
        MySqlConnection conn;
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;
        Form1 context;
        WebDriverWait wait;
        IWebElement myDynamicElement;

        public insta_procedure(Form1 context)
        {
            this.context = context;
            String strConn = "Server=exampledbinstance.cidnqdbj34y7.ap-northeast-2.rds.amazonaws.com;Database=Insta_bot;Uid=ost;Pwd=dpffhd12;";
            conn = new MySqlConnection(strConn);
        }

        public void start()
        {
            driver = new ChromeDriver("C:\\Program Files (x86)\\Google\\Chrome\\Application");
            baseURL = "https://www.instagram.com";
            verificationErrors = new StringBuilder();

            driver.Navigate().GoToUrl(baseURL + "/");
        }

        private DataRow SelectData(int index)
        {
            DataSet ds = new DataSet();
            try
            {


                //MySqlDataAdapter 클래스를 이용하여 비연결 모드로 데이타 가져오기
                string sql = "SELECT * FROM insta_users";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "members");
                if (ds.Tables.Count > 0)
                {
                    //foreach (DataRow r in ds.Tables[0].Rows)
                    //{
                    //    Console.WriteLine(r["ID"]);
                    //    textBox1.Text += r["ID"].ToString();
                    //}
                    return ds.Tables[0].Rows[index];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                context.button1.Text = "Failed";
            }
            return ds.Tables[0].Rows[index];
        }
        

        public void login()
        {
            DataRow r = SelectData(0);

            driver.FindElement(By.LinkText("로그인")).Click();
            driver.FindElement(By.Name("username")).Clear();
            driver.FindElement(By.Name("username")).SendKeys(r["email"].ToString());
            driver.FindElement(By.Name("password")).Clear();
            driver.FindElement(By.Name("password")).SendKeys(r["password"].ToString());
            driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/div[2]/div/div/form/span/button")).Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("input._9x5sw._qy55y")));
        }

        public void go_to_there(string where_to)
        {
            driver.Navigate().GoToUrl(baseURL + where_to);
        }

        public void follow_account()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            myDynamicElement = wait.Until(d => d.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span[2]/span/button")));
            
            if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span[2]/span/button")))
            {
                driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span[2]/span/button")).Click();
            }
        }

        public void like_loop()
        {

            int i = 0;

            context.textBox1.Text = "pImage_" + i;


            if (IsElementPresent(By.Id("pImage_" + i)))
            {
                IWebElement img_element = driver.FindElement(By.Id("pImage_" + i));
                img_element = img_element.FindElement(By.XPath(".."));
                img_element = img_element.FindElement(By.XPath(".."));
                img_element = img_element.FindElement(By.XPath(".."));
                context.textBox1.Text = img_element.Text;
                img_element.Click();
                while (true)
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    myDynamicElement = wait.Until(d => d.FindElement(By.LinkText("다음")));

                    if (IsElementPresent(By.CssSelector("span._soakw.coreSpriteHeartOpen")))
                    {
                        driver.FindElement(By.CssSelector("span._soakw.coreSpriteHeartOpen")).Click();
                        Thread.Sleep(2000);
                    }

                    driver.FindElement(By.LinkText("다음")).Click();

                    

                }



            }
        }

        public void logout()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            myDynamicElement = wait.Until(d => d.FindElement(By.LinkText("프로필")));

            driver.FindElement(By.LinkText("프로필")).Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            myDynamicElement = wait.Until(d => d.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/div/button")));

            driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/div/button")).Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._4y3e3")));

            driver.FindElement(By.CssSelector("button._4y3e3")).Click();
                    
               

            
        }







        public void SetupTest()
        {
            driver = new ChromeDriver();
            baseURL = "https://www.instagram.com/";
            verificationErrors = new StringBuilder();
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
