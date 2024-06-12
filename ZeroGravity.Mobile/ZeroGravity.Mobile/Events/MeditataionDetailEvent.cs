using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Events
{
    public class MeditataionDetailEvent : PubSubEvent<MeditataionDetailEvent>
    {
        public StreamContentType StreamContentType;
        public string PageName { get; set; }
        public DateTime DateTime { get; set; }

        public List<StreamContentProxy> StreamContent;

        public StreamContentProxy SelectedMediaVideoItem;

    }
}
