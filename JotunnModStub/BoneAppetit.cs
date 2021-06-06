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
    [BepInDependency(Jotunn.Main.ModGuid, "2.0.11")]
    internal class BoneAppetit : BaseUnityPlugin
    {
        public const string PluginGUID = "com.rockerkitten.boneappetit";
        public const string PluginName = "BoneAppetit";
        public const string PluginVersion = "1.1.1";
        private AssetBundle assetBundle;
        private AssetBundle customfood;
        //public Sprite CookingSprite;
        //private Skills.SkillType rkCookingSkill = 0;
        public ConfigEntry<bool> ConesEnable;
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

        public ConfigEntry<bool> IceCreamEnable { get; private set; }

        public ConfigEntry<bool> PorridgeEnable;
        private EffectList buildstone;
        private EffectList cookingsound;
        private GameObject icecream_prefab;
        private CustomItem icecream;
        private GameObject porkrind_prefab;
        private CustomItem porkrind;
        private GameObject kabob_prefab;
        private CustomItem kabob;
        private GameObject friedlox_prefab;
        private CustomItem friedlox;
        private GameObject glazedcarrot_prefab;
        private CustomItem glazedcarrot;
        private GameObject bacon_prefab;
        private CustomItem bacon;
        private GameObject smokedfish_prefab;
        private CustomItem smokedfish;
        private GameObject pancake_prefab;
        private CustomItem pancake;
        private GameObject pizza_prefab;
        private CustomItem pizza;
        private GameObject coffee_prefab;
        private CustomItem coffee;
        private GameObject latte_prefab;
        private CustomItem latte;
        private GameObject firecream_prefab;
        private CustomItem firecream;
        private CustomItem electriccream;
        private GameObject electriccream_prefab;
        private CustomItem acidcream;
        private GameObject acidcream_prefab;
        private GameObject porridge_prefab;
        private CustomItem porridge;

        private void Awake()
        {
            CreatConfigValues();
            AssetLoad();
            //AddSkills();
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
        private void CreatConfigValues()
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
            IceCreamEnable = Config.Bind("Ice Cream", "Enable", true, new ConfigDescription("Ice Cream Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            PorridgeEnable = Config.Bind("Porridge", "Enable", true, new ConfigDescription("Porridge Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        private void LoadFood()
        {
            icecream.Recipe.Recipe.m_enabled = IceCreamEnable.Value;
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
            firecream.Recipe.Recipe.m_enabled = IceCreamEnable.Value;
            electriccream.Recipe.Recipe.m_enabled = IceCreamEnable.Value; //idfk where the rest is going off what I see here
            acidcream.Recipe.Recipe.m_enabled = IceCreamEnable.Value; //cuz it gets used for all your ice cream
            porridge.Recipe.Recipe.m_enabled = PorridgeEnable.Value;
        }
        private void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("grill", Assembly.GetExecutingAssembly());
            customfood = AssetUtils.LoadAssetBundleFromResources("customfood", Assembly.GetExecutingAssembly());
            //CookingSprite = customfood.LoadAsset<Sprite>("rkcookingsprite");
        }


        /*public void AddSkills()
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
        }*/
        private void LoadSounds()
        {
            try
            {
                var sfxstone = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_stone");
                var vfxstone = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_stone_wall_2x1");
                var sfxcook = PrefabManager.Cache.GetPrefab<GameObject>("sfx_cooking_station_done");

                buildstone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstone }, new EffectList.EffectData { m_prefab = vfxstone } } };
                cookingsound = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxcook } } };

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
            //piece_grill

            var grillfab = assetBundle.LoadAsset<GameObject>("rk_grill");
            var grill = new CustomPiece(grillfab,
                new PieceConfig
                {
                    CraftingStation = "forge",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Stone", Amount = 10, Recover = true },
                        new RequirementConfig { Item = "Iron", Amount = 1, Recover = true }
                    }
                   
                });
            var grillthing = grillfab.GetComponent<Piece>();
            grillthing.m_placeEffect = buildstone;

            var grillstation = grillfab.GetComponent<CraftingStation>();
            grillstation.m_craftItemEffects = cookingsound;
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
            griddlething.m_placeEffect = buildstone;

            var griddlestation = griddlefab.GetComponent<CraftingStation>();
            griddlestation.m_craftItemEffects = cookingsound;
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
            ovenpiece.m_placeEffect = buildstone;

            PieceManager.Instance.AddPiece(oven);
        }
        private void IceCream()
        {
            icecream_prefab = customfood.LoadAsset<GameObject>("rk_icecream");
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
            porkrind_prefab = customfood.LoadAsset<GameObject>("rk_porkrind");
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
            kabob_prefab = customfood.LoadAsset<GameObject>("rk_kabob");
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
            friedlox_prefab = customfood.LoadAsset<GameObject>("rk_friedloxmeat");
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
            glazedcarrot_prefab = customfood.LoadAsset<GameObject>("rk_glazedcarrots");
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
            bacon_prefab = customfood.LoadAsset<GameObject>("rk_bacon");
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
            smokedfish_prefab = customfood.LoadAsset<GameObject>("rk_smokedfish");
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
            pancake_prefab = customfood.LoadAsset<GameObject>("rk_pancake");
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
             pizza_prefab = customfood.LoadAsset<GameObject>("rk_pizza");
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
             coffee_prefab = customfood.LoadAsset<GameObject>("rk_coffee");
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
             latte_prefab = customfood.LoadAsset<GameObject>("rk_latte");
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
            firecream_prefab = customfood.LoadAsset<GameObject>("rk_firecream");
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
            electriccream_prefab = customfood.LoadAsset<GameObject>("rk_electriccream");
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
            acidcream_prefab = customfood.LoadAsset<GameObject>("rk_acidcream");
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
               porridge_prefab = customfood.LoadAsset<GameObject>("rk_porridge");
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

    }
}
