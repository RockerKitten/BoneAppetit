using HarmonyLib;
using JetBrains.Annotations;
using System;

namespace BoneAppetit
{
  /// <summary>
  /// Harmony Patches
  /// </summary>
  public static class Patch
  {
    #region CookingStation

    /// <summary>
    /// Patch the cooking station
    /// </summary>
    [HarmonyPatch(typeof(CookingStation))]
    public static class PatchCookingStation
    {
      /// <summary>
      /// Patch the CookItem method
      /// </summary>
      /// <param name="__result">Was item placed on the cooking station successfully?</param>
      [HarmonyPostfix]
      [HarmonyPatch(nameof(CookingStation.CookItem))]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void Postfix(ref bool __result)
      {
        try
        {
          Boneappetit.BoneAppetit.Instance.OnCookingStationCookItem(ref __result);
        }
        catch (Exception e)
        {
          Jotunn.Logger.LogError(e);
        }
      }
    }

    #endregion

    /// <summary>
    /// Patch the Players Inventory
    /// </summary>
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.AddItem), typeof(string), typeof(int), typeof(int), typeof(int), typeof(long), typeof(string), typeof(bool)]
    public static class PatchInventory
    {
      /// <summary>
      /// Patch the AddItem method
      /// </summary>
      /// <param name="name">Name of the item</param>
      /// <param name="stack">Stack size</param>
      /// <param name="quality">Quality level</param>
      /// <param name="variant">Variant to use</param>
      /// <param name="crafterID">Id of the player who is crafting</param>
      /// <param name="crafterName">Name of the player who is crafting</param>
      [HarmonyPostfix]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void Postfix(string name, int stack, int quality, int variant, long crafterID, string crafterName, bool pickedUp)
      {
        try
        {
          Jotunn.Logger.LogDebug($"PatchInventoryPostfix");
          if (Player.m_localPlayer == null)
          {
            Jotunn.Logger.LogDebug("Player is null");
            return;
          }
          Boneappetit.BoneAppetit.Instance.OnInventoryAddItemPostFix(name, stack, quality, variant, crafterID, crafterName);
        }
        catch (Exception e)
        {
          Jotunn.Logger.LogError(e);
        }
      }
    }
  }
}
