# Festival Feedback

**This is a tool for mod developers. It may be required as a dependency for their mods. It does nothing on its own!**

## What It Does

This mod creates conversation topics which last for a single day after the festival, based on the items placed in the pot at the Luau and the items submitted for judging at the Stardew Valley Fair.

These conversation topics can be used in any Game State Query to trigger things like events, mail or dialogue.

Topics are generated as the player goes to sleep.

## Topic Format
* Luau topics take the format of `Y{year}_LuauIngredient_{ItemId}`
* One Luau topic is created for each item placed in the pot.
* Stardew Fair Grange topics take the format of `Y{year}_GrangeDisplay_{ItemId}`
* One Grange Display topic is created for each display slot which contains an item.


