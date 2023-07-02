namespace subreddit.Code {

    public class LinkParser {

        public static string? GetPostID(string url) {
            string[] parts = url.Split("/");

            if (parts.Length >= 7) {
                string action = parts[5];
                string postID = parts[6];

                if (action.ToLower() == "comments") {
                    return postID;
                }
            }

            return null;
        }

    }
}
