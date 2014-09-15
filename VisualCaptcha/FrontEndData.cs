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
        public List<string> Values { get; set; }
        public string ImageName { get; set; }
        public string ImageFieldName { get; set; }
        public string AudioFieldName { get; set; }
    }
}