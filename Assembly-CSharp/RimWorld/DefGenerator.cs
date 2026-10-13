using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FA RID: 6394
	public static class DefGenerator
	{
		// Token: 0x06009155 RID: 37205 RVA: 0x002AC1E0 File Offset: 0x002AA3E0
		public static void GenerateImpliedDefs_PreResolve(bool hotReload = false)
		{
			foreach (TerrainDef def in TerrainDefGenerator_Carpet.ImpliedTerrainDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<TerrainDef>(def, hotReload);
			}
			foreach (ThingDef def2 in ThingDefGenerator_Buildings.ImpliedBlueprintAndFrameDefs(hotReload).Concat(ThingDefGenerator_Meat.ImpliedMeatDefs(hotReload)).Concat(ThingDefGenerator_Techprints.ImpliedTechprintDefs(hotReload)).Concat(ThingDefGenerator_Corpses.ImpliedCorpseDefs(hotReload)))
			{
				DefGenerator.AddImpliedDef<ThingDef>(def2, hotReload);
			}
			DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.Silent);
			foreach (TerrainDef def3 in TerrainDefGenerator_Stone.ImpliedTerrainDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<TerrainDef>(def3, hotReload);
			}
			foreach (RecipeDef def4 in RecipeDefGenerator.ImpliedRecipeDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<RecipeDef>(def4, hotReload);
			}
			foreach (PawnColumnDef def5 in PawnColumnDefGenerator.ImpliedPawnColumnDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<PawnColumnDef>(def5, hotReload);
			}
			foreach (ThingDef def6 in ThingDefGenerator_Neurotrainer.ImpliedThingDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<ThingDef>(def6, hotReload);
			}
			foreach (GeneDef def7 in GeneDefGenerator.ImpliedGeneDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<GeneDef>(def7, hotReload);
			}
			foreach (ThoughtDef def8 in GeneDefGenerator.ImpliedThoughtDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<ThoughtDef>(def8, hotReload);
			}
			AnimationDefGenerator_Flying.InitializeNeededDefs(hotReload);
			foreach (GraphicStateDef def9 in AnimationDefGenerator_Flying.ImpliedGraphicStateDefs())
			{
				DefGenerator.AddImpliedDef<GraphicStateDef>(def9, hotReload);
			}
			foreach (AnimationDef def10 in AnimationDefGenerator_Flying.ImpliedAnimationDefs())
			{
				DefGenerator.AddImpliedDef<AnimationDef>(def10, hotReload);
			}
		}

		// Token: 0x06009156 RID: 37206 RVA: 0x002AC474 File Offset: 0x002AA674
		public static void GenerateImpliedDefs_PostResolve(bool hotReload = false)
		{
			foreach (KeyBindingCategoryDef def in KeyBindingDefGenerator.ImpliedKeyBindingCategoryDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<KeyBindingCategoryDef>(def, hotReload);
			}
			foreach (KeyBindingDef def2 in KeyBindingDefGenerator.ImpliedKeyBindingDefs(hotReload))
			{
				DefGenerator.AddImpliedDef<KeyBindingDef>(def2, hotReload);
			}
		}

		// Token: 0x06009157 RID: 37207 RVA: 0x002AC4FC File Offset: 0x002AA6FC
		public static void AddImpliedDef<T>(T def, bool hotReload = false) where T : Def, new()
		{
			def.generated = true;
			def.ResolveDefNameHash();
			ModContentPack modContentPack = def.modContentPack;
			if (modContentPack != null)
			{
				modContentPack.AddDef(def, "ImpliedDefs");
			}
			def.PostLoad();
			if (hotReload && DefDatabase<T>.GetNamed(def.defName, false) != null)
			{
				return;
			}
			DefDatabase<T>.Add(def);
		}

		// Token: 0x04005C7D RID: 23677
		public const int StandardItemPathCost = 14;
	}
}
