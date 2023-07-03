namespace subreddit.Models {

    /// <summary>
    ///     Model that contains all the information needed to view a post
    /// </summary>
    public class ViewPost {

        /// <summary>
        ///     The post itself
        /// </summary>
        public RedditPost Post { get; set; } = new();

        /// <summary>
        ///     The tree of comments 
        /// </summary>
        public CommentTree Comments { get; set; } = new();

    }

    /// <summary>
    ///     Model that contains all the information needed to view a comment
    /// </summary>
    public class ViewComment {

        /// <summary>
        ///     How deep into the comment tree is this comment
        /// </summary>
        public int Depth { get; set; } = 0;

        /// <summary>
        ///     The comment itself
        /// </summary>
        public RedditComment Comment { get; set; } = new();

    }

}
