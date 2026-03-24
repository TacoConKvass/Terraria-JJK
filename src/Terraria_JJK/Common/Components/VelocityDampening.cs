namespace Terraria_JJK.Components;

[EntityComponent.Component]
public record struct DampenVelocity(float Factor, float MinVelocity)
{
	[DaybreakHooks.GlobalProjectileHooks.AI]
	internal static void ApplyToProjectile(Terraria.Projectile projectile) {
		if (!projectile.TryGet<DampenVelocity>(out var data)) return;

		if (projectile.velocity.LengthSquared() <= data.MinVelocity * data.MinVelocity) {
			projectile.Disable<DampenVelocity>();
			projectile.velocity.Normalize();
			projectile.velocity *= data.MinVelocity;
		}

		projectile.velocity *= 1f - data.Factor;
	}
}