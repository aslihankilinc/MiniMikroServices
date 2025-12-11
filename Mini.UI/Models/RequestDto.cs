using Mini.UI.Utility;

namespace Mini.UI.Models
{
    public class RequestDto
    {
        public EnumApiType ApiType { get; set; } = EnumApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
        public EnumContentType ContentType { get; set; } = EnumContentType.Json;

    }
}
