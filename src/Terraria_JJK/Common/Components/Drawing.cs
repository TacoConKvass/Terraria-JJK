namespace Terraria_JJK.Components;

[EC.Component]
public record struct DrawPositionAdjustment(FNA.Vector2 Translation, FNA.Vector2 Origin) { }

file class DrawingAdjustments
{
	[DaybreakHooks.GlobalProjectileHooks.PreDraw]
	public static bool PreDraw(DaybreakHooks.GlobalProjectileHooks.PreDraw.Original orig, Terraria.Projectile projectile, ref FNA.Color lightColor) {
		if (
			!projectile.TryGet(out DrawPositionAdjustment positionAdjustment)
		) return orig(projectile, ref lightColor);

		var texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
		var frameUniform = projectile.frame / (float)Terraria.Main.projFrames[projectile.type];

		DoDraw(
			texture, projectile.Center + positionAdjustment.Translation,
			frameUniform, Terraria.Main.projFrames[projectile.type], lightColor, projectile.rotation, positionAdjustment.Origin, projectile.scale
		);

		return false;
	}

	public static void DoDraw(FNA.Graphics.Texture2D texture, FNA.Vector2 position, float frameUniform, float frameCount, FNA.Color color, float rotation, FNA.Vector2 origin, float scale) {
		Terraria.Main.EntitySpriteDraw(
			texture,
			position - Terraria.Main.screenPosition,
			texture.Bounds with { Y = (int)(texture.Bounds.Height * frameUniform), Height = (int)(texture.Height / frameCount) },
			color, rotation, origin, scale, FNA.Graphics.SpriteEffects.None
		);
	}
}