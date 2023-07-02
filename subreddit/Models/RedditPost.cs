namespace subreddit.Models {

    public class RedditPost {

        public string ID { get; set; } = "";

        public string Title { get; set; } = "";

        public DateTime PostedAt { get; set; }

        public string Author { get; set; } = "";

        public string Content { get; set; } = "";

        public string Data { get; set; } = "";

    }
}
