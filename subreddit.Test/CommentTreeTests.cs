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
            head.AddChild(c1a);
            head.AddChild(c1b);

            CommentTree c2a = new() { Root = _Create("4") };
            CommentTree c2b = new() { Root = _Create("5") };
            c1a.AddChild(c2a);
            c1a.AddChild(c2b);

            CommentTree c3a = new() { Root = _Create("6") };
            CommentTree c3b = new() { Root = _Create("7") };
            CommentTree c3c = new() { Root = _Create("8") };
            c1b.AddChild(c3a);
            c1b.AddChild(c3b);
            c1b.AddChild(c3c);

            CommentTree? five = head.GetChild("5");
            Assert.IsNotNull(five);
            Assert.IsTrue(five.Root.Comment.ID == "5");

            CommentTree? three = head.GetChild("3");
            Assert.IsNotNull(three);
            Assert.AreEqual(three.Root.Comment.ID, "3");

            CommentTree? six = head.GetChild("6");
            Assert.IsNotNull(six);
            Assert.AreEqual(six.Root.Comment.ID, "6");
        }

        [TestMethod]
        public void Test_GetList() {
            CommentTree head = new() { Root = _Create("1") };
            CommentTree c1a = new() { Root = _Create("2") };
            CommentTree c1b = new() { Root = _Create("3") };
            head.AddChild(c1a);
            head.AddChild(c1b);

            CommentTree c2a = new() { Root = _Create("4") };
            CommentTree c2b = new() { Root = _Create("5") };
            c1a.AddChild(c2a);
            c1a.AddChild(c2b);

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