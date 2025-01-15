using UnityEngine;

namespace Editor.Helpers {
    /// <summary>
    /// A Balanced BST (AVL-style) with subtree sizes for order-statistic queries:
    /// - Insert in O(log n)
    /// - Remove in O(log n)
    /// - GetMin, GetMax, and GetQuantile in O(log n)
    /// </summary>
    public class OrderStatisticTree {
        private Node _root;

        /// <summary>
        /// Returns how many elements are currently in the tree.
        /// </summary>
        public int Count { get; private set; }

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
        /// Returns the minimum element in O(log n) (or O(1) if you store a pointer).
        /// If the tree is empty, returns 0.
        /// </summary>
        public float GetMin() {
            if (_root == null) return 0f;
            var node = _root;
            while (node.Left != null) node = node.Left;
            return node.Value;
        }

        /// <summary>
        /// Returns the maximum element in O(log n) (or O(1) if you store a pointer).
        /// If the tree is empty, returns 0.
        /// </summary>
        public float GetMax() {
            if (_root == null) return 0f;
            var node = _root;
            while (node.Right != null) node = node.Right;
            return node.Value;
        }

        /// <summary>
        /// Gets an approximate quantile in the range [0..1].
        /// For example, 0.5 -> median, 0.25 -> Q1, 0.75 -> Q3, etc.
        /// If the tree is empty, returns 0.
        /// </summary>
        /// <param name="q">Quantile in [0..1]</param>
        public float GetQuantile(float q) {
            if (_root == null || Count == 0) return 0f;
            // We do a 0-based rank, so rank = floor(q * (Count - 1))
            int rank = Mathf.FloorToInt(q * (Count - 1));
            return GetByRank(_root, rank);
        }

        // --------------------- Internal AVL-Like Implementation ---------------------

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

            if (node != null) {
                UpdateNode(node);
                node = Balance(node);
            }

            return node;
        }

        private Node FindMin(Node node) {
            while (node.Left != null) node = node.Left;
            return node;
        }

        // Returns k-th the smallest element (0-based)
        private float GetByRank(Node node, int rank) {
            if (node == null) return 0f;

            int leftSize = SizeOf(node.Left);
            if (rank < leftSize) {
                return GetByRank(node.Left, rank);
            }
            else if (rank == leftSize) {
                return node.Value;
            }
            else {
                return GetByRank(node.Right, rank - leftSize - 1);
            }
        }

        // Recompute height and subtree size
        private void UpdateNode(Node node) {
            node.Height = 1 + Mathf.Max(HeightOf(node.Left), HeightOf(node.Right));
            node.SubtreeSize = 1 + SizeOf(node.Left) + SizeOf(node.Right);
        }

        // Balances the node according to the AVL balance factor
        private Node Balance(Node node) {
            int balanceFactor = HeightOf(node.Left) - HeightOf(node.Right);

            // Left heavy
            if (balanceFactor > 1) {
                // If left subtree is right heavy, rotate left first
                if (HeightOf(node.Left.Right) > HeightOf(node.Left.Left)) {
                    node.Left = RotateLeft(node.Left);
                    UpdateNode(node.Left);
                }

                node = RotateRight(node);
            }
            // Right heavy
            else if (balanceFactor < -1) {
                // If right subtree is left heavy, rotate right first
                if (HeightOf(node.Right.Left) > HeightOf(node.Right.Right)) {
                    node.Right = RotateRight(node.Right);
                    UpdateNode(node.Right);
                }

                node = RotateLeft(node);
            }

            UpdateNode(node);
            return node;
        }

        private Node RotateLeft(Node node) {
            var r = node.Right;
            node.Right = r.Left;
            r.Left = node;
            UpdateNode(node);
            UpdateNode(r);
            return r;
        }

        private Node RotateRight(Node node) {
            var l = node.Left;
            node.Left = l.Right;
            l.Right = node;
            UpdateNode(node);
            UpdateNode(l);
            return l;
        }

        private int HeightOf(Node node) => node?.Height ?? 0;
        private int SizeOf(Node node) => node?.SubtreeSize ?? 0;

        // Basic Node structure
        private class Node {
            public float Value;
            public Node Left;
            public Node Right;
            public int Height;
            public int SubtreeSize; // # of nodes in this subtree

            public Node(float value) {
                Value = value;
                Height = 1;
                SubtreeSize = 1;
            }
        }
    }
}
