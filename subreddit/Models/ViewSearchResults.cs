namespace subreddit.Models {

    /// <summary>
    ///     Model for the view that shows the results of a search
    /// </summary>
    public class ViewSearchResults {

        /// <summary>
        ///     Input query
        /// </summary>
        public string Query { get; set; } = "";

        /// <summary>
        ///     What terms were search for
        /// </summary>
        public List<string> Terms { get; set; } = new();

        /// <summary>
        ///     Offset into the search
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        ///     Results of the search
        /// </summary>
        public List<SearchResult> Results { get; set; } = new();

    }
}
