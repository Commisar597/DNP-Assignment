using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Start CLI app...");

IUserRepository userRepository = new UserInMemoryRepositories();
ICommentRepository commentRepository = new CommentInMemoryRepositories();
IPostRepository postRepository = new PostInMemoryRepositories();

CLIApp cliApp = new CLIApp(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();