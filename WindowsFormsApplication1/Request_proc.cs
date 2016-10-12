using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WindowsFormsApplication1
{
    class Request_proc : insta_procedure
    {

        public Request_proc(Form1 context, sql_connection_manager conn_manager) : base(context, conn_manager)
        {
            
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
                        //Update request follow done
                        conn_manager.update_follow_done();

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
                        //Update request follow done
                        conn_manager.update_follow_done();

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
                            //Update request follow done
                            conn_manager.update_follow_done();


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
                    ////Update request like done
                    //conn_manager.update_like_done();
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
                        //Update request follow done
                        conn_manager.update_follow_done();



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
                        //Update request follow done
                        conn_manager.update_follow_done();

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
                        //Update request follow done
                        conn_manager.update_follow_done();

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
                        //Update request follow done
                        conn_manager.update_follow_done();

                        log("기다렸다가 팔로우 했습니다4 --이곳입니다");
                    }
                    Thread.Sleep(rnd.Next(1000, 3000));
                }
                catch (Exception ex) { }
            }

            follow_time = DateTime.Now;


        }

        public int require_follow_count()
        {
            DataRow require_table = conn_manager.select_request();
            context.log("follow required" + ((int)require_table["request_follow"] - (int)require_table["done_follow"]).ToString());
            return (int)require_table["request_follow"] - (int)require_table["done_follow"];

        }

        public int require_like_count()
        {
            DataRow require_table = conn_manager.select_request();
            context.log("like required" + ((int)require_table["request_like"] - (int)require_table["done_like"]).ToString());
            return (int)require_table["request_like"] - (int)require_table["done_like"];

        }

        public void require()
        {
            DataRow require_table = conn_manager.select_request();
            go_to_there(require_table["account"].ToString());
            Thread.Sleep(rnd.Next(1000, 3000));
        }

       


    }
}
