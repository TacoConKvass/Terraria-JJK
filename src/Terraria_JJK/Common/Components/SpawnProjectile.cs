using static Terraria.Utils;
using TypeQueue = (int[] Types, bool Random);

namespace Terraria_JJK.Components;

[EC.Component]
public record struct SpawnProjectile(
	int Type,
	TypeQueue? Queue,
	System.Func<FNA.Vector2>? RelativePosition,
	System.Func<FNA.Vector2>? Velocity,
	int Damage,
	System.Action<Terraria.Projectile>? Callback,
	float? KnockBack,
	int? Count
) : ITriggerable, ITimeable
{
	[DaybreakHooks.GlobalProjectileHooks.AI]
	public static void TriggerOnProjectile(Terraria.Projectile projectile) {
		if (!projectile.TryGet(out SpawnProjectile data)) return;

		data.Trigger(projectile, null!, TargetType.Self);
		projectile.Disable<SpawnProjectile>();
	}

	public void Trigger(Terraria.Entity source, Terraria.Entity target, TargetType targetType) {
		if (targetType == TargetType.Victim) {
			Trigger(target, source, TargetType.Self);
			return;
		}

		for (int i = 0; i < (Count ?? 1); i++) {
			var spawned = Terraria.Projectile.NewProjectileDirect(
				spawnSource: source.GetSource_FromThis(),
				position: source.Center + (RelativePosition?.Invoke() ?? FNA.Vector2.Zero),
				velocity: Velocity?.Invoke() ?? FNA.Vector2.Zero,
				type: Queue is TypeQueue inst ?
					(inst.Random ? Terraria.Main.rand.NextFromList(inst.Types) : inst.Types[i % inst.Types.Length]) :
					Type,
				damage: Damage,
				knockback: KnockBack ?? 0,
				owner: source switch {
					Terraria.Player player => player.whoAmI,
					Terraria.NPC npc => npc.releaseOwner,
					Terraria.Projectile projectile => projectile.owner,
					_ => -1
				}
			);
			Callback?.Invoke(spawned);
		}
	}
}