using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Net;

namespace SDService.Model.Utils
{
   public  class RequestFieldHelper
    {
        public static Collection<string> GetPartialResponseFields(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
           

            var queryOption = HttpUtility.ParseQueryString(request.RequestUri.Query)["fields"];

            if (queryOption != null)
            {
                Collection<string> fields;

                if (!Fields.TryParse(queryOption, out fields))
                {
                    // Not much choice but to throw a HttpResponseException inside a MediaTypeFormatter (no access 
                    // to the HttpResponseMessage).
                    throw new Exception(HttpStatusCode.BadRequest.ToString());
                }

                return fields;
            }

            return null;
        }

        public static Collection<string> GetFirstLevelFields(Collection<string> fields)
        {
            if (fields == null)
            {
                return null;
            }
            Collection<string> firstLevelFields = new Collection<string>();
            foreach (string field in fields)
            {
                string firstLevelField = field;
                if (field.Contains("/"))
                {
                    firstLevelField = field.Split('/')[0];
                }
                if (!firstLevelFields.Contains(firstLevelField))
                {
                    firstLevelFields.Add(firstLevelField);
                }
            }
            return firstLevelFields;
        }

        public static Dictionary<string, Collection<string>> GetSecondLevelFields(Collection<string> fields)
        {
            if (fields == null)
            {
                return null;
            }
            Dictionary<string, Collection<string>> topTwoLevelFields = new Dictionary<string, Collection<string>>();
           
            foreach (string field in fields)
            {
                if (!field.Contains("/"))
                {
                    topTwoLevelFields.Add(field, new Collection<string>());
                }
                else
                {
                  string[]  fieldArray = field.Split('/');
                  string firstLevelField = fieldArray[0];
                  string secondLevelField = fieldArray[1];
                  if (topTwoLevelFields.ContainsKey(firstLevelField))
                  {
                      topTwoLevelFields[firstLevelField].Add(secondLevelField);
                  }
                  else
                  {
                      topTwoLevelFields.Add(firstLevelField, new Collection<string>());
                      topTwoLevelFields[firstLevelField].Add(secondLevelField);
                  }
                }
            }
            return topTwoLevelFields;
        }

        
    }
    internal static class Fields
    {
        private const string ValidationPattern = @"^\s*(\*|([^\/&^*&^,&^\s]+(/[^\/&^*&^,&^\s]+)*(/\*)?))(\s*,\s*(\*|([^\/&^*&^,&^\s]+(/[^\/&^*&^,&^\s]+)*(/\*)?)))*\s*$";

        internal static bool TryParse(string s, out Collection<string> result)
        {
            var temp = new Collection<string>();

            if (s.Trim() != "")
            {
                if (!GetFields(null, s, temp))
                {
                    result = null;

                    return false;
                }
            }

            result = temp;

            return true;
        }

        private static bool Validate(string fields)
        {
            if (!Regex.IsMatch(fields, ValidationPattern))
            {
                return false;
            }

            return true;
        }

        private static bool GetFields(string basePath, string fields, Collection<string> result)
        {
            if (!Validate(fields))
            {
                return false;
            }

            var parenthesisCount = 0;
            var firstParenthesis = -1;
            var pathStart = 0;
            var parenthesisClosed = false;

            for (var i = 0; i < fields.Length; i++)
            {
                var character = fields[i];

                if (parenthesisClosed)
                {
                    if (character != ',' && character != ')' && character != ' ' && character != '\t')
                    {
                        return false;
                    }
                }

                if (character == '(')
                {
                    if (parenthesisCount == 0)
                    {
                        firstParenthesis = i;
                    }

                    parenthesisCount++;
                }

                if (character == ')')
                {
                    parenthesisCount--;

                    if (parenthesisCount < 0)
                    {
                        return false;
                    }

                    parenthesisClosed = true;
                }

                if (character == ',' && parenthesisCount == 0)
                {
                    if (firstParenthesis > -1)
                    {
                        var newBasePath = PathUtilities.CombinePath(basePath, fields.Substring(pathStart, firstParenthesis - pathStart));

                        if (!GetFields(newBasePath, fields.Substring(firstParenthesis + 1, i - firstParenthesis - 2), result))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        result.Add(PathUtilities.CombinePath(basePath, fields.Substring(pathStart, i - pathStart).Trim()));
                    }

                    firstParenthesis = -1;
                    pathStart = i + 1;
                    parenthesisClosed = false;
                }

                if (i == fields.Length - 1)
                {
                    if (parenthesisCount != 0)
                    {
                        return false;
                    }

                    if (firstParenthesis > -1)
                    {
                        var newBasePath = PathUtilities.CombinePath(basePath, fields.Substring(pathStart, firstParenthesis - pathStart));

                        if (!GetFields(newBasePath, fields.Substring(firstParenthesis + 1, i - firstParenthesis - 1), result))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        result.Add(PathUtilities.CombinePath(basePath, fields.Substring(pathStart, i - pathStart + 1).Trim()));
                    }
                }
            }

            return true;
        }
    }

    internal static class PathUtilities
    {
        internal static string CombinePath(string path, string name)
        {
            if (string.IsNullOrEmpty(path))
            {
                return name;
            }

            return string.Format("{0}/{1}", path, name);
        }
    }
}
