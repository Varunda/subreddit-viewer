﻿namespace subreddit.Models {

    public class CommentTree {

        public ViewComment Root { get; set; } = new();

        private List<CommentTree> Children { get; set; } = new();

        public void AddChild(CommentTree tree) {
            this.Children.Add(tree);
            this.Children.Sort((a, b) => {
                return a.Root.Comment.PostedAt.CompareTo(b.Root.Comment.PostedAt);
            });
        }

        /// <summary>
        ///     Get a node within a tree
        /// </summary>
        /// <param name="commentID">ID of the comment to get</param>
        public CommentTree? GetChild(string commentID) {
            foreach (CommentTree tree in Children) {
                if (tree.Root.Comment.ID == commentID) {
                    return tree;
                }

                CommentTree? child = tree.GetChild(commentID);
                if (child != null) {
                    return child;
                }
            }

            return null;
        }

        /// <summary>
        ///     Get the roots of each node within a tree using a depth first iteration
        /// </summary>
        /// <returns></returns>
        public List<ViewComment> GetList() {
            List<ViewComment> tree = new();
            tree.Add(Root);

            foreach (CommentTree child in Children) {
                List<ViewComment> childList = child.GetList();
                tree.AddRange(childList);
            }

            return tree;
        }

    }

}
