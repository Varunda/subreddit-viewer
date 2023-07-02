namespace subreddit.Models {

    public class RedditComment {

        public string ID { get; set; } = "";

        public string LinkID { get; set; } = "";

        public string ParentID { get; set; } = "";

        public DateTime PostedAt { get; set; }

        public string Author { get; set; } = "";

        public string Content { get; set; } = "";

    }
}
