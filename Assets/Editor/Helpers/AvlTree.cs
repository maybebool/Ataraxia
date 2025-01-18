using UnityEngine;

namespace Editor.Helpers {
    
    public class Node {
        public float Value;
        public Node Left;
        public Node Right;
        public int Height;
        public int SubtreeSize;

        public Node(float value) {
            Value = value;
            Height = 1;
            SubtreeSize = 1;
        }
    }
    // TODO: Ask Andi if its really all log n and if red black tree would make more sense 
    /// <summary>
    /// Balanced BST (AVL-style) :
    /// - Insert in O(log n)
    /// - Remove in O(log n)
    /// - GetMin, GetMax, and GetQuantile in O(log n)
    /// </summary>
    public class AvlTree {
        
        // TODO: Maybe intensive calculations could be done with multi threading. But then we need to create 
        // lists with n Count, resulting in O(n). Ask Andi if this is a good idea
        private Node _root;
        
        /// <summary>
        /// Returns how many elements are currently in the tree.
        /// </summary>
        public int Count { get; private set; }
        private int HeightOf(Node node) => node?.Height ?? 0;
        private int SizeOf(Node node) => node?.SubtreeSize ?? 0;

        /// <summary>
        /// Inserts a new float value into the BST, maintaining balance and subtree sizes.
        /// </summary>
        public void Insert(float value) {
            _root = Insert(_root, value);
            Count++;
        }

        /// <summary>
        /// Removes a float value (first match) from the BST, if it exists.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        public void Remove(float value) {
            if (_root == null) return;
            bool removed;
            _root = Remove(_root, value, out removed);
            if (removed) Count--;
        }

        /// <summary>
        /// Removes all elements from the BST.
        /// </summary>
        public void Clear() {
            _root = null;
            Count = 0;
        }

        /// <summary>
        /// Returns the min element
        /// If the tree is empty, returns 0.
        /// </summary>
        public float GetMin() {
            if (_root == null) return 0f;
            var node = _root;
            while (node.Left != null) node = node.Left;
            return node.Value;
        }

        /// <summary>
        /// Returns the max element
        /// If the tree is empty, returns 0.
        /// </summary>
        public float GetMax() {
            if (_root == null) return 0f;
            var node = _root;
            while (node.Right != null) node = node.Right;
            return node.Value;
        }

        /// <summary>
        /// Gets an approximate quantile in the range [0 to 1].
        /// 0.5 -> median, 0.25 -> Q1, 0.75 -> Q3, etc.
        /// </summary>
        /// <param name="q">Quantile in [0 to 1]</param>
        public float GetQuantile(float q) {
            if (_root == null || Count == 0) return 0f;
            //  0-based rank, so rank = floor(q * (Count - 1))
            var rank = Mathf.FloorToInt(q * (Count - 1));
            return GetByRank(_root, rank);
        }

        /// <summary>
        /// Inserts a new float value into the AVL tree, maintaining balance and subtree sizes.
        /// </summary>
        /// <param name="value">The float value to be inserted into the tree.</param>
        private Node Insert(Node node, float value) {
            if (node == null) return new Node(value);

            if (value < node.Value) {
                node.Left = Insert(node.Left, value);
            }
            else {
                node.Right = Insert(node.Right, value);
            }

            UpdateNode(node);
            return Balance(node);
        }

        /// <summary>
        /// Removes a float value (first match) from the BST, if it exists.
        /// </summary>
        /// <param name="value">The value to be removed from the BST.</param>
        /// <returns>True if the value was successfully removed; otherwise, false.</returns>
        private Node Remove(Node node, float value, out bool removed) {
            removed = false;
            if (node == null) return null;

            if (Mathf.Approximately(value, node.Value)) {
                removed = true;
                // Remove this node
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                // If two children, replace node with in-order successor
                var successor = FindMin(node.Right);
                node.Value = successor.Value;
                node.Right = Remove(node.Right, successor.Value, out _);
            }
            else if (value < node.Value) {
                node.Left = Remove(node.Left, value, out removed);
            }
            else {
                node.Right = Remove(node.Right, value, out removed);
            }

            UpdateNode(node);
            node = Balance(node);

            return node;
        }

        /// <summary>
        /// Finds the node with the smallest value in the subtree rooted at the given node.
        /// </summary>
        /// <param name="node">The root of the subtree to search for the minimum value.</param>
        /// <returns>The node with the smallest value in the subtree, or null if the subtree is empty.</returns>
        private Node FindMin(Node node) {
            while (node.Left != null) node = node.Left;
            return node;
        }
        
        /// <summary>
        /// Gets the k-th smallest element (0-based rank) from the BST.
        /// </summary>
        /// <param name="node">The current node being evaluated in the recursion.</param>
        /// <param name="rank">The 0-based rank of the element to retrieve.</param>
        /// <returns>The value of the k-th smallest element, or 0 if the tree is empty or the rank is out of bounds.</returns>
        private float GetByRank(Node node, int rank) {
            if (node == null) return 0f;

            var leftSize = SizeOf(node.Left);
            if (rank < leftSize) {
                return GetByRank(node.Left, rank);
            }

            return rank == leftSize ? node.Value : GetByRank(node.Right, rank - leftSize - 1);
        }


        /// <summary>
        /// Updates the properties of a given node, including its height and subtree size, based on its child nodes.
        /// </summary>
        /// <param name="node">The node whose properties need to be updated.</param>
        private void UpdateNode(Node node) {
            node.Height = 1 + Mathf.Max(HeightOf(node.Left), HeightOf(node.Right));
            node.SubtreeSize = 1 + SizeOf(node.Left) + SizeOf(node.Right);
        }

        
        /// <summary>
        /// Balances the given node in the AVL tree, ensuring the balance factor is within the allowable range.
        /// standard range +1 or -1
        /// </summary>
        /// <param name="node">The node to balance.</param>
        /// <returns>The balanced node, potentially with its subtree structure adjusted.</returns>
        private Node Balance(Node node) {
            var balanceFactor = HeightOf(node.Left) - HeightOf(node.Right);

            switch (balanceFactor) {
                // Left heavy
                case > 1: {
                    // If left subtree is right heavy, rotate left first
                    if (HeightOf(node.Left.Right) > HeightOf(node.Left.Left)) {
                        node.Left = RotateLeft(node.Left);
                        UpdateNode(node.Left);
                    }

                    node = RotateRight(node);
                    break;
                }
                // Right heavy
                case < -1: {
                    // If right subtree is left heavy, rotate right first
                    if (HeightOf(node.Right.Left) > HeightOf(node.Right.Right)) {
                        node.Right = RotateRight(node.Right);
                        UpdateNode(node.Right);
                    }

                    node = RotateLeft(node);
                    break;
                }
            }

            UpdateNode(node);
            return node;
        }

        /// <summary>
        /// Performs a left rotation on the given node in the AVL tree to maintain balance.
        /// </summary>
        /// <param name="node">The node to perform the left rotation on.</param>
        /// <returns>The new root node after the left rotation.</returns>
        private Node RotateLeft(Node node) {
            var r = node.Right;
            node.Right = r.Left;
            r.Left = node;
            UpdateNode(node);
            UpdateNode(r);
            return r;
        }

        /// <summary>
        /// Performs a right rotation on the given node to balance the AVL tree.
        /// </summary>
        /// <param name="node">The node to perform the right rotation on.</param>
        /// <returns>The new root node after the rotation.</returns>
        private Node RotateRight(Node node) {
            var l = node.Left;
            node.Left = l.Right;
            l.Right = node;
            UpdateNode(node);
            UpdateNode(l);
            return l;
        }
    }
}
