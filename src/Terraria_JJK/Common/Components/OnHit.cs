namespace Terraria_JJK.Components;

public enum TargetType : byte
{
	Self,
	Victim,
	None,
}

[EC.Component(Wraps = [
	typeof(Fade), typeof(ApplyBuff), typeof(Shoots)
])]
public record struct OnHit<T>(T Inner, TargetType Target) where T : struct
{
	static OnHit() {
		DaybreakHooks.GlobalProjectileHooks.OnHitNPC.Event += OnProjectileHitNPC;
	}

	internal static void OnProjectileHitNPC(DaybreakHooks.GlobalProjectileHooks.OnHitNPC.Original orig, TML.GlobalProjectile self, Terraria.Projectile projectile, Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone) {
		orig(projectile, target, hit, damageDone);

		if (!projectile.TryGet<OnHit<T>>(out var data)) return;

		T? _ = data.Target switch {
			TargetType.Self => projectile.With(data.Inner),
			TargetType.Victim => target.With(data.Inner),
			TargetType.None => TryTrigger(projectile, data.Inner),
			_ => null,
		};
	}

	static T? TryTrigger(Terraria.Projectile entity, T data) {
		if (data is not Core.ITriggerable<Terraria.Projectile> i) return null;

		entity.With(data);
		i.Trigger(entity);
		return null;
	}
}