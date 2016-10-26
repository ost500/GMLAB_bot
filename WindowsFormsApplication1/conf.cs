using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easygram
{
    class conf
    {

        int like_delay;
        int follow_delay;
        
        public conf(int like_delay = 10, int follow_delay = 200)
        {
            this.like_delay = like_delay;
            this.follow_delay = follow_delay;
        }
    }
}
