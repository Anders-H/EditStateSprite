using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using EditStateSprite.Serialization;

namespace EditStateSprite
{
    public class SpriteList : List<SpriteRoot>
    {
        public void PaintPreview(Graphics g)
        {
            foreach (var s in this)
            {
                s.ColorMap.PaintPreview(g);
            }
        }

        public void Load(string filename)
        {
            string s;
            
            using (var sr = new StreamReader(filename, Encoding.UTF8))
            {
                s = sr.ReadToEnd();
                sr.Close();
            }

            Deserialize(s);
        }

        public void Save(string filename)
        {
            var s = new StringBuilder();
            Serialize(s);

            using (var sw = new StreamWriter(filename, false, Encoding.UTF8))
            {
                sw.Write(s.ToString());
                sw.Flush();
                sw.Close();
            }
        }

        public void Serialize(StringBuilder s)
        {
            s.AppendLine($"BEGIN FILE ({DateTime.Now:yyyy-MM-dd})");
            s.AppendLine("DOCUMENT TYPE=SPRIDEF2");
            s.AppendLine("DOCUMENT VERSION=1.0");
            s.AppendLine($"BEGIN SPRITES ({Count})");

            var count = 0;
            foreach (var sprite in this)
            {
                count++;
                s.AppendLine();
                s.AppendLine($"BEGIN SPRITE ({count}/{Count})");
                sprite.Serialize(s);
                s.AppendLine($"END SPRITE ({count}/{Count})");
            }

            s.AppendLine();
            s.AppendLine("END SPRITES");
            s.Append("END FILE");
        }

        public void Deserialize(string s)
        {
            try
            {
                Clear();

                var lines = s.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries
                );

                for (var i = 0; i < lines.Length; i++)
                    lines[i] = lines[i].Trim().ToUpper();

                var index = 0;

                var beginFile = Regex.Match(lines[index], @"^BEGIN FILE \(([0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9])\)$");

                if (!beginFile.Success)
                    throw new SerializationException("The first line should be a BEGIN FILE declaration.");

                var saveDateString = beginFile.Groups[1].Value.Split('-');

                try
                {
                    var _ = new DateTime(int.Parse(saveDateString[0]), int.Parse(saveDateString[1]), int.Parse(saveDateString[2]));
                }
                catch
                {
                    throw new SerializationException("BEGIN FILE declaration is incorrect.");
                }

                index++;

                if (lines[index] != "DOCUMENT TYPE=SPRIDEF2")
                    throw new SerializationException("Incorrect DOCUMENT TYPE.");

                index++;

                if (!lines[index].StartsWith("DOCUMENT VERSION="))
                    throw new SerializationException("Expected DOCUMENT VERSION.");

                var versionParts = lines[index].Split('=');
                var version = float.Parse(versionParts[1], NumberStyles.Any, CultureInfo.InvariantCulture);

                if (version > 1.0)
                    throw new SerializationException("This document is created using a newer version of SPRDEF2.");

                index++;

                if (!lines[index].StartsWith("BEGIN SPRITES ("))
                    throw new SerializationException("Expected BEGIN SPRITES followed by sprite count.");

                var beginSprites = Regex.Match(lines[index], @"^BEGIN SPRITES \(([0-9]+)\)$");

                if (!beginSprites.Success)
                    throw new SerializationException("Incorrect format on BEGIN SPRITES.");

                var count = int.Parse(beginSprites.Groups[1].Value);

                index++;

                var spritesData = new List<List<string>>();

                for (var i = 0; i < count; i++)
                {
                    var spriteData = new List<string>();
                    do
                    {
                        spriteData.Add(lines[index]);
                        index++;
                    } while (!spriteData.Last().StartsWith("END SPRITE ("));

                    spritesData.Add(spriteData);
                }

                if (lines[index] != "END SPRITES")
                    throw new SerializationException("Expected END SPRITES.");

                index++;

                if (lines[index] != "END FILE")
                    throw new SerializationException("Expected END FILE.");

                foreach (var spriteData in spritesData)
                {
                    var chunk = new SpriteChunkParser();
                    chunk.AddRange(spriteData);
                    Add(SpriteRoot.Parse(chunk));
                }
            }
            catch (Exception e)
            {
                throw new SerializationException($"This file contain errors: {e.Message}");
            }
        }
    }
}