# uQuake

Load Quake 3 maps into Unity3D

### What is this?

It takes a normal Quake 3 .bsp and loads it into a Unity scene, making gameobjects and such as it goes.

### What parts of the map are recreated?

Currently it recreates the geometry of the level fully, save for bez curves and billboards.  Keep in mind water/slime/lava will be solid.  It also rips and applies texture coords to the surfaces, so once textures are loaded they'll look like they should.  I hope.  Right now it makes a gameobject for every single face, which isn't the best way I'm sure.  Collision is also working, as are the uvs (which are just the tex coords) so lighting works as well.

### How do I use it?

Take a .bsp map of your choice (not a .pk3, you have to open it up and pull the .bsp out) and name it test.bsp and put it in a folder called "Maps" in your assets folder.  Inside the uQuake folder attach the script "GenerateMap" to any gameobject in the scene (a directional light is a good choice) and then start the scene.

Entities aren't parsed at the moment, so you'll need to add a character controller and position it manually to be able to walk around.  It also doesn't add lights to the map, so I recommend attaching one to your character controller so you can walk around and inspect the map.

### Is this done?

No, I'm still working away at it fiendishly.  I will create good docs and tidy up the code once it's finished.
