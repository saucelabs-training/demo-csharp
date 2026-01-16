using System.Collections.Generic;

namespace Common.SauceLabs
{
    public class SauceLabsCapabilities
    {
        private List<string> _tags;

        public bool IsUsingSauceLabs { get; }

        public SauceLabsCapabilities()
        {
            _tags = new List<string>();
            IsUsingSauceLabs = true;
        }

        public bool IsDebuggingEnabled { get; set; }

        public List<string> Tags
        {
            get => _tags;
            set => _tags = value;
        }

        public static string BuildName { get; set; }
        public static string TunnelIdentifier => "tunnelIdentifier";

        public bool IsHeadless { get; set; }
    }
}