using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boneappetit
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid, "2.7.0")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public class BoneAppetit : BaseUnityPlugin
    {
        public const string PluginGUID = "com.rockerkitten.boneappetit";
        public const string PluginName = "BoneAppetit";
        public const string PluginVersion = "3.2.0";
        public AssetBundle assetBundle;
        public AssetBundle customFood;
        public static BoneAppetit Instance;
        private Harmony _harmony;
        public Sprite CookingSprite;
        public Skills.SkillType rkCookingSkill;
        public static ConfigEntry<int> NexusId;
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
        // public ConfigEntry<bool> SkillEnable;
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
        public ConfigEntry<bool> CheffHatEnable;
        public ConfigEntry<bool> MeadEnable;
        public ConfigEntry<bool> CookingSkillEnable;
        public ConfigEntry<bool> BonusWhenCookingEnabled;
        public ConfigEntry<bool> HatSEMessage;
        public ConfigEntry<float> HatXpGain;
        //public ConfigEntry<bool> ScrapsRecipe;

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
        public GameObject boiledeggFab;
        public CustomItem boiledegg;
        public GameObject carrotstickFab;
        public CustomItem carrotstick;
        public GameObject meadFab;
        public CustomItem mead;
        public GameObject hatFab;
        public CustomItem hat;
        public GameObject fireFab1;
        public CustomPiece fire1;
        public GameObject fireFab2;
        public CustomPiece fire2;
        public GameObject fireFab3;
        public CustomPiece fire3;

        public GameObject eggFab;
        public GameObject deggFab;
        public GameObject porkFab;

        public BoneAppetit()
        {
          Instance = this;
        }

        [UsedImplicitly]
        public void Awake()
        {
            CreateConfigValues();
            AssetLoad();
            //LoadDropFab();
            AddSkills();
            PrefabManager.OnVanillaPrefabsAvailable += LoadSounds;
            ItemManager.OnItemsRegistered += NewDrops;

            //PrefabManager.OnPrefabsRegistered += DropMachine;

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
            _harmony = Harmony.CreateAndPatchAll(typeof(BoneAppetit).Assembly, PluginGUID);
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
          _harmony?.UnpatchSelf();
        }

        public void CreateConfigValues()
        {
            Config.SaveOnConfigSet = true;

            NexusId = Config.Bind("Hidden", "NexusId", 1250, new ConfigDescription("NexusId", null, new ConfigurationManagerAttributes { IsAdminOnly = false, Browsable = false, ReadOnly = true}));
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
            // SkillEnable = Config.Bind("Skill", "Enable", true, new ConfigDescription("Skill Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
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
            CheffHatEnable = Config.Bind("Chef Hat", "Enable", true, new ConfigDescription("Chef Hat Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            MeadEnable = Config.Bind("Mead", "Enable", true, new ConfigDescription("Mead Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            GrillOriginal = Config.Bind("Original", "Enable", true, new ConfigDescription("Use original grill", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            SmokelessEnable = Config.Bind("Smokeless", "Enable", true, new ConfigDescription("Enable to allow building of smokeless fires", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            CookingSkillEnable = Config.Bind("Cooking Skill", "Enable Cooking Skill", true, new ConfigDescription("Enable Cooking Skill", null, new ConfigurationManagerAttributes { IsAdminOnly = true, Order = 1}));
            BonusWhenCookingEnabled = Config.Bind("Cooking Skill", "Enable Cooking Bonus", true, new ConfigDescription("Enable Cooking Bonus", null, new ConfigurationManagerAttributes { IsAdminOnly = true, Order = 2}));
            HatXpGain = Config.Bind("Cooking Skill", "Chef Hat XP Gain", 5f, new ConfigDescription("XP Gain multiplier when cooking while wearing the Chef Hat", null, new ConfigurationManagerAttributes { IsAdminOnly = true, Order = 3}));
            HatSEMessage = Config.Bind("Cooking Skill", "Enable Chef Hat Message", true, new ConfigDescription("Enable Message when equipping the Chef Hat", null, new ConfigurationManagerAttributes { IsAdminOnly = false, Order = 4}));
            //ScrapsRecipe = Config.Bind("Leather Scraps Recipe", "Enable", true, new ConfigDescription("Enabled add a Deer Hide to Leather Scraps recipe", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

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
            mead.Recipe.Recipe.m_enabled = MeadEnable.Value;
            //fire1.Recipe.Recipe.m_enabled = SmokelessEnable.Value;
            fire1.Piece.m_enabled = SmokelessEnable.Value;
            fire2.Piece.m_enabled = SmokelessEnable.Value;
            fire3.Piece.m_enabled = SmokelessEnable.Value;

        }
        public void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("grill", Assembly.GetExecutingAssembly());
            customFood = AssetUtils.LoadAssetBundleFromResources("customfood", Assembly.GetExecutingAssembly());
            CookingSprite = customFood.LoadAsset<Sprite>("rkcookingsprite");

            Jotunn.Logger.LogMessage("Prepping Kitchen...");
            LoadDropFab();
            Jotunn.Logger.LogMessage("Big thanks to MeatwareMonster!");
            
           
        }

        public void LoadSounds()
        {
            try
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
                


                buildStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstone }, new EffectList.EffectData { m_prefab = vfxstone } } };
                cookingSound = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxcook } } };
                breakStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxbreak } } };
                hitStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxstonehit } } };
                buildKitten = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxstone } } };// new EffectList.EffectData { m_prefab = kittenPoof } } };
                hearthAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxaddfuel }, new EffectList.EffectData { m_prefab = sfxadd } } };
                fireAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxadd }, new EffectList.EffectData { m_prefab = sfxadd } } };

                Jotunn.Logger.LogMessage("Loaded Game VFX and SFX");
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
                Haggis();
                CandiedTurnip();
                Moochi();
                Broth();
                FishStew();
                BloodSausage();
                Burger();
                Omlette();
                BoiledEgg();
                CarrotSticks();
                ChefHatt();
                Mead();
                LoadItem();
                LoadGriddle();
                Oven();
                LoadFire();
                LoadHearth();
                Prepstation();
                Brazier();

                fireVol = AudioMan.instance.m_ambientLoopSource;
            }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while running OnVanillaLoad: {ex.Message}");
            }
            finally
            {
                Jotunn.Logger.LogMessage("Load Complete. Bone Appetit yall.");

                PrefabManager.OnVanillaPrefabsAvailable -= LoadSounds;
            }
        }
        public void NewDrops()
        {
            var boarFab = PrefabManager.Instance.GetPrefab("Boar");
            var hatchlingFab = PrefabManager.Instance.GetPrefab("Hatchling");
            var seagullFab = PrefabManager.Instance.GetPrefab("Seagal");
            var crowFab = PrefabManager.Instance.GetPrefab("Crow");
            var porkFab = PrefabManager.Instance.GetPrefab("rk_pork");
            var eggFab = PrefabManager.Instance.GetPrefab("rk_egg");
            var deggFab = PrefabManager.Instance.GetPrefab("rk_dragonegg");
           /* var scrapsFab = PrefabManager.Instance.GetPrefab("LeatherScraps");
            var scraps = scrapsFab.GetComponent<ItemDrop>();
            var deerFab = ItemManager.Instance.GetItem("DeerHide");
            var deerHide = deerFab.ItemDrop;

            new Recipe
            {
                m_item = scraps,
                m_amount = 3,
                m_enabled = ScrapsRecipe.Value,
                m_resources = new[]
                {
                    new Piece.Requirement {m_amount = 1, m_resItem = deerHide}
                }
            };*/

            var seagul = seagullFab.GetComponent<DropOnDestroyed>();
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

            var crow = crowFab.GetComponent<DropOnDestroyed>();
            crow.m_dropWhenDestroyed.m_drops.Add(new DropTable.DropData

            {
                m_item = eggFab,
                m_stackMin = 1,
                m_stackMax = 1,
                m_weight = 1f,
            });

            crow.m_dropWhenDestroyed.m_oneOfEach = true;
            crow.m_dropWhenDestroyed.m_dropMax = 2;
            crow.m_dropWhenDestroyed.m_dropMin = 2;
            crow.m_dropWhenDestroyed.m_dropChance = 1;

            crow.m_spawnYStep = 0.3f;
            crow.m_spawnYOffset = 0.5f;

            boarFab.GetComponent<CharacterDrop>().m_drops.Add(new CharacterDrop.Drop()
            {
                m_prefab = porkFab,
                m_amountMin = 1,
                m_amountMax = 1,
                m_chance = 1f,
                m_levelMultiplier = true,
                m_onePerPlayer = false,
            });
            hatchlingFab.GetComponent<CharacterDrop>().m_drops.Add(new CharacterDrop.Drop()
            {
                m_prefab = deggFab,
                m_amountMin = 1,
                m_amountMax = 1,
                m_chance = 1f,
                m_levelMultiplier = true,
                m_onePerPlayer = false,
            });

            /*var seagul = seagullFab.AddComponent<DropOnDestroyed>();

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

            //var boar = PrefabManager.Cache.GetPrefab<GameObject>("Boar");
            boarFab.GetComponent<CharacterDrop>().m_drops.Add(new CharacterDrop.Drop()
            {
                m_prefab = porkFab,
                m_amountMin = 1,
                m_amountMax = 1,
                m_chance = 100f,
                m_levelMultiplier = true,
                m_onePerPlayer = false,
            });

               
              
            }; 
            hatchlingFab.GetComponent<CharacterDrop>().m_drops.Add(new CharacterDrop.Drop()
            {
                m_prefab = deggFab,
                m_amountMin = 1,
                m_amountMax = 1,
                m_chance = 100f,
                m_levelMultiplier = true,
                m_onePerPlayer = false,
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
            ItemManager.OnItemsRegistered -= NewDrops;
            
             /*public void AddSkills()
             {

                 if (SkillEnable.Value == true)
                 // Test adding a skill with a texture
                 {
                     CookingSkillIdentifier = SkillManager.Instance.AddSkill(new SkillConfig
                     {

                         Identifier = "rkCookingSkill",
                         Name = "Gore-mand",
                         Description = "Learn to cook and eat like a Viking!",
                         Icon = CookingSprite,
                         IncreaseStep = 1f,
                     });
                 }*/
        }

        /// <summary>
        /// Adds the cooking skill to the game.
        /// </summary>
        public void AddSkills()
        {
          if (CookingSkillEnable.Value) // Test adding a skill with a texture
          {
            rkCookingSkill = SkillManager.Instance.AddSkill(new SkillConfig
            {
              Identifier = PluginGUID
              , Name = "Gore-mand"
              , Description = "Learn to cook and eat like a Viking!"
              , Icon = CookingSprite
              , IncreaseStep = 1f,
            });
          }
        }

        public void LoadDropFab()
        {
            porkFab = customFood.LoadAsset<GameObject>("rk_pork");
            ItemManager.Instance.AddItem(new CustomItem(porkFab, false));

            eggFab = customFood.LoadAsset<GameObject>("rk_egg");
            ItemManager.Instance.AddItem(new CustomItem(eggFab, false));

            deggFab = customFood.LoadAsset<GameObject>("rk_dragonegg");
            ItemManager.Instance.AddItem(new CustomItem(deggFab, false));
        }

        private void LoadItem()
        {
            var grillFab = GrillOriginal.Value ? assetBundle.LoadAsset<GameObject>("rk_grill") : customFood.LoadAsset<GameObject>("rk_grill");

            //piece_grill

            var grill = new CustomPiece(grillFab, fixReference: false,
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
            var griddle = new CustomPiece(griddlefab, fixReference: false,
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
            var oven = new CustomPiece(ovenfab, fixReference: false,
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
            var prep = new CustomPiece(prepFab, fixReference: false,
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
            icecream = new CustomItem(icecream_prefab, fixReference: false,
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
            nut_ella = new CustomItem(nut_ellaFab, fixReference: false,
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
            carrotstick = new CustomItem(carrotstickFab, fixReference: false,
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
        private void BoiledEgg()
        {
            boiledeggFab = customFood.LoadAsset<GameObject>("rk_boiledegg");
            boiledegg = new CustomItem(boiledeggFab, fixReference: false,
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
            butter = new CustomItem(butterFab, fixReference: false,
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
            broth = new CustomItem(brothFab, fixReference: false,
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
            fishStew = new CustomItem(fishStewFab, fixReference: false,
                new ItemConfig
                {
                    Name = "Fish Stew",
                    Enabled = FishStewEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_prep",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "rk_broth", Amount = 1},
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
            burger = new CustomItem(burgerFab, fixReference: false,
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
            bloodsausage = new CustomItem(bloodsausageFab, fixReference: false,
                new ItemConfig
                {
                    Name = "Blood Sausage",
                    Enabled = BloodSausageEnable.Value,
                    Amount = 2,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Entrails", Amount = 2},
                        new RequirementConfig { Item = "Bloodbag", Amount = 1},
                        new RequirementConfig { Item = "Thistle", Amount = 2},
                        new RequirementConfig {Item = "rk_pork", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(bloodsausage);
        }
        private void Omlette()
        {
            omletteFab = customFood.LoadAsset<GameObject>("rk_omlette");
            omlette = new CustomItem(omletteFab, fixReference: false,
                new ItemConfig
                {
                    Name = "Omlette",
                    Enabled = OmletteEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_griddle",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "rk_egg", Amount = 2},
                        new RequirementConfig { Item = "Thistle", Amount = 2},
                        new RequirementConfig {Item = "rk_pork", Amount = 1},
                        new RequirementConfig {Item = "rk_butter", Amount = 1}
                    }
                });

            ItemManager.Instance.AddItem(omlette);
        }
        private void PorkRind()
        {
            porkrind_prefab = customFood.LoadAsset<GameObject>("rk_porkrind");
            porkrind = new CustomItem(porkrind_prefab, fixReference: false,
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
            haggis = new CustomItem(haggisFab, fixReference: false,
                new ItemConfig
                {
                    Name = "Haggis",
                    Enabled = HaggisEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_prep",
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
            candiedTurnip = new CustomItem(candiedTurnipFab, fixReference: false,
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
            moochi = new CustomItem(moochiFab, fixReference: false,
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
            kabob = new CustomItem(kabob_prefab, fixReference: false,
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
            friedlox = new CustomItem(friedlox_prefab, fixReference: false,
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
                        new RequirementConfig { Item = "rk_egg", Amount = 1},
                        new RequirementConfig { Item = "rk_butter", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(friedlox);

        }
        private void GlazedCarrot()
        {
            glazedcarrot_prefab = customFood.LoadAsset<GameObject>("rk_glazedcarrots");
            glazedcarrot = new CustomItem(glazedcarrot_prefab, fixReference: false,
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
            bacon = new CustomItem(bacon_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Bacon",
                    Enabled = BaconEnable.Value,
                    Amount = 2,
                    CraftingStation = "rk_griddle",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "rk_pork", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(bacon);
        }
        private void SmokedFish()
        {
            smokedfish_prefab = customFood.LoadAsset<GameObject>("rk_smokedfish");
            smokedfish = new CustomItem(smokedfish_prefab, fixReference: false,
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
            pancake = new CustomItem(pancake_prefab, fixReference: false,
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
            pizza = new CustomItem(pizza_prefab, fixReference: false,
              new ItemConfig
              {
                  Name = "Pizza",
                  Enabled = PizzaEnable.Value,
                  Amount = 1,
                  CraftingStation = "rk_grill",
                  MinStationLevel = 2,
                  Requirements = new[]
                  {
                        new RequirementConfig { Item = "Mushroom", Amount = 2},
                        new RequirementConfig { Item = "BarleyFlour", Amount = 3},
                        new RequirementConfig { Item = "rk_egg", Amount = 2},
                        new RequirementConfig { Item = "RawMeat", Amount = 2}
                  }
              });

            ItemManager.Instance.AddItem(pizza);
        }

        private void Coffee()
        {
            coffee_prefab = customFood.LoadAsset<GameObject>("rk_coffee");
            coffee = new CustomItem(coffee_prefab, fixReference: false,
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
            latte = new CustomItem(latte_prefab, fixReference: false,
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
            firecream = new CustomItem(firecream_prefab, fixReference: false,
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
            electriccream = new CustomItem(electriccream_prefab, fixReference: false,
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
            acidcream = new CustomItem(acidcream_prefab, fixReference: false,
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
            porridge = new CustomItem(porridge_prefab, fixReference: false,
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
            pbj = new CustomItem(pbj_prefab, fixReference: false,
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
            cake = new CustomItem(cake_prefab, fixReference: false,
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
        private void ChefHatt()
        {
            hatFab = customFood.LoadAsset<GameObject>("rk_chef");
            hat = new CustomItem(hatFab, fixReference: false,
               new ItemConfig
               {
                   Name = "Chef Hat",
                   Description = "Improves Cooking Skill XP Earned.",
                   Enabled = CheffHatEnable.Value,
                   Amount = 1,
                   CraftingStation = "",
                   Requirements = new[]
                   {
                          new RequirementConfig { Item = "Dandelion", Amount = 5}
                   },
               });

            var itemDrop = hat.ItemDrop;
            var hat_se = ScriptableObject.CreateInstance<SE_CheffHat>();
            itemDrop.m_itemData.m_shared.m_equipStatusEffect = hat_se;
            ItemManager.Instance.AddItem(hat);
        }

        private void Mead()
        {
            meadFab = customFood.LoadAsset<GameObject>("rk_mead");
            mead = new CustomItem(meadFab, fixReference: false,
               new ItemConfig
               {
                   Name = "Mead",
                   Enabled = MeadEnable.Value,
                   Amount = 1,
                   CraftingStation = "rk_prep",
                   MinStationLevel = 1,
                   Requirements = new[]
                   {
                          new RequirementConfig { Item = "Barley", Amount = 3},
                          new RequirementConfig { Item = "Honey", Amount = 4},
                   }
               });

            ItemManager.Instance.AddItem(mead);

        }
        //private void Haggis()
        private void LoadFire()
        {
            fireFab1 = assetBundle.LoadAsset<GameObject>("rk_campfire");
            fire1 = new CustomPiece(fireFab1, fixReference: false,
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
            var firebuild = fireFab1.GetComponent<Piece>();
            firebuild.m_placeEffect = buildStone;

            var firedecay = fireFab1.GetComponent<WearNTear>();
            firedecay.m_destroyedEffect = breakStone;

            var addFuel = fireFab1.GetComponent<Fireplace>();
            addFuel.m_fuelAddedEffects = fireAddFuel;

            fireVol = fireFab1.GetComponentInChildren<AudioSource>();

            PieceManager.Instance.AddPiece(fire1);
        }
        private void LoadHearth()
        {
            fireFab2 = assetBundle.LoadAsset<GameObject>("rk_hearth");
            fire2 = new CustomPiece(fireFab2, fixReference: false,
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
            var firebuild = fireFab2.GetComponent<Piece>();
            firebuild.m_placeEffect = buildStone;

            var firedecay = fireFab2.GetComponent<WearNTear>();
            firedecay.m_destroyedEffect = breakStone;

            var addFuel = fireFab2.GetComponent<Fireplace>();
            addFuel.m_fuelAddedEffects = hearthAddFuel;

            fireVol = fireFab2.GetComponentInChildren<AudioSource>();

            PieceManager.Instance.AddPiece(fire2);
        }
        private void Brazier()
        {
            fireFab3 = assetBundle.LoadAsset<GameObject>("rk_brazier");
            fire3 = new CustomPiece(fireFab3, fixReference: false,
                new PieceConfig
                {
                    CraftingStation = "forge",
                    AllowedInDungeons = false,
                    Enabled = SmokelessEnable.Value,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Bronze", Amount = 5, Recover = true },
                        new RequirementConfig { Item = "Coal", Amount = 2, Recover = true },
                        new RequirementConfig { Item = "Chain", Amount = 1, Recover = true }
                    }
                });
            var firebuild = fireFab3.GetComponent<Piece>();
            firebuild.m_placeEffect = buildStone;

            var firedecay = fireFab3.GetComponent<WearNTear>();
            firedecay.m_destroyedEffect = breakStone;

            var addFuel = fireFab3.GetComponent<Fireplace>();
            addFuel.m_fuelAddedEffects = hearthAddFuel;

            fireVol = fireFab3.GetComponentInChildren<AudioSource>();

            PieceManager.Instance.AddPiece(fire3);
        }

        /// <summary>
        /// Patch for CookingStation.
        /// </summary>
        /// <param name="__result">Was item placed on the cooking station successfully?</param>
        public void OnCookingStationCookItem(ref bool __result)
        {
          LogDebug($"__result : {__result}");
          if (!__result) return;
          if (!CookingSkillEnable.Value) return;
          RaiseCookingSkill();
        }

        /// <summary>
        /// Raises Cooking skills
        /// </summary>
        private void RaiseCookingSkill()
        {
          PrintCookingSkillInfo();
          Player.m_localPlayer.RaiseSkill(rkCookingSkill);
          LogDebug($"Cooking Skill Raised");
          PrintCookingSkillInfo();
        }

        /// <summary>
        /// Print to the log details about the current cooking crafting skill level if a DEBUG Build
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private void PrintCookingSkillInfo()
        {
          LogDebug($"[Skill Level Info] Current Level: {Player.m_localPlayer.GetSkills().m_skillData.FirstOrDefault(s => s.Key == rkCookingSkill).Value?.m_level ?? 0} ({(Player.m_localPlayer.GetSkills().m_skillData.FirstOrDefault(s => s.Key == rkCookingSkill).Value?.GetLevelPercentage() ?? 0) * 100}%), " +
                   $"Next Level: {Player.m_localPlayer.GetSkills().m_skillData.FirstOrDefault(s => s.Key == rkCookingSkill).Value?.m_accumulator ?? 0}/{Player.m_localPlayer.GetSkills().m_skillData.FirstOrDefault(s => s.Key == rkCookingSkill).Value?.GetNextLevelRequirement() ?? 0}");
        }

        /// <summary>
        /// Check if the current crafting station is one used for cooking.
        /// </summary>
        /// <param name="currentCraftingStationName">Name of the crafting station</param>
        /// <returns>true if the current crafting station is one used for cooking else false</returns>
        private bool IsValidCookingCraftingStation(string currentCraftingStationName)
        {
          LogDebug($"currentCraftingStationName : {currentCraftingStationName}");
          switch (currentCraftingStationName)
          {
            case "rk_griddle(Clone)":
            case "rk_grill(Clone)":
            case "rk_prep(Clone)":
            case "piece_cauldron(Clone)":
              return true;
          }

          return false;
        }

        /// <summary>
        /// Check if the item is being added via crafting.
        /// </summary>
        /// <param name="crafterID">Id of the player who is crafting</param>
        /// <param name="crafterName">Name of the player who is crafting</param>
        /// <returns></returns>
        private bool IsFromCrafting(long crafterID, string crafterName)
        {
          return !string.IsNullOrEmpty(crafterName) && crafterID >= 1;
        }

        /// <summary>
        /// Check if an item is a Consumable Type
        /// </summary>
        /// <param name="prefabName">Name of the item</param>
        /// <returns>true if the item is a Consumable else false.</returns>
        private bool IsConsumable(string prefabName)
        {
          var itemPrefab = ObjectDB.instance.GetItemPrefab(prefabName);
          if (itemPrefab == null) return false;
          var itemDrop = itemPrefab.GetComponent<ItemDrop>();
          if (itemDrop == null) return false;
          return itemDrop.m_itemData.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Consumable;
        }

        /// <summary>
        /// Patch for AddItem method.
        /// </summary>
        /// <param name="itemName">Name of the item</param>
        /// <param name="stack">Stack size</param>
        /// <param name="quality">Quality level</param>
        /// <param name="variant">Variant to use</param>
        /// <param name="crafterID">Id of the player who is crafting</param>
        /// <param name="crafterName">Name of the player who is crafting</param>
        public void OnInventoryAddItemPostFix(string itemName, int stack, int quality, int variant, long crafterID, string crafterName)
        {
          if (_isAddingExtraItem) return; // Recursive loop detection. 
          LogDebug($"itemName: {itemName}, crafterID: {crafterID}, crafterName: {crafterName}");
          LogDebug($"CookingSkillEnable.Value : {CookingSkillEnable?.Value}");
          if (!CookingSkillEnable?.Value ?? false) return;
          if (!IsFromCrafting(crafterID, crafterName)) return; // Item is being bought from trader.
          if (!IsConsumable(itemName)) return;
          if (!IsValidCookingCraftingStation(Player.m_localPlayer.GetCurrentCraftingStation()?.name)) return;

          LogDebug($"BonusWhenCookingEnabled.Value : {BonusWhenCookingEnabled?.Value}");
          if (BonusWhenCookingEnabled?.Value ?? false)
          {
            var skillLevel = Player.m_localPlayer.GetSkills().m_skillData.FirstOrDefault(s => s.Key == rkCookingSkill).Value?.m_level ?? 0;
            LogDebug($"skillLevel : { skillLevel }");
            // 1-100% chance to craft an extra item. 1% per level of skill.
            if (IsCrafterLucky(skillLevel))
            {
              LogDebug($"[1][Start] -------------- ");
              AddExtraItem(itemName);
              LogDebug($"[1][End] ---------------- ");
            }

            // Max 25% chance to craft a 2nd extra after getting to skill level 25.
            if (skillLevel > 25f && IsCrafterLucky(skillLevel / 4))
            {
              LogDebug($"[2][Start] -------------- ");
              AddExtraItem(itemName);
              LogDebug($"[2][End] ---------------- ");
            }
          }

          if (!CookingSkillEnable.Value) return;
          RaiseCookingSkill();
        }

        /// <summary>
        /// Adds an extra item to the player inventory.
        /// Checks that the player has room in their
        /// inventory before trying to add the item.
        /// </summary>
        /// <param name="itemName"></param>
        private void AddExtraItem(string itemName)
        {
          var itemPrefab = ObjectDB.instance.GetItemPrefab(itemName);
          if (!Player.m_localPlayer.GetInventory().CanAddItem(itemPrefab, 1)) return;
          LogDebug($"Trying to add extra item: {itemName}");
          AddItem(itemName);
          LogDebug($"Added extra item: {itemName}");
        }

        /// <summary>
        /// AddItem Recursive loop flag
        /// </summary>
        private static bool _isAddingExtraItem;

        /// <summary>
        /// Adds an item to the players inventory.
        /// All checks for the player having space for a new
        /// item must be done before calling this method.
        /// 
        /// This is a recursive loop because the AddItem
        /// method is being patched. To break it, we are
        /// setting a flag to track this.
        /// </summary>
        /// <param name="itemName">Name of item to add.</param>
        private void AddItem(string itemName)
        {
          _isAddingExtraItem = true; // Recursive loop flag.
          Player.m_localPlayer.GetInventory().AddItem(itemName, 1, 1, 0, Player.m_localPlayer.GetPlayerID(), Player.m_localPlayer.GetPlayerName());
          _isAddingExtraItem = false; // Reset flag.
        }

        /// <summary>
        /// Calculate crafter's luck.
        /// </summary>
        /// <param name="skillLevel">Current skill level</param>
        /// <returns>true if crafter is lucky else false</returns>
        private bool IsCrafterLucky(float skillLevel)
        {
          if (skillLevel < 1) return false;
          var rand = Random.Range(1, 100);
          LogDebug($"Skill Level: {skillLevel} - Rand: {rand}");
          LogDebug($"rand < skillLevel : {rand < skillLevel}");
          return rand < skillLevel;
        }

        /// <summary>
        /// Writes Debug messages if a DEBUG Build
        /// </summary>
        /// <param name="msg">Message to print to the log.</param>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogDebug(string msg)
        {
          Jotunn.Logger.LogDebug(msg);
        }
    }
}
