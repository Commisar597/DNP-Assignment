using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CLIApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public CLIApp(
        IUserRepository userRepository,
        ICommentRepository commentRepository,
        IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("=== WARHAMMER FORUM CLI ===");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Create Post");
            Console.WriteLine("3. Add Comment to Post");
            Console.WriteLine("4. View Posts Overview");
            Console.WriteLine("5. View Single Post with Comments");
            Console.WriteLine("6. View Users");
            Console.WriteLine("7. Exit");

            Console.Write("\nSelect an option (1-7): ");

            string? choice = Console.ReadLine();

            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await CreateUserAsync();
                        break;

                    case "2":
                        await CreatePostAsync();
                        break;

                    case "3":
                        await AddCommentAsync();
                        break;

                    case "4":
                        ViewPostsOverview();
                        break;

                    case "5":
                        await ViewSinglePostAsync();
                        break;

                    case "6":
                        ViewUsers();
                        break;

                    case "7":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        continue;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Error]: {ex.Message}");
            }

            if (running)
            {
                Console.WriteLine("\nPress ANY key to return to main menu...");
                Console.ReadKey();
            }
        }
    }

    private async Task CreateUserAsync()
    {
        Console.WriteLine("--- Create New User ---");

        Console.Write("Enter Username: ");
        string name = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter Password: ");
        string password = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Username and password cannot be empty!");
            return;
        }

        bool exists = userRepository
            .GetMany()
            .Any(u => u.Name.Equals(
                name,
                StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            Console.WriteLine($"User with name '{name}' already exists!");
            return;
        }

        User created = await userRepository.AddAsync(
            new User(name, password));

        Console.WriteLine();
        Console.WriteLine("--> User successfully created!");
        Console.WriteLine($"    Username: {created.Name}");
        Console.WriteLine($"    ID: {created.UserId}");
    }

    private async Task CreatePostAsync()
    {
        Console.WriteLine("--- Create New Post ---");

        User? author = SelectUser();

        if (author == null)
        {
            return;
        }

        Console.WriteLine();

        Console.Write("Enter Title: ");
        string title = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter Body text: ");
        string body = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(title) ||
            string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Title and body cannot be empty!");
            return;
        }

        Post created = await postRepository.AddAsync(
            new Post(body, title, author.UserId));

        Console.WriteLine();
        Console.WriteLine("--> Post successfully created!");
        Console.WriteLine($"    Title: {created.Title}");
        Console.WriteLine($"    Author: {author.Name}");
        Console.WriteLine($"    Post ID: {created.PostId}");
    }

    private async Task AddCommentAsync()
    {
        Console.WriteLine("--- Add Comment ---");

        Post? post = SelectPost();

        if (post == null)
        {
            return;
        }

        Console.WriteLine();

        User? author = SelectUser();

        if (author == null)
        {
            return;
        }

        Console.WriteLine();

        Console.Write("Enter Comment text: ");
        string body = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment cannot be empty!");
            return;
        }

        Comment comment = new Comment(
            body,
            post.PostId,
            author.UserId);

        Comment created = await commentRepository.AddAsync(comment);

        Console.WriteLine();
        Console.WriteLine("--> Comment added successfully!");
        Console.WriteLine($"    Comment ID: {created.CommentId}");
        Console.WriteLine($"    Author: {author.Name}");
        Console.WriteLine($"    Post: {post.Title}");
    }

    private User? SelectUser()
    {
        List<User> users = userRepository.GetMany().ToList();

        if (!users.Any())
        {
            Console.WriteLine("No users available.");
            return null;
        }

        Console.WriteLine("\nAvailable users:");

        foreach (User user in users)
        {
            Console.WriteLine($"[{user.UserId}] {user.Name}");
        }

        Console.Write("\nEnter Username: ");

        string username = Console.ReadLine() ?? string.Empty;

        User? selectedUser = users.FirstOrDefault(
            u => u.Name.Equals(
                username,
                StringComparison.OrdinalIgnoreCase));

        if (selectedUser == null)
        {
            Console.WriteLine($"User '{username}' does not exist.");
            return null;
        }

        return selectedUser;
    }

    private Post? SelectPost()
    {
        List<Post> posts = postRepository.GetMany().ToList();

        if (!posts.Any())
        {
            Console.WriteLine("No posts available.");
            return null;
        }

        Console.WriteLine("\nAvailable posts:");

        foreach (Post post in posts)
        {
            Console.WriteLine($"[{post.PostId}] {post.Title}");
        }

        Console.Write("\nEnter Post ID: ");

        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid Post ID format.");
            return null;
        }

        Post? selectedPost = posts.FirstOrDefault(
            p => p.PostId == postId);

        if (selectedPost == null)
        {
            Console.WriteLine($"Post with ID {postId} does not exist.");
            return null;
        }

        return selectedPost;
    }

    private void ViewPostsOverview()
    {
        Console.WriteLine("--- Posts Overview ---");

        List<Post> posts = postRepository.GetMany().ToList();

        if (!posts.Any())
        {
            Console.WriteLine("No posts available.");
            return;
        }

        foreach (Post post in posts)
        {
            Console.WriteLine($"[ID: {post.PostId}] {post.Title}");
        }
    }

    private async Task ViewSinglePostAsync()
    {
        Console.WriteLine("--- View Single Post ---");

        Post? post = SelectPost();

        if (post == null)
        {
            return;
        }

        User author = await userRepository.GetSingleAsync(post.OwnerId);

        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine($"Title:  {post.Title}");
        Console.WriteLine($"Author: {author.Name}");
        Console.WriteLine($"Post ID: {post.PostId}");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine(post.Body);
        Console.WriteLine("========================================");

        List<Comment> comments = commentRepository
            .GetMany()
            .Where(c => c.PostId == post.PostId)
            .ToList();

        Console.WriteLine();
        Console.WriteLine($"Comments ({comments.Count}):");

        if (!comments.Any())
        {
            Console.WriteLine("  (No comments yet)");
            return;
        }

        foreach (Comment comment in comments)
        {
            User commenter = await userRepository.GetSingleAsync(
                comment.OwnerId);

            Console.WriteLine();
            Console.WriteLine($"[{comment.CommentId}] {commenter.Name}");
            Console.WriteLine(comment.Body);
            Console.WriteLine("----------------------------------------");
        }
    }

    private void ViewUsers()
    {
        Console.WriteLine("--- Users ---");

        List<User> users = userRepository.GetMany().ToList();

        if (!users.Any())
        {
            Console.WriteLine("No users available.");
            return;
        }

        foreach (User user in users)
        {
            Console.WriteLine();
            Console.WriteLine($"[{user.UserId}] {user.Name}");
            Console.WriteLine("----------------------------------------");
        }
    }
}