using System;
using System.Text.RegularExpressions;

using Azon.Helpers.Asserts;

namespace Azon.Helpers.Utils {
    public static class Template {
        private static readonly Regex _regex = new Regex(@"{(\w+)}");

        /// <summary>
        /// Replaces text occurences enclosed in braces with the string representation
        /// of a corresponding property value of a specified object.
        /// </summary>
        /// <param name="template">A composite string format.</param>
        /// <param name="params">An object that contains zero or more properties to format.</param>
        /// <returns>A copy of string in which the format items have been replaced by
        /// the string representation of a corresponding property value of a specified object.</returns>
        /// <example>Template.Apply("Hello {world}", new { world = "Earth" });</example>
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