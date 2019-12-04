using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public class Day4
    {
        private int start;
        private int end;

        public int CountPossible(int start, int end)
        {
            int count = 0;
            for (int i = start; i <= end; i++)
            {
                if (MeetCriteria(i)) ++count;
            }

            return count;
        }
        public bool MeetCriteria(int x)
        {
            int[] digit = new int[6];
            var temp = x.ToString();
            if (temp.Length != 6) return false;
            for (int i = 0; i < temp.Length; i++)
            {
                digit[i] = temp[i] - '0';
            }
            // never decrease
            for (int i = 0; i < 5; i++)
            {
                if (digit[i] > digit[i + 1]) return false;
            }
            // pair
            for (int i = 0; i < 5; i++)
            {
                bool stillOk = true;
                if (digit[i] == digit[i + 1])
                {
                    if (i > 0 && digit[i - 1] == digit[i]) stillOk = false;
                    if (i < 4 && digit[i + 1] == digit[i + 2]) stillOk = false;
                    if (stillOk) return true;
                }
            }

            return false;
        }

    }
}
