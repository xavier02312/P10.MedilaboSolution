using MongoDB.Bson;
using PatientNote.Domain;
using PatientNote.Models.InputModels;
using PatientNote.Models.OutputModels;
using PatientNote.Repositories;

namespace PatientNote.Service
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _repositoryNote;
        public NoteService(INoteRepository repositoryNote)
        {  
            _repositoryNote = repositoryNote; 
        }
        public async Task<List<NoteOutputModel>> GetNotes(int patientId)
        {
            try
            {
                var notes = await _repositoryNote.ListAsync(patientId);
                var output = new List<NoteOutputModel>();
                foreach (var note in notes)
                {
                    output.Add(ToOutputModel(note));
                }
                return output.OrderByDescending(n => n.Id.CreationTime).ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<NoteOutputModel> Create(NoteInputModel noteModel)
        {
            try
            {
                return ToOutputModel(await _repositoryNote.CreateAsync(new Note
                {
                    Content = noteModel.Content,
                    PatientId = noteModel.PatientId,
                }));
            }
            catch
            {
                throw;
            }
        }

        public async Task<NoteOutputModel?> Delete(ObjectId id)
        {
            try
            {
                var note = await _repositoryNote.DeleteAsync(id);
                if (note is null)
                {
                    return null;
                }
                return ToOutputModel(note);
            }
            catch
            {
                throw;
            }
        }

        public async Task<NoteOutputModel?> GetById(ObjectId id)
        {
            try
            {
                var note = await _repositoryNote.GetByIdAsync(id);
                if (note is not null)
                {
                    return ToOutputModel(note);
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<NoteOutputModel?> Update(NoteInputModel noteModel, ObjectId id)
        {
            try
            {
                var noteUpdated = await _repositoryNote.UpdateAsync(new Note
                {
                    Id = id,
                    Content = noteModel.Content,
                    PatientId = noteModel.PatientId,
                });

                if (noteUpdated is not null)
                {
                    return ToOutputModel(noteUpdated);
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        private NoteOutputModel ToOutputModel(Note note)
        {
            return new NoteOutputModel
            {
                Id = note.Id,
                Content = note.Content,
                PatientId = note.PatientId,
            };
        }
    }
}
