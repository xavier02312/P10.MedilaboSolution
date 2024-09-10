using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using PatientNote.Data;
using PatientNote.Domain;

namespace PatientNote.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly MongoDbContext _db;
        public NoteRepository(MongoDbContext db)
        {
            _db = db;
        }
        public async Task<Note> CreateAsync(Note note)
        {
            try
            {
                await _db.Notes.AddAsync(note);
                await _db.SaveChangesAsync();
                return note;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Note?> DeleteAsync(ObjectId id)
        {
            try
            {
                var note = await _db.Notes.FirstOrDefaultAsync(note => note.Id == id);
                if (note is not null)
                {
                    _db.Notes.Remove(note);
                    await _db.SaveChangesAsync();
                }
                return note;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Note>> ListAsync(int patientId)
        {
            try
            {
                return await _db.Notes.Where(note => note.PatientId == patientId).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Note?> GetByIdAsync(ObjectId id)
        {
            try
            {
                var note = await _db.Notes.FirstOrDefaultAsync(note => note.Id == id);
                return note;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Note?> UpdateAsync(Note note)
        {
            try
            {
                var noteToUpdate = await _db.Notes.FirstOrDefaultAsync(n => n.Id == note.Id);
                if (noteToUpdate is not null)
                {
                    noteToUpdate.Content = note.Content;
                    noteToUpdate.PatientId = note.PatientId;
                    await _db.SaveChangesAsync();
                }
                return noteToUpdate;
            }
            catch
            {
                throw;
            }
        }
    }
}
