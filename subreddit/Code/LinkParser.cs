namespace subreddit.Code {

    public class LinkParser {

        /// <summary>
        ///     Given a URL get the ID of the post 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
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
