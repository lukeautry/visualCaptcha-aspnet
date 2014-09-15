using System.Collections.Generic;

namespace VisualCaptcha
{
    public class FrontEndData
    {
        public List<string> Values { get; set; }
        public string ImageName { get; set; }
        public string ImageFieldName { get; set; }
        public string AudioFieldName { get; set; }
    }
}