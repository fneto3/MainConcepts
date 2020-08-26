using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Identifier = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Identifier = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Identifier { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }
    }
}
