using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using Jotunn.Utils;
using System.Reflection;
using Jotunn.Entities;
using Jotunn.Configs;
using Jotunn.Managers;
using System;

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

        public ConfigEntry<bool> ConesEnable { get; private set; }

        public ConfigEntry<bool> PorridgeEnable;
        public ConfigEntry<bool> PBJEnable;
        public ConfigEntry<bool> CakeEnable;
        public ConfigEntry<bool> GrillOriginal;

        public EffectList buildStone;
        public EffectList cookingSound;
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

        public void Awake()
        {
            CreatConfigValues();
            AssetLoad();
            AddSkills();
            ItemManager.OnVanillaItemsAvailable += LoadSounds;
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
            GrillOriginal = Config.Bind("Original", "Enable", true, new ConfigDescription("Use original grill", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
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
        }
        public void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("grill", Assembly.GetExecutingAssembly());
            customFood = AssetUtils.LoadAssetBundleFromResources("customfood", Assembly.GetExecutingAssembly());
            CookingSprite = customFood.LoadAsset<Sprite>("rkcookingsprite");
            //fireclip = customFood.LoadAsset<AudioClip>("")


        }


        public void AddSkills()
        {

            if (SkillEnable.Value == true)
            // Test adding a skill with a texture
            {
                rkCookingSkill = SkillManager.Instance.AddSkill(new SkillConfig
                {

                    Identifier = "rkCookingSkill",
                    Name = "Cooking Skill",
                    Description = "Learn to cook like a Viking!",
                    Icon = CookingSprite,
                    IncreaseStep = 1f,
                });
            }
        }
        public void LoadSounds()
        {
            try
            {
                var sfxstone = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_stone");
                var vfxstone = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_stone_wall_2x1");
                var sfxcook = PrefabManager.Cache.GetPrefab<GameObject>("sfx_cooking_station_done");
                

                //var sfxvol = AudioMan.instance.m_ambientLoopSource;

                buildStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstone }, new EffectList.EffectData { m_prefab = vfxstone } } };
                cookingSound = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxcook } } };

                Jotunn.Logger.LogMessage("Loaded Game VFX and SFX");
                Jotunn.Logger.LogMessage("Prepping Kitchen...");
                LoadItem();
                LoadGriddle();
                Oven();
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

                Jotunn.Logger.LogMessage("Load Complete. Bone Appetit yall.");
            }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while running OnVanillaLoad: {ex.Message}");
            }
            finally
            {
                ItemManager.OnVanillaItemsAvailable -= LoadSounds;
            }

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
            grillThing.m_placeEffect = buildStone;

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
            PieceManager.Instance.AddPiece(griddle);
        }
        private void Oven()
        {
            //piece_griddle

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
        private void IceCream()
        {
            icecream_prefab = customFood.LoadAsset<GameObject>("rk_icecream");
            icecream = new CustomItem(icecream_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Ice Cream",
                    Enabled = ConesEnable.Value,
                    Amount = 2,
                    CraftingStation = "piece_cauldron",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "FreezeGland", Amount = 4},
                        new RequirementConfig { Item = "Blueberries", Amount = 8},
                        new RequirementConfig { Item = "Honey", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(icecream);

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
                        new RequirementConfig { Item = "LeatherScraps", Amount = 1}
                    }
                });

            ItemManager.Instance.AddItem(porkrind);

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
                        new RequirementConfig { Item = "Thistle", Amount = 1},
                        new RequirementConfig { Item = "Cloudberry", Amount = 2}
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
                        new RequirementConfig { Item = "RawMeat", Amount = 1}
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
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig { Item = "BarleyFlour", Amount = 3},
                        new RequirementConfig { Item = "Cloudberry", Amount = 5}
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
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig { Item = "BarleyFlour", Amount = 3},
                        new RequirementConfig { Item = "Mushroom", Amount = 2},
                        new RequirementConfig { Item = "CookedMeat", Amount = 2}
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
                    CraftingStation = "piece_cauldron",
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
                    CraftingStation = "piece_cauldron",
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
                    CraftingStation = "piece_cauldron",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "SurtlingCore", Amount = 4},
                        new RequirementConfig { Item = "Raspberry", Amount = 8},
                        new RequirementConfig { Item = "Honey", Amount = 2}
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
                    CraftingStation = "piece_cauldron",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Crystal", Amount = 4},
                        new RequirementConfig { Item = "Cloudberry", Amount = 8},
                        new RequirementConfig { Item = "Honey", Amount = 2}
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
                    CraftingStation = "piece_cauldron",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Guck", Amount = 4},
                        new RequirementConfig { Item = "MushroomYellow", Amount = 8},
                        new RequirementConfig { Item = "Honey", Amount = 2}
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
                          new RequirementConfig { Item = "Honey", Amount = 2}
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
                          new RequirementConfig { Item = "Flax", Amount = 4}
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
                          new RequirementConfig { Item = "Cloudberry", Amount = 4}
                   }
               });

            ItemManager.Instance.AddItem(cake);
           
        }
        
    }

}
