using subreddit.Models;
using subreddit.Services.Db;
using subreddit.Services.Db.Readers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IDbHelper, DbHelper>();
builder.Services.AddSingleton<IDataReader<SearchResult>, SearchDataReader>();
builder.Services.AddSingleton<IDataReader<RedditPost>, RedditPostReader>();
builder.Services.AddSingleton<IDataReader<RedditComment>, RedditCommentReader>();
builder.Services.AddSingleton<SearchDb>();
builder.Services.AddSingleton<PostDb>();
builder.Services.AddSingleton<CommentDb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: "index",
        pattern: "/{action}",
        defaults: new { controller = "Home", action = "Index" }
    );

    endpoints.MapControllerRoute(
        name: "post",
        pattern: "/post/{id}/*",
        defaults: new { controller = "Home", action = "Post" }
    );

});

app.Run();
