namespace Terraria_JJK.Components;

[EntityComponent.Component]
public record struct ApplyBuff(int Type, int Duration)
{
	[DaybreakHooks.GlobalNPCHooks.AI]
	internal static void ApplyBuffToNPC(Terraria.NPC npc) {
		if (!npc.TryGet<ApplyBuff>(out var data)) return;

		npc.AddBuff(data.Type, data.Duration);
		npc.Disable<ApplyBuff>();
	}
}
