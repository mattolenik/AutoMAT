This is an old tool made for the [Jedi Knight](https://en.wikipedia.org/wiki/Star_Wars_Jedi_Knight:_Dark_Forces_II) [modding community](https://www.massassi.net).

Jedi Knight's SITH engine supported a 16-bit RGB565 or RGBA5551 bitmaps for in-game textures. Tools had existed for years for converting common bitmap files into MAT format, but were antiquated, slow and painful to use, and produced mediocre results, particularly when it came to dithering or generating [mipmaps](https://en.wikipedia.org/wiki/Mipmap).

I made this tool to address the shortcomings of the old tools. By this point people had hacked the engine and were using hi-res 1024x1024 textures and the like, but were still stuck with ancient tools for converting textures.

AutoMAT provides the following:
 * Uses file watching to automatically convert textures as they are saved in their image editor
 * Supports Photoshop `PSD` files as an input format, allowing artists to save the file and have it instantly converted into a MAT without an intermediary bitmap export step
 * Batch converstion with a simple CLI
 * Automatic creation of mipmaps using Lanczos filtering, without the edge artifacts found in other tools
 * Optional dithering of the source image from true color to 16-bit, with choice of several dithering algorithms
 * Is very, very fast, can convert hundreds of textures in seconds on modest hardware

The speed and flexibility gives modders of this ancient game something resembling an asset pipeline in a modern game. I'm not sure how widespread its use was/still is, but at least a few folks told me they found it very useful for their projects :)

Implemented with the cross-platform image library, [DevIL](http://openil.sourceforge.net).
