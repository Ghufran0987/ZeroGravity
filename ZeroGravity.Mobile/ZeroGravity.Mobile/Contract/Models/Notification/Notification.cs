using System;
using Prism.Mvvm;

namespace ZeroGravity.Mobile.Contract.Models.Notification
{

    public class Notification : BindableBase
    {
        private string _title;

        public Notification(bool hasBeenRead = false)
        {
            Id = Guid.NewGuid();
            HasBeenRead = hasBeenRead;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _content;

        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        private bool _hasBeenRead;

        public bool HasBeenRead
        {
            get { return _hasBeenRead; }
            set { SetProperty(ref _hasBeenRead, value); }
        }

        private Guid _id;

        public Guid Id
        {
            get { return _id; }
            private set { SetProperty(ref _id, value); }
        }

    }
}
