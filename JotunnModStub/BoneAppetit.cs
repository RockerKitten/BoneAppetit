using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using Jotunn.Utils;
using System.Reflection;
using Jotunn.Entities;
using Jotunn.Configs;
using Jotunn.Managers;
using System;

namespace BoneAppetit
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    internal class BoneAppetit : BaseUnityPlugin
    {
        public const string PluginGUID = "com.rockerkitten.boneappetit";
        public const string PluginName = "BoneAppetit";
        public const string PluginVersion = "1.2.0";
        private AssetBundle assetBundle;
        private AssetBundle customfood;
        

        private void Awake()
        {

            AssetLoad();
            //AddSkills();
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
            //Cake();
            //PBJ();


        }
        private void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("grill", Assembly.GetExecutingAssembly());
            customfood = AssetUtils.LoadAssetBundleFromResources("customfood", Assembly.GetExecutingAssembly());
            //CookingSprite = customfood.LoadAsset<Sprite>("rkcookingsprite");
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
                    Enabled = true,
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
                    Enabled = true,
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
                    Name = "Kabob",
                    Enabled = true,
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
                    Enabled = true,
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
                    Enabled = true,
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
            var bacon_prefab = customfood.LoadAsset<GameObject>("rk_bacon");
            var bacon = new CustomItem(bacon_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Bacon",
                    Enabled = true,
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
                    Enabled = true,
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
                    Enabled = true,
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
                    Enabled = true,
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
                    Enabled = true,
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
                    Enabled = true,
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
            var firecream_prefab = customfood.LoadAsset<GameObject>("rk_firecream");
            var firecream = new CustomItem(firecream_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Fire Cream",
                    Enabled = true,
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
            var electriccream_prefab = customfood.LoadAsset<GameObject>("rk_electriccream");
            var electriccream = new CustomItem(electriccream_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Electric Cream",
                    Enabled = true,
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
            var acidcream_prefab = customfood.LoadAsset<GameObject>("rk_acidcream");
            var acidcream = new CustomItem(acidcream_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Acid Cream Cone",
                    Enabled = true,
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
            var porridge_prefab = customfood.LoadAsset<GameObject>("rk_porridge");
            var porridge = new CustomItem(porridge_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Porridge",
                    Enabled = true,
                    Amount = 1,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                          new RequirementConfig { Item = "Barley", Amount = 2},
                          new RequirementConfig { Item = "Cloudberry", Amount = 4},
                          new RequirementConfig { Item = "Honey", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(porridge);

        }
        /*private void Cake()
        {
            var cake_prefab = customfood.LoadAsset<GameObject>("rk_birthday");
            var cake = new CustomItem(cake_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Birthday Cake",
                    Enabled = true,
                    Amount = 1,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                          new RequirementConfig { Item = "BarleyFlour", Amount = 3},
                          new RequirementConfig { Item = "Cloudberry", Amount = 4},
                          new RequirementConfig { Item = "Honey", Amount = 2}
                    }
                });

            ItemManager.Instance.AddItem(cake);

        }
        private void PBJ()
        {
            var pbj_prefab = customfood.LoadAsset<GameObject>("rk_pbj");
            var pbj = new CustomItem(pbj_prefab, fixReference: false,
                new ItemConfig
                {
                    Name = "Jimmy's PBJ",
                    Enabled = true,
                    Amount = 4,
                    CraftingStation = "rk_grill",
                    Requirements = new[]
                    {
                          new RequirementConfig { Item = "Bread", Amount = 1},
                          new RequirementConfig { Item = "QueensJam", Amount = 1},
                          new RequirementConfig { Item = "Flax", Amount = 5}
                    }
                });

            ItemManager.Instance.AddItem(pbj);

        }*/

    }
}