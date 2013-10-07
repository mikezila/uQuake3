# uQuake

Load Quake 3 maps into Unity3D

### What is this?

It takes a normal Quake 3 .bsp and loads it into a Unity scene.

### What parts of the map are recreated?

- Faces and meshes.
- Textures and their mappings on said faces and meshes.

### What parts are not recreated?

- Entities are not used it any way.
- Bezier curves are implimented but broken horribly.
- Lightmaps.
- Normals.  Unity recalcuates them on its own, though.
- Liquids, skies, and any other texture that is produced by a shader in Quake 3.

### How do I use it?

Take a .bsp map and put it in the Resources/Maps/ folder.  It has to be a .bsp on its own, not a .pk3 containing a .bsp.  You can open a .pk3 as a .zip file and take the .bsp out of the maps directory and use that if you have a map you want to test.

Open the uQuake scene I've provided, and select the "worldSpawn" object in the scene.  Change the map name to the name of your .bsp, including the ".bsp" part.  So "q3dm1.bsp" is valid, "q3dm1" is not.

If you've ripped the textures out of Quake 3's .pk3 files you can place them (the textures directory and all its contents from the pak0.pk3) inside the resources directory and they'll be used.  Otherwise put a checkmark in "replace textures", and put a material with a texture of your choosing, which will be applied to all the surfaces.  I've included a material with some bricks on it for you to use in the Materials folder.

If you use a different map than the one I included you'll need to start the scene, pause it right away, and them move the fps character controller to a spot inside the map.  I don't process entities yet, so you have to setup your spawn point on your own.

### Is this done?

No, I'm still working away at it fiendishly.  I will create good docs and tidy up the code once it's finished.

I'm struggling with forming meshes from Quake 3's bezier patches right now, so if you have any input on how to do that I'd be your best friend forever.
