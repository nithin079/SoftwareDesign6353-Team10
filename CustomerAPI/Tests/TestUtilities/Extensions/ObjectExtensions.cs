using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace TestUtilities.Extensions
{
    public static class ObjectExtensions
    {
        public static StringContent AsJson(this object o)
            => new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
    }

    public static class StringExtensions
    {
        public static TResult ChangeType<TResult>(this object obj)
            => (TResult)Convert.ChangeType(obj, typeof(TResult));
    }
}
