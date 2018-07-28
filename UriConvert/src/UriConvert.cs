using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ycode.UriConvert
{
    public static class UriConvert
    {
        private static List<Type> _notSupportedType = new List<Type>
        {
            typeof(IDictionary<,>),
            typeof(Dictionary<,>)
        };

        public static string SerializeObject(object value)
        {
            return Convert(value).AbsoluteUri;
        }

        public static Uri Convert(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            PropertyInfo baseUriProperty, pathProperty;
            string baseUri, path;
            IDictionary<string, string> queryParameters;
            extract(value, out baseUriProperty, out pathProperty, out baseUri, out path, out queryParameters);

            if (baseUriProperty == null)
                throw new UriFormatException("The object did not have Base URI.");
            if (baseUri == null)
                throw new UriFormatException($"The object's Base URI Property {baseUriProperty.Name} was null.");
            
            Uri uri = new Uri(baseUri);
            if (path != null)
                uri = (uri == null ? new Uri(path) : new Uri(uri, path));
            if (queryParameters != null && queryParameters.Any())
                uri = (uri == null
                       ? new Uri("?" + queryParameters.SerializeQueryParameters())
                       : new Uri(uri, "?" + queryParameters.SerializeQueryParameters()));
            return uri;
        }

        public static IDictionary<string, string> ExtractQueryParameters(object value, bool isToEncode = false)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            PropertyInfo baseUriProperty, pathProperty;
            string baseUri, path;
            IDictionary<string, string> queryParameters;
            extract(value, out baseUriProperty, out pathProperty, out baseUri, out path, out queryParameters, isToEncode);
            return queryParameters;
        }

        /// <summary>
        /// Extract URI elements from object.
        /// </summary>
        /// <remarks>
        /// * The priority order
        ///   * UriIgnoreAttribute
        ///   * UriBaseAttribute/UriPathAttribute
        ///   * UriQueryParameterAttribute
        ///   * Property named "Base"/"Path"
        /// </remarks>
        /// <param name="value">Value.</param>
        /// <param name="uriBaseProperty">URI base property.</param>
        /// <param name="uriPathProperty">URI path property.</param>
        /// <param name="uriBase">URI base.</param>
        /// <param name="path">Path.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <param name="isToEncode">If set to <c>true</c> is to encode.</param>
        private static void extract(
            object value,
            out PropertyInfo uriBaseProperty,
            out PropertyInfo uriPathProperty,
            out string uriBase,
            out string path,
            out IDictionary<string, string> queryParameters,
            bool isToEncode = true)
        {
            var uriBaseProperties = value.GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<UriBaseAttribute>() != null
                    && p.GetCustomAttribute<UriIgnoreAttribute>() == null);
            switch (uriBaseProperties.Count())
            {
                case 0:
                    uriBaseProperty = null; // will look up it by property name
                    break;
                case 1:
                    uriBaseProperty = uriBaseProperties.Single();
                    break;
                default:
                    throw new InvalidOperationException("UriBaseAttribute should be only specified to a single property in a class, but found multiple. " +
                                                        $"The properties are {string.Join(", ", uriBaseProperties.Select(b => b.Name))} in {value.GetType()}");
            }
                
            var uriPathProperties = value.GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<UriPathAttribute>() != null
                    && p.GetCustomAttribute<UriIgnoreAttribute>() == null);
            switch (uriPathProperties.Count())
            {
                case 0:
                    uriPathProperty = null; // will look up it by property name
                    break;
                case 1:
                    uriPathProperty = uriPathProperties.Single();
                    break;
                default:
                    throw new InvalidOperationException("UriPathAttribute should be only specified to a single property in a class, but found multiple. " +
                                                        $"The properties are {string.Join(", ", uriBaseProperties.Select(b => b.Name))} in {value.GetType()}");
            }
                
            var parameterProperties = new List<PropertyInfo>();

            foreach (var p in value.GetType().GetProperties())
            {
                if (!p.CanRead)
                    continue;

                if (_notSupportedType.Any(t => t.IsAssignableFrom(p.PropertyType.IsGenericType ? p.PropertyType.GetGenericTypeDefinition() : p.PropertyType)))
                    continue;

                if (p.GetCustomAttribute<UriIgnoreAttribute>() != null)
                    continue;

                switch (p.Name)
                {
                    case "Base":
                        if (uriBaseProperty == null
                            && p.GetCustomAttribute<UriQueryParameterAttribute>() == null)
                        {
                            uriBaseProperty = p;
                            continue;
                        }
                        break;
                    case "Path":
                        if (uriPathProperty == null
                            && p.GetCustomAttribute<UriQueryParameterAttribute>() == null)
                        {
                            uriPathProperty = p;
                            continue;
                        }
                        break;
                }

                if ((uriBaseProperty?.Name.Equals(p.Name) ?? false)
                        && p.GetCustomAttribute<UriQueryParameterAttribute>() == null)
                    continue;
                if ((uriPathProperty?.Name.Equals(p.Name) ?? false)
                        && p.GetCustomAttribute<UriQueryParameterAttribute>() == null)
                    continue;
                
                parameterProperties.Add(p);
            }

            uriBase = uriBaseProperty?.GetValue(value)?.ToString();
            path = uriPathProperty?.GetValue(value)?.ToString();

            var grouped = parameterProperties.GroupBy(p => p.GetCustomAttribute<UriQueryParameterAttribute>()?.Name ?? p.Name);

            var duplicates = grouped
                .Where(g => g.Count() > 1)
                .Select(g => $"Query parameter name \"{g.Key}\" is used by property {string.Join(" and ", g.Select(p => p.Name))}.");
            if (duplicates.Any())
                throw new InvalidOperationException($"There are duplicates in query parameters in type {value.GetType().FullName}. {string.Join(" ", duplicates)}");

            queryParameters = grouped.ToDictionary(
                g => g.Key,
                g =>
                {
                    var val = g.First().GetValue(value, null);
                    var str = convertPropertyValueToString(val);
                    if (isToEncode)
                        str = str?.Escape();
                    return str;
                });
        }

        private static string convertPropertyValueToString(object value)
        {
            if (value == null)
                return null;
            
            string valueString = null;

            if (!typeof(string).IsAssignableFrom(value.GetType())
                && ( typeof(IEnumerable).IsAssignableFrom(value.GetType())
                    || (value.GetType().IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(value.GetType().GetGenericTypeDefinition())) ) )
            {
                
                var items = new List<string>();
                var enumarable = (IEnumerable)value;
                foreach (var item in enumarable)
                {
                    var itemString = item.ToString();
                    items.Add(itemString);
                }
                valueString = string.Join(",", items);
            }
            else
                valueString = value.ToString();

            return valueString;
        }

        private static string Escape(this string value)
            => Uri.EscapeDataString(value);

        private static string SerializeQueryParameters(this IDictionary<string, string> parameters)
            => string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
    }
}
