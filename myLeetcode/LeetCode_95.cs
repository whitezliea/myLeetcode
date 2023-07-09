using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

namespace myLeetcode
{

    /*
     * https://leetcode.cn/problems/unique-binary-search-trees-ii/
     * 不同的二叉搜索树2
     * 二分搜索树（英语：Binary Search Tree），也称为 二叉查找树 、二叉搜索树 、有序二叉树或排序二叉树。满足以下几个条件：
     * 若它的左子树不为空，左子树上所有节点的值都小于它的根节点。
     * 若它的右子树不为空，右子树上所有的节点的值都大于它的根节点。   
     * 它的左、右子树也都是二分搜索树。
     */
    internal class LeetCode_95
    {
        public class TreeNode
        {
            public int val;
            public TreeNode left;
            public TreeNode right;
            public TreeNode(int val = 0, TreeNode left = null!, TreeNode right = null!)
            {
                this.val = val;
                this.left = left;
                this.right = right;
            }
        }

        public static class Solution
        {
            /*
             * 解法1：回溯法
             * 二叉搜索树关键的性质是根节点的值大于左子树所有节点的值，小于右子树所有节点的值，且左子树和右子树也同样为二叉搜索树
             */
            public static IList<TreeNode> GenerateTrees_01(int n)   // 缺点：会有很多重复性操作，同时也不好理解
            {


                if (n == 0) return default!;

                return generateTrees(1, n);

                IList<TreeNode> generateTrees(int start, int end)
                {
                    IList<TreeNode> list = new List<TreeNode>();
                    if (start > end)
                    {
                        list.Add(null!);
                        return list;
                    }

                    // 枚举可行的根节点                                                                           l     v    r
                    for (int i = start; i <= end; i++)  // 这里已经进行排序了，从小到大，因此可以划分左子树区与右子树区 1 2 3 4 5 6 7
                    {
                        //获取所有可行的左子树集合
                        IList<TreeNode> leftTree = generateTrees(start, i - 1);

                        //获取所有可行的右子树集合
                        IList<TreeNode> rightTree = generateTrees(i + 1, end);

                        // 从左子树集合中选出一棵左子树，从右子树集合中选出一棵右子树，拼接到根节点上
                        foreach (var left in leftTree)
                        {
                            foreach (var right in rightTree)
                            {
                                TreeNode currTree = new TreeNode(i);
                                currTree.left = left;
                                currTree.right = right;
                                list.Add(currTree);
                            }
                        }
                    }
                    return list;
                }


            }

            // 记忆化存储
            public static IList<TreeNode> GenerateTrees_02(int n)
            {
                Dictionary<Tuple<int, int>, IList<TreeNode>> dicts = new();
                Tuple<int, int> key = new Tuple<int, int>(1, n);
                if (n == 0) return default!;
                GenerateTree(1, n);
                return dicts[key];

                IList<TreeNode> GenerateTree(int start, int end)
                {
                    IList<TreeNode> list = new List<TreeNode>();
                    if (start > end)
                    {
                        list.Add(null!);
                        return list;
                    }

                    Tuple<int, int> curKey = new Tuple<int, int>(start, end);

                    if (dicts.ContainsKey(curKey) != false)
                    {
                        return dicts[curKey];
                    }

                    for (int i = start; i <= end; i++)
                    {
                        IList<TreeNode> leftTree = GenerateTree(start, i - 1);
                        IList<TreeNode> rightTree = GenerateTree(i + 1, end);

                        foreach (var left in leftTree)
                        {
                            foreach (var right in rightTree)
                            {
                                list.Add(new TreeNode(i, left, right));
                            }
                        }
                    }

                    return dicts[curKey] = list;    // 如果没有遇到过的情况就加入记忆map里
                }
            }


            // 动态规划
            /*
             *  dp数组的含义：dp[i]dp[i]dp[i]表示序列[1,2,3,...,i]能够形成的所有不同BST。
                basecase：i=0 时为空树，i=1 时为只有一个根节点1的树
                状态转移：为了得到dp[i]，我们在区间[1,i]枚举所有的j，以j作为BST的根节点，
                        可以得到左边序列的所有二叉搜索树dp[j−1]、j右边序列的所有平衡二叉树dp[i−j]
            //https://leetcode.cn/problems/unique-binary-search-trees-ii/solutions/11327/xiang-xi-tong-su-de-si-lu-fen-xi-duo-jie-fa-by-2-7/
             */
            public static IList<TreeNode> GenerateTrees_03(int n)
            {
                IList<TreeNode>[] dp = new IList<TreeNode>[n + 1];
                dp[0] = new List<TreeNode>();
                dp[0]?.Add(null!);

                TreeNode TreeBias(TreeNode root, int offset)
                {
                    if (root == null) return null!;

                    TreeNode curNode = new TreeNode(root.val + offset);
                    curNode.left = TreeBias(root.left, offset);
                    curNode.right = TreeBias(root.right, offset);
                    return curNode;
                }

                for (int i = 1; i <= n; i++)
                {
                    dp[i] = new List<TreeNode>();
                    //将不同的数字作为根节点，只需要考虑到 i
                    for (int root = 1; root <= i; root++)
                    {
                        int left = root - 1;    //左子树的长度
                        int right = i - root;   //右子树的长度

                        foreach (TreeNode leftTree in dp[left])
                        {
                            foreach (TreeNode rightTree in dp[right])
                            {
                                TreeNode nowTree = new TreeNode(root);
                                nowTree.left = leftTree;
                                nowTree.right = TreeBias(rightTree, root);   //克隆右子树并且加上偏差
                                dp[i].Add(nowTree);
                            }
                        }

                    }
                }

                return dp[n];

            }

        }

        public static void leetcode_95() //前序打印二叉树
        {
            var list = Solution.GenerateTrees_03(3);

            foreach (var treeroot in list)
            {

                InorderPrint(treeroot);
                WriteLine();
            }
            static void InorderPrint(TreeNode root)
            {
                // 如果当前节点为空，直接返回
                if (root == null)
                {
                    //Write("null ");
                    return;
                }

                // 打印当前节点的值
                Console.Write(root.val + " ");
                // 递归地打印左子树
                InorderPrint(root.left);

                // 递归地打印右子树
                InorderPrint(root.right);
            }
        }
    }
}
