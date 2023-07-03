namespace subreddit.Models {

    /// <summary>
    ///     Represents data about a single reddit post
    /// </summary>
    public class RedditPost {

        /// <summary>
        ///     Unique ID of the post
        /// </summary>
        public string ID { get; set; } = "";

        /// <summary>
        ///     Title of the post
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        ///     When (in UTC) this post was made
        /// </summary>
        public DateTime PostedAt { get; set; }

        /// <summary>
        ///     Author of the post. Does not contain a leading /u/
        /// </summary>
        public string Author { get; set; } = "";

        /// <summary>
        ///     Content of the post itself
        /// </summary>
        public string Content { get; set; } = "";

        /// <summary>
        ///     How many upvotes - downvotes this post has
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        ///     JSON data
        /// </summary>
        public string Data { get; set; } = "";

    }
}
