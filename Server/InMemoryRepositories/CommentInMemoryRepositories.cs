using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepositories : ICommentRepository
{
    public List<Comment> comments = [];

    public CommentInMemoryRepositories()
    {
        _ = AddAsync(new Comment("Preach! Big E is to blame here, he never explained anything to his sons.", 1, 1)).Result;
        _ = AddAsync(new Comment("Leman Russ did exactly what needed to be done. Should have broken his spine harder.", 1, 2)).Result;
        _ = AddAsync(new Comment("Tzeentch played both of them like a fiddle anyway.", 1, 2)).Result;
        _ = AddAsync(new Comment("He did do nothing wrong... except everything he actually did.", 1, 1)).Result;
        _ = AddAsync(new Comment("Someone call the Inquisition to purge this entire thread.", 1, 4)).Result;
        
        _ = AddAsync(new Comment("Finally, some pure, unadulterated facts.", 2, 2)).Result;
        _ = AddAsync(new Comment("Prospero burned beautifully. Would do it again.", 2, 3)).Result;
        _ = AddAsync(new Comment("The Space Wolves are just mindless attack dogs with no honor.", 2, 2)).Result;
        _ = AddAsync(new Comment("Yeah, but at least we have cool beards and actual wolves.", 2, 1)).Result;

        _ = AddAsync(new Comment("WAAAAAGH! ORKS IZ DA BEST! DA REST OF YA 'UMIEZ ARE JUS' WEAKLIETZ!", 3, 1)).Result;
        _ = AddAsync(new Comment("At least the T'au fight for the Greater Good, not for a rotting corpse on a chair.", 3, 2)).Result;
        _ = AddAsync(new Comment("Space communists go home, go learn how to throw a punch first.", 3, 4)).Result;
        _ = AddAsync(new Comment("The Eldar literally birthed Slaanesh into existence. They win the worst award hands down.", 3, 4)).Result;
        
        _ = AddAsync(new Comment("Hydra Dominatus! (By the way, I am Alpharius).", 4, 4)).Result;
        _ = AddAsync(new Comment("No, wait, I AM ALPHARIUS!", 4, 3)).Result;
        _ = AddAsync(new Comment("Roboute Guilliman is the best logician in the galaxy, just admit you are jealous.", 4, 4)).Result;
        _ = AddAsync(new Comment("He just has a big tiddie Eldar girlfriend carrying his logistics, that's his secret.", 4, 1)).Result;
        
        _ = AddAsync(new Comment("BLOOD FOR THE BLOOD GOD! SKULLS FOR THE SKULL THRONE!", 5, 1)).Result;
        _ = AddAsync(new Comment("Bro, that doesn't answer his question at all...", 5, 1)).Result;
        _ = AddAsync(new Comment("Just read the first three books, then jump straight into the Siege of Terra.", 5, 2)).Result;
        _ = AddAsync(new Comment("Honestly, just go watch a 3-hour lore deep dive video on YouTube.", 5, 3)).Result;
        
        _ = AddAsync(new Comment("Two thin coats! That's the golden rule of Citadel painting.", 6, 2)).Result;
        _ = AddAsync(new Comment("Just drown the whole mini in Nuln Oil. It hides all mistakes.", 6, 4)).Result;
        _ = AddAsync(new Comment("Save your sanity and buy an airbrush. Brushes are pain.", 6, 3)).Result;
        _ = AddAsync(new Comment("Is that blue exactly Codex compliant? The Inquisition might want a word.", 6, 1)).Result;
    }
    
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.CommentId = comments.Any()
            ? comments.Max(c => c.CommentId) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }
    
    public Task UpdateAsync(Comment comment)
    {
        Comment? existingPost = comments.SingleOrDefault(c => c.CommentId == comment.CommentId);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.CommentId}' not found");
        }

        comments.Remove(existingPost);
        comments.Add(comment);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        Comment? postToRemove = comments.SingleOrDefault(c => c.CommentId == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        comments.Remove(postToRemove);
        return Task.CompletedTask;
    }
    
    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = comments.SingleOrDefault(p => p.CommentId == id);
        if(comment is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return Task.FromResult(comment);
    }
    
    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}