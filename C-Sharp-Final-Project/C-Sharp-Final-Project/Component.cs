using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_Final_Project
{
    class Component
    {
        //cool down timer: return false while decrement current time until 0, then reset it to cool down max.
        public static bool CoolDown(ref int coolDownCurrentTime, int coolDownMax)
        {
            if (coolDownCurrentTime > 0)
            {
                coolDownCurrentTime--;
            } else
            {
                coolDownCurrentTime = coolDownMax;
                return true;
            }
            return false;
        }
    }
}
