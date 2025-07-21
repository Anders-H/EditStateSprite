#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace EditStateSprite.Serialization;

public class SpriteRootListDeserializer
{
    private readonly string _source;

    public SpriteRootListDeserializer(string source)
    {
        _source = source;
    }

    public void ParseTo(SpriteList spriteList)
    {
        try
        {
            spriteList.Clear();
            var lines = _source.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < lines.Length; i++)
                lines[i] = lines[i].Trim();

            var index = 0;
            var beginFile = Regex.Match(lines[index], @"^BEGIN FILE \(([0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9])\)$");

            if (!beginFile.Success)
                throw new SerializationException("The first line should be a BEGIN FILE declaration.");

            var saveDateString = beginFile.Groups[1].Value.Split('-');

            try
            {
                _ = new DateTime(int.Parse(saveDateString[0]), int.Parse(saveDateString[1]), int.Parse(saveDateString[2]));
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
            var docVersionInt = (int)Math.Round(SpriteList.DocVersion * 100);
            var versionInt = (int)Math.Round(version * 100);

            if (versionInt > docVersionInt)
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
                spriteList.Add(SpriteRoot.Parse(chunk));
            }
        }
        catch (Exception e)
        {
            throw new SerializationException($"This file contain errors: {e.Message}");
        }
    }
}