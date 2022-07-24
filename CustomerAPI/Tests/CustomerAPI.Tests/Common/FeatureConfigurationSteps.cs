using TestUtilities;

namespace CustomerAPI.Tests.Common
{
    public static class FeatureConfigurationSteps
    {
        public static TFeature GivenACleanSlate<TFeature>(this IApiFeature<TFeature> feature)
            where TFeature : class
        {
            feature.Client.DefaultRequestHeaders.Clear();
            feature.DeleteAllTableData();

            return feature as TFeature;
        }
    }
}
