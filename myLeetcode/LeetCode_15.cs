using System.ComponentModel;

namespace myLeetcode
{
    /*
     * 15. 三数之和
     * https://leetcode.cn/problems/3sum/
     */
    internal class LeetCode_15
    {
        public static class Solution
        {
            /*
             * 初始方法：暴力循环破解
             * 方法复杂度O(n^3) ==> 同时会包含重复的答案
             */
            public static IList<IList<int>> ThreeSum_00(int[] nums)
            {
                Array.Sort(nums);
                IList<IList<int>> result = new List<IList<int>>();
                //IList<int> list = new List<int>();

                for (int i = 0; i < nums.Length; i++)
                {
                    for (int j = i + 1; j < nums.Length; j++)
                    {
                        for (int k = j + 1; k < nums.Length; k++)
                        {
                            if (nums[i] + nums[j] + nums[k] == 0)
                            {
                                result.Add(new List<int> { nums[i], nums[j], nums[k] });
                            }
                        }
                    }
                }

                return result;
            }

            /*
             * 改进方法：参考两数之和的方法，n1+n2+n3=0 ==> n1+n2=0-a3; 
             * 
             */
            public static IList<IList<int>> ThreeSum_01(int[] nums)
            {
                IList<IList<int>> result = new List<IList<int>>();
                HashSet<int> visited = new HashSet<int>();
                HashSet<string> usedNum = new();

                for (int i = 0; i < nums.Length; i++)
                {
                    twosum(nums, i, 0 - nums[i]);
                }

                List<int> twosum(int[] nums, int index, int target)
                {
                    //Dictionary<int, IList<int>> map = new();    // 值-位置
                    visited.Clear();

                    for (int i = 0; i < nums.Length; i++)
                    {
                        if (i == index)
                        {
                            continue;
                        }

                        if (visited.Contains(target - nums[i]))
                        {
                            var res = new List<int>() { target - nums[i], nums[i] };
                            res!.Add(nums[index]);
                            res.Sort();
                            var s = res[0].ToString() + res[1].ToString() + res[2].ToString();
                            if (!usedNum.Contains(s))
                            {
                                usedNum.Add(s);
                                result.Add(res);
                            }
                        }

                        visited.Add(nums[i]);
                    }

                    return default;
                }

                return result;


            }


            /*
             * 方法2：排序+双指针
             */
            public static IList<IList<int>> ThreeSum_02(int[] nums)
            {
                Array.Sort(nums);   //第一步先排序

                IList<IList<int>> result  = new List<IList<int>>();

                // 枚举A
                for (int one = 0; one < nums.Length;one++)
                {
                    // 需要和上一次的枚举的数不相同
                    if (one > 0 && nums[one] == nums[one-1])
                    {
                        continue;
                    }

                    //C对应的指针初始指向数组的最右端
                    int third = nums.Length - 1;
                    int target = -nums[one];

                    // 枚举B
                    for (int second = one + 1;second < nums.Length;second++)
                    {
                        // 需要和上一次枚举的数不同
                        if (second > one + 1 && nums[second] == nums[second -1])
                        {
                            continue;
                        }

                        // 需要保证B的指针在C的指针的左侧
                        while (second < third && nums[second] + nums[third] > target)
                        {
                            third--;
                        }

                        // 如果指针重合，随着B的后续的增加，
                        // 就不会满足a+b+c = 0 ，并且 b<c 的 c 了，可以退出循环

                        if (second == third)
                        {
                            break;
                        }

                        if (nums[second] + nums[third] == target) 
                        {
                            IList<int> temp = new List<int>();
                            temp!.Add(nums[one]);
                            temp!.Add(nums[second]);
                            temp!.Add(nums[third]);
                            result.Add(temp);
                        }
                    }
                }

                return result;
            }

        }

        public static void leetcode_15()
        {
            int[] nums1 = { -1, 0, 1, 2, -1, -4, -2, -3, 3, 0, 4 };
            int[] nums3 = { -1, 0, 1, 2, -1, -4, -2, -3, 3, 0, 4 };
            int[] nums2 = { -1, 0, 1, 2, -1, -4 };
            var res = Solution.ThreeSum_02(nums1);

            foreach (var re in res)
            {
                foreach (var v in re)
                {
                    Console.Write(v + " ");
                }
                WriteLine();
            }
        }
    }
}

/*
[[0,1,-1],[-1,-1,2],[1,3,-4],[0,2,-2],[1,2,-3],[0,-4,4]] 

[[-4,0,4],[-4,1,3],[-3,-1,4],[-3,0,3],[-3,1,2],[-2,-1,3],[-2,0,2],[-1,-1,2],[-1,0,1]]
 */