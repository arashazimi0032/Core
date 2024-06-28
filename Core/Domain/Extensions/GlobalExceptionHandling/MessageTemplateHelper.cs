using Core.Domain.Exceptions;

namespace Core.Domain.Extensions.GlobalExceptionHandling;

public static class MessageTemplateHelper
{
    public static string GetNestedTypes(ExceptionMessage exceptionMessage, int nestedLevel = 1)
    {
        var toReturn = "";
        var tab = string.Concat(Enumerable.Repeat("\t", nestedLevel));
        if (exceptionMessage is not null)
        {
            foreach (var property in exceptionMessage.GetType().GetProperties())
            {
                if (property.PropertyType.Equals(typeof(ExceptionMessage)))
                {
                    toReturn += $"\t{tab}{property.Name}: " + "{\n";
                    toReturn += $"{GetNestedTypes((ExceptionMessage)property.GetValue(exceptionMessage), nestedLevel + 1)}" +
                        $"\t{tab}" + "}\n";
                }
                else
                {
                    toReturn += $"\t{tab}{property.Name}: {property.GetValue(exceptionMessage)}\n";
                }
            }
        }
        else
        {
            toReturn += $"\t{tab}null\n";
        }

        return toReturn;
    }

    public static string GetDictionaryString(Dictionary<string, string> dictionary)
    {
        var toReturn = "";

        foreach (var key in dictionary.Keys)
        {
            toReturn += $"\t\t{key}: {dictionary[key]}\n";
        }
        return toReturn;
    }
}
