#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using EditStateSprite.Serialization;

namespace EditStateSprite;

public class SpriteList : List<SpriteRoot>
{
    public const double DocVersion = 1.1;

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
        using var sw = new StreamWriter(filename, false, Encoding.UTF8);
        sw.Write(s.ToString());
        sw.Flush();
        sw.Close();
    }

    public void Serialize(StringBuilder s)
    {
        s.AppendLine($"BEGIN FILE ({DateTime.Now:yyyy-MM-dd})");
        s.AppendLine("DOCUMENT TYPE=SPRIDEF2");
        s.AppendLine($"DOCUMENT VERSION={DocVersion.ToString("0.0", CultureInfo.InvariantCulture)}");
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

    public void Deserialize(string s) =>
        new SpriteRootListDeserializer(s).ParseTo(this);

    public List<SpriteRoot> GetAll() =>
        this.ToList();
}