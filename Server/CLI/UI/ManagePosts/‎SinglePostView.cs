using Entities;

namespace CLI.UI.ManagePosts;

public class SinglePostView : ListPostView
{
    public SinglePostView()
    { }

    public int FindSinglePost(Post post)
    {
        return Posts.IndexOf(post);
    }
    
    public void Display(Post post)
    {
        Console.WriteLine($"\nPostID: {post.PostId}" +
                          $"\nTitle: {post.Title}" +
                          $"\nBody: {post.Body}" +
                          $"\nAuthor ID: {post.OwnerId}");
    }
}