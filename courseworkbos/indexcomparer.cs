using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseworkbos
{
    class indexcomparer :IComparer<int>
    {
        public int Compare(int obj1, int obj2)
        {
            if (obj1 > obj2)
            {
                return 1;
            }
            else if (obj1 < obj2)
            {
                return -1;
            }
            else return 0;
        }
    }
}
