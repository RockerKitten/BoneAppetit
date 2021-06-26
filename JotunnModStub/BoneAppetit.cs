using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

namespace Boneappetit
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid, "2.0.12")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class BoneAppetit : BaseUnityPlugin
    {
        public const string PluginGUID = "com.rockerkitten.boneappetit";
        public const string PluginName = "BoneAppetit";
        public const string PluginVersion = "2.0.1";
        public AssetBundle assetBundle;
        public AssetBundle customFood;
        public Sprite CookingSprite;
        public Skills.SkillType rkCookingSkill = 0;
        public ConfigEntry<bool> PorkRindEnable;
        public ConfigEntry<bool> KabobEnable;
        public ConfigEntry<bool> FriedLoxEnable;
        public ConfigEntry<bool> GlazedCarrotEnable;
        public ConfigEntry<bool> BaconEnable;
        public ConfigEntry<bool> SmokedFishEnable;
        public ConfigEntry<bool> PancakesEnable;
        public ConfigEntry<bool> PizzaEnable;
        public ConfigEntry<bool> CoffeeEnable;
        public ConfigEntry<bool> LatteEnable;
        public ConfigEntry<bool> SkillEnable;
        public ConfigEntry<bool> SmokelessEnable;
        public ConfigEntry<bool> HaggisEnable;
        public ConfigEntry<bool> CandiedTurnipEnable;
        public ConfigEntry<bool> MoochiEnable;
        public ConfigEntry<bool> ConesEnable { get; private set; }
        public ConfigEntry<bool> Nut_EllaEnable;
        public ConfigEntry<bool> BrothEnable;
        public ConfigEntry<bool> FishStewEnable;
        public ConfigEntry<bool> ButterEnable;
        public ConfigEntry<bool> BloodSausageEnable;
        public ConfigEntry<bool> OmletteEnable;
        public ConfigEntry<bool> BurgerEnable;
        public ConfigEntry<bool> PorridgeEnable;
        public ConfigEntry<bool> PBJEnable;
        public ConfigEntry<bool> BoiledEggEnable;
        public ConfigEntry<bool> CakeEnable;
        public ConfigEntry<bool> GrillOriginal;
        public ConfigEntry<bool> CarrotSticksEnable;

        public EffectList buildStone;
        public EffectList cookingSound;
        public EffectList breakStone;
        public EffectList hitStone;
        public EffectList buildKitten;
        public EffectList hearthAddFuel;
        public EffectList fireAddFuel;

        public Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();

        public GameObject icecream_prefab;
        public CustomItem icecream;
        public GameObject porkrind_prefab;
        public CustomItem porkrind;
        public GameObject kabob_prefab;
        public CustomItem kabob;
        public GameObject friedlox_prefab;
        public CustomItem friedlox;
        public GameObject glazedcarrot_prefab;
        public CustomItem glazedcarrot;
        public GameObject bacon_prefab;
        public CustomItem bacon;
        public GameObject smokedfish_prefab;
        public CustomItem smokedfish;
        public GameObject pancake_prefab;
        public CustomItem pancake;
        public GameObject pizza_prefab;
        public CustomItem pizza;
        public GameObject coffee_prefab;
        public CustomItem coffee;
        public GameObject latte_prefab;
        public CustomItem latte;
        public GameObject firecream_prefab;
        public CustomItem firecream;
        public CustomItem electriccream;
        public GameObject electriccream_prefab;
        public CustomItem acidcream;
        public GameObject acidcream_prefab;
        public GameObject porridge_prefab;
        public CustomItem porridge;
        public GameObject pbj_prefab;
        public CustomItem pbj;
        public GameObject cake_prefab;
        public CustomItem cake;
        public AudioSource fireVol;
        public GameObject haggisFab;
        public CustomItem haggis;
        public GameObject candiedTurnipFab;
        public CustomItem candiedTurnip;
        public GameObject moochiFab;
        public CustomItem moochi;
        public GameObject omletteFab;
        public CustomItem omlette;
        public GameObject fishStewFab;
        public CustomItem fishStew;
        public GameObject brothFab;
        public CustomItem broth;
        public GameObject butterFab;
        public CustomItem butter;
        public GameObject bloodsausageFab;
        public CustomItem bloodsausage;
        public GameObject burgerFab;
        public CustomItem burger;
        public GameObject nut_ellaFab;
        public CustomItem nut_ella;
        public GameObject eggFab;
        public GameObject boiledeggFab;
        public CustomItem boiledegg;
        public GameObject carrotstickFab;
        public CustomItem carrotstick;
      
        public GameObject eggFab;
        public GameObject deggFab;
        public GameObject porkFab;
        public GameObject feathers;



        public void Awake()
        {
            CreatConfigValues();
            AssetLoad();
            //AddSkills();
            ItemManager.OnVanillaItemsAvailable += LoadSounds;
            PrefabManager.OnPrefabsRegistered += NewDrops;

          SynchronizationManager.OnConfigurationSynchronized += (obj, attr) =>
            {
                if (attr.InitialSynchronization)
                {
                    Jotunn.Logger.LogMessage("Initial Config sync event received");
                    LoadFood();
                }
                else
                {
                    Jotunn.Logger.LogMessage("Config sync event received");

                }
            };

        }
        public void CreatConfigValues()
        {
            Config.SaveOnConfigSet = true;

            ConesEnable = Config.Bind("Cones", "Enable", true, new ConfigDescription("All Cones Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            PorkRindEnable = Config.Bind("Pork Rind", "Enable", true, new ConfigDescription("Pork Rind Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            KabobEnable = Config.Bind("Kabob", "Enable", true, new ConfigDescription("Kabob Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            FriedLoxEnable = Config.Bind("Fried Lox", "Enable", true, new ConfigDescription("Fried Lox Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            GlazedCarrotEnable = Config.Bind("Glazed Carrots", "Enable", true, new ConfigDescription("Glazed Carrots Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            BaconEnable = Config.Bind("Bacon", "Enable", true, new ConfigDescription("Bacon Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            SmokedFishEnable = Config.Bind("Smoked Fish", "Enable", true, new ConfigDescription("Smoked Fish Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            PancakesEnable = Config.Bind("Pancakes", "Enable", true, new ConfigDescription("Pancakes Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            PizzaEnable = Config.Bind("Pizza", "Enable", true, new ConfigDescription("Pizza Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            CoffeeEnable = Config.Bind("Coffee", "Enable", true, new ConfigDescription("Coffee Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            LatteEnable = Config.Bind("Latte", "Enable", true, new ConfigDescription("Latte Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            SkillEnable = Config.Bind("Skill", "Enable", true, new ConfigDescription("Skill Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            PorridgeEnable = Config.Bind("Porridge", "Enable", true, new ConfigDescription("Porridge Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            PBJEnable = Config.Bind("PBJ", "Enable", true, new ConfigDescription("PBJ Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            CakeEnable = Config.Bind("Cake", "Enable", true, new ConfigDescription("Birthday Cake Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            HaggisEnable = Config.Bind("Haggis", "Enable", true, new ConfigDescription("Haggis Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            CandiedTurnipEnable = Config.Bind("Candied Turnip", "Enable", true, new ConfigDescription("Candied Turnip Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            MoochiEnable = Config.Bind("Moochi", "Enable", true, new ConfigDescription("Moochi Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Nut_EllaEnable = Config.Bind("Nut_Ella", "Enable", true, new ConfigDescription("Nut_Ella Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            BurgerEnable = Config.Bind("Burger", "Enable", true, new ConfigDescription("Burger Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            OmletteEnable = Config.Bind("Omlette", "Enable", true, new ConfigDescription("Omlette Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            BrothEnable = Config.Bind("Broth", "Enable", true, new ConfigDescription("Broth Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            FishStewEnable = Config.Bind("Fish Stew", "Enable", true, new ConfigDescription("Fish Stew Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            ButterEnable = Config.Bind("Butter", "Enable", true, new ConfigDescription("Butter Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            BloodSausageEnable = Config.Bind("Blood Sausage", "Enable", true, new ConfigDescription("Blood Sausage Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            BoiledEggEnable = Config.Bind("Boiled Egg", "Enable", true, new ConfigDescription("Boiled Egg Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            CarrotSticksEnable = Config.Bind("Carrot Sticks", "Enable", true, new ConfigDescription("Carrot Sticks Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            GrillOriginal = Config.Bind("Original", "Enable", true, new ConfigDescription("Use original grill", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            SmokelessEnable = Config.Bind("Smokeless", "Enalbe", true, new ConfigDescription("Enable to allow building of smokeless fires", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

        }

        public void LoadFood()
        {
            icecream.Recipe.Recipe.m_enabled = ConesEnable.Value;
            porkrind.Recipe.Recipe.m_enabled = PorkRindEnable.Value;
            kabob.Recipe.Recipe.m_enabled = KabobEnable.Value;
            friedlox.Recipe.Recipe.m_enabled = FriedLoxEnable.Value;
            glazedcarrot.Recipe.Recipe.m_enabled = GlazedCarrotEnable.Value;
            bacon.Recipe.Recipe.m_enabled = BaconEnable.Value;
            smokedfish.Recipe.Recipe.m_enabled = SmokedFishEnable.Value;
            pancake.Recipe.Recipe.m_enabled = PancakesEnable.Value;
            pizza.Recipe.Recipe.m_enabled = PizzaEnable.Value;
            coffee.Recipe.Recipe.m_enabled = CoffeeEnable.Value;
            latte.Recipe.Recipe.m_enabled = LatteEnable.Value;
            firecream.Recipe.Recipe.m_enabled = ConesEnable.Value;
            electriccream.Recipe.Recipe.m_enabled = ConesEnable.Value; //idfk where the rest is going off what I see here
            acidcream.Recipe.Recipe.m_enabled = ConesEnable.Value; //cuz it gets used for all your ice cream
            porridge.Recipe.Recipe.m_enabled = PorridgeEnable.Value;
            pbj.Recipe.Recipe.m_enabled = PBJEnable.Value;
            cake.Recipe.Recipe.m_enabled = CakeEnable.Value;
            haggis.Recipe.Recipe.m_enabled = HaggisEnable.Value;
            candiedTurnip.Recipe.Recipe.m_enabled = CandiedTurnipEnable.Value;
            moochi.Recipe.Recipe.m_enabled = MoochiEnable.Value;
            nut_ella.Recipe.Recipe.m_enabled = Nut_EllaEnable.Value;
            burger.Recipe.Recipe.m_enabled = BurgerEnable.Value;
            omlette.Recipe.Recipe.m_enabled = OmletteEnable.Value;
            broth.Recipe.Recipe.m_enabled = BrothEnable.Value;
            fishStew.Recipe.Recipe.m_enabled = FishStewEnable.Value;
            butter.Recipe.Recipe.m_enabled = ButterEnable.Value;
            bloodsausage.Recipe.Recipe.m_enabled = BloodSausageEnable.Value;
            boiledegg.Recipe.Recipe.m_enabled = BoiledEggEnable.Value;
            carrotstick.Recipe.Recipe.m_enabled = CarrotSticksEnable.Value;

        }
        public void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("grill", Assembly.GetExecutingAssembly());
            customFood = AssetUtils.LoadAssetBundleFromResources("customfood", Assembly.GetExecutingAssembly());
            CookingSprite = customFood.LoadAsset<Sprite>("rkcookingsprite");
                feathers = PrefabManager.Cache.GetPrefab<GameObject>("Feathers");
                
            


            Jotunn.Logger.LogMessage("Prepping Kitchen...");
                LoadDropFab();
            LoadItem();
            LoadGriddle();
            Oven();
                Butter();
                Nut_Ella();
            IceCream();
            PorkRind();
            Kabob();
            FriedLox();
            GlazedCarrot();
            Bacon();
            SmokedFish();
            Pancakes();
            Pizza();
            Coffee();
            Latte();
            FireCream();
            ElectricCream();
            AcidCream();
            Porridge();
            PBJ();
            Cake();
            LoadFire();
            LoadHearth();
            Haggis();
            CandiedTurnip();
            Moochi();
            Broth();
            FishStew();
            BloodSausage();
            Burger();
            Omlette();
            BoiledEgg();
            Prepstation();
            CarrotSticks();
            Egg();
        }

        public void LoadSounds()
        {
            var sfxstone = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_stone");
            var vfxstone = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_stone_wall_2x1");
            var sfxcook = PrefabManager.Cache.GetPrefab<GameObject>("sfx_cooking_station_done");
            var sfxbreak = PrefabManager.Cache.GetPrefab<GameObject>("sfx_rock_destroyed");
            var kittenPoof = assetBundle.LoadAsset<GameObject>("vfx_rainbowkitten");
            var vfxadd = PrefabManager.Cache.GetPrefab<GameObject>("vfx_FireAddFuel");
            var sfxadd = PrefabManager.Cache.GetPrefab<GameObject>("sfx_FireAddFuel");
            var sfxstonehit = PrefabManager.Cache.GetPrefab<GameObject>("sfx_Rock_Hit");
            var vfxaddfuel = PrefabManager.Cache.GetPrefab<GameObject>("vfx_HearthAddFuel");

            //var sfxvol = AudioMan.instance.m_ambientLoopSource;


            buildStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstone }, new EffectList.EffectData { m_prefab = vfxstone } } };
            cookingSound = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxcook } } };
            breakStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxbreak } } };
            hitStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxstonehit } } };
            buildKitten = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstone }, new EffectList.EffectData { m_prefab = kittenPoof } } };
            hearthAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxaddfuel }, new EffectList.EffectData { m_prefab = sfxadd } } };
            fireAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxadd }, new EffectList.EffectData { m_prefab = sfxadd } } };

            Jotunn.Logger.LogMessage("Loaded Game VFX and SFX");
            Jotunn.Logger.LogMessage("Prepping Kitchen...");

            fireVol = AudioMan.instance.m_ambientLoopSource;
            Jotunn.Logger.LogMessage("Load Complete. Bone Appetit yall.");

            ItemManager.OnVanillaItemsAvailable -= LoadSounds;

        }
        public void NewDrops()

          
            {
            var boarFab = PrefabManager.Instance.GetPrefab("Boar");
            var hatchlingFab = PrefabManager.Instance.GetPrefab("Hatchling");
            var seagullFab = PrefabManager.Instance.GetPrefab("Seagal");

            var porkFab = PrefabManager.Instance.GetPrefab("rk_pork");
            var eggFab = PrefabManager.Instance.GetPrefab("rk_egg");

            
            var seagul = seagullFab.AddComponent<DropOnDestroyed>();

            seagul.m_dropWhenDestroyed.m_drops.Add(new DropTable.DropData

            {
                m_item = eggFab,
                m_stackMin = 1,
                m_stackMax = 1,
                m_weight = 1f,
            });
           
            seagul.m_dropWhenDestroyed.m_oneOfEach = true;
            seagul.m_dropWhenDestroyed.m_dropMax = 2;
            seagul.m_dropWhenDestroyed.m_dropMin = 2;
            seagul.m_dropWhenDestroyed.m_dropChance = 1;

            seagul.m_spawnYStep = 0.3f;
            seagul.m_spawnYOffset = 0.5f;

            boarFab.GetComponent<CharacterDrop>().m_drops.Add(new CharacterDrop.Drop()
                {
                    m_prefab = porkFab,
                    m_amountMin = 1,
                    m_amountMax = 1,
                    m_chance = 100f,
                    m_levelMultiplier = true,
                    m_onePerPlayer = false,
                });

            /*
             Var deggFab = customFood.LoadAsset<GameObject>("rk_dragonegg");
             var hatchling = hatchlingFab.GetComponent<CharacterDrop>();
                 hatchling.m_drops.Add(new CharacterDrop.Drop()
                 {
                     m_prefab = deggFab,
                     m_amountMin = 1,
                     m_amountMax = 1,
                         m_chance = 100f,
                         m_levelMultiplier = true,
                         m_onePerPlayer = false,

                 });*/
                     });
           

            //var seaweedFab = customFood.LoadAsset<GameObject>("rk_seaweed");
            /*var neck = PrefabManager.Instance.GetPrefab("Neck");
             neck.GetComponent<CharacterDrop>().m_drops.Add(new CharacterDrop.Drop()
             {
                 m_prefab = seaweedFab,
                 m_amountMin = 1,
                 m_amountMax = 1,
                 m_chance = 0.8f,
                 m_levelMultiplier = true,
                 m_onePerPlayer = false,
             });
            
            ItemManager.OnVanillaItemsAvailable -= AddCharacterDrops;*/
            PrefabManager.OnPrefabsRegistered -= NewDrops;
        }

        /*public void AddSkills()
        {

            if (SkillEnable.Value == true)
            // Test adding a skill with a texture
            {
                rkCookingSkill = SkillManager.Instance.AddSkill(new SkillConfig
                {

                    Identifier = "rkCookingSkill",
                    Name = "Gore-mand",
                    Description = "Learn to cook and eat like a Viking!",
                    Icon = CookingSprite,
                    IncreaseStep = 1f,
                });
            }*/
        //}
        public void LoadDropFab()
        {
            porkFab = customFood.LoadAsset<GameObject>("rk_pork");
            PrefabManager.Instance.AddPrefab(porkFab);

            eggFab = customFood.LoadAsset<GameObject>("rk_egg");
            PrefabManager.Instance.AddPrefab(eggFab);
            
            deggFab = customFood.LoadAsset<GameObject>("rk_dragonegg");
            PrefabManager.Instance.AddPrefab(deggFab);
            
        }
        
        private void LoadItem()
        {
            var grillFab = GrillOriginal.Value ? assetBundle.LoadAsset<GameObject>("rk_grill") : customFood.LoadAsset<GameObject>("rk_grill");

            //piece_grill

            var grill = new CustomPiece(grillFab,
                new PieceConfig
                {
                    CraftingStation = "forge",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Stone", Amount = 10, Recover = true },
                        new RequirementConfig { Item = "Iron", Amount = 2, Recover = true }
                    }

                });
            var grillThing = grillFab.GetComponent<Piece>();
            grillThing.m_placeEffect = buildKitten;

            var grillBreak = grillFab.GetComponent<WearNTear>();
            grillBreak.m_hitEffect = hitStone;
            grillBreak.m_destroyedEffect = breakStone;

            var grillStation = grillFab.GetComponent<CraftingStation>();
            grillStation.m_craftItemEffects = cookingSound;
            PieceManager.Instance.AddPiece(grill);

        }
        private void LoadGriddle()
        {
            //piece_griddle

            var griddlefab = assetBundle.LoadAsset<GameObject>("rk_griddle");
            var griddle = new CustomPiece(griddlefab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Stone", Amount = 10, Recover = true }
                    }
                });
            var griddlething = griddlefab.GetComponent<Piece>();
            griddlething.m_placeEffect = buildStone;

            var griddlestation = griddlefab.GetComponent<CraftingStation>();
            griddlestation.m_craftItemEffects = cookingSound;

            var griddleBreak = griddlefab.GetComponent<WearNTear>();
            griddleBreak.m_destroyedEffect = breakStone;
            griddleBreak.m_hitEffect = hitStone;

            PieceManager.Instance.AddPiece(griddle);
        }
        private void Oven()
        {

            var ovenfab = assetBundle.LoadAsset<GameObject>("rk_oven");
            var oven = new CustomPiece(ovenfab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_HammerPieceTable",
                    ExtendStation = "rk_grill",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "SurtlingCore", Amount = 2, Recover = true },
                        new RequirementConfig { Item = "TrophySurtling", Amount = 1, Recover = true },
                        new RequirementConfig { Item = "Stone", Amount = 10, Recover = true }
                    }
                });
            var ovenpiece = ovenfab.GetComponent<Piece>();
            ovenpiece.m_placeEffect = buildStone;

            PieceManager.Instance.AddPiece(oven);
        }
        private void Prepstation()
        {
            var prepFab = assetBundle.LoadAsset<GameObject>("rk_prep");
            var prep = new CustomPiece(prepFab,
                new PieceConfig
                {
                    PieceTable = "_HammerPieceTable",
                    AllowedInDungeons = false,
                    CraftingStation = "forge",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 4, Recover = true},
                        new RequirementConfig {Item = "Tin", Amount = 5, Recover = true},
                        new RequirementConfig {Item = "Stone", Amount = 3, Recover = true}
                    }
                });
            var prepBuild = prepFab.GetComponent<Piece>();
            prepBuild.m_placeEffect = buildKitten;

            var prepDestroy = prepFab.GetComponent<WearNTear>();
            prepDestroy.m_hitEffect = hitStone;
            prepDestroy.m_destroyedEffect = breakStone;

            fireVol = prepFab.GetComponentInChildren<AudioSource>();

            PieceManager.Instance.AddPiece(prep);

        }
        private void IceCream()
        {
            icecream_prefab = customFood.LoadAsset<GameObject>("rk_icecream");
            icecream = new CustomItem(icecream_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "Ice Cream",
                    Enabled = ConesEnable.Value,
                    Amount = 2,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "FreezeGland", Amount = 4},
                        new RequirementConfig { Item = "Blueberries", Amount = 8},
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig {Item = "rk_dragonegg", Amount = 1}
                    }
                });

            ItemManager.Instance.AddItem(icecream);

        }
        private void Nut_Ella()
        {
            nut_ellaFab = customFood.LoadAsset<GameObject>("rk_nut_ella");
            nut_ella = new CustomItem(nut_ellaFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Nut-Ella",
                    Enabled = Nut_EllaEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "BeechSeeds", Amount = 6},
                        new RequirementConfig { Item = "rk_butter", Amount = 1}
                    }
                });

            ItemManager.Instance.AddItem(nut_ella);
        }
        private void CarrotSticks()
        {
            carrotstickFab = customFood.LoadAsset<GameObject>("rk_carrotsticks");
            carrotstick = new CustomItem(carrotstickFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Carrot Sticks",
                    Enabled = CarrotSticksEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Carrot", Amount = 2},
                        new RequirementConfig {Item = "rk_nut_ella", Amount = 1}
                    }
                });

            ItemManager.Instance.AddItem(carrotstick);
        }
        private void Egg()
        {
            eggFab = customFood.LoadAsset<GameObject>("rk_egg");
            PrefabManager.Instance.AddPrefab(eggFab);
        }
        private void BoiledEgg()
        {
            boiledeggFab = customFood.LoadAsset<GameObject>("rk_boiledegg");
            boiledegg = new CustomItem(boiledeggFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Boiled Egg",
                    Enabled = BoiledEggEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "rk_egg", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(boiledegg);
        }
        private void Butter()
        {
            butterFab = customFood.LoadAsset<GameObject>("rk_butter");
            butter = new CustomItem(butterFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Carrot Butter",
                    Enabled = ButterEnable.Value,
                    Amount = 2,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "CarrotSeeds", Amount = 8}
                    }
                });

            ItemManager.Instance.AddItem(butter);
        }
        private void Broth()
        {
            brothFab = customFood.LoadAsset<GameObject>("rk_broth");
            broth = new CustomItem(brothFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Bone Broth",
                    Enabled = BrothEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "BoneFragments", Amount = 2},
                        new RequirementConfig {Item = "rk_butter", Amount = 1}
                    }
                });

            ItemManager.Instance.AddItem(broth);
        }
        private void FishStew()
        {
            fishStewFab = customFood.LoadAsset<GameObject>("rk_fishstew");
            fishStew = new CustomItem(fishStewFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Fish Stew",
                    Enabled = FishStewEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "rk_broth", Amount = 2},
                        new RequirementConfig { Item = "FishRaw", Amount = 2},
                        new RequirementConfig { Item = "Thistle", Amount = 2},
                        new RequirementConfig {Item = "rk_egg", Amount = 2}

                    }
                });

            ItemManager.Instance.AddItem(fishStew);
        }
        private void Burger()
        {
            burgerFab = customFood.LoadAsset<GameObject>("rk_burger");
            burger = new CustomItem(burgerFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Burger",
                    Enabled = BurgerEnable.Value,
                    Amount = 2,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "RawMeat", Amount = 2},
                        new RequirementConfig { Item = "LoxMeat", Amount = 2},
                        new RequirementConfig { Item = "Turnip", Amount = 2},
                        new RequirementConfig {Item = "Bread", Amount = 1}

                    }
                });

            ItemManager.Instance.AddItem(burger);
        }
        private void BloodSausage()
        {
            bloodsausageFab = customFood.LoadAsset<GameObject>("rk_bloodsausage");
            bloodsausage = new CustomItem(bloodsausageFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Blood Sausage",
                    Enabled = BurgerEnable.Value,
                    Amount = 2,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Entrails", Amount = 2},
                        new RequirementConfig { Item = "Bloodbag", Amount = 2},
                        new RequirementConfig { Item = "Turnip", Amount = 1},
                        new RequirementConfig {Item = "rk_pork", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(bloodsausage);
        }
        private void Omlette()
        {
            omletteFab = customFood.LoadAsset<GameObject>("rk_omlette");
            omlette = new CustomItem(omletteFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Omlette",
                    Enabled = BurgerEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "rk_egg", Amount = 2},
                        new RequirementConfig { Item = "Thistle", Amount = 2},
                        new RequirementConfig {Item = "rk_pork", Amount = 1},
                        new RequirementConfig {Item = "rk_butter", Amount = 1}
                    }
                });

            ItemManager.Instance.AddItem(bloodsausage);
        }
        private void PorkRind()
        {
            porkrind_prefab = customFood.LoadAsset<GameObject>("rk_porkrind");
            porkrind = new CustomItem(porkrind_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "Pork Rinds",
                    Enabled = PorkRindEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_griddle",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "LeatherScraps", Amount = 1},
                        new RequirementConfig {Item = "rk_pork", Amount = 1}
                    }
                });

            ItemManager.Instance.AddItem(porkrind);

        }
        private void Haggis()
        {
            haggisFab = customFood.LoadAsset<GameObject>("rk_haggis");
            haggis = new CustomItem(haggisFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Haggis",
                    Enabled = HaggisEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "RawMeat", Amount = 1},
                        new RequirementConfig { Item = "Carrot", Amount = 2},
                        new RequirementConfig { Item = "Entrails", Amount = 2},
                        new RequirementConfig { Item = "Turnip", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(haggis);

        }
        private void CandiedTurnip()
        {
            candiedTurnipFab = customFood.LoadAsset<GameObject>("rk_candiedturnip");
            candiedTurnip = new CustomItem(candiedTurnipFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Candied Turnip",
                    Enabled = CandiedTurnipEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Thistle", Amount = 1},
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig { Item = "Turnip", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(candiedTurnip);

        }
        private void Moochi()
        {
            moochiFab = customFood.LoadAsset<GameObject>("rk_moochi");
            moochi = new CustomItem(moochiFab, fixReference: true,
                new ItemConfig
                {
                    Name = "Moochi",
                    Enabled = MoochiEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "rk_dragonegg", Amount = 1},
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig { Item = "FreezeGland", Amount = 1},
                        new RequirementConfig {Item = "Blueberries", Amount = 4}
                    }
                });

            ItemManager.Instance.AddItem(moochi);

        }
        private void Kabob()
        {
            kabob_prefab = customFood.LoadAsset<GameObject>("rk_kabob");
            kabob = new CustomItem(kabob_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "Kabob",
                    Enabled = KabobEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Turnip", Amount = 1},
                        new RequirementConfig { Item = "Carrot", Amount = 2},
                        new RequirementConfig { Item = "RawMeat", Amount = 1},
                        new RequirementConfig { Item = "BoneFragments", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(kabob);

        }
        private void FriedLox()
        {
            friedlox_prefab = customFood.LoadAsset<GameObject>("rk_friedloxmeat");
            friedlox = new CustomItem(friedlox_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "Chicken Fried Lox Meat",
                    Enabled = FriedLoxEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "LoxMeat", Amount = 2},
                        new RequirementConfig { Item = "BarleyFlour", Amount = 2},
                        new RequirementConfig { Item = "Thistle", Amount = 1},
                        new RequirementConfig { Item = "rk_butter", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(friedlox);

        }
        private void GlazedCarrot()
        {
            glazedcarrot_prefab = customFood.LoadAsset<GameObject>("rk_glazedcarrots");
            glazedcarrot = new CustomItem(glazedcarrot_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "Honey Glazed Carrots",
                    Enabled = GlazedCarrotEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_griddle",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Carrot", Amount = 3},
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig { Item = "Dandelion", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(glazedcarrot);

        }
        private void Bacon()
        {
            bacon_prefab = customFood.LoadAsset<GameObject>("rk_bacon");
            bacon = new CustomItem(bacon_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "Bacon",
                    Enabled = BaconEnable.Value,
                    Amount = 2,
                    CraftingStation = "rk_griddle",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "rk_pork", Amount = 1}
                    }
                });

            ItemManager.Instance.AddItem(bacon);
        }
        private void SmokedFish()
        {
            smokedfish_prefab = customFood.LoadAsset<GameObject>("rk_smokedfish");
            smokedfish = new CustomItem(smokedfish_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "SmokedFish",
                    Enabled = SmokedFishEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_griddle",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "FishRaw", Amount = 1}
                    }
                });

            ItemManager.Instance.AddItem(smokedfish);

        }
        private void Pancakes()
        {
            pancake_prefab = customFood.LoadAsset<GameObject>("rk_pancake");
            pancake = new CustomItem(pancake_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "Pancakes",
                    Enabled = PancakesEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_grill",
                    MinStationLevel = 2,
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig { Item = "BarleyFlour", Amount = 3},
                        new RequirementConfig { Item = "rk_butter", Amount = 5},
                        new RequirementConfig {Item = "rk_egg", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(pancake);

        }
        private void Pizza()
        {
            pizza_prefab = customFood.LoadAsset<GameObject>("rk_pizza");
             pizza = new CustomItem(pizza_prefab, fixReference: true,
               new ItemConfig
               {
                   Name = "Pizza",
                   Enabled = PizzaEnable.Value,
                   Amount = 1,
                   CraftingStation = "rk_grill",
                   MinStationLevel = 2,
                   Requirements = new[]
                   {
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig { Item = "BarleyFlour", Amount = 3},
                        new RequirementConfig { Item = "rk_egg", Amount = 2},
                        new RequirementConfig { Item = "CookedMeat", Amount = 2}
                   }
               });

            ItemManager.Instance.AddItem(pizza);
        }

        private void Coffee()
        {
            coffee_prefab = customFood.LoadAsset<GameObject>("rk_coffee");
             coffee = new CustomItem(coffee_prefab, fixReference: true,
               new ItemConfig
               {
                   Name = "Coffee",
                   Enabled = CoffeeEnable.Value,
                   Amount = 1,
                   CraftingStation = "rk_prep",
                   Requirements = new[]
                   {
                         new RequirementConfig { Item = "AncientSeed", Amount = 2}
                   }
               });

            ItemManager.Instance.AddItem(coffee);

        }
        private void Latte()
        {
            latte_prefab = customFood.LoadAsset<GameObject>("rk_latte");
             latte = new CustomItem(latte_prefab, fixReference: true,
               new ItemConfig
               {
                   Name = "Spice Latte",
                   Enabled = LatteEnable.Value,
                   Amount = 2,
                   CraftingStation = "rk_prep",
                   Requirements = new[]
                   {
                         new RequirementConfig { Item = "Crystal", Amount = 2},
                         new RequirementConfig { Item = "Barley", Amount = 2},
                         new RequirementConfig { Item = "Honey", Amount = 10}
                   }
               });

            ItemManager.Instance.AddItem(latte);

        }
        private void FireCream()
        {
            firecream_prefab = customFood.LoadAsset<GameObject>("rk_firecream");
            firecream = new CustomItem(firecream_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "Fire Cream",
                    Enabled = ConesEnable.Value,
                    Amount = 2,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "SurtlingCore", Amount = 4},
                        new RequirementConfig { Item = "Raspberry", Amount = 8},
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig {Item = "rk_dragonegg", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(firecream);

        }
        private void ElectricCream()
        {
            electriccream_prefab = customFood.LoadAsset<GameObject>("rk_electriccream");
            electriccream = new CustomItem(electriccream_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "Electric Cream",
                    Enabled = ConesEnable.Value,
                    Amount = 2,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Crystal", Amount = 4},
                        new RequirementConfig { Item = "Cloudberry", Amount = 8},
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig {Item = "rk_dragonegg", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(electriccream);

        }
        private void AcidCream()
        {
            acidcream_prefab = customFood.LoadAsset<GameObject>("rk_acidcream");
            acidcream = new CustomItem(acidcream_prefab, fixReference: true,
                new ItemConfig
                {
                    Name = "Acid Cream Cone",
                    Enabled = ConesEnable.Value,
                    Amount = 2,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Guck", Amount = 4},
                        new RequirementConfig { Item = "MushroomYellow", Amount = 8},
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig {Item = "rk_dragonegg", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(acidcream);

        }
        private void Porridge()
        {
            porridge_prefab = customFood.LoadAsset<GameObject>("rk_porridge");
               porridge = new CustomItem(porridge_prefab, fixReference: true,
               new ItemConfig
               {
                   Name = "Porridge",
                   Enabled = PorridgeEnable.Value,
                   Amount = 1,
                   CraftingStation = "rk_grill",
                   MinStationLevel = 2,
                   Requirements = new[]
                   {
                          new RequirementConfig { Item = "Barley", Amount = 2},
                          new RequirementConfig { Item = "Cloudberry", Amount = 4},
                          new RequirementConfig { Item = "Honey", Amount = 2},
                          new RequirementConfig {Item = "rk_butter", Amount = 1}
                   }
               });

            ItemManager.Instance.AddItem(porridge);

        }
        private void PBJ()
        {
            pbj_prefab = customFood.LoadAsset<GameObject>("rk_pbj");
            pbj = new CustomItem(pbj_prefab, fixReference: true,
               new ItemConfig
               {
                   Name = "Jimmy's PBJ",
                   Enabled = PBJEnable.Value,
                   Amount = 4,
                   CraftingStation = "rk_grill",
                   MinStationLevel = 1,
                   Requirements = new[]
                   {
                          new RequirementConfig { Item = "Bread", Amount = 1},
                          new RequirementConfig { Item = "QueensJam", Amount = 1},
                          new RequirementConfig { Item = "rk_nut_ella", Amount = 4}
                   }
               });

            ItemManager.Instance.AddItem(pbj);

        }
        private void Cake()
        {
            cake_prefab = customFood.LoadAsset<GameObject>("rk_birthday");
            cake = new CustomItem(cake_prefab, fixReference: true,
               new ItemConfig
               {
                   Name = "Birthday Cake",
                   Enabled = CakeEnable.Value,
                   Amount = 1,
                   CraftingStation = "rk_grill",
                   MinStationLevel = 2,
                   Requirements = new[]
                   {
                          new RequirementConfig { Item = "BarleyFlour", Amount = 2},
                          new RequirementConfig { Item = "Honey", Amount = 4},
                          new RequirementConfig { Item = "Cloudberry", Amount = 4},
                        new RequirementConfig {Item = "rk_egg", Amount = 2}
                   }
               });

            ItemManager.Instance.AddItem(cake);

        }
        //private void Haggis()
        private void LoadFire()
        {
            var fireFab = assetBundle.LoadAsset<GameObject>("rk_campfire");
            var fire = new CustomPiece(fireFab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = SmokelessEnable.Value,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Stone", Amount = 5, Recover = true },
                        new RequirementConfig { Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            var firebuild = fireFab.GetComponent<Piece>();
            firebuild.m_placeEffect = buildStone;

            var firedecay = fireFab.GetComponent<WearNTear>();
            firedecay.m_destroyedEffect = breakStone;

            var addFuel = fireFab.GetComponent<Fireplace>();
            addFuel.m_fuelAddedEffects = fireAddFuel;

            fireVol = fireFab.GetComponentInChildren<AudioSource>();

            PieceManager.Instance.AddPiece(fire);
        }
        private void LoadHearth()
        {
            var fireFab = assetBundle.LoadAsset<GameObject>("rk_hearth");
            var fire = new CustomPiece(fireFab,
                new PieceConfig
                {
                    CraftingStation = "piece_stonecutter",
                    AllowedInDungeons = false,
                    Enabled = SmokelessEnable.Value,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Stone", Amount = 15, Recover = true }
                    }
                });
            var firebuild = fireFab.GetComponent<Piece>();
            firebuild.m_placeEffect = buildStone;

            var firedecay = fireFab.GetComponent<WearNTear>();
            firedecay.m_destroyedEffect = breakStone;

            var addFuel = fireFab.GetComponent<Fireplace>();
            addFuel.m_fuelAddedEffects = hearthAddFuel;

            fireVol = fireFab.GetComponentInChildren<AudioSource>();

            PieceManager.Instance.AddPiece(fire);
        }

    }

}
