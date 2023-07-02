namespace subreddit.Code {

    public class QueryParser {

        public static List<string> Parse(string q) {
            return q.Replace(",", "").Replace(".", "").Replace("'", "")
                .ToLower().Split(" ").Where(iter => iter.Length > 1).ToList();
        }

    }
}
