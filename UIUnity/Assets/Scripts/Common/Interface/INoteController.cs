using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Note;

namespace Common.Interface
{
    public interface INoteController: IController
    {
        Task<IList<NoteModel>> GetNotesPackAsync(int size);
        Task<IList<NoteModel>> GetFirstNotesPackAsync(int size);
        Task<(string noteId, string message)> AddNewNote(NoteModel value);
    }
}