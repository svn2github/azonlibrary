using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Azon.Helpers.Asserts;
using Azon.Helpers.Extensions;

namespace Azon.Helpers.Utils {
    /// <summary>
    /// Provides methods to work with templates.
    /// </summary>
    public static class Template {
        /// <summary>
        /// Replaces text occurences enclosed in braces with the string representation
        /// of a corresponding property value of a specified object.
        /// </summary>
        /// <param name="template">A composite string format.</param>
        /// <param name="params">An object that contains zero or more properties to format.</param>
        /// <param name="replaceRegex">A regex to match parameters that need replacement.</param>
        /// <returns>A copy of string in which the format items have been replaced by
        /// the string representation of a corresponding property value of a specified object.</returns>
        /// <example>Template.Apply("Hello {world}", new { world = "Earth" });</example>
        public static string Apply(
            string template,
            object @params,
            string replaceRegex = "{(\\w+)}"
            ) {
            Require.NotNull(template, "template");
            Require.NotNull(@params, "params");

            var paramsDict = @params.GetType()
                                    .GetProperties()
                                    .ToDictionary(
                                        pi => pi.Name,
                                        pi => pi.GetValue(@params).With(x => x.ToString()));

            return Apply(template, paramsDict, replaceRegex);
        }

        public static string Apply(
            string template,
            IDictionary<string, string> @params,
            string replaceRegex = "{(\\w+)}"
            ) {
            Require.NotNull(template, "template");
            Require.NotNull(@params, "params");

            var regex = new Regex(replaceRegex);

            return regex.Replace(template, match => {
                var paramName = match.Groups[1].Value;

                Require.NotNull<InvalidOperationException>(
                    @params.ContainsKey(paramName),
                    "A value for parameter {{{0}}} was not supplied.",
                    paramName
                    );

                return @params[paramName] ?? string.Empty;
            });
        }
    }
}