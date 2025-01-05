namespace AzureAPI.Entities;

public class Note
{
    public int Id { get; set; }
    public string Title { get; set; }

    public int NoteDetailId { get; set; }
    public NoteDetail Detail { get; set; }
}