using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008E6 RID: 2278
	public class ThingWithComps : Thing
	{
		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06003A69 RID: 14953 RVA: 0x0013628E File Offset: 0x0013448E
		public List<ThingComp> AllComps
		{
			get
			{
				if (this.comps == null)
				{
					return ThingWithComps.EmptyCompsList;
				}
				return this.comps;
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06003A6A RID: 14954 RVA: 0x001362A4 File Offset: 0x001344A4
		// (set) Token: 0x06003A6B RID: 14955 RVA: 0x0013632C File Offset: 0x0013452C
		public override Color DrawColor
		{
			get
			{
				CompColorable comp = this.GetComp<CompColorable>();
				if (comp != null && comp.Active)
				{
					return comp.Color;
				}
				foreach (ThingComp thingComp in this.AllComps)
				{
					Color? color = thingComp.ForceColor();
					if (color != null)
					{
						return color.Value;
					}
				}
				return base.DrawColor;
			}
			set
			{
				this.SetColor(value, true);
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06003A6C RID: 14956 RVA: 0x00136338 File Offset: 0x00134538
		public override string LabelNoCount
		{
			get
			{
				string text = base.LabelNoCount;
				if (this.comps != null)
				{
					int i = 0;
					int count = this.comps.Count;
					while (i < count)
					{
						text = this.comps[i].TransformLabel(text);
						i++;
					}
				}
				return text;
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06003A6D RID: 14957 RVA: 0x00136380 File Offset: 0x00134580
		public override string DescriptionFlavor
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.DescriptionFlavor);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string descriptionPart = this.comps[i].GetDescriptionPart();
						if (!descriptionPart.NullOrEmpty())
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.AppendLine();
								stringBuilder.AppendLine();
							}
							stringBuilder.Append(descriptionPart);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x00136400 File Offset: 0x00134600
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostMake();
				}
			}
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x00136448 File Offset: 0x00134648
		public T GetComp<T>() where T : ThingComp
		{
			if (this.comps == null)
			{
				return default(T);
			}
			int count = this.comps.Count;
			if (count >= 3)
			{
				if (this.compsByType != null)
				{
					ThingComp[] array;
					if (this.compsByType.TryGetValue(typeof(T), out array))
					{
						return (T)((object)array[0]);
					}
					if (typeof(T).IsSealedWithCache())
					{
						return default(T);
					}
				}
				for (int i = 0; i < count; i++)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
				return default(T);
			}
			T t2 = this.comps[0] as T;
			if (t2 != null)
			{
				return t2;
			}
			if (count == 2)
			{
				T t3 = this.comps[1] as T;
				if (t3 != null)
				{
					return t3;
				}
			}
			return default(T);
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x0013654B File Offset: 0x0013474B
		public IEnumerable<T> GetComps<T>() where T : class
		{
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						yield return t;
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x0013655C File Offset: 0x0013475C
		public ThingComp GetCompByDefType(CompProperties def)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].props.compClass == def.compClass)
					{
						return this.comps[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x001365B8 File Offset: 0x001347B8
		public void InitializeComps()
		{
			if (this.def.comps.Any<CompProperties>())
			{
				this.comps = new List<ThingComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					ThingComp thingComp = null;
					try
					{
						thingComp = (ThingComp)Activator.CreateInstance(this.def.comps[i].compClass);
						thingComp.parent = this;
						this.comps.Add(thingComp);
						thingComp.Initialize(this.def.comps[i]);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate or initialize a ThingComp: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
						this.comps.Remove(thingComp);
					}
				}
				this.compsByType = (from c in this.comps
				group c by c.GetType()).ToDictionary((IGrouping<Type, ThingComp> g) => g.Key, (IGrouping<Type, ThingComp> g) => g.ToArray<ThingComp>());
				this.compStyleable = this.GetComp<CompStyleable>();
				this.compForbiddable = this.GetComp<CompForbiddable>();
				this.compQuality = this.GetComp<CompQuality>();
			}
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x00136728 File Offset: 0x00134928
		public override string GetCustomLabelNoCount(bool includeHp = true)
		{
			string text = base.GetCustomLabelNoCount(includeHp);
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					text = this.comps[i].TransformLabel(text);
					i++;
				}
			}
			return text;
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x00136774 File Offset: 0x00134974
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostExposeData();
				}
			}
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x001367C4 File Offset: 0x001349C4
		public void BroadcastCompSignal(string signal)
		{
			this.ReceiveCompSignal(signal);
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].ReceiveCompSignal(signal);
					i++;
				}
			}
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x0013680C File Offset: 0x00134A0C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSpawnSetup(respawningAfterLoad);
				}
			}
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x00136854 File Offset: 0x00134A54
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDeSpawn(map, mode);
				}
			}
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x001368A0 File Offset: 0x00134AA0
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map mapHeld = base.MapHeld;
			base.Destroy(mode);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDestroy(mode, mapHeld);
				}
			}
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x001368EC File Offset: 0x00134AEC
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			Pawn pawn = this as Pawn;
			Map prevMap;
			if (pawn != null)
			{
				prevMap = (pawn.prevMap ?? base.MapHeld);
			}
			else
			{
				prevMap = base.MapHeld;
			}
			base.Kill(dinfo, exactCulprit);
			if (this.comps == null)
			{
				return;
			}
			foreach (ThingComp thingComp in this.comps)
			{
				thingComp.Notify_Killed(prevMap, dinfo);
			}
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00136974 File Offset: 0x00134B74
		protected override void Tick()
		{
			if (this.comps == null)
			{
				return;
			}
			int i = 0;
			int count = this.comps.Count;
			while (i < count)
			{
				this.comps[i].CompTick();
				i++;
			}
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x001369B4 File Offset: 0x00134BB4
		protected override void TickInterval(int delta)
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTickInterval(delta);
					i++;
				}
			}
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x001369F4 File Offset: 0x00134BF4
		public override void TickRare()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTickRare();
					i++;
				}
			}
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x00136A34 File Offset: 0x00134C34
		public override void TickLong()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTickLong();
					i++;
				}
			}
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x00136A74 File Offset: 0x00134C74
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPreApplyDamage(ref dinfo, out absorbed);
					if (absorbed)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x00136AC4 File Offset: 0x00134CC4
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostApplyDamage(dinfo, totalDamageDealt);
				}
			}
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x00136B0A File Offset: 0x00134D0A
		public virtual IEnumerable<ThingDefCountClass> GetAdditionalLeavings(DestroyMode mode)
		{
			int num;
			for (int i = 0; i < this.AllComps.Count; i = num + 1)
			{
				foreach (ThingDefCountClass thingDefCountClass in this.AllComps[i].GetAdditionalLeavings(base.Map, mode))
				{
					yield return thingDefCountClass;
				}
				IEnumerator<ThingDefCountClass> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x00136B21 File Offset: 0x00134D21
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (!this.CompsPreventDrawing())
			{
				base.DrawAt(drawLoc, flip);
			}
			this.Comps_DrawAt(drawLoc, flip);
			this.Comps_PostDraw();
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x00136B44 File Offset: 0x00134D44
		protected void Comps_DrawAt(Vector3 drawLoc, bool flip)
		{
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].DrawAt(drawLoc, flip);
			}
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x00136B84 File Offset: 0x00134D84
		protected void Comps_PostDraw()
		{
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDraw();
			}
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x00136BC4 File Offset: 0x00134DC4
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDrawExtraSelectionOverlays();
			}
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x00136C08 File Offset: 0x00134E08
		public override void Print(SectionLayer layer)
		{
			if (!this.CompsPreventDrawing())
			{
				base.Print(layer);
			}
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostPrintOnto(layer);
			}
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x00136C58 File Offset: 0x00134E58
		private bool CompsPreventDrawing()
		{
			if (this.comps == null)
			{
				return false;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].DontDrawParent())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x00136C9C File Offset: 0x00134E9C
		public virtual void PrintForPowerGrid(SectionLayer layer)
		{
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompPrintForPowerGrid(layer);
			}
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x00136CDA File Offset: 0x00134EDA
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (Gizmo gizmo2 in this.comps[i].CompGetGizmosExtra())
					{
						yield return gizmo2;
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x00136CEC File Offset: 0x00134EEC
		public override bool TryAbsorbStack(Thing other, bool respectStackLimit)
		{
			if (!this.CanStackWith(other))
			{
				return false;
			}
			int count = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PreAbsorbStack(other, count);
				}
			}
			return base.TryAbsorbStack(other, respectStackLimit);
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x00136D48 File Offset: 0x00134F48
		public override Thing SplitOff(int count)
		{
			Thing thing = base.SplitOff(count);
			if (thing != null && this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSplitOff(thing);
				}
			}
			return thing;
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x00136D94 File Offset: 0x00134F94
		public override bool CanStackWith(Thing other)
		{
			if (!base.CanStackWith(other))
			{
				return false;
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (!this.comps[i].AllowStackWith(other))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x00136DE4 File Offset: 0x00134FE4
		public override TipSignal GetTooltip()
		{
			string text = base.LabelNoParenthesisCap.AsTipTitle() + GenLabel.LabelExtras(this, true, true);
			text = text + "\n\n" + this.DescriptionDetailed;
			if (this.def.useHitPoints)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					this.HitPoints.ToString(),
					" / ",
					base.MaxHitPoints.ToString()
				});
			}
			text += "\n";
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					string text2 = this.comps[i].CompTipStringExtra();
					if (!text2.NullOrEmpty())
					{
						text = text + "\n" + text2;
					}
				}
			}
			return new TipSignal(text, this.thingIDNumber * 251235);
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x00136ED0 File Offset: 0x001350D0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			string text = this.InspectStringPartsFromComps();
			if (!text.NullOrEmpty())
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x00136F20 File Offset: 0x00135120
		protected string InspectStringPartsFromComps()
		{
			if (this.comps == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (Prefs.DevMode && char.IsWhiteSpace(text[text.Length - 1]))
					{
						Type type = this.comps[i].GetType();
						Log.ErrorOnce(((type != null) ? type.ToString() : null) + " CompInspectStringExtra ended with whitespace: " + text, 25612);
						text = text.TrimEndNewlines();
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x00136FE8 File Offset: 0x001351E8
		public override void DrawGUIOverlay()
		{
			base.DrawGUIOverlay();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].DrawGUIOverlay();
				}
			}
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x0013702A File Offset: 0x0013522A
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			ThingWithComps.tmpFloatMenuOptions.Clear();
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					try
					{
						foreach (FloatMenuOption item in this.comps[i].CompFloatMenuOptions(selPawn))
						{
							ThingWithComps.tmpFloatMenuOptions.Add(item);
						}
					}
					catch (Exception ex)
					{
						string[] array = new string[8];
						array[0] = "Exception in CompFloatMenuOptions for ";
						int num = 1;
						Type type = this.comps[i].GetType();
						array[num] = ((type != null) ? type.ToString() : null);
						array[2] = " of ";
						array[3] = ((this != null) ? this.ToString() : null);
						array[4] = " at ";
						array[5] = ((selPawn != null) ? selPawn.ToString() : null);
						array[6] = ": ";
						int num2 = 7;
						Exception ex2 = ex;
						array[num2] = ((ex2 != null) ? ex2.ToString() : null);
						Log.Error(string.Concat(array));
					}
				}
			}
			foreach (FloatMenuOption floatMenuOption2 in ThingWithComps.tmpFloatMenuOptions)
			{
				yield return floatMenuOption2;
			}
			List<FloatMenuOption>.Enumerator enumerator3 = default(List<FloatMenuOption>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x00137041 File Offset: 0x00135241
		public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetMultiSelectFloatMenuOptions(selPawns))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (FloatMenuOption floatMenuOption2 in this.comps[i].CompMultiSelectFloatMenuOptions(selPawns))
					{
						yield return floatMenuOption2;
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x00137058 File Offset: 0x00135258
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PrePreTraded(action, playerNegotiator, trader);
				}
			}
			base.PreTraded(action, playerNegotiator, trader);
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x001370A0 File Offset: 0x001352A0
		public override void PostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			base.PostGeneratedForTrader(trader, forTile, forFaction);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostGeneratedForTrader(trader, forTile, forFaction);
				}
			}
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x001370E8 File Offset: 0x001352E8
		protected override void PrePostIngested(Pawn ingester)
		{
			base.PrePostIngested(ingester);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PrePostIngested(ingester);
				}
			}
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x0013712C File Offset: 0x0013532C
		protected override void PostIngested(Pawn ingester)
		{
			base.PostIngested(ingester);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostIngested(ingester);
				}
			}
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x00137170 File Offset: 0x00135370
		public override void Notify_DefsHotReloaded()
		{
			base.Notify_DefsHotReloaded();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_DefsHotReloaded();
				}
			}
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x001371B4 File Offset: 0x001353B4
		public override void PostMapInit()
		{
			base.PostMapInit();
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostMapInit();
			}
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x001371F8 File Offset: 0x001353F8
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_SignalReceived(signal);
				}
			}
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x0013723C File Offset: 0x0013543C
		public override void Notify_RecipeProduced(Pawn pawn)
		{
			base.Notify_RecipeProduced(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_RecipeProduced(pawn);
				}
			}
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x00137280 File Offset: 0x00135480
		public override void Notify_LordDestroyed()
		{
			base.Notify_LordDestroyed();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_LordDestroyed();
				}
			}
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x001372C4 File Offset: 0x001354C4
		public override void Notify_Equipped(Pawn pawn)
		{
			base.Notify_Equipped(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_Equipped(pawn);
				}
			}
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x00137308 File Offset: 0x00135508
		public override void Notify_Unequipped(Pawn pawn)
		{
			base.Notify_Unequipped(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_Unequipped(pawn);
				}
			}
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x0013734C File Offset: 0x0013554C
		public override void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
			base.Notify_UsedVerb(pawn, verb);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_UsedVerb(pawn, verb);
				}
			}
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x00137394 File Offset: 0x00135594
		public override void Notify_UsedWeapon(Pawn pawn)
		{
			base.Notify_UsedWeapon(pawn);
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				IdeoWeaponDisposition dispositionForWeapon = pawn.Ideo.GetDispositionForWeapon(this.def);
				if (dispositionForWeapon == IdeoWeaponDisposition.Despised)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedDespisedWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
				else if (dispositionForWeapon == IdeoWeaponDisposition.Noble)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedNobleWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_UsedWeapon(pawn);
				}
			}
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x00137444 File Offset: 0x00135644
		public void Notify_KilledPawn(Pawn pawn)
		{
			if (ModsConfig.IdeologyActive && pawn.Ideo != null && this.def.IsWeapon)
			{
				IdeoWeaponDisposition dispositionForWeapon = pawn.Ideo.GetDispositionForWeapon(this.def);
				if (dispositionForWeapon == IdeoWeaponDisposition.Despised)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KillWithDespisedWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
				else if (dispositionForWeapon == IdeoWeaponDisposition.Noble)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KillWithNobleWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_KilledPawn(pawn);
				}
			}
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x001374FC File Offset: 0x001356FC
		public override void Notify_AbandonedAtTile(PlanetTile tile)
		{
			base.Notify_AbandonedAtTile(tile);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_AbandonedAtTile(tile);
				}
			}
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x00137540 File Offset: 0x00135740
		public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
			base.Notify_KilledLeavingsLeft(leavings);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_KilledLeavingsLeft(leavings);
				}
			}
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x00137584 File Offset: 0x00135784
		public override void Notify_Studied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
		{
			List<ThingComp> allComps = this.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				IThingStudied thingStudied = allComps[i] as IThingStudied;
				if (thingStudied != null)
				{
					thingStudied.OnStudied(studier, amount, category);
				}
			}
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x001375C4 File Offset: 0x001357C4
		public override void Notify_ColorChanged()
		{
			base.Notify_ColorChanged();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_ColorChanged();
				}
			}
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x00137606 File Offset: 0x00135806
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					IEnumerable<StatDrawEntry> enumerable = this.comps[i].SpecialDisplayStats();
					if (enumerable != null)
					{
						foreach (StatDrawEntry statDrawEntry2 in enumerable)
						{
							yield return statDrawEntry2;
						}
						enumerator = null;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x00137618 File Offset: 0x00135818
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			CompWakeUpDormant comp = this.GetComp<CompWakeUpDormant>();
			if (comp != null && (explosion.Position - base.Position).LengthHorizontal <= explosion.radius)
			{
				comp.Activate(explosion.instigator, true, false, false);
			}
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x00137668 File Offset: 0x00135868
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			if (this.def.notifyMapRemoved && this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_MapRemoved();
				}
			}
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x001376B8 File Offset: 0x001358B8
		public override void PreSwapMap()
		{
			base.PreSwapMap();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PreSwapMap();
				}
			}
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x001376FC File Offset: 0x001358FC
		public override void PostSwapMap()
		{
			base.PostSwapMap();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSwapMap();
				}
			}
		}

		// Token: 0x04002A8E RID: 10894
		private List<ThingComp> comps;

		// Token: 0x04002A8F RID: 10895
		private Dictionary<Type, ThingComp[]> compsByType;

		// Token: 0x04002A90 RID: 10896
		[Unsaved(false)]
		public CompStyleable compStyleable;

		// Token: 0x04002A91 RID: 10897
		[Unsaved(false)]
		public CompForbiddable compForbiddable;

		// Token: 0x04002A92 RID: 10898
		[Unsaved(false)]
		public CompQuality compQuality;

		// Token: 0x04002A93 RID: 10899
		private static readonly List<ThingComp> EmptyCompsList = new List<ThingComp>();

		// Token: 0x04002A94 RID: 10900
		private static List<FloatMenuOption> tmpFloatMenuOptions = new List<FloatMenuOption>();
	}
}
