using Microsoft.VisualStudio.TestTools.UnitTesting;
using subreddit.Models;
using System.Collections.Generic;

namespace subreddit.Test {

    [TestClass]
    public class CommentTreeTests {

        [TestMethod]
        public void Test_GetChild() {
            CommentTree head = new() { Root = _Create("1") };
            CommentTree c1a = new() { Root = _Create("2") };
            CommentTree c1b = new() { Root = _Create("3") };
            head.Children.Add(c1a);
            head.Children.Add(c1b);

            CommentTree c2a = new() { Root = _Create("4") };
            CommentTree c2b = new() { Root = _Create("5") };
            c1a.Children.Add(c2a);
            c1a.Children.Add(c2b);

            CommentTree? five = head.GetChild("5");
            Assert.IsNotNull(five);
            Assert.IsTrue(five.Root.Comment.ID == "5");

            CommentTree? three = head.GetChild("3");
            Assert.IsNotNull(three);
            Assert.AreEqual(three.Root.Comment.ID, "3");
        }

        [TestMethod]
        public void Test_GetList() {
            CommentTree head = new() { Root = _Create("1") };
            CommentTree c1a = new() { Root = _Create("2") };
            CommentTree c1b = new() { Root = _Create("3") };
            head.Children.Add(c1a);
            head.Children.Add(c1b);

            CommentTree c2a = new() { Root = _Create("4") };
            CommentTree c2b = new() { Root = _Create("5") };
            c1a.Children.Add(c2a);
            c1a.Children.Add(c2b);

            List<ViewComment> list = head.GetList();

            Assert.IsTrue(list.Count == 5);
            Assert.AreEqual(list[0].Comment.ID, "1");
            Assert.AreEqual(list[1].Comment.ID, "2");
            Assert.AreEqual(list[2].Comment.ID, "4");
            Assert.AreEqual(list[3].Comment.ID, "5");
            Assert.AreEqual(list[4].Comment.ID, "3");
        }

        public ViewComment _Create(string ID) {
            return new ViewComment() {
                Comment = new RedditComment() {
                    ID = ID
                }
            };
        }

    }
}