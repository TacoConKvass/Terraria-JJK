using Enumerable = System.Linq.Enumerable;
using AttributeExt = System.Reflection.CustomAttributeExtensions;

namespace Terraria_JJK.EC;

file class Component<Data> : Component where Data : struct
{
	internal static Data?[] NPCs = [];
	internal static Data?[] Players = [];
	internal static Data?[] Projectiles = [];

	internal static Data?[] Init(Terraria.Entity[] entities) => new Data?[entities.Length];

	void Component.Init() => (NPCs, Players, Projectiles) = (Init(Terraria.Main.npc), Init(Terraria.Main.player), Init(Terraria.Main.projectile));
	void Component.Deinit() => (NPCs, Players, Projectiles) = (null!, null!, null!);
}

public static class ComponentExtensions
{
	public static void Set<T>(this Terraria.NPC npc, T data) where T : struct {
		if (!Terraria.Main.gameMenu)
			Component<T>.NPCs[npc.whoAmI] = data;
	}
	public static void Set<T>(this Terraria.Player player, T data) where T : struct {
		if (!Terraria.Main.gameMenu)
			Component<T>.Players[player.whoAmI] = data;
	}
	public static void Set<T>(this Terraria.Projectile projectile, T data) where T : struct {
		if (!Terraria.Main.gameMenu)
			Component<T>.Projectiles[projectile.whoAmI] = data;
	}

	public static T? Get<T>(this Terraria.NPC npc) where T : struct => Component<T>.NPCs[npc.whoAmI];
	public static T? Get<T>(this Terraria.Player player) where T : struct => Component<T>.Players[player.whoAmI];
	public static T? Get<T>(this Terraria.Projectile projectile) where T : struct => Component<T>.Projectiles[projectile.whoAmI];
}

[System.AttributeUsage(System.AttributeTargets.Struct)]
public class ComponentAttribute : System.Attribute;

file interface Component
{
	internal abstract void Init();
	internal abstract void Deinit();
}


file class ComponentLoader : TML.ModSystem
{
	private Component?[] components = [];

	public override void Load() {
		var types = Enumerable.Where(Mod.Code.GetTypes(), type => AttributeExt.GetCustomAttribute<ComponentAttribute>(type) != null);
		components = Enumerable.ToArray(
			Enumerable.Select(types, T => (Component?)System.Activator.CreateInstance(typeof(Component<>).MakeGenericType(T)))
		);
	}

	public override void OnWorldLoad() {
		foreach (var c in components) {
			c?.Init();
		}
	}

	public override void OnWorldUnload() {
		foreach (var c in components) {
			c?.Deinit();
		}
	}
}
