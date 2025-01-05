namespace AzureAPI.Dtos;

class UpdateNoteRequestDto {
    public int NoteId { get; set; }
    public string NoteTitle { get; set; }
    public string NoteDetail { get; set; }
}