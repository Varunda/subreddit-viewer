namespace subreddit.Models {

    public class SearchResult {

        /// <summary>
        ///     ID of the post this result is for
        /// </summary>
        public string PostID { get; set; } = "";

        /// <summary>
        ///     ID of either the submission or the comment
        /// </summary>
        public string ID { get; set; } = "";

        /// <summary>
        ///     What type this result is for. Either "post" or "comment"
        /// </summary>
        public string Type { get; set; } = "";

        /// <summary>
        ///     Who posted this submission or comment
        /// </summary>
        public string Author { get; set; } = "";

        /// <summary>
        ///     Title of the submission, or title of the submission this comment is for
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        ///     When this submission/comment was posted
        /// </summary>
        public DateTime PostedAt { get; set; }

        /// <summary>
        ///     The content of this submission/comment
        /// </summary>
        public string Content { get; set; } = "";

        public int Score { get; set; }

    }
}
