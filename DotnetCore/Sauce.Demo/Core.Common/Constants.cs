using System;

namespace Core.Common
{
    public static class Constants
    {
        public static string BaseUrl => "https://www.saucedemo.com";

        public static string BuildId { get; set; } = DateTime.Now.ToString("F");
    }
}