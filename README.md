# uQuake 3

Load Quake 3 maps into Unity3D

### What is this?

It takes a normal Quake 3 .bsp and loads it into a Unity scene.

### What parts of the map are recreated?

- Face geometry is 100% working.
    - Polygons
    - Meshes
    - Bezier patches
- Textures pulled from the .pk3 and applied.
- Baked lightmaps are pulled from the map and applied.

### What parts are not recreated?

- Entities are not used in any way.
- Billboards, like torch flames, aren't rendered at all.
- Liquids, skies, and any other texture that is produced by a shader in Quake 3.
    - Shader textures will be replaced with non-shader versions where possible.
- Animated textures don't animate, or are missing.
- Textures with transparency (banners, flags, etc) don't have it.
    - Well actually they do, but currently I'm using a shader for the lightmaps that doesn't support transparency.

### How do I use it?

Create an empty scene and place an empty at the origin with the "GenerateMap.cs" script on it.  Create a folder called "baseq3" inside your Assets folder.  Place the .pk3 files from a retail/Steam/demo Quake 3 installation into that folder.  You can add .pk3 files containing custom maps if you like, as well.  On the empty you placed in the scene, put a material to use on surfaces that can't be textured correctly into the "Replacement Texture" field.  This will be used on surfaces that have missing textures for any reason.  Now type in the map name you would like to load, including the .bsp file extension.  "q3dm1.bsp" is the first map from Quake 3, if your memory is failing you.  Set the other options as you like.  Make sure "Map is Inside PK3" is checked if you're loading the maps that came with Quake 3, or a custom map that is inside of a .pk3 file.

I recommend placing a camera in the scene and attaching the included "MouseCam.cs" script, so you can fly around the rendered level easily.

You can load a .bsp from outside of the .pk3 files as well if you like.  Just make a "maps" folder inside the "baseq3" folder and place the loose maps in there, just like real Quake 3.  In this case make sure "Map is Inside PK3" is unchecked.

Let it rip.  It shouldn't take more than a few seconds on smaller maps, and maybe ten or so seconds for larger maps.  Performance is decent on modern video cards.  Anything as powerful as an Intel 4000 should have no issues rendering even big maps.

### Is this done?

No, I'm still working away at it.  I recently picked this project up again after negelcting it for a few months.  In those few months my C# skill increased significantly.  I'm in the process of cleaning up some bad design and fixing dumb mistakes as I find them.  Once I think the project is ready for more mass consumption I'll create better documentation and likely provide a .dll to use, which will make adding Quake 3 maps support to your projects and tools easy.  The source will always be available here.

### What games does this support?

I have only tested it extensivly with Quake 3: Arena maps.  In theory it will work with any game that uses the Quake 3 map and archive format.  This doesn't meany any game that uses the Quake 3 engine will work, only ones that use the Quake 3 map and archive format exactly.

### Will you had Quake 1/2/4 support?  Half-Life 1/2?

Short answer: no.

Long answer: Probably not.  I have a sister project to this one that loads Quake 1 maps, but Quake 1 maps are very different than Quake 3 maps.  Quake 2 is somewhere in the middle.  Half-Life is very very close to Quake 1.  Half-Life 2 is more complex than the others, but would be doable I think.  It would require its own project, though.  I'm not sure it would work very well.  Half-Life 2's levels require a lot of props and image effects for their final look and feel, and that would be hard to recreate in Unity with just the data in the map.

## Can I fork/use this?  What license is this?

By all means.  You don't need to give me any credit or anything.  Have fun.  This project is in the public domain.
