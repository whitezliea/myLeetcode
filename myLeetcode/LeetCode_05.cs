using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLeetcode
{
    internal class LeetCode_05
    {
        public static class Solution
        {
            /*
         * 方法1：双指针法
         * 时间复杂度：O(n^2)
         * 空间复杂度：O(1)
         * 首先确定回文串，就是找中心然后想两边扩散看是不是对称的就可以了
         */
            public static string LongestPalindrome_01(string s)
            {
                int left_num = 0, right_num = 0, MaxLength = 0; //左指针，有指针，最大回文长度
                if (s == null || s.Length == 0)
                    return default!;

                for (int i = 0; i < s.Length; i++) //从0到n-1 依次遍历
                {
                    //一个元素可以作为中心点，两个元素也可以作为中心点。
                    check(ref s, i, i, s.Length);   // 以i为中心,向两边检查是否为回文 //检查aba这种情况
                    check(ref s, i, i + 1, s.Length);   //以i+1为中心，向两边检查是否为回文。//检查abba这种情况
                }



                void check(ref string ss, int left_flag, int right_flag, int length)
                {
                    while (left_flag >= 0   //左下标大于0
                        && right_flag < length //右下标小于长度
                        && ss[left_flag] == ss[right_flag])
                    {
                        if (right_flag - left_flag + 1 > MaxLength)
                        {
                            left_num = left_flag;
                            right_num = right_flag;
                            MaxLength = right_flag - left_flag + 1;
                        }

                        left_flag--;
                        right_flag++;
                    }
                }

                return s.Substring(left_num, MaxLength);
            }


            public static string LongestPalindrome_02(string s)
            {

                /* 
                 * 方法2：动态规划
                 * 时间复杂度：O(n^2)
                 * 空间复杂度：O(n^2)
                 * 1-确定dp数组以及下标的含义
                 *  布尔类型的dp[i][j]：表示区间范围[i,j] （注意是左闭右闭）的子串是否是回文子串，
                 *  如果是dp[i][j]为true，否则为false。
                 *  2-确定递推公式
                 *  3-dp数组初始化
                 *  4-确定遍历顺序
                 *  5-举例推导dp数组
                 */

                int len = s.Length;
                //int[][] dp = new int[len][];
                List<List<int>> dp = new List<List<int>>(len);
                for (int i = 0; i < len; i++)
                {
                    dp.Add(new List<int>(new int[len]));
                }

                int left_index = 0, right_index = 0, maxLength = 0;
                for (int i = len - 1; i >= 0; i--)
                {
                    for (int j = i; j < len; j++)
                    {
                        if (s[i] == s[j])
                        {
                            if (j - i <= 1)
                            {
                                dp[i][j] = 1;
                            }
                            else if (dp[i + 1][j - 1] != 0)
                            {
                                dp[i][j] = 1;
                            }
                        }

                        if (dp[i][j] != 0
                                && j - i + 1 > maxLength)
                        {
                            maxLength = j - i + 1;
                            left_index = i;
                            right_index = j;
                        }
                    }

                }

                return s.Substring(left_index, maxLength);
            }

        }

        public static void leetcode_05()
        {
            string s = "abacz";
            WriteLine(Solution.LongestPalindrome_02(s));
        }
    }
}
