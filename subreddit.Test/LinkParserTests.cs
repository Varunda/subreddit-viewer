using Microsoft.VisualStudio.TestTools.UnitTesting;
using subreddit.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subreddit.Test {

    [TestClass]
    public class LinkParserTests {

        [TestMethod]
        [DataRow("https://old.reddit.com/r/Planetside/comments/ezmqm3/some_cool_stuff_were_going_to_be_seeing_in/", "ezmqm3")]
        [DataRow("https://www.reddit.com/r/Planetside/comments/145qnan/rplanetside_will_be_going_private_on_june_12th/?utm_source=share&utm_medium=ios_app&utm_name=ioscss&utm_content=2&utm_term=1", "145qnan")]
        public void GetPostID_Test(string url, string postID) {

            string? foundPostID = LinkParser.GetPostID(url);
            Console.WriteLine($"Parsing {postID} from '{url}'");
            Console.WriteLine($"Found {foundPostID}");

            Assert.IsNotNull(foundPostID);
            Assert.AreEqual(foundPostID, postID);
        }

    }
}
