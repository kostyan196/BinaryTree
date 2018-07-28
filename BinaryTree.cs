using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable
    {
        class Node
        {
            public T Value { get; set; }
            public Node Parent { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public int LeftSize { get; set; }
            public Node(T value)
            {
                Value = value;
            }
        }

        Node Root { get; set; }
        public int Count { get; private set; }

        public T this[int index]
        {
            get { return GetIndex(index); }
        }

        private T GetIndex(int index)
        {
            if (Root == null || index > Count - 1 || index < 0)
                throw new IndexOutOfRangeException();
            if (Root.LeftSize == index) return Root.Value;
            return GetIndex(index, Root);
        }

        private T GetIndex(int index, Node root)
        {
            int tempIndex = 0;
            while (true)
            {
                if (root.LeftSize + tempIndex > index)
                {
                    root = root.Left;
                }
                else if (tempIndex + root.LeftSize == index)
                    return root.Value;
                else
                {
                    tempIndex += root.LeftSize + 1;
                    root = root.Right;
                }
            }
        }

        public void Add(T value)
        {
            if (Root == null)
            {
                Root = new Node(value);
                Count++;
                return;
            }
            Add(value, Root);
        }

        private void Add(T value, Node root)
        {
            while (true)
            {
                if (value.CompareTo(root.Value) < 0)
                {
                    root.LeftSize++;
                    if (root.Left == null)
                    {
                        var tempNode = new Node(value);
                        tempNode.Parent = root;
                        root.Left = tempNode;
                        Count++;
                        return;
                    }
                    root = root.Left;
                }
                else
                {
                    if (root.Right == null)
                    {
                        var tempNode = new Node(value);
                        tempNode.Parent = root;
                        root.Right = tempNode;
                        Count++;
                        return;
                    }
                    root = root.Right;
                }
            }
        }

        public bool Contains(T value)
        {
            if (Root == null) return false;
            if (value.CompareTo(Root.Value) == 0)
                return true;
            return Contains(value, Root);
        }

        private bool Contains(T value, Node root)
        {
            while (true)
            {
                if (value.CompareTo(root.Value) == 0)
                    return true;
                else if (value.CompareTo(root.Value) < 0)
                {
                    if (root.Left == null)
                        return false;
                    root = root.Left;
                }
                else
                {
                    if (root.Right == null)
                        return false;
                    root = root.Right;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Root == null) return null;
            return InOrder(Root).GetEnumerator();
        }

        private IEnumerable<T> InOrder(Node root)
        {
            int count = 0;
            var visited = new HashSet<Node>();
            while (count != Count)
            {
                while (root.Left != null && !visited.Contains(root.Left))
                    root = root.Left;
                if (!visited.Contains(root))
                {
                    yield return root.Value;
                    visited.Add(root);
                    count++;
                }

                if (root.Right != null && !visited.Contains(root.Right))
                    root = root.Right;
                else if (root.Parent != null) root = root.Parent;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
