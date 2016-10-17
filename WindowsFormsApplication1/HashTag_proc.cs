using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class HashTag_proc : insta_procedure
    {
        public HashTag_proc(Form1 context, sql_connection_manager conn_manager) : base(context, conn_manager)
        {
            
        }



        public void hash_tag_search()
        {
            go_to_there("#"+conn_manager.Select_tag()["tag"].ToString());
            log(baseURL + "/explore/tags/" + conn_manager.Select_tag()["tag"]);
            //log("해쉬태그 검색");
            Thread.Sleep(rnd.Next(1000, 3000));
        }
    }
}
