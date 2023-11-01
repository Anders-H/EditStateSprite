using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EditStateSprite.Serialization
{
    public class SpriteChunkParser : List<string>
    {
        public bool GetMulticolor()
        {
            const string multiColorRegex = @"^[\s]*multicolor[\s]*=[\s]*(yes|no)?[\s]*$";
            const string isSetRegex = @"^[\s]*multicolor[\s]*=[\s]*(yes)?[\s]*$";
            var multicolor = Pop(multiColorRegex);

            if (string.IsNullOrEmpty(multicolor))
                throw new SystemException("Serialized sprite did not contain multicolor information.");

            return Is(isSetRegex, multicolor);
        }

        public string GetName()
        {
            const string nameRegex = @"^[\s]*name[\s]*=[\s]*(.*)[\s]*$";
            var name = Pop(nameRegex);

            if (string.IsNullOrEmpty(name))
                throw new SystemException("Serialized sprite did not contain any name information.");

            var match = Regex.Match(name, nameRegex, RegexOptions.IgnoreCase);

            if (match.Success)
                return match.Groups[1].Value.Trim();
            
            throw new SystemException("Serialized sprite did not contain any correct name information.");
        }

        private string Pop(string regex)
        {
            var r = new Regex(regex, RegexOptions.IgnoreCase);

            foreach (var s in this.Where(s => r.IsMatch(s)))
            {
                Remove(s);
                return s;
            }

            return null;
        }

        private static bool Is(string regex, string value)
        {
            var r = new Regex(regex, RegexOptions.IgnoreCase);
            return r.IsMatch(value);
        }
    }
}