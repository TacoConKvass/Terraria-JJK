namespace Terraria_JJK.Components;

[EntityComponent.Component]
public struct RightClickable
{
	public System.Action<Terraria.Player> Effect;
}

internal class RightClickable_Impl
{
	[DaybreakHooks.GlobalItemHooks.CanRightClick]
	internal static bool Item_RightClickable(DaybreakHooks.GlobalItemHooks.CanRightClick.Original orig, Terraria.Item item) {
		return item.Enabled<RightClickable>() ? true : orig(item);
	}

	[DaybreakHooks.GlobalItemHooks.RightClick]
	internal static void Item_TriggerEffect(Terraria.Item item, Terraria.Player player) {
		if (!item.TryGet<RightClickable>(out var data)) return;
		data.Effect(player);
	}
}