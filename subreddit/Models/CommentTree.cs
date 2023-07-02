﻿namespace subreddit.Models {

    public class CommentTree {

        public ViewComment Root { get; set; } = new();

        public List<CommentTree> Children { get; set; } = new();

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

        public List<ViewComment> GetList() {
            List<ViewComment> tree = new();
            tree.Add(Root);

            foreach (CommentTree child in Children) {
                List<ViewComment> childList = child.GetList();
                childList.Sort((a, b) => {
                    return a.Comment.PostedAt.CompareTo(b.Comment.PostedAt);
                });
                tree.AddRange(childList);
            }

            return tree;
        }

    }

}