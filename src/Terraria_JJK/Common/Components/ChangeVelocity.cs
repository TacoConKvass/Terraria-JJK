namespace Terraria_JJK.Components;

[EC.Component]
public record struct ChangeVelocity(System.Func<FNA.Vector2, FNA.Vector2> Change) : ITimeable
{
	[DaybreakHooks.GlobalProjectileHooks.AI]
	static void AlterProjectile(Terraria.Projectile projectile) {
		if (!projectile.TryGet(out ChangeVelocity data)) return;

		projectile.velocity = data.Change(projectile.velocity);
		projectile.Disable<ChangeVelocity>();
	}
}