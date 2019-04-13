using System;

namespace Compute
{
    [Serializable]
    public class Config
    {
        public int InstanceCount { get; set; }
        public string PacketLocation { get; set; }
    }
}