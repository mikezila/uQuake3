# uQuake

Load Quake 3 maps into Unity3D

### What is this?

It takes a normal Quake 3 .bsp and loads it into a Unity scene.

### What parts of the map are recreated?

- Level geometry is 100% working.
    - Polygons
    - Meshes
    - Bezier patches
- Textures are applied and mapped correctly.
- Baked lightmaps are pulled from the .bsp and applied.  Sort of.  They're broken at current until I find a better way to apply them.  They are supported though.

### What parts are not recreated?

- Entities are not used in any way.  Yet.
- Billboards, like torches, aren't rendered at all yet.
- Liquids, skies, and any other texture that is produced by a shader in Quake 3.
    - Shader textures will be replaced with non-shader versions where possible.
- Animated textures don't animate, or are missing.
- Textures with transparency (banners, flags, etc) don't have it.
    - This could be worked around by writing a shader, but I haven't done it.

### How do I use it?

You'll need the PAK0.PK3 file from a retail Quake 3 install.  The shareware/demo version will probably work fine as well.  Place it in the empty baseq3 folder and open Unity.  You can try using the uQuake.unity scene I provided, but chances are it won't work because of reasons.  If it doesn't, create a new scene, and place an empty at the origin, then attach the "GenerateMap" script to it.  Make sure there's a camera, and put the "MouseCam" script on it, so you can fly around the results.  On the empty you created, there are several options.  Choose what you like.  Lightmaps are broken at current, so they will either not work at all, cause the map generation to fail, or some other oddness will occur.  Turn on rip textures to parse the textures from the PAK0.PK3 file if you like.  Make sure that the "Replacement Texture" material slot has something in it.  I provided a texture of some bricks that you can slap in there on a diffuse material that works well enough.  Set the tesselation to something reasonable, like 5 or 8.  Type in the map name you want, including the .bsp extension.  Like "q3dm1.bsp" or similar.  Make sure that "Map is Inside PK3" is checked.  

You can load a .bsp from outside of the .pk3 as well if you like.  Just make a "maps" folder inside the baseq3 folder and place it in there, just like real Quake 3.  In this case make sure "Map is Inside PK3" is unchecked.

Let it rip.  Hit play, wait a while, currently around 20 seconds for larger maps on quicker computer, and then fly around.  That's it.

### Is this done?

No, I'm still working away at it.  I will create better documentation and tidy up the code once it's finished.  I just recently returned to this after sharpening my C# skills a ton, so I'm currently in the process of refactoring some pretty embarassing code, and cleaning up here and there.  I'll see about maybe providing a simple .dll to use once I have it hammered out, so you can have Quake 3 map support in your games and tools
