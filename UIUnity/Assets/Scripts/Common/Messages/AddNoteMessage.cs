using System;
using Common.Interface;
using Model.Note;

namespace Common.Messages
{
    public class AddNoteMessage : IMessage
    {
        public Type Receiver { get; set; }
        public IView Sender { get; set; }
        public NoteModel NewNote { get; set; }
    }
}