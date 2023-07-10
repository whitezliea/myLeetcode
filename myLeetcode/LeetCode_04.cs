namespace myLeetcode
{
    /*
     * https://leetcode.cn/problems/median-of-two-sorted-arrays/
     * 4. 寻找两个正序数组的中位数
     * 算法的时间复杂度应该为 O(log (m+n))  ==> 建立二叉树
     */
    internal class LeetCode_04
    {
        public static class Solution
        {


            /*方法1：暴力遍历法
             * 时间复杂度：遍历一半的数组 (m+n)/2
             * 空间复杂度：开辟了一个数组，保存合并后的两个数组，但是只保存一半 O(m+n)/2
             */
            public static double FindMedianSortedArrays_01(int[] nums1, int[] nums2)
            {


                int n1 = nums1.Length;
                int n2 = nums2.Length;
                IList<int> list = new List<int>();
                //int[] list = new int[n1+n2];
                int n = (n1 + n2);

                if (n1 == 0)
                {
                    if (n2 % 2 == 0)
                    {
                        return (nums2[n2 / 2 - 1] + nums2[n2 / 2]) / 2.0;
                    }
                    else
                    {
                        return nums2[n2 / 2];
                    }
                }

                if (n2 == 0)
                {
                    if (n1 % 2 == 0)
                    {
                        return (nums1[n1 / 2 - 1] + nums1[n1 / 2]) / 2.0;
                    }
                    else
                    {
                        return nums1[n1 / 2];
                    }
                }


                int count = 0;
                int flag1 = 0, flag2 = 0;
                while (count < n1 + n2)  // 合并数组
                {
                    if (flag1 == n1)
                    {
                        while (flag2 != n2)     // 如果nums1数组都已经添加到list中，现在只添加nums2数组
                        {
                            //list[count++] = nums2[flag2++];
                            list.Add(nums2[flag2++]);
                            count++;
                        }
                        break;
                    }

                    if (flag2 == n2)
                    {
                        while (flag1 != n1)
                        {
                            list.Add(nums1[flag1++]);
                            count++;
                        }
                        break;
                    }

                    if (nums1[flag1] < nums2[flag2])
                    {
                        list.Add(nums1[flag1++]);
                        count++;
                    }
                    else
                    {
                        list.Add(nums2[flag2++]);
                        count++;
                    }

                    if (count > n) //当合并到中位数前后的数时，返回
                        break;

                }

                if (n % 2 == 0)
                {
                    return (list[n / 2] + list[n / 2 - 1]) / 2.0;
                }
                else
                {
                    return (list[n / 2]);
                }

            }

            /*
             * 方法2 ：扫描法，和方法1 类似，但是不需要创建数组了
             * 时间复杂度： O(M+N)
             * 空间复杂度： O(1)
             */
            public static double FindMedianSortedArrays_02(int[] nums1, int[] nums2)
            {
                int n1 = nums1.Length;
                int n2 = nums2.Length;

                int len = n1 + n2;

                int left = -1, right = -1; //中位数的左数，中位数的右数

                int flag1 = 0, flag2 = 0;  // 遍历nums1,nums2数组的下标指针

                for (int i = 0; i <= len / 2; i++)    // 需要经过 (n1+n2)/2 + 1的遍历
                {
                    left = right;       // 每次遍历开始，都是默认左数等于右数
                    if (flag1 < n1 &&   // 如果 flag1下标未出界
                        (flag2 >= n2 || nums1[flag1] < nums2[flag2]))   // 同时flag2下标出界 或者此时 下标1的数 < 下标2的数 
                    {
                        right = nums1[flag1++];  //左数为下标数1 同时下标1右移一位
                    }
                    else
                    {
                        right = nums2[flag2++];  //左数为下标数2 同时下标2右移一位
                    }
                }

                if (len % 2 == 0)
                {
                    return (left + right) / 2.0;
                }
                else
                {
                    return right;
                }

            }

            /*
             * https://leetcode.cn/problems/median-of-two-sorted-arrays/solutions/8999/xiang-xi-tong-su-de-si-lu-fen-xi-duo-jie-fa-by-w-2/
             * 方法3： 求第k小数的特殊形式，通过二分的方式
             * 时间复杂度：O(log(m+n))
             * 空间复杂度：O(1)
             */
            public static double FindMedianSortedArrays_03(int[] nums1, int[] nums2)
            {
                int n1 = nums1.Length;
                int n2 = nums2.Length;

                /*
                 * 当n1+n2
                 * 为奇数，left = right 第left的位置为中位数，即求第left小的数
                 * 为偶数时，left为中位数左值，right为中位数右数，即求 (第left小的数+ 第right小的数) /2
                 */
                int left = (n1 + n2 + 1) / 2;
                int right = (n1 + n2 + 2) / 2;

                // 将偶数和奇数的情况合并，如果是奇数，会求两次同样的 k。
                return (getKthNum(nums1, 0, n1 - 1, nums2, 0, n2 - 1, left)
                        + getKthNum(nums1, 0, n1 - 1, nums2, 0, n2 - 1, right)) / 2.0;

                static int getKthNum(int[] nums1, int start1, int end1,
                                int[] nums2, int start2, int end2,
                                int k)
                {
                    //因为索引和算数不同6-0=6，但是是有7个数的，因为end初始就是数组长度-1构成的。
                    //最后len代表当前数组(也可能是经过递归排除后的数组)，符合当前条件的元素的个数
                    int len1 = end1 - start1 + 1;
                    int len2 = end2 - start2 + 1;

                    // 让 len1 的长度小于 len2，这样就能保证如果有数组空了，一定是 len1 
                    // 就是如果len1长度小于len2，把getKth()中参数互换位置，即原来的len2就变成了len1，即len1，永远比len2小
                    if (len1 > len2)
                        return getKthNum(nums2, start2, end2, nums1, start1, end1, k);

                    //如果一个数组中没有了元素，那么即从剩余数组nums2的其实start2开始加k再-1.
                    //因为k代表个数，而不是索引，那么从nums2后再找k个数，那个就是start2 + k-1索引处就行了。
                    //因为还包含nums2[start2]也是一个数。因为它在上次迭代时并没有被排除
                    if (len1 == 0)
                        return nums2[start2 + k - 1];

                    //如果k=1，表明最接近中位数了，即两个数组中start索引处，谁的值小，中位数就是谁
                    //(start索引之前表示经过迭代已经被排出的不合格的元素，
                    //即数组没被抛弃的逻辑上的范围是nums[start]--->nums[end])。
                    if (k == 1)
                        return Math.Min(nums1[start1], nums2[start2]);

                    //为了防止数组长度小于 k/2,每次比较都会从当前数组所生长度和k/2作比较，取其中的小的(如果取大的，数组就会越界)
                    //然后素组如果len1小于k / 2，表示数组经过下一次遍历就会到末尾，然后后面就会在那个剩余的数组中寻找中位数
                    int flag1 = start1 + Math.Min(len1, k / 2) - 1;
                    int flag2 = start2 + Math.Min(len2, k / 2) - 1;

                    //如果nums1[flag1] > nums2[flag2]，表示nums2数组中包含flag2索引，之前的元素，逻辑上全部淘汰，即下次从flag2+1开始。
                    //而k则变为k - (flag2 - start2 + 1)，即减去逻辑上排出的元素的个数(要加1，因为索引相减，相对于实际排除的时要少一个的)
                    if (nums1[flag1] > nums2[flag2])
                    {
                        return getKthNum(nums1, start1, end1, nums2, flag2 + 1, end2,
                                k - (flag2 - start2 + 1));
                    }
                    else
                    {
                        return getKthNum(nums1, flag1 + 1, end1, nums2, start2, end2,
                                k - (flag1 - start1 + 1));
                    }

                }

            }

            /*
             * 方法4：中位数特值解法
             * 时间复杂度：O(log（min（m，n））
             * 空间复杂度：O(1)
             * 思路的就是切割区域 =》左半部分的长度等于右半部分
             */
            public static double FindMedianSortedArrays_04(int[] nums1, int[] nums2)
            {
                int n1 = nums1.Length;
                int n2 = nums2.Length;

                // n2 永远大于 n1
                if (n1 > n2)
                {
                    return FindMedianSortedArrays_04(nums2, nums1);
                }

                int iMin = 0, iMax = n1;
                while (iMin <= iMax)    // 最终iMin > iMax
                {
                    int flag1 = (iMin + iMax) / 2;
                    int flag2 = (n1 + n2 + 1) / 2 - flag1;

                    if (flag2 != 0 && flag1 != n1
                            && nums2[flag2 - 1] > nums1[flag1])   //iMin增大 => flag1增大
                    {
                        iMin = flag1 + 1;
                    }
                    else if (flag1 != 0 && flag2 != n2
                            && nums1[flag1 - 1] > nums2[flag2])   //iMax减小 => flag1减小
                    {
                        iMax = flag1 - 1;
                    }
                    else // 达到要求，并且将边界条件列出来单独考虑 中位数就可以表示 =》（左半部分最大值 + 右半部分最小值 ）/ 2。
                    {
                        int maxLeft = 0;
                        if (flag1 == 0)
                        {
                            maxLeft = nums2[flag2 - 1];
                        }
                        else if (flag2 == 0)
                        {
                            maxLeft = nums1[flag1 - 1];
                        }
                        else
                        {
                            maxLeft = Math.Max(nums1[flag1 - 1], nums2[flag2 - 1]);
                        }

                        if ((n1+n2) % 2 == 1)
                        {
                            return maxLeft; // 奇数的话不需要考虑右半部分
                        }

                        int minRight = 0;
                        if ( flag1 == n1)
                        {
                            minRight = nums2[flag2];
                        }
                        else if(flag2 == n2)
                        {
                            minRight = nums1[flag1];
                        }
                        else
                        {
                            minRight = Math.Min(nums1[flag1], nums2[flag2]);
                        }

                        return (minRight + maxLeft) / 2.0;

                    }
                }

                return default;
            }
        }

        public static void leetcode_04()
        {
            int[] n1 = {2 };
            int[] n2 = {  };

            var vs = Solution.FindMedianSortedArrays_04(n1, n2);
            WriteLine(vs);
        }
    }
}
