using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLeetcode
{
    /*
     * 1. 两数之和
     *  https://leetcode.cn/problems/two-sum/description/
     */
    internal class LeetCode_01
    {

        public static class Solution
        {
            /*
             * 方法1：建立哈希表
             * 但是需要注意的是Csharp的Dictornary是不允许添加重复键的，解决方法就是使用线程安全的ConcurrentDictionary，
             * 这样会过滤的重复的键值 
             * 因为不允许插入重复的键-值对，所以可以让值可变可扩展
             */
            public static int[] TwoSum_01(int[] nums, int target)
            {
                //int[] result = new int[2];
                //ConcurrentDictionary<int, List<int>> hashmap = new();   // 值-位置
                Dictionary<int, List<int>> hashmap = new();   // 值-位置
                for (int i = 0;i < nums.Length;i++)
                {
                    if (hashmap.ContainsKey(target - nums[i]))
                    {
                        return new int[] { hashmap.GetValueOrDefault(target - nums[i])!.FirstOrDefault() ,i };
                    }

                    if (hashmap.ContainsKey(nums[i]))
                    {
                        hashmap[nums[i]].Add(i);
                    }
                    else
                    {
                        hashmap.Add(nums[i], new List<int>() {i});
                    }
                }


                return default;
            }
        }

        public static void leetcode_01() 
        {
            int[] ints = { 1, 11, 15, 2, 7 }; int target1 = 9;
            int[] test2 = { 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 7, 1, 1, 1, 1, 1 }; int target2 = 11;

            //var res = Solution.TwoSum_01(ints, target1);
            var res = Solution.TwoSum_01(test2, target2);
            foreach (int i in res)
            {
                Console.Write(i+" ");
            }
        }
    }
}
