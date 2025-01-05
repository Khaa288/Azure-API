using AzureAPI.Entities;
using AzureAPI.Dtos;
using AzureAPI.Database;

using Microsoft.EntityFrameworkCore;

namespace AzureAPI.Extensions;

internal static class NoteApiExtensions {
    public static WebApplication AddNoteMinimalApi(this WebApplication app) {
        app.MapGet("api/note", async (AzureDbContext context) =>
        {
            var notes = await context.Notes.Include(n => n.Detail).ToListAsync();

            var response = new List<ListNotesResponseDto>();

            foreach(var value in notes) {
                response.Add(new ListNotesResponseDto() { NoteId = value.Id, NoteTitle = value.Title, NoteDetail = value.Detail.Detail});
            }

            return response;
        })
        .WithName("GetNotes")
        .WithOpenApi();

        app.MapPost("api/note", async (AzureDbContext context, CreateNotesRequestDto request) =>
        {
            var detail = new NoteDetail {
                Detail = request.NoteDetail
            };

            var note = new Note {
                Title = request.NoteTitle,
                Detail =  new NoteDetail {
                    Detail = request.NoteDetail
                }
            };

            await context.AddAsync(note);
            await context.SaveChangesAsync();
        })
        .WithName("CreateNote")
        .WithOpenApi();

        app.MapPut("api/note",  async (AzureDbContext context, UpdateNoteRequestDto request) => {
            var note = await context.Notes
                .Include(n => n.Detail)
                .FirstOrDefaultAsync(n => n.Id == request.NoteId);

            if (note is null) {
                return;
            }

            note.Detail.Detail = request.NoteDetail;
            note.Title = request.NoteTitle;

            context.Update(note);
            await context.SaveChangesAsync();
        })
        .WithName("UpdateNote")
        .WithOpenApi();

        app.MapDelete("api/note",  async (AzureDbContext context, int noteId) => {
            var note = await context.Notes.Include(n => n.Detail).FirstOrDefaultAsync(n => n.Id == noteId);

            if (note is null) {
                return;
            }

            context.NoteDetails.Remove(note.Detail);
            await context.SaveChangesAsync();
        })
        .WithName("DeleteNote")
        .WithOpenApi();

        return app;
    }
} 