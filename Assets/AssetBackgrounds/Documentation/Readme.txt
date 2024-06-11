This is a simple space parallax for space shooter games

The prefabs are ready to go for the scene, just make new scene and drag and drop the prefabs and set their scale and position according to your camera.

if you want to make you own new background with these textures you just have to use a quad and resize it accourding to your camera and make a new texture in accourdance of your new background and apply to that quad. 

you can use different shader preperties for different behaviour e.g using additive for glowing.

after applying the material you need to assign them the scripts to objects , apply MainMenuScroller.cs to the materialed objects such as quad.

there are also saperate shapes in the projest files, you need to assign the ScrollingItems.cs to single shapes if you are using them in your background 