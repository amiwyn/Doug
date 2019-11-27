using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace ImageCropper
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles("icons");
            foreach (var file in files)
            {
                using (var image = Image.Load(file))
                {
                    image.Mutate(ctx => ctx.Crop(new Rectangle(8, 8, 32, 32)));
                    image.Save(file);
                    Console.WriteLine($"Processed {file}");
                }
            }
        }
    }
}
