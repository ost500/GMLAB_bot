using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace easygram
{
    class HashTag_proc : insta_procedure
    {



        public HashTag_proc(Form1 context, sql_connection_manager conn_manager) : base(context, conn_manager)
        {

        }


        public new void like_loop(int follow_count, int like_count = 1000)
        {


            int i = 0;
            Thread.Sleep(rnd.Next(1000, 3000));

            if (follows_count < limit_follows)
            {   //팔로우
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

                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                                .Click();
                            //Save Follow data
                            saveFollowData();
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

                            driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button")).Click();

                            //Save Follow data
                            saveFollowData();
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
            IWebElement img_element;
            bool image_startflag = false;

            if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/div[2]/div/div/a/div")))
            {


                img_element = driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/div[2]/div/div/a/div"));
                image_startflag = true;

            }
            else if (IsElementPresent(By.XPath("//span[@id='react-root']/section/main/article/div[1]/div/div/a/div")))
            {

                img_element = driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/div[1]/div/div/a/div"));

                image_startflag = true;

            }
            else { return; }

            //finding first picture
            if (image_startflag)
            {
                //  IWebElement img_element = driver.FindElement(By.XPath("//span[@id='react-root']/section/main/article/div/div/div/a/div"));

                img_element.Click();


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
                            //waiting the page ready
                        }

                    }
                    catch (Exception e)
                    {
                        //failed to load the page
                        context.log(" [인스타 루프] 페이지 로딩에 실패했습니다");

                        Thread.Sleep(rnd.Next(1000, 3000));

                        break;
                    }

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
                        else
                        //"좋아요"가 클릭 돼 있을 때
                        {

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
                                    driver.FindElement(By.XPath("//header/span/button")).Click();


                                    //Save Follow data
                                    saveFollowData();
                                    //Set Follows Count for today
                                    follows_count = follows_count + 1;

                                    context.log(" [인스타 루프] : 팔로우 했습니다");

                                    follow_count--;


                                    if (comments_count < limit_comments)
                                    {
                                        //팔로우하면 댓글 자동
                                        t = conn_manager.Select_comments();

                                        string comment = t.Rows[0]["comment"].ToString();

                                        //팔로우를 찾아서 있으면 진행 없으면 에러
                                        driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(comment);
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

                    if (comments_count < limit_comments)
                    {
                        //comment portion
                        if (IsElementPresent(By.CssSelector("input._7uiwk._qy55y")))
                        {
                            if (comment_time_gap(delay_comment))
                            {
                                try
                                {
                                    t = conn_manager.Select_comments();

                                    string comment = t.Rows[0]["comment"].ToString();

                                    //팔로우를 찾아서 있으면 진행 없으면 에러
                                    driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(comment);
                                    driver.FindElement(By.CssSelector("input._7uiwk._qy55y")).SendKeys(Keys.Enter);
                                    //update worknumber  of comment
                                    conn_manager.Update_comment_worknum(comment);
                                    //Set Comments Count for today
                                    comments_count = comments_count + 1;

                                    context.log(" [인스타 루프] : 댓글을 입력했습니다");
                                }
                                catch (Exception)
                                {
                                    context.log(" [인스타 루프] : 입력할 댓글이 없습니다");
                                }
                                

                            }
                            Thread.Sleep(rnd.Next(1000, 3000));
                            Thread.Sleep(rnd.Next(1000, 3000));
                        }
                    }
                    else { context.log("Comment Limit reached"); }

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
                            //If follow time gap was done
                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                                .Click();

                            //Save Follow data
                            saveFollowData();
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
                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/button"))
                                .Click();

                            //Save Follow data
                            saveFollowData();
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
                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button"))
                                .Click();

                            //Save Follow data
                            saveFollowData();
                            //Set Follows Count for today
                            follows_count = follows_count + 1;

                            context.log(" [인스타 루프] : 팔로우 했습니다");
                        }
                        else
                        {

                            //대기 타다가 팔로우
                            DateTime now = DateTime.Now;
                            Thread.Sleep(200000 - ((int)(now.Subtract(follow_time).TotalSeconds) * 1000));
                            driver.FindElement(
                                    By.XPath("//span[@id='react-root']/section/main/article/header/div[2]/div/span/span/button"))
                                .Click();

                            //Save Follow data
                            saveFollowData();
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


        public void hash_tag_search()
        {
            go_to_there("#" + conn_manager.Select_tag()["tag"].ToString());
            context.log(baseURL + "/explore/tags/" + conn_manager.Select_tag()["tag"]);
            //log("해쉬태그 검색");
            Thread.Sleep(rnd.Next(1000, 3000));
        }
    }
}
