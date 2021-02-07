# AsLegacy-ASCII
C# ASCII prototype of the As Legacy game concept, built using SadConsole.

## Release
The latest release is 0.1.1 (02/07/2021), for Windows only, found in the releases folder.

## Controls
W,A,S,D (or arrow keys) - Move, when in normal mode.<br>
Space - Toggle between normal and attack mode.<br>
Hold-Alt - Activate defense mode.<br>
Left mouse click to target a Character, either through the map or the map sidebar.<br>

## Notes
An NPC can be damaged by targeting one, being adjacent to it (left, right, up, down), and 
being in attack mode. It will die if it loses all of its health.<br>
<br>
A Beast NPC will transfer all of its legacy to its killer upon death, while the 
Goblin will only transfer half of its current legacy; its remaining legacy will pass on 
to its successor.
<br>
Currently, the Player's character is named "Player of Game", where "Player" is the name and 
"Game" is the name of the character's lineage (its legacy).

## Follow Us
Development - https://trello.com/b/l1MBNnHy/as-legacy-c-ascii