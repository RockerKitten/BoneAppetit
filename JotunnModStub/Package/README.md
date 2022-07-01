# Introducing the newest fad in Viking cuisine! Hope you enjoy, Bone Appetit!

Description revamp coming soon!

## Changelog:

-v. 1.0.0
	-release includes 2 food crafting stations that require a fire underneath to cook on, and 5 new food items, one for each biome.
-v. 1.0.1 	
	-hotfix for those not using V+
-v. 1.1.0 	
	-fix for the grill making it difficult to load the fire underneath
	-fix for Fried Lox not auto picking up
	-5 new foods. Pancakes, Smoked Fish, Bacon, Coffee, and Pizza
-v. 1.1.1
	-updated visuals on bacon
	-Fixed collider on Griddle
	-Fixed attach point on kabob 
-v. 2.0.0	  
	-Code rework
	-Updated Bacon Visuals (thicker) and fixed attach point
	-Added "Elemental" Cream Cones updated visuals on Ice Cream
	-First Rebalance pass
	-Added Porridge
	-Added A PJB for a special little boy by request.
	-Added CONFIG!!! (only a true/false for enabling/disabling recipes, likely will stay this way) Server synced. 
	-Everything defaults to on, when loading for the first time.
	-Redid all "Flavor" text
        
			 ### -MAKE SURE YOU HAVE THE MOD AND CONFIG ON THE SERVER FOR THE SERVER SYNC TO WORK-
	
-v. 2.0.1
	-Fixed "CloudBerry" so we can have CAKE! (Big thanks to PROXiCiDE for pointing this out to me)
-v. 3.0.0
	-Added MasterChef 2.0 assets. Omlette, Nut-ella, Bloodsausage, Broth, Carrot Butter, Burgers, and Fish Stew.
	-New Drops! You can now loot Pork from Boars, smaller Transportable Dragon Eggs from Drakes, and eggs from seagulls.
	-New food station, the Prep Staton. Unlocks with Tin.
	-More new food, Haggis, Moochi, Carrot Sticks, Boiled Eggs, and Candied Turnips.
	-Major balance pass, changed some recipes and food values, moved all food out of the cauldron and spread between Prep, griddle, and grill.
	-New Smokeless fires, for those of you struggling with FPS drops due to smoke particle physics, or who just want an indoor kitchen. There is now a campfire, a hearth that no longer produce particle smoke. (can be disabled in the config).
v. 3.0.1
	-Hotfix for boar drops when using with CLLC calculate Amount
	-Fix for missing Omlette
	-Added smokeless Braziers by request.
v. 3.0.2
	-Load order fix
	-maybe fix for prep table fire (hopefully)
- v. 3.1.0
    - Added Cooking Skill
      - Gain XP when cooking at a **Cooking Station**, or cooking a Consumable at the **rk_griddle**, **rk_grill**, **rk_prep** or **piece_cauldron**. Each level of cooking skill adds a 1% chance to craft an additional consumable. after lvl 25, your total cooking level is divided by 4 and the results is the % chance to crafted a 2nd additional consumable. 
      - e.g. At level 100, there is a 100% chance to craft an additional consumable and 25% chance to craft a 2nd additional consumable on top of that. At level 100 a food that crafts in stacks of x5, if lucky could output 7 total items.
      - Set **CookingSkillEnable** = false to disable.
    - Added SE_CheffHat
      - Improves Cooking Skill XP Earned while wearing the Chef Hat. 
      - Set **HatXpGain** = 1 to disable.
      - Taking the Chef Hat on and off displays a random Julia Child's quote. 
        - Displaying this message can be disabled by setting **HatSEMessage** to false.
    - Added Configs
      - CookingSkillEnable (server synced, enforced)
      - BonusWhenCookingEnabled (server synced, enforced)
      - HatXpGain (server synced, enforced)
      - HatSEMessage (local)
    - Added NexusId for update notice mod.
v. 3.2.0
	- updated for Jotun
	- fixed versioning so it shows correct version everywhere.


## Installation:

