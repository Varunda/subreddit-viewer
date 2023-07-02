namespace subreddit.Models {

    public class ViewSearchResults {

        public string Query { get; set; } = "";

        public List<string> Terms { get; set; } = new();

        public int Offset { get; set; }

        public List<SearchResult> Results { get; set; } = new();

    }
}
