using Microsoft.AspNetCore.Mvc;
using subreddit.Code;
using subreddit.Models;
using subreddit.Services.Db;

namespace subreddit.Controllers {

    public class HomeController : Controller {

        private readonly ILogger<HomeController> _Logger;
        private readonly SearchDb _SearchDb;
        private readonly PostDb _PostDb;
        private readonly CommentDb _CommentDb;

        public HomeController(ILogger<HomeController> logger,
            SearchDb searchDb, PostDb postDb, CommentDb commentDb) {

            _Logger = logger;

            _SearchDb = searchDb;
            _PostDb = postDb;
            _CommentDb = commentDb;
        }

        public IActionResult Index() {
            return View();
        }

        /// <summary>
        ///     Search posts for keywords
        /// </summary>
        /// <param name="q">Keywords to search by</param>
        /// <param name="o">Offset into the results to search</param>
        /// <returns></returns>
        public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int o = 0) {
            if (string.IsNullOrEmpty(q)) {
                return BadRequest("missing query");
            }

            if (o < 0) {
                return BadRequest("cannot have a negative offset");
            }

            _Logger.LogInformation($"searching for '{q}' offset {o}");
            try {
                List<string> terms = QueryParser.Parse(q);

                List<SearchResult> results = await _SearchDb.GetByTerms(terms, o, CancellationToken.None);

                ViewSearchResults model = new();
                model.Query = q;
                model.Terms = terms;
                model.Results = results;
                model.Offset = o;

                return View(model);
            } catch (Exception ex) {
                _Logger.LogError(ex, $"exception in query for '{q}'");
                return StatusCode(500, $"Exception while querying '{q}': {ex.Message}");
            }
        }

        public async Task<IActionResult> Author([FromQuery] string u) {
            if (string.IsNullOrEmpty(u)) {
                return BadRequest("no user provided");
            }

            _Logger.LogInformation($"getting submissions//comments from {u}");

            try {
                List<SearchResult> results = await _SearchDb.GetByAuthor(u, CancellationToken.None);

                ViewSearchResults model = new();
                model.Query = $"";
                model.Results = results;
                model.Offset = 0;

                return View("Search", model);
            } catch (Exception ex) {
                _Logger.LogError(ex, $"exception while getting content for {u}");
                return StatusCode(500, $"Exception while getting content for {u}: {ex.Message}");
            }
        }

        public async Task<IActionResult> Post(string id) {
            RedditPost? post = await _PostDb.GetByID(id);
            _Logger.LogInformation($"View post {id}: {post?.Title}");

            if (post == null) {
                return NotFound($"Failed to find reddit post {id}");
            }

            List<RedditComment> comments = await _CommentDb.GetByPostID(post.ID);
            List<ViewComment> view = new(comments.Count);

            foreach (RedditComment c in comments) {
                view.Add(new ViewComment() {
                    Depth = -1,
                    Comment = c
                });
            }
            Dictionary<string, ViewComment> cmts = view.ToDictionary(iter => iter.Comment.ID);

            int failsafeBreak = 100;

            CommentTree head = new();

            // create the comment tree
            while (failsafeBreak-- > 0) {
                bool needsReiter = false;

                foreach (ViewComment v in view) {
                    if (v.Depth != -1) {
                        continue;
                    }

                    needsReiter = true;

                    // t3_ means a thread
                    // t1_ means a comment
                    if (v.Comment.ParentID.StartsWith("t3_")) {
                        v.Depth = 0;
                        head.AddChild(new CommentTree() {
                            Root = v
                        });
                    } else if (v.Comment.ParentID.StartsWith("t1_")) {
                        string parentID = v.Comment.ParentID[3..];
                        if (cmts.TryGetValue(parentID, out ViewComment? parent) == false) {
                            _Logger.LogError($"missing comment {parentID} while loading post {id}");
                            continue;
                        }

                        if (parent.Depth != -1) {
                            v.Depth = parent.Depth + 1;

                            CommentTree? parentNode = head.GetChild(parentID);
                            if (parentNode == null) {
                                _Logger.LogError($"missing comment tree node {parentID} while loading post {id}");
                                continue;
                            }

                            parentNode.AddChild(new CommentTree() {
                                Root = v
                            });
                        }
                    }
                }

                if (needsReiter == false) {
                    break;
                }
            }

            List<ViewComment> list = head.GetList();

            ViewPost model = new();
            model.Post = post;
            model.Comments = head;

            return View(model);
        }

        public IActionResult Link(string l) {
            string? postID = subreddit.Code.LinkParser.GetPostID(l);

            if (postID == null) {
                return BadRequest($"failed to parse '{l}' to a valid post ID");
            }

            return RedirectToAction("Post", new { id = postID });
        }

    }
}
