namespace subreddit.Models {

    public class ViewPost {

        public RedditPost Post { get; set; } = new();

        public CommentTree Comments { get; set; } = new();

    }

    public class ViewComment {

        public int Depth { get; set; } = 0;

        public RedditComment Comment { get; set; } = new();
    }

}
