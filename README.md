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
- Baked lightmaps are pulled from the .bsp and applied.

### What parts are not recreated?

- Entities are not used in any way.  Yet.
- Billboards, like torches, aren't rendered at all yet.
- Liquids, skies, and any other texture that is produced by a shader in Quake 3.
    - Shader textures will be replaced with non-shader versions where possible.
- Animated textures don't animate, or are missing.
- Textures with transparency (banners, flags, etc) don't have it.
    - This could be worked around by writing a shader, but I haven't done it.

### How do I use it?

Take a .bsp map and put it in the Resources/Maps/ folder.  It has to be a .bsp on its own, not a .pk3 containing a .bsp.  You can open a .pk3 as a .zip file and take the .bsp out of the maps directory inside.

Open the uQuake scene I've provided, and select the "worldSpawn" object in the scene.  Change the map name to the name of your .bsp, including the ".bsp" part.  So "q3dm1.bsp" is valid, "q3dm1" is not.

To use Quake 3's real textures, open pak0.pak and copy the entire "Textures" folder into the Resources folder inside the Unity project.  Create a folder in the Resources folder called "models", and inside pak0.pak open the models folder.  Copy the "mapobjects" folder into the models folder in the Unity Project.  Then make sure that there is a check in the "Use Ripped Textures" box on the worldSpawn object in the scene.

If you don't have/don't want to use Quake 3's actual textures, then make sure "Use Ripped Textures" is unchecked, and then drag a material onto "Replacement Texture".  Inside the materials folder is a material with some bricks I have provided for you.

If you use a different map than the one I included you'll need to start the scene, pause it right away, and them move the fps character controller to a spot inside the map.  I don't process entities yet, so you have to setup your spawn point on your own.

Lightmaps from the .bsp will be loaded and applied if you check the option for it, but with Unity's shaders the lighting doesn't pop as much as in the original game.  They may look better once I have lights placed around where the light enenties are supposed to be, but I doubt it.  More likely I'll need to find a shader that handles the rgb lightmap better.  Quake 3 also used "lightvols" which were a grid of areas that all would receive fake dynamic lighting for things that weren't the map hull.  When you walk past a light in Quake 3 and you see the light on your gun, it's not because of a dynamic light, it's because of one of those lightvols.  Adding them to Unity is going to be tough, since Unity doesn't have a comparable technology in it already.  It'd probably be easier to just place realtime lights at the points specified in the bsp and tweak from there.

### Is this done?

No, I'm still working away at it fiendishly.  I will create better documentation and tidy up the code once it's finished.
