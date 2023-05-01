using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace MoarSlots
{
    public class Patches
    {
        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.Awake))]
        public static class HeightPatch
        {
            public static void Prefix(Inventory ___m_inventory)
            {
                ___m_inventory.m_height = MoarSlotsMod.Slots!.Value;
            }
        }

        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Awake))]
        public static class ScrollerPatch
        {
            public static void Postfix(InventoryGui __instance)
            {
                var t = __instance.transform.Find("root/Player/PlayerGrid");
                var c = t.Find("Root");
                var mask = t.gameObject.GetComponent<RectMask2D>();
                var scroll = t.gameObject.AddComponent<ScrollRect>();
                scroll.horizontal = false;
                scroll.scrollSensitivity = 15;
                scroll.content = c.gameObject.GetComponent<RectTransform>();
                mask.enabled = true;
            }
        }
    }
}