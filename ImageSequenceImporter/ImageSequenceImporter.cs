using ImageSequenceVideoContent;
using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.IO;

[ContentImporter(".seq", DisplayName = "JPEG Sequence Importer", DefaultProcessor = "ImageSequenceProcessor")]
public class ImageSequenceImporter : ContentImporter<ImageSequenceContent>
{
    public override ImageSequenceContent Import(string filename, ContentImporterContext context)
    {
        string folder = Path.GetDirectoryName(filename);
        string[] files = Directory.GetFiles(folder, "*.jpg");
        Array.Sort(files);

        return new ImageSequenceContent
        {
            FolderPath = folder,
            FileNames = files
        };
    }
}
