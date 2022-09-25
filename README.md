# Note
This repository is a clone of the orginal repository hosted on https://github.gatech.edu/. The link to the original is below.
https://github.gatech.edu/sshahryar3/TeamLeftovers

# BaroqueBreakout
3rd person single player stealth game built with Unity
CS 4455 / 6457
Team Leftovers - Baroque Breakout

# Members
Bryan Herrera / bryan.herrera@gatech.edu / 
Eric Gu / egu9@gatech.edu / egu9
Sahir Shahryar / 
Jin Hyung Park / jpark702@gatech.edu / jpark702
Seong Ryoo / sryoo6@gatech.edu / sryoo6


# BUILD INSTRUCTIONS
Run BaroqueBreakout.exe to play the game.In the main menu, the player has the option to play the game directly or play the tutorial level. We were not able to build a macOS version; Unity warns us that the "Mac IL2CPP player can only be built on Mac."


# GAMEPLAY
The game is a stealth game and your objective is to steal all 8 vases from different sections of the area before the time runs out! You have 15 minutes to steal and escape the night museum or you will lose. You start with 100 health points and, if your health reaches 0, the game is over . If you avoid  taking damage for a while, you will regenerate health over time. You can view your health bar in the lower-left corner of the HUD.


There are two nonplayer characters (NPC). The first NPC are security guards who patrol their assigned areas. They are equipped with a flashlight which helps them see in the night. You should avoid being spotted in the light, otherwise the security guard will begin his pursuit. You can escape the pursuit by either running away and getting away from their line of sight or hiding in a painting. The guards will lose track of you once you are in 2D.  However, you should be wary about entering a painting, because, even though it hides you from foes, it also takes away from your hp. If you sneak up from behind a guard, you have the ability to knock out the guard and permanently keep them from coming back. This gives the opportunity to steal a guard’s flashlight, which can be toggled on and off from that point on to improve your vision. Furthermore, you can also set off an alarm if you walk over a laser trip mine. This will not deal damage to you, but this will alert the guard closest to the area to investigate the area where the alarm was set off, so you have to be very careful where you step.


The second NPCs are crawling spiders that deal 50 damage to you if they get near their striking distance. You can only run away or hide from the spiders. You do not have the option to kill the spiders. However, if you have the guard’s flashlight, you can directly point the light at the spiders to stall them. Spiders are more aware of their surroundings, which means they can spot you in a 360 view. However, they have a very short viewing range. If you keep your distance from the spider, you will never get detected by it.
 
The map is broken into two sections. In the first section, you must collect all 4 vases before you can advance to the second half of the map. Once you collect all 4 vases, the sliding doors which are located in the middle of the map will open. This will give you access to the second half of the map. Once you are given access to the second part of the map, you must collect the last 4 remaining vases. In total, you will steal 8 precious and valuable vases from the museum. You must finish before 15 minutes otherwise you lose the game.


*** Additional features, there are waypoints to help the player navigate across this whole map. There are checkpoints just in case the player dies or gets stuck in a glitch. There is a tutorial map that the player can access from the main menu which helps them learn the game before they enter the real map.


# CONTROLS
The player can use the WASD keyboard keys to move the character around in the environment. The player can press “E” on the keyboard to enter and exit out of a painting. Holding “shift” while moving allows the player to sprint which does drain the player’s stamina bar.  When the player is right behind (has to come in contact) a patrolling guard, the player can press the “Q” key to knock the guard out. This will allow the player to loot the guard’s flashlight, which can be toggled on/off by the “R” key. To pause the game the player can press “ESC” and then press it again to unpause the game. In the pause menu, the player can either restart the level, load the last checkpoint, close the game, resume the gameplay, or exit to the main menu.


# KNOWN BUGS/DEFICIENCIES
There are 89 unique paintings on this map. We have had a few issues with the player getting stuck in the walls. It has been very hard and tedious to find and know which paintings are glitched. So the player may encounter a few glitched paintings. No worries there, the player can simply load their last checkpoint without having to restart the whole level again.


Player takes 25 damage upon entering Chapter 1 for an unknown reason.


# EXTERNAL ASSETS
PB_Spider: Model/texture/animation: https://assetstore.unity.com/packages/3d/characters/animals/insects/animated-spider-22986


Bodyguards: 
-Model/textures: https://assetstore.unity.com/packages/3d/characters/humanoids/humans/bodyguards-31711
-Animation: https://assetstore.unity.com/packages/3d/characters/humanoids/zombie-30232
-Footstep sound: https://freesound.org/people/shall555/sounds/140219/
-Grunt sound: https://freesound.org/people/Dirtjm/sounds/262279/
-Body collapse sound: https://freesound.org/people/Suburbanwizard/sounds/417994/
-Flashlight popup image: https://www.coolthings.com/nitecore-tiny-monster-tm16gt/
-Unity’s official guide for enemy AI : https://www.youtube.com/watch?v=mBGUY7EUxXQ&t=17s 
-Unity Community Wiki’s cone gameobject editor: https://wiki.unity3d.com/index.php/CreateCone


Main Character:
-Animation: https://assetstore.unity.com/publishers/36307
-Footstep noise: https://freesound.org/people/Yoyodaman234/sounds/166509/


Game over, shift in, shift out, and objective complete sound effects : https://assetstore.unity.com/packages/audio/sound-fx/sound-fx-retro-pack-121743


Level theme and Tense theme music: https://assetstore.unity.com/packages/audio/music/orchestral/free-game-music-collection-177094
Speed up powerup: https://assetstore.unity.com/packages/3d/cola-can-96659
