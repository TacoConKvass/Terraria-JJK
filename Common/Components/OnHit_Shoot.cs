namespace Terraria_JJK.Components.OnHit;

[EntityComponent.Component]
public struct Shoot
{
	public (int[] Types, bool Random) Queue;
	public int Count;
	public System.Func<FNA.Vector2> Velocity;
	public System.Func<FNA.Vector2> RelativePosition;
}

internal class Shoot_Impl
{
	[DaybreakHooks.GlobalProjectileHooks.OnHitNPC]
	internal static void Projectile_HitNPC(Terraria.Projectile projectile, Terraria.NPC target, Terraria.NPC.HitInfo hit, int damageDone) {
		if (EC.TryGet<Shoot>(projectile, out var data)) {
			var types = data.Queue.Types;
			for (int i = 0; i < data.Count; i++)
				Terraria.Projectile.NewProjectile(
					projectile.GetSource_FromThis(),
					data.RelativePosition() + target.Center,
					data.Velocity(),
					data.Queue.Random ? Terraria.Utils.NextFromList(Terraria.Main.rand, types) : types[i % types.Length],
					projectile.damage,
					projectile.knockBack,
					projectile.owner
				);
		}
	}
}