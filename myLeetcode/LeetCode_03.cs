namespace myLeetcode
{
    /*
     *  https://leetcode.cn/problems/longest-substring-without-repeating-characters/
     *  关键词：滑动窗口，字符串
    */
    internal class LeetCode_03
    {
        /*
           解法1：滑动窗口
           维持一个滑动窗口
       */
        public static int LengthOfLongestSubstring_1(string s)
        {
            if (s.Length == 0)
                return 0;
            //Dictionary<char, int> map = new(); 
            HashSet<char> maps = new(); //这个就是滑动窗口，用hashset是为了利用其检索性能
            int maxStr = 0;                                         //        i
            int left = 0;   // left是滑动窗口的起点,i是滑动窗口的终点     a  b  c  a  b  c  b  b
            for (int i = 0; i < s.Length; i++)                   // left
            {
                while (maps.Contains(s[i]) == true) // 如果滑动窗口中检查到了重复的字母，那么移除字母
                {
                    maps.Remove(s[left]);
                    left++;
                }
                maxStr = Math.Max(maxStr, i - left + 1);
                maps.Add(s[i]);
            }

            return maxStr;
        }

        public static void leetcode_03()
        {
            string[] ss = { "abcabcbb" ,
                            "bbbbb",
                            "pwwkew"};
            Console.WriteLine("3. 无重复字符的最长子串\r\n方法1：滑动窗口");
            foreach (var s in ss)
            {
                Console.Write(LengthOfLongestSubstring_1(s) + "   ");
            }
        }
    }
}
