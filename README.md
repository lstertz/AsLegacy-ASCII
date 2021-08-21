# AsLegacy-ASCII #
C# ASCII prototype of the As Legacy game concept, built using SadConsole.

## Release ##
The latest release is 0.2.1 (08/21/2021), for Windows only, found in the releases folder.

## Controls ##
W,A,S,D (or arrow keys) - Move, when in normal mode.<br>
Space - Toggle between normal and attack mode.<br>
Hold-Alt - Activate defense mode.<br>
Left mouse click to target a Character, either through the map or the map sidebar.<br>
1-6 - Activate corresponding equipped skill.

## Notes ##
An NPC can be damaged by targeting one, being adjacent to it (left, right, up, down), and 
being in attack mode. Using an attack skill, such as Shock Ring, can also damage an NPC. 
It will die if it loses all of its health.<br>
<br>
Any damage dealing action will earn a Talent point, which can be spent in the Talents panel on 
a Concept or a Passive by clicking the corresponding '+'. Diamonds next to a Concept are the 
Skills that can be learned from that Concept; clicking the diamond opens a prompt to learn the 
Skill, which is equipped to the first available Skill slot. Passive investments 
are applied passively. Every two points invested in a Passive earns a Successor Point. 
Successor points accumulate over generations and can be applied to the passives of each 
new successor.<br>
<br>
A Beast NPC will transfer all of its legacy to its killer upon death, while the 
Goblin and Player Character will only transfer half of its current legacy; its remaining legacy 
will pass on to its successor.<br>
<br>
An NPC will automatically attack anything adjacent to itself and has a chance to defend against 
an attack under the right conditions.<br>
<br>
The current goal is to achieve at least 25 legacy points.

## Follow Us ##
Development - https://trello.com/b/l1MBNnHy/as-legacy-c-ascii

## Project Details ##
### Coding Convention ###
The As Legacy code project conforms to the following guidelines:
* Class members are organized from top to bottom in order of:
	1. Nested classes
	2. Enums
	3. Constants
	4. Static Properties/Fields
	5. Static Functions
	6. Instance Properties/Fields (primarily ordered with public scope first)
	7. Constructors
	8. Instance methods
* Classes/Interfaces
	* Naming follows PascalCase.
	* Interfaces have a prefixed 'I' to their name (e.g. ICharacter).
	* Abstract classes can only have protected or private constructors.
* Enums
	* Naming follows PascalCase.
	* Named in the singular, unless it's a set of bits/flags.
* Constant members
	* Includes fields defined as 'const' or 'static readonly'.
	* Naming follows PascalCase.
* Static member naming follows PascalCase.
* Functions/Methods
	* Naming follows PascalCase.
	* Parameter naming follows camelCase.
* Property naming follows PascalCase.
* Fields
	* Public and Protected fields are not permitted, these should be Properties.
	* Private field naming follows camelCase, with a prefixed '_' (e.g. _value).
	* Readonly applied to any fields intended to be set on construction.
* XML Documentation
	* Requied for all public and protected members.
	* Required for all methods/functions.
	* Not required for protected or private constructors.
* File and folder structure follows similarly to any nested/inheriting class structure, 
where a folder is warranted whenever a class has at least one nested class, consists of 
several partial classes, or belong to a shared overarching concept with inheriting classes.