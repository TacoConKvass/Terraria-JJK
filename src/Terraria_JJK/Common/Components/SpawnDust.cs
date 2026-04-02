using static Terraria.Utils;

namespace Terraria_JJK.Components;

[EC.Component]
public record struct SpawnDust(
	int Type,
	(int[] Types, bool Random)? Queue,
	int Count,
	System.Func<FNA.Vector2> Velocity,
	System.Func<FNA.Vector2> RelativePosition,
	FNA.Color Color,
	int Alpha,
	float? Scale,
	System.Action? Callback
) : ITriggerable, ITimeable
{
	[DaybreakHooks.GlobalProjectileHooks.AI]
	internal static void ProjectileUpdate(Terraria.Projectile projectile) {
		if (!projectile.TryGet(out SpawnDust data)) return;

		(data as ITriggerable).Trigger(projectile, null!, TargetType.Self);
	}

	void ITriggerable.Trigger(Terraria.Entity source, Terraria.Entity target, TargetType targetType) {
		if (targetType == TargetType.Self) {
			var center = source.Center;
			var bounds = source.Hitbox;

			for (int i = 0; i < Count; i++) {
				var type = Type;
				if (Queue is (int[] Types, bool Random))
					type = Random ? Terraria.Main.rand.NextFromList(Types) : Types[i % Types.Length];

				var velocity = Velocity();

				Terraria.Dust.NewDust(
					center + RelativePosition(),
					bounds.Width, bounds.Height,
					type, velocity.X, velocity.Y,
					Alpha: Alpha, newColor: Color, Scale: Scale ?? 1
				);
			}

			Callback?.Invoke();
			source.Disable<SpawnDust>();
			return;
		}

		target.With(this);
		return;
	}
}