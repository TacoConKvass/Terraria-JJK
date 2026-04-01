namespace Terraria_JJK.Components;

public enum TargetType : byte
{
	Self,
	Victim,
}

public interface ITriggerable
{
	public void Trigger(Terraria.Entity source, Terraria.Entity target, Components.TargetType targetType);

	public static void Default<T>(Terraria.Entity source, Terraria.Entity target, Components.TargetType targetType, T data) where T : struct {
		if (targetType == Components.TargetType.Self) {
			source.With(data);
			return;
		}

		target.With(data);
	}
}

[EC.Component(Wraps = typeof(ITriggerable))]
public record struct OnHit<T>(T Inner, TargetType Target = TargetType.Victim) where T : struct, ITriggerable
{
	static OnHit() {
		DaybreakHooks.GlobalProjectileHooks.OnHitNPC.Event += OnProjectileHitNPC;
	}

	internal static void OnProjectileHitNPC(DaybreakHooks.GlobalProjectileHooks.OnHitNPC.Original orig, TML.GlobalProjectile self, Terraria.Projectile projectile, Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone) {
		orig(projectile, target, hit, damageDone);

		if (!projectile.TryGet<OnHit<T>>(out var data)) return;
		data.Inner.Trigger(projectile, target, data.Target);
	}
}