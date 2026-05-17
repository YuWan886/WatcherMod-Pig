namespace Watcher.Code.Extensions;

public static class StringExtensions
{
    public static string CardImagePath(this string path)
    {
        return $"res://{WatcherMainFile.ModId}/images/card_portraits/{path}";
    }

    public static string PowerImagePath(this string path)
    {
        return $"res://{WatcherMainFile.ModId}/images/powers/{path}";
    }

    public static string BigRelicImagePath(this string path)
    {
        return $"res://{WatcherMainFile.ModId}/images/relics/{path}";
    }

    public static string TresRelicImagePath(this string path)
    {
        return $"res://{WatcherMainFile.ModId}/images/atlases/relic_atlas.sprites/{path}";
    }

    public static string PackedPotionImagePath(this string path)
    {
        return $"res://{WatcherMainFile.ModId}/images/potions/tres/{path}";
    }
}