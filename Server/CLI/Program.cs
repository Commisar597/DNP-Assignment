using CLI.UI;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Start CLI app...");

IUserRepository userRepository = new UserFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();
IPostRepository postRepository = new PostFileRepository();

CLIApp cliApp = new CLIApp(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();