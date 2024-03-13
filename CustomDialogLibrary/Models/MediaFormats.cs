namespace CustomDialogLibrary.Models;

public static class MediaFormats
{
    public static HashSet<string> ImageExtensions =>
    [
        ..new[]
        {
            ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi", ".webp", ".tiff", ".tif", ".psd", ".raw", ".arw", ".cr2",
            ".nrw", ".k25", ".bmp", ".dib", ".heif", ".ind", ".jp2", ".j2k", ".jpf", ".jpx", ".jpm", ".mj2", ".svg",
            ".ai", ".eps", ".pdf", ".png", ".gif"
        }
    ];
    
    public static HashSet<string> VideoExtensions =>
    [
        ..new[]
        {
            ".mkv", ".flv", ".vob", ".ogv", ".ogg", ".drc", ".mng", ".avi", ".MTS", ".M2TS", ".TS", ".mov",
            ".qt", ".wmv", ".yuv", ".rmvb", ".viv", ".asf", ".amv", ".mp4", ".m4p", ".m4v", ".mpg", ".mp2", ".mpeg",
            ".mpe", ".mpv", ".m2v", ".svi", ".3g2", ".mxf", ".roq", ".nsv", ".flv", ".f4v", ".f4p", ".f4a", ".f4b"
        }
    ];
    
    public static HashSet<string> AudioExtensions =>
    [
        ..new[]
        {
            ".3gp", ".aa", ".aac", ".aax", ".act", ".aiff", ".amr", ".ape", ".au", ".awb", ".dss", ".dvf", ".flac",
            ".gsm", ".ivs", ".m4a", ".m4b", ".m4p", ".mmf", ".mp3", ".mpc", ".msv", ".nmf", ".ogg", ".oga", ".mogg",
            ".opus", ".re", ".rm", ".raw", ".rf64", ".tta", ".voc", ".vox", ".wav", ".wma", ".wv", ".webm", ".8svx",
            ".cda"
        }
    ];
}