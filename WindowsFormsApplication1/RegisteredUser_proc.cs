using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace easygram
{
    class RegisteredUser_proc : insta_procedure
    {

        public RegisteredUser_proc(Form1 context, sql_connection_manager conn_manager) : base(context, conn_manager)
        {
            
        }
      

        public void random_user()
        {



            if (IsElementPresent(By.CssSelector("button._3eajp")))
            {
                try
                {
                    // 닫기
                    // close

                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                    myDynamicElement = wait.Until(d => d.FindElement(By.CssSelector("button._3eajp")));


                    driver.FindElement(By.CssSelector("button._3eajp")).Click();
                }
                catch (Exception e)
                {
                    //닫기가 없으면 그냥 패스~
                }
            }



            DataTable random_user_table = conn_manager.Select_RandomUser(2);

            go_to_there(random_user_table.Rows[0]["user_id"].ToString());
            Thread.Sleep(rnd.Next(1000, 3000));
            //like_follow_loop();

            //log("유저 검색");

        }
    }
}
