namespace myLeetcode
{
    /*
     * https://leetcode.cn/problems/binary-tree-inorder-traversal/description/
     * 关键词：二叉树，中序排序
     */
    internal class LeetCode_94
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
            static IList<int> list = new List<int>();
            public static void Inorder_traversal(TreeNode root) //中序遍历，遍历顺序为左，根，右
            {
                if (root == null) return;
                Inorder_traversal(root.left);
                list.Add(root.val);
                Inorder_traversal(root.right);
            }
            public static void Preorder_traversal(TreeNode root) //前序遍历，遍历顺序为根，左，右
            {
                if (root == null) return;
                list.Add(root.val);
                Preorder_traversal(root.left);
                Preorder_traversal(root.right);
            }
            public static void PostOrder_traversal(TreeNode root) //后序遍历，遍历顺序为左，右，根
            {
                if (root == null) return;
                PostOrder_traversal(root.left);
                PostOrder_traversal(root.right);
                list.Add(root.val);
            }

            public static IList<int> InorderTraversal(TreeNode root)
            {
                list.Clear();
                Inorder_traversal(root);
                return list;
            }
        }

        static TreeNode BuildTree(ref List<object> list, ref int index) // 前序遍历创建二叉树
        {
            if (index >= list.Count || list[index] == null) // 如果索引超出范围或者遇到null，表示空节点，返回null
            {
                index++; // 索引加一
                return null!;
            }

            TreeNode root = new TreeNode((int)list[index]); // 创建根节点
            index++; // 索引加一
            root.left = BuildTree(ref list, ref index); // 递归创建左子树
            root.right = BuildTree(ref list, ref index); // 递归创建右子树
            return root; // 返回根节点
        }
        static TreeNode CreateTree(ref List<object> list)
        {
            int index = 0; // 初始索引值为0
            return BuildTree(ref list, ref index); // 调用辅助函数，返回二叉树的根节点
        }

        public static void leetcode_94()
        {
            List<object> digits = new List<object> { 1, null!, 2, 3 };
            TreeNode root = CreateTree(ref digits);
            IList<int> list = new List<int>();

            //list = Solution.InorderTraversal(root);
            list = Extension_Solution.General_iteration_Inorder(root);
            Console.WriteLine("中序遍历结果为：");
            foreach (var item in list)
            {
                Write(item + " ");
            }
        }



        public static class Extension_Solution
        {
            public static IList<int> General_iteration_Inorder(TreeNode root) //普通迭代运算-中序
            {
                /*
                 * https://leetcode.cn/problems/binary-tree-inorder-traversal/solutions/34581/die-dai-fa-by-jason-2/
                 * 思路：每到一个节点 A，因为根的访问在中间，将 A 入栈。
                 *      然后遍历左子树，接着访问 A，最后遍历右子树。
                 *      在访问完 A 后，A 就可以出栈了。因为 A 和其左子树都已经访问完成。
                 */

                Stack<TreeNode> st = new Stack<TreeNode>();
                IList<int> nlist = new List<int>();

                if (root == null) return nlist;
                TreeNode rt = root;
                while ((rt !=null) || st.Count > 0)
                {
                    while (rt != null)
                    {
                        st.Push(rt);
                        rt = rt.left;
                    }

                    rt = st.Pop();
                    nlist.Add(rt.val);
                    rt = rt.right;
                }
               
                return nlist;
            }

            public static IList<int> General_iteration_PreOrder(TreeNode root)  //迭代运算-前序
            {
                /*
                 * 递归思路：先树根，然后左子树，然后右子树。每棵子树递归。
                            在迭代算法中，思路演变成，每到一个节点 A，就应该立即访问它。
                            因为，每棵子树都先访问其根节点。对节点的左右子树来说，也一定是先访问根。
                            在 A 的两棵子树中，遍历完左子树后，再遍历右子树。
                            因此，在访问完根节点后，遍历左子树前，要将右子树压入栈。
                 */

                Stack<TreeNode> st = new Stack<TreeNode>();
                IList<int> nlist = new List<int>();

                if (root == null) return nlist;
                TreeNode rt = root;
                while(rt != null || st.Count>0) 
                {
                    while (rt != null )
                    {
                        st.Push(rt.right);
                        nlist.Add(rt.val);
                        rt = rt.left;
                    }
                    rt = st.Pop();
                }

                return nlist;
            }


            public static IList<int> General_iteration_PostOrder(TreeNode root) //迭代运算 后序遍历
            {

                /* 
                 * 方法1：
                 * 可以用与前序遍历相似的方法完成后序遍历。
                    后序遍历与前序遍历相对称。
                    思路： 每到一个节点 A，就应该立即访问它。 然后将左子树压入栈，再次遍历右子树。
                    遍历完整棵树后，结果序列逆序即可。
                 */
                IList<int> nlist1 = new List<int>();
                static void function1(TreeNode root,ref IList<int> nlist1)
                {
                    Stack<TreeNode> st1 = new Stack<TreeNode>();
                    TreeNode rt = root;
                    while (rt != null || st1.Count > 0)
                    {
                        while(rt != null )
                        {
                            st1.Push(rt.left);
                            nlist1.Add(rt.val);
                            rt = rt.right;
                        }
                        rt = st1.Pop();
                    }

                    st1.Reverse();
                }
                function1(root,ref nlist1);


                /*
                 * 按照左子树-根-右子树的方式，将其转换成迭代方式。
                    思路：每到一个节点 A，因为根要最后访问，将其入栈。然后遍历左子树，遍历右子树，最后返回到 A。
                    但是出现一个问题，无法区分是从左子树返回，还是从右子树返回。
                    因此，给 A 节点附加一个标记T。在访问其右子树前，T 置为 True。之后子树返回时，当 T 为 True表示从右子树返回，否则从左子树返回。
                    当 T 为 false 时，表示 A 的左子树遍历完，还要访问右子树。
                    同时，当 T 为 True 时，表示 A 的两棵子树都遍历过了，要访问 A 了。并且在 A 访问完后，A 这棵子树都访问完成了。
                 */
                IList<int> nlist2 = new List<int>();
                static void function2(TreeNode root, ref IList<int> nlist2)
                {
                    Dictionary<TreeNode,int?> dicts = new Dictionary<TreeNode,int?>();
                    Stack<TreeNode> st2 = new Stack<TreeNode>();
                    TreeNode rt = root;
                    while (rt != null || st2.Count > 0)
                    {
                        while (rt != null )     // 遍历左子树
                        {
                            st2.Push(rt);
                            rt = rt.left;
                        }

                        while (st2.Count > 0 && dicts[st2.Peek()] != null)
                        {
                            nlist2.Add(st2.Pop().val);
                        }

                        if (st2.Count > 0)
                        {
                            rt = st2.Peek().right;
                            dicts[st2.Peek()] = 1;
                        }
                    }

                }
                function2(root,ref nlist2);

                return nlist1;
                //return nlist2;
            }
        }
    }
}
