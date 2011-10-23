using System;
using System.Text.RegularExpressions;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Utils {
    public static class Template {
        private static readonly Regex _regex = new Regex(@"{(\w+)}");

        public static string Apply(string template, object @params) {
            Require.NotNull(template, "template");
            Require.NotNull(@params, "params");

            return _regex.Replace(
                template,
                match => {
                    var parameterName = match.Groups[1].Value;
                    var property = @params.GetType().GetProperty(parameterName);

                    Require.NotNull<InvalidOperationException>(
                        property,
                        "A value for parameter {{{0}}} was not supplied.",
                        parameterName
                    );

                    var parameterValue = property.GetValue(@params, null);
                    return parameterValue == null
                               ? string.Empty
                               : parameterValue.ToString();
                }
            );
        }
    }
}