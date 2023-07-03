namespace subreddit.Code {

    public class QueryParser {

        /// <summary>
        ///     Break a query into the terms used. Removes some punctuation and single letter terms
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static List<string> Parse(string q) {
            return q.Replace(",", "").Replace(".", "").Replace("'", "")
                .ToLower().Split(" ").Where(iter => iter.Length > 1).ToList();
        }

    }
}