Place the dll  in the plugins folder, load the game to the start  screen and *quit*. this will generate the config for the first time.
Known Issue: Food *WILL NOT* show up to be crafted the verry first time you load in after install and config generation. please 
###***generate the config, quit and restart.***
			

Note: this mod requires JVL (Jotunn the Valheim Lib) and it's dependent HookGenPatcher, it will not work without it.


### Cooking Stations:

Griddle		- buildable with 10 stone and a hammer. Allows for custom food as soon as you find a place to build it.
Grill  		- requires a forge, 1 iron and 10 stone to build. unlock higher tier grilled foods.
Oven		- piece addon for the grill. (added in 2.0.0) requires surtling cores, and surtling trophies.
Prep Table	- New station in v3.0.0, all ice cream was moved here. requires Tin

### Food By release:

note: all food that uses "drake egg" uses the new drake egg drop from Drakes. and "egg" is from seagulls.

#### Initial Release v1.0.0

- Pork Rinds 
	- pork
	- scraps
- Honey Glazed Carrots 
	- Carrots
	- Honey
	- Dandelion
- Kabobs
	- Raw meat
	- Bone fragments
	- Carrot
	- Turnip
- Ice Cream Cone
	- freeze gland
	- drake egg
	- Honey
	- blueberry
- Country Fried Lox Meat
	- lox meat
	- butter
	- barley flour
	- egg (new seagull drop)


#### v. 1.1.0

- Smoked Fish
	- raw fish


##### Food Menu Assets Courtesy of zarboz

-Bacon
	- raw pork
-Coffee
	- ancient seeds
-Pizza
	- mushroom
	- flour
	- egg
	- raw meat
-Pancakes
	- flour
	- egg
	- honey
	- butter


#### v. 2.0.0

- Porridge
	- barley
	- cloudberry
	- honey
	- butter
- Fire cream cone
	- SurtlingCore
	- Raspberry
	- honey
	- drake egg
- Electric cream cone
	- Crystal
	- Cloudberry
	- Honey
	- drake egg
- Acid cream cone
	- Guck
	- MushroomYellow
	- Honey
	- drake egg
- PBJ
	- Queens Jam
	- Bread
	- Nut-ella
- Cake
	- egg
	- flour
	- cloud berry
	- honey


#### v. 3.0.0

- Haggis
	- entrails
	- raw meat
	- carrot
	- turnip
- Moochi
	- freeze gland
	- blueberry
	- drake egg
	- honey
- Candied Turnips
	- honey
	- turnip
	- thistle
- Boiled Eggs
	- eggs
- Carrot Sticks
	- carrots
	- nut-ella


##### Master Chef2.0 Assets

- Omlette
	- egg
	- raw pork
	- thistle
	- butter
- Broth
	- bone fragments
	- butter
- Butter
	- carrot seeds
- Fish Stew
	- raw fish
	- broth
	- thistle
	- egg
- Burger
	- raw lox meat
	- raw meat
	- bread
	- turnip
- Bloodsausage
	- entrails
	- raw pork
	- thistle
	- blood bag
- Nut-ella
	- Beech Seeds
	- butter


### Cooking Stations by unlock

Griddle - Stone
Prep Station - Tin
Grill - Iron
Oven - Addon for grill (Grill level +1) -Surtling Cores, and Trophies


##### To Do/ Current Ideas

x-add food menu items (done)
x-expand menu (done untill H&H is released)
-localizaion
x-add another new processing station for food and expansions (done)
x-add new food drops from creatures(done)
-meals (delayed until H&H)
x-config sync (done)

This mod is routinely tested on a dedicated server with a great many other mods. To ensure your crafting stations don't disappear, and that food doesn't turn to dust, please put this on the dedicated server as well as ALL clients.


Huge thanks to zarboz, GraveBear, and plumga for helping me get going, setting me up, and encouraging me the whole way! This mod wouldn't exist without them.
Also a big thanks to my "players" on my server (my husband and our good friend) Itsmesds, and JaxomFaux who've helped with ideas and balance from the start. (Bone Appetit's name came from us haning out when itmesds said how about "bone appetit" and it stuck)
