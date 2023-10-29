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
            return Is(isSetRegex, multicolor);
        }

        private string Pop(string regex)
        {
            var r = new Regex(regex, RegexOptions.IgnoreCase);

            foreach (var s in this.Where(s => r.IsMatch(s)))
            {
                Remove(s);
                return s;
            }

            throw new SystemException("Serialized sprite did not contain multicolor information.");
        }

        private static bool Is(string regex, string value)
        {
            var r = new Regex(regex, RegexOptions.IgnoreCase);
            return r.IsMatch(value);
        }
    }
}