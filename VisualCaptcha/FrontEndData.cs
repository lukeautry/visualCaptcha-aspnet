using System.Collections.Generic;

namespace VisualCaptcha
{
    public class FrontEndData
    {
        public List<string> values { get; set; }
        public string imageName { get; set; }
        public string imageFieldName { get; set; }
        public string audioFieldName { get; set; }
    }
}