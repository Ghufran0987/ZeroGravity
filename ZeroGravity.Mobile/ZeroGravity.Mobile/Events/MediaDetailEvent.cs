using Prism.Events;
using System;

namespace ZeroGravity.Mobile.Events
{
    public class MediaDetailEvent : PubSubEvent<MediaDetailEvent>
    {
        public TimeSpan MediaPlayTime { get; set; }
    }
}
