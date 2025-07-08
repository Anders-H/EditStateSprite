#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace EditStateSprite.SpriteModifiers;

public enum PreviewAnimationBehaviour
{
    Animate,
    ShowAlways,
    Hide
}

public static class PreviewAnimationBehaviourHelper
{
    public static List<PreviewAnimationBehaviour> GetAll() =>
        Enum.GetValues(typeof(PreviewAnimationBehaviour)).Cast<PreviewAnimationBehaviour>().ToList();

    public static string GetDescription(PreviewAnimationBehaviour behaviour) =>
        behaviour switch
        {
            PreviewAnimationBehaviour.Animate => "Animate",
            PreviewAnimationBehaviour.ShowAlways => "Show always",
            PreviewAnimationBehaviour.Hide => "Hide",
            _ => throw new ArgumentOutOfRangeException(nameof(behaviour), behaviour, null)
        };

    public static PreviewAnimationBehaviour GetValue(string description) =>
        description switch
        {
            "Animate" => PreviewAnimationBehaviour.Animate,
            "Show always" => PreviewAnimationBehaviour.ShowAlways,
            "Hide" => PreviewAnimationBehaviour.Hide,
            _ => throw new ArgumentException($@"Unknown description: {description}", nameof(description))
        };

    public static string Serialize(PreviewAnimationBehaviour behaviour) =>
        behaviour switch
        {
            PreviewAnimationBehaviour.Animate => "Animate",
            PreviewAnimationBehaviour.ShowAlways => "ShowAlways",
            PreviewAnimationBehaviour.Hide => "Hide",
            _ => throw new ArgumentOutOfRangeException(nameof(behaviour), behaviour, null)
        };

    public static PreviewAnimationBehaviour Deserialize(string? value) =>
        value switch
        {
            "Animate" => PreviewAnimationBehaviour.Animate,
            "ShowAlways" => PreviewAnimationBehaviour.ShowAlways,
            "Hide" => PreviewAnimationBehaviour.Hide,
            _ => throw new ArgumentException($@"Unknown value: {value}", nameof(value))
        };

    public static List<string> GetDescriptions() =>
        GetAll().Select(GetDescription).ToList();
}