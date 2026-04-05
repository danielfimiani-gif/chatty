using System;

[Serializable]
public class ChatMessage
{
    public string Id;
    public string Author;
    public string Content;
    public string ReplyToId;

    public ChatMessage(string author, string content, string replyToId)
    {
        this.Id = Guid.NewGuid().ToString();
        this.Author = author;
        this.Content = content;
        this.ReplyToId = replyToId;
    }

    public string Format()
    {
        return string.Concat(Author, " : ", Content);
    }
}