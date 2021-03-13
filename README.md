# AsLegacy-ASCII #
C# ASCII prototype of the As Legacy game concept, built using SadConsole.

## Release ##
The latest release is 0.1.3 (03/12/2021), for Windows only, found in the releases folder.

## Controls ##
W,A,S,D (or arrow keys) - Move, when in normal mode.<br>
Space - Toggle between normal and attack mode.<br>
Hold-Alt - Activate defense mode.<br>
Left mouse click to target a Character, either through the map or the map sidebar.<br>

## Notes ##
An NPC can be damaged by targeting one, being adjacent to it (left, right, up, down), and 
being in attack mode. It will die if it loses all of its health.<br>
<br>
A Beast NPC will transfer all of its legacy to its killer upon death, while the 
Goblin and Player Character will only transfer half of its current legacy; its remaining legacy 
will pass on to its successor.<br>
<br>
An NPC will automatically attack anything adjacent to itself and has a chance to defend against 
an attack under the right conditions.

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
	* Not required for private constructors.
* File and folder structure follows similarly to any nested/inheriting class structure, 
where a folder is warranted whenever a class has at least one nested class, consists of 
several partial classes, or belong to a shared overarching concept with inheriting classes.