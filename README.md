# uQuake

Load Quake 3 maps into Unity3D

### What is this?

It takes a normal Quake 3 .bsp and loads it into a Unity scene.

### What parts of the map are recreated?

- Level geometry is 100% working.
-- Faces
-- Meshes
-- Bezier patches
- Textures are applies and mapped correctly.

### What parts are not recreated?

- Entities are not used in any way.  Yet.
- Billboards, like torches.
- Lightmaps are ripped and recreated as textures, but they're broken right now.
- Normals.  Unity recalcuates them on its own, though.
- Liquids, skies, and any other texture that is produced by a shader in Quake 3.
- Animated textures are missing/don't animate.
- Textures with transparency (banners, flags, etc) don't have it, yet.

### How do I use it?

Take a .bsp map and put it in the Resources/Maps/ folder.  It has to be a .bsp on its own, not a .pk3 containing a .bsp.  You can open a .pk3 as a .zip file and take the .bsp out of the maps directory inside.

Open the uQuake scene I've provided, and select the "worldSpawn" object in the scene.  Change the map name to the name of your .bsp, including the ".bsp" part.  So "q3dm1.bsp" is valid, "q3dm1" is not.

To use Quake 3's real textures, open pak0.pak and copy the entire "Textures" folder into the Resources folder inside the Unity project.  Create a folder in the Resources folder called "models", and inside pak0.pak open the models folder.  Copy the "mapobjects" folder into the models folder in the Unity Project.  Then make sure that there is a check in the "Use Ripped Textures" box on the worldSpawn object in the scene.

If you don't have/don't want to use Quake 3's actual textures, then make sure "Use Ripped Textures" is unchecked, and then drag a material onto "Replacement Texture".  Inside the materials folder is a material with some bricks I have provided for you.

If you use a different map than the one I included you'll need to start the scene, pause it right away, and them move the fps character controller to a spot inside the map.  I don't process entities yet, so you have to setup your spawn point on your own.

If you want to look at the (currently broken) lightmap support, turn off all the lights in the scene and put a check in "Use Lightmaps".  You must use ripped textures for this to work.

### Is this done?

No, I'm still working away at it fiendishly.  I will create better documentation and tidy up the code once it's finished.
