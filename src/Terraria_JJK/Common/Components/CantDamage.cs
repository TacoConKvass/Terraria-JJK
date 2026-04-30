namespace Terraria_JJK.Components;

[EC.Component]
public record struct CantDamage : ITimeable
{
	[DaybreakHooks.GlobalProjectileHooks.CanHitNPC]
	static bool? ProjectileCanHitNPC(DaybreakHooks.GlobalProjectileHooks.CanHitNPC.Original orig, Terraria.Projectile projectile, Terraria.NPC target) {
		return projectile.Enabled<CantDamage>() ? false : orig(projectile, target);
	}
}