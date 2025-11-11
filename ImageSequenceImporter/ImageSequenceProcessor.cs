using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ImageSequenceVideoContent
{
    [ContentProcessor(DisplayName = "JPEG Sequence Processor")]
    public class ImageSequenceProcessor : ContentProcessor<ImageSequenceContent, Texture2D[]>
    {
        public override Texture2D[] Process(ImageSequenceContent input, ContentProcessorContext context)
        {
            var textures = new List<Texture2D>();

            foreach (var file in input.FileNames)
            {
                // Build and load each JPEG as a runtime Texture2D
                var texture = context.BuildAndLoadAsset<Texture2DContent, Texture2D>(
                    new Microsoft.Xna.Framework.Content.Pipeline.ExternalReference<Texture2DContent>(file),
                    "TextureProcessor");

                textures.Add(texture);
            }

            return textures.ToArray();
        }
    }
}
