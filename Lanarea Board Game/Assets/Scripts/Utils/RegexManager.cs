using System.Text.RegularExpressions;
using UnityEngine;

public class RegexManager
{
    public const int MIN_NAME_LENGTH = 3;
    public const int MAX_NAME_LENGTH = 26;
    public const int MIN_DESCRIPTION_LENGTH = 3;
    public const int MAX_DESCRIPTION_LENGTH = 127;

    public const string NAMERX = @"[^0-9a-zA-Z\s]*$";


    private RegexManager() { }

    public static bool NameValidator(string name)
    {
        if (name.Length < MIN_NAME_LENGTH || name.Length > MAX_NAME_LENGTH) { return false; }
        if (!RegexCheck(name)) { { return false; } }
        return true;

    }
    //TODO allow for dots (...)
    public static bool DescriptionValidator(string description)
    {
        if (description.Length < MIN_DESCRIPTION_LENGTH || description.Length > MAX_DESCRIPTION_LENGTH) { return false; }
        if (!RegexCheck(description)) { { return false; } }
        return true;
    }

    private static bool RegexCheck(string text)
    {
        Regex rx = new Regex(NAMERX);
        Match match = rx.Match(text);
        //Debug.Log("Value of Match: " + match.Value + " T/F " + match.Value.Length);
        if (match.Value.Length > 0)
        {
            return false;
        }
        return true;
    }

    public static string NameFixer(string name)
    {
        string newName = name;
        if (name.Length < MIN_NAME_LENGTH) { newName = name + "aaa"; }
        if (name.Length > MAX_NAME_LENGTH) { newName = name.Substring(0, MAX_NAME_LENGTH); }
        return Regex.Replace(newName, @"[^0-9a-zA-Z-\s]+", "");
    }
}
