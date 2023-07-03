namespace subreddit.Models {

    /// <summary>
    ///     Represents data about a single reddit comment
    /// </summary>
    public class RedditComment {

        /// <summary>
        ///     Unique ID of the comment
        /// </summary>
        public string ID { get; set; } = "";

        /// <summary>
        ///     ID to the <see cref="RedditPost"/> this comment is for. This includes the leading t3_
        /// </summary>
        public string LinkID { get; set; } = "";

        /// <summary>
        ///     Parent of the comment. If it starts with t3_, it means the parent is another comment.
        ///     If t1_, it means the parent is a post (aka, this is a top level comment)
        /// </summary>
        public string ParentID { get; set; } = "";

        /// <summary>
        ///     When (in UTC) this comment was made
        /// </summary>
        public DateTime PostedAt { get; set; }

        /// <summary>
        ///     Who made this post. Does not include /u/
        /// </summary>
        public string Author { get; set; } = "";

        /// <summary>
        ///     Content of the comment itself. 0 formatting is done here
        /// </summary>
        public string Content { get; set; } = "";

        /// <summary>
        ///     upvotes - downvotes on this comment
        /// </summary>
        public int Score { get; set; }

    }
}
