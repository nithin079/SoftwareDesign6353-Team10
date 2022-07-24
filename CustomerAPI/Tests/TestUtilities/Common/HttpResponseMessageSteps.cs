using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TestUtilities.Extensions;

namespace TestUtilities.Common
{
    public static class HttpResponseMessageSteps
    {
        public static TFeature ThenTheResponseShouldReturn<TFeature>(
            this IApiFeature<TFeature> feature,
            HttpStatusCode statusCode) where TFeature : class
        {
            feature.Response.Should().NotBeNull();
            feature.Response.StatusCode.Should().Be(statusCode);

            return feature as TFeature;
        }

        public static TFeature ThenTheResponseShouldMatch<TFeature, TResult>(
            this IApiFeature<TFeature> feature,
            HttpStatusCode statusCode,
            Action<TResult> assertion,
            bool IsActionResult = true) where TFeature : class
        {
            feature.ThenTheResponseShouldReturn(statusCode);

            var content = "";

            Task.Run(async () => content = await feature.Response.Content.ReadAsStringAsync()).Wait();

            var item = content.JsonDeserializeFromString<TResult>(statusCode, IsActionResult);

            assertion(item);

            return feature as TFeature;
        }

        private static TResult JsonDeserializeFromString<TResult>(this string content, HttpStatusCode statusCode, bool isActionResult)
        {
            TResult item;

            if(isActionResult)
            {
                switch(statusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Accepted:
                    case HttpStatusCode.Created:
                        var data = ((JObject)JsonConvert.DeserializeObject(content))["Result"].ToString();
                        if (data is string && string.IsNullOrWhiteSpace(data))
                            return "".ChangeType<TResult>();
                        else if (data is string && !data.Contains('{') && !data.Contains("[]"))
                            return data.ChangeType<TResult>();
                        else
                            item = JsonConvert.DeserializeObject<TResult>(data);

                        break;
                    default:
                        var errors = ((JObject)JsonConvert.DeserializeObject(content))["errors"].FirstOrDefault().ToString();
                        item = JsonConvert.DeserializeObject<TResult>(errors);
                        break;
                }
            }
            else
            {
                item = JsonConvert.DeserializeObject<TResult>(content);
            }

            return item;
        }
    }
}
