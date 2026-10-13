using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E3D RID: 15933
	public abstract class WorldObjectComp
	{
		// Token: 0x17003662 RID: 13922
		// (get) Token: 0x06015C56 RID: 89174 RVA: 0x00681A19 File Offset: 0x0067FC19
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x17003663 RID: 13923
		// (get) Token: 0x06015C57 RID: 89175 RVA: 0x00681A28 File Offset: 0x0067FC28
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		// Token: 0x06015C58 RID: 89176 RVA: 0x00681A4C File Offset: 0x0067FC4C
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06015C59 RID: 89177 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompTick()
		{
		}

		// Token: 0x06015C5A RID: 89178 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CompTickInterval(int delta)
		{
		}

		// Token: 0x06015C5B RID: 89179 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06015C5C RID: 89180 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x06015C5D RID: 89181 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06015C5E RID: 89182 RVA: 0x00681A55 File Offset: 0x0067FC55
		public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			return Enumerable.Empty<IncidentTargetTagDef>();
		}

		// Token: 0x06015C5F RID: 89183 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x06015C60 RID: 89184 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06015C61 RID: 89185 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06015C62 RID: 89186 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostPostRemove()
		{
		}

		// Token: 0x06015C63 RID: 89187 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostDestroy()
		{
		}

		// Token: 0x06015C64 RID: 89188 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostMyMapRemoved()
		{
		}

		// Token: 0x06015C65 RID: 89189 RVA: 0x000026EA File Offset: 0x000008EA
		public void PostMyMapSettled()
		{
		}

		// Token: 0x06015C66 RID: 89190 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostMapGenerate()
		{
		}

		// Token: 0x06015C67 RID: 89191 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		// Token: 0x06015C68 RID: 89192 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostExposeData()
		{
		}

		// Token: 0x06015C69 RID: 89193 RVA: 0x00681A5C File Offset: 0x0067FC5C
		public override string ToString()
		{
			return string.Format("{0}(parent={1} at={2})", base.GetType().Name, this.parent, (this.parent != null) ? this.parent.Tile.tileId : -1);
		}

		// Token: 0x0400F179 RID: 61817
		public WorldObject parent;

		// Token: 0x0400F17A RID: 61818
		public WorldObjectCompProperties props;
	}
}
