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
        public const string PluginVersion = "1.1.2";
        private AssetBundle assetBundle;
        private AssetBundle customfood;
        public Sprite CookingSprite;
        private Skills.SkillType rkCookingSkill = 0;
        public ConfigEntry<bool> IceCreamEnable;
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

        private void Awake()
        {
            CreatConfigValues();
            AssetLoad();
            AddSkills();
            //LoadFood();
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
            
            SynchronizationManager.OnConfigurationSynchronized += (obj, attr) =>
            {
                if (attr.InitialSynchronization)
                {
                    Jotunn.Logger.LogMessage("Initial Config sync event received");
                   // LoadFood();
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
           
            IceCreamEnable = Config.Bind("Ice Cream", "Enable", true, new ConfigDescription("Ice Cream Enable", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
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

        }


        private void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("grill", Assembly.GetExecutingAssembly());
            customfood = AssetUtils.LoadAssetBundleFromResources("customfood", Assembly.GetExecutingAssembly());
            CookingSprite = customfood.LoadAsset<Sprite>("rkcookingsprite");
        }

        public void AddSkills()
        {
            #region Cooking Skill Config

            #endregion

            // Test adding a skill with a texture
            rkCookingSkill = SkillManager.Instance.AddSkill(new SkillConfig
            {

                Identifier = "rkCookingSkill",
                Name = "Cooking Skill",
                Description = "Learn to cook like a Viking!",
                Icon = CookingSprite,
                IncreaseStep = 1f
            });
        }
       /* private void LoadFood()
        {
            #region Pork Rind Config
            var prkrd = PrefabManager.Instance.GetPrefab("rk_porkrind").GetComponent<ItemDrop>();
            enabled = (bool)PorkRindEnable.Value;
            #endregion
            #region Ice Cream Config
            var icrm = PrefabManager.Instance.GetPrefab("rk_icecream").GetComponent<ItemDrop>();
            enabled = (bool)IceCreamEnable.Value;
            #endregion
            #region Kabob Config
            var kbb = PrefabManager.Instance.GetPrefab("rk_kabob").GetComponent<ItemDrop>();
            enabled = (bool)KabobEnable.Value;
            #endregion
            #region Fried Lox Config
            var flm = PrefabManager.Instance.GetPrefab("rk_friedloxmeat").GetComponent<ItemDrop>();
            enabled = (bool)FriedLoxEnable.Value;
            #endregion
            #region Glazed Carrots Config
            var glzdcrt = PrefabManager.Instance.GetPrefab("rk_glazedcarrots").GetComponent<ItemDrop>();
            enabled = (bool)GlazedCarrotEnable.Value;
            #endregion
            #region Bacon Config
            var bcn = PrefabManager.Instance.GetPrefab("rk_bacon").GetComponent<ItemDrop>();
            enabled = (bool)BaconEnable.Value;
            #endregion
            #region Smoked Fish Config
            var smkdfsh = PrefabManager.Instance.GetPrefab("rk_smokedfish").GetComponent<ItemDrop>();
            enabled = (bool)SmokedFishEnable.Value;
            #endregion
            #region Pancake Config
            var pnck = PrefabManager.Instance.GetPrefab("rk_pancake").GetComponent<ItemDrop>();
            enabled = (bool)PancakesEnable.Value;
            #endregion
            #region Pizza Config
            var pzz = PrefabManager.Instance.GetPrefab("rk_pizza").GetComponent<ItemDrop>();
            enabled = (bool)PizzaEnable.Value;
            #endregion
            #region Coffee Config
            var cff = PrefabManager.Instance.GetPrefab("rk_coffee").GetComponent<ItemDrop>();
            enabled = (bool)CoffeeEnable.Value;
            #endregion
            #region Latte Config
            var ltt = PrefabManager.Instance.GetPrefab("rk_latte").GetComponent<ItemDrop>();
            enabled = (bool)LatteEnable.Value;
            #endregion
        }*/

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
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "SurtlingCore", Amount = 2, Recover = true },
                        new RequirementConfig { Item = "TrophySurtling", Amount = 1, Recover = true },
                        new RequirementConfig { Item = "Stone", Amount = 10, Recover = true }
                    }
                });
            PieceManager.Instance.AddPiece(oven);
        }
        private void IceCream()
        {
            var icecream_prefab = customfood.LoadAsset<GameObject>("rk_icecream");
            var icecream = new CustomItem(icecream_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Ice Cream",
                    Enabled = IceCreamEnable.Value,
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
            var porkrind_prefab = customfood.LoadAsset<GameObject>("rk_porkrind");
            var porkrind = new CustomItem(porkrind_prefab, fixReference: false,
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
            var kabob_prefab = customfood.LoadAsset<GameObject>("rk_kabob");
            var kabob = new CustomItem(kabob_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Kebab",
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
            var friedlox_prefab = customfood.LoadAsset<GameObject>("rk_friedloxmeat");
            var friedlox = new CustomItem(friedlox_prefab, fixReference: false,
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
            var glazedcarrot_prefab = customfood.LoadAsset<GameObject>("rk_glazedcarrots");
            var glazedcarrot = new CustomItem(glazedcarrot_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Honey Glazed Carrots",
                    Enabled = GlazedCarrotEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_griddle",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Carrot", Amount = 2},
                        new RequirementConfig { Item = "Honey", Amount = 2},
                        new RequirementConfig { Item = "Dandelion", Amount = 1 }
                    }
                });

            ItemManager.Instance.AddItem(glazedcarrot);

        }
        private void Bacon()
        {
            var bacon_prefab = customfood.LoadAsset<GameObject>("rk_bacon");
            var bacon = new CustomItem(bacon_prefab, fixReference: false,
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
            var smokedfish_prefab = customfood.LoadAsset<GameObject>("rk_smokedfish");
            var smokedfish = new CustomItem(smokedfish_prefab, fixReference: false,
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
            var pancake_prefab = customfood.LoadAsset<GameObject>("rk_pancake");
            var pancake = new CustomItem(pancake_prefab, fixReference: false,
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
            var pizza_prefab = customfood.LoadAsset<GameObject>("rk_pizza");
            var pizza = new CustomItem(pizza_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Pizza",
                    Enabled = PizzaEnable.Value,
                    Amount = 1,
                    CraftingStation = "rk_grill",
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
            var coffee_prefab = customfood.LoadAsset<GameObject>("rk_coffee");
            var coffee = new CustomItem(coffee_prefab, fixReference: false,
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
            var latte_prefab = customfood.LoadAsset<GameObject>("rk_latte");
            var latte = new CustomItem(latte_prefab, fixReference: false,
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
       
    }
}