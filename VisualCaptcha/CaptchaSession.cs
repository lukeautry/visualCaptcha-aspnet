using System.Collections.Generic;

namespace VisualCaptcha
{
    /// <summary>
    /// Session state container
    /// </summary>
    public sealed class CaptchaSession
    {
        public Dictionary<string, string> Images { get; set; }
        public Dictionary<string, string> Audios { get; set; }
        public KeyValuePair<string, string> ValidImageOption { get; set; }
        public KeyValuePair<string, string> ValidAudioOption { get; set; }
        public FrontEndData FrontEndData { get; set; }
    }
}