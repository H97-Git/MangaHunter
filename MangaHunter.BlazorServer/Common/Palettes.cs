using MudBlazor;

namespace MangaHunter.BlazorServer.Common;

public static class Palettes
{
    public static Palette GetPaletteFromString(string? theme)
    {
        return theme switch
        {
            "Latte" => Latte,
            "Frappé" => Frappe,
            "Macchiato" => Macchiato,
            "Mocha" => Mocha,
            _ => Latte
        };
    }

    public static string GetStringFromPalette(Palette? palette)
    {
        return palette == Frappe
            ? "Frappé"
            : palette == Macchiato
                ? "Macchiato"
                : palette == Mocha
                    ? "Mocha"
                    : "Latte";
    }

    public static Palette Latte { get; } = new()
    {
        Primary = "#1e66f5",
        Secondary = "#ea76cb",
        Tertiary = "#179299",
        Surface = "#dce0e8",
        Background = "#eff1f5",
        BackgroundGrey = "#ccd0da", // MangaUpdateTable Scan Groups
        AppbarBackground = "#1e66f5",
        TextPrimary = "#4c4f69",
        TextSecondary = "#5c5f77",
        Info = "#7287fd",
        Success = "#40a02b",
        Warning = "#fe640b",
        Error = "#e64553",
    };

    public static Palette Frappe { get; } = new()
    {
        Primary = "#8caaee",
        Secondary = "#f4b8e4",
        Tertiary = "#81c8be",
        Surface = "#292c3c",
        Background = "#303446",
        BackgroundGrey = "#414559", // MangaUpdateTable Scan Groups
        AppbarBackground = "#8caaee",
        TextPrimary = "#c6d0f5",
        TextSecondary = "#b5bfe2",
        Info = "#babbf1",
        Success = "#a6d189",
        Warning = "#ef9f76",
        Error = "#ea999c",
    };

    public static Palette Macchiato { get; } = new()
    {
        Primary = "#8aadf4",
        Secondary = "#f5bde6",
        Tertiary = "#8bd5ca",
        Surface = "#1e2030",
        Background = "#24273a",
        BackgroundGrey = "#363a4f", // MangaUpdateTable Scan Groups
        AppbarBackground = "#8aadf4",
        TextPrimary = "#cad3f5",
        TextSecondary = "#b8c0e0",
        Info = "#b7bdf8",
        Success = "#a6da95",
        Warning = "#f5a97f",
        Error = "#ee99a0",
    };

    public static Palette Mocha { get; } = new()
    {
        Primary = "#89b4fa",
        Secondary = "#f5c2e7",
        Tertiary = "#94e2d5",
        Surface = "#181825",
        Background = "#1e1e2e",
        BackgroundGrey = "#313244", // MangaUpdateTable Scan Groups
        AppbarBackground = "#89b4fa",
        TextPrimary = "#cdd6f4",
        TextSecondary = "#bac2de",
        Info = "#b4befe",
        Success = "#a6e3a1",
        Warning = "#fab387",
        Error = "#eba0ac",
    };
}