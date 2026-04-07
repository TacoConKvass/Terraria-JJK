namespace Terraria_JJK.Components;

[EC.Component]
public record struct RotateWithVelocity(float AdditionalRotation)
{
	[DaybreakHooks.GlobalProjectileHooks.AI]
	internal static void RotateProjectile(Terraria.Projectile projectile) {
		if (!projectile.TryGet<RotateWithVelocity>(out var data)) return;

		projectile.rotation = Terraria.Utils.ToRotation(projectile.velocity) + data.AdditionalRotation;
	}

	[DaybreakHooks.GlobalNPCHooks.AI]
	internal static void RotateNPC(Terraria.NPC npc) {
		if (!npc.TryGet<RotateWithVelocity>(out var data)) return;

		npc.rotation = Terraria.Utils.ToRotation(npc.velocity) + data.AdditionalRotation;
	}
}