Date:	2015-08-15 16:23
Author:	Matthias Kellers (GER)

VersionEditor-Plugin for Unity

This small Plugin for Unity allows you to define a global Version for your Unity Project. Its creates a new menu "Version" under the existing menu "Edit => Project Settings".
In the submenu you will find the following options:
With "Edit Version" the "Edit Version - Window" will open. (Mentioned below.)
With "Configuration" the "VersionEditor Settings - Window" will open. (Mentioned below.)


**********************************************
First start
**********************************************
By default you do not need to set any further configurations. Just hit "Edit Version" in the "Version" menu you will find under "Edit => Project Settings".
By pressing on "Save" the plugin creates a new class with the versioninformation you set.
You can use this auto-generated class in your project to show any versioninformation on runtime. 
In addition the Plugin set the bundleVersion-Property in your PlayerSettings. 
So you do not need to double set the version in any static class for your game and in the playersetting for mobile devices.



Windows:
**********************************************
Edit Version - Window:
**********************************************
Here you can set the Version of your project inspired by the "Assembly Information"-Window of Visual Studio. (Just the important fields (imho))
By pressing on "Save" the plugin will create an new class with static fields containing the information you set.
In addition, the PlayerSettings.bundleVersion and (optional) the PlayerSettings.bundleIdentifier will be set.
Also you can set the name of your company and the title of your project which will optionaly passed to the bundleIdentifier as mentioned above.


**********************************************
VersionEditor Settings - Window
**********************************************
With this window you can set general settings vor the VersionEditor-Plugin.

You can choose in which language the classfile should be generated (JavaScript or C#)
You can set the location (relative to the Asset-Folder of the Unity project), where the class will be saved.
You can set the namespace which will used in the auto-generated class.
You can set the name of the auto-generated class and implicitly the filename of the class.

By pressing on "Save" the plugin will create a new class, if you made any important changes (i.e. namespace, classname, language). 
If you only changed the location, the class will be moved to the new directory. (Only if the class already exists!)

By pressing on "Force refresh AssetDataBase" you force Unity to refresh its Asset-Database. Which is also done if you hit "Save". 
This is necessary so that the generated class will be visible (or updated) in your Unity project.

By pressing on "Force generate new class" you force the plugin to generate the class without any further validation.

Just use these two buttons if you have any problems with generating the class or refreshing the AssetDataBase.
