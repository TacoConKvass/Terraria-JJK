namespace Terraria_JJK.Components;

[EC.Component]
public record struct StickTo(Terraria.Entity Target, System.Func<FNA.Vector2>? WithOffset) : Core.ITriggerable
{
	[DaybreakHooks.GlobalProjectileHooks.AI]
	internal static void MoveStuckProjectile(Terraria.Projectile projectile) {
		if (!projectile.TryGet<StickTo>(out var data)) return;
		if (!data.Target.active) {
			projectile.Kill();
			return;
		}
		projectile.Center = data.Target.Center + (data.WithOffset?.Invoke() ?? FNA.Vector2.Zero);
	}

	public void Trigger(Terraria.Entity source, Terraria.Entity target, TargetType targetType) {
		var offset = source.Center - target.Center;
		Core.ITriggerable.Default(source, target, targetType, new StickTo {
			Target = targetType == TargetType.Self ? target : source,
			WithOffset = WithOffset ?? (() => offset),
		});
		source.Disable<OnHit<StickTo>>();
	}
}