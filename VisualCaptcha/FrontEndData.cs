using System;
using System.Collections.Generic;

namespace VisualCaptcha
{
    /// <summary>
    /// Serializable class for use by front-end library
    /// </summary>
    [Serializable]
    public sealed class FrontEndData
    {
        public List<string> Values { get; internal set; }
        public string ImageName { get; internal set; }
        public string ImageFieldName { get; internal set; }
        public string AudioFieldName { get; internal set; }

        internal FrontEndData()
        {
            
        }
    }
}