using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Common.Helpers.Functions
{
    public static class FormDataUtils
    {
        public static List<T>? RecoverListFromForm<T>(
            List<T>? currentValue,
            string formKey,
            IHttpContextAccessor accessor,
            ILogger logger)
        {
            if (currentValue != null && currentValue.Count > 0)
                return currentValue;

            var httpContext = accessor.HttpContext;
            var form = httpContext?.Request?.Form;
            if (form == null)
                return currentValue;

            if (!form.TryGetValue(formKey, out var values))
                return currentValue;

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                var parsedList = new List<T>();

                foreach (var raw in values)
                {
                    var json = raw?.ToString()?.Trim();
                    if (string.IsNullOrWhiteSpace(json)) continue;

                    if (json.StartsWith("["))
                    {
                        var list = JsonSerializer.Deserialize<List<T>>(json, options);
                        if (list != null) parsedList.AddRange(list);
                    }
                    else if (json.StartsWith("{"))
                    {
                        var obj = JsonSerializer.Deserialize<T>(json, options);
                        if (obj != null) parsedList.Add(obj);
                    }
                    else
                    {
                        json = json.Replace('\'', '"');

                        if (json.StartsWith("["))
                        {
                            var list = JsonSerializer.Deserialize<List<T>>(json, options);
                            if (list != null) parsedList.AddRange(list);
                        }
                        else if (json.StartsWith("{"))
                        {
                            var obj = JsonSerializer.Deserialize<T>(json, options);
                            if (obj != null) parsedList.Add(obj);
                        }
                    }
                }

                if (parsedList.Count > 0)
                {
                    logger.LogInformation("Recovered {Count} items for key {Key}.",
                                          parsedList.Count, formKey);

                    return parsedList;
                }
            }
            catch (JsonException ex)
            {
                logger.LogWarning(ex, "Invalid JSON for {Key} in multipart form.", formKey);
            }

            return currentValue;
        }
    }

}
