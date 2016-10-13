﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Main_Manager
    {
        private Form1 context;
        public sql_connection_manager conn_manager;

        //Thread
        private Thread like_thr;

        insta_procedure insta_run;

        public static IPchange ipchanger;


        public static bool ip_changing = false;





        public Main_Manager(Form1 context)
        {
            try
            {
                this.context = context;
                conn_manager = new sql_connection_manager(context);


                DataRow r = conn_manager.version_control();
                if (r == null) { throw new NullReferenceException(); }

                context.log("EASYGRAM Version. " + r["LatestVersion"].ToString() + "\n");


                //모바일 연결 mobile connection
                ipchanger = new IPchange(context, conn_manager);

                insta_procedure.follow_time = DateTime.Now;
                insta_procedure.like_time = DateTime.Now;



                like_thr = new Thread(ipchanger.StartListening);
                like_thr.Start();
            }
            catch (NullReferenceException ex) { context.log("No Result Found!!!!"); }


        }

        public void like_proc()
        {

            while (true)
            {

                try
                {

                    for (int i = 1; i <= 10; i++)
                    {
                        if (ip_changing)
                        {
                            context.log("아이피 변경 중입니다 " + i + " / 10");
                            // Ip is changing
                            context.log(ip_changing.ToString());
                            Thread.Sleep(1500);
                            if (i == 10)
                            {
                                context.log("아이피 변경에 실패했습니다");
                                // Ip_change was failed
                                return;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    insta_run = new insta_procedure(context, conn_manager);

                    //시작


                    insta_run.start();

                    insta_run.language_check();

                    if (insta_run.block_check())
                    {
                        //막힌 계정이면 끄고 루프 탈출
                        insta_run.quit();
                        continue;
                    }
                    //막힌 계정이 아니면 로그인



                    ////1.해시태그 검색
                    //HashTag_proc hash_run = new HashTag_proc(this, conn_manager);
                    //hash_run.hash_tag_search();

                    ////좋아요 루프
                    //hash_run.like_loop(1);




                    ////2. 등록된 유저 검색



                    //RegisteredUser_proc regiuser_run = new RegisteredUser_proc(this, conn_manager);
                    //regiuser_run.random_user();

                    //regiuser_run.like_loop(1);




                    //3. 요청 유저
                    //팔로우, 좋아요
                    Request_proc req_run = new Request_proc(context, conn_manager);
                    req_run.require();

                    req_run.like_loop(1, req_run.require_like_count());


                    //call unfollow
                    insta_run.unfollow();

                    //store of two numbers
                    insta_run.store_followers();


                    insta_run.quit();


                    ipchanger.send_change();





                }
                catch (NullReferenceException ex)
                {
                    context.log("No Result Found!!!!");
                }

                catch (Exception ex)
                {





                    context.log("에러 발생");

                    //  textBox1.Text ="ERROR";



                }
                finally
                {
                    insta_run.quit();
                }


            }

        }


    }
}