namespace Terraria_JJK.Components;

[EC.Component]
public record struct RightClickable(System.Func<Terraria.Entity, bool> When, System.Func<Terraria.Player, bool?> Effect)
{
	[DaybreakHooks.GlobalItemHooks.AltFunctionUse]
	internal static bool Item_RightClickable(DaybreakHooks.GlobalItemHooks.AltFunctionUse.Original orig, Terraria.Item item, Terraria.Player player) {
		return item.TryGet(out RightClickable data) ? data.When(player) : orig(item, player);
	}

	[DaybreakHooks.GlobalItemHooks.UseItem]
	internal static bool? Item_TriggerEffect(Terraria.Item item, Terraria.Player player) {
		if (!item.TryGet<RightClickable>(out var data) || player.altFunctionUse != 2) return null;
		return data.Effect?.Invoke(player);
	}
}
