using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002893 RID: 10387
	public abstract class ScenPart : IExposable
	{
		// Token: 0x17002293 RID: 8851
		// (get) Token: 0x0600E347 RID: 58183 RVA: 0x00427930 File Offset: 0x00425B30
		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		// Token: 0x17002294 RID: 8852
		// (get) Token: 0x0600E348 RID: 58184 RVA: 0x00427937 File Offset: 0x00425B37
		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17002295 RID: 8853
		// (get) Token: 0x0600E349 RID: 58185 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool OverrideDangerMusic
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E34A RID: 58186 RVA: 0x00427949 File Offset: 0x00425B49
		public virtual bool Valid()
		{
			return this.def != null;
		}

		// Token: 0x0600E34B RID: 58187 RVA: 0x00427954 File Offset: 0x00425B54
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		// Token: 0x0600E34C RID: 58188 RVA: 0x00427966 File Offset: 0x00425B66
		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		// Token: 0x0600E34D RID: 58189 RVA: 0x0042797A File Offset: 0x00425B7A
		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		// Token: 0x0600E34E RID: 58190 RVA: 0x00427987 File Offset: 0x00425B87
		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		// Token: 0x0600E34F RID: 58191 RVA: 0x00427996 File Offset: 0x00425B96
		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		// Token: 0x0600E350 RID: 58192 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x0600E351 RID: 58193 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Randomize()
		{
		}

		// Token: 0x0600E352 RID: 58194 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x0600E353 RID: 58195 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		// Token: 0x0600E354 RID: 58196 RVA: 0x004279A3 File Offset: 0x00425BA3
		public virtual IEnumerable<Page> GetConfigPages()
		{
			return Enumerable.Empty<Page>();
		}

		// Token: 0x0600E355 RID: 58197 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return true;
		}

		// Token: 0x0600E356 RID: 58198 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
		}

		// Token: 0x0600E357 RID: 58199 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
		}

		// Token: 0x0600E358 RID: 58200 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		// Token: 0x0600E359 RID: 58201 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreConfigure()
		{
		}

		// Token: 0x0600E35A RID: 58202 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostWorldGenerate()
		{
		}

		// Token: 0x0600E35B RID: 58203 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostIdeoChosen()
		{
		}

		// Token: 0x0600E35C RID: 58204 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreMapGenerate()
		{
		}

		// Token: 0x0600E35D RID: 58205 RVA: 0x00419816 File Offset: 0x00417A16
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x0600E35E RID: 58206 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GenerateIntoMap(Map map)
		{
		}

		// Token: 0x0600E35F RID: 58207 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x0600E360 RID: 58208 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void MapRemoved(Map map)
		{
		}

		// Token: 0x0600E361 RID: 58209 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostGameStart()
		{
		}

		// Token: 0x0600E362 RID: 58210 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Tick()
		{
		}

		// Token: 0x0600E363 RID: 58211 RVA: 0x003FD4A0 File Offset: 0x003FB6A0
		public virtual IEnumerable<Alert> GetAlerts()
		{
			return Enumerable.Empty<Alert>();
		}

		// Token: 0x0600E364 RID: 58212 RVA: 0x004279AA File Offset: 0x00425BAA
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.def == null)
			{
				Type type = base.GetType();
				yield return ((type != null) ? type.ToString() : null) + " has null def.";
			}
			yield break;
		}

		// Token: 0x0600E365 RID: 58213 RVA: 0x004279BA File Offset: 0x00425BBA
		public virtual bool HasNullDefs()
		{
			return this.def == null;
		}

		// Token: 0x0600E366 RID: 58214 RVA: 0x004279C5 File Offset: 0x00425BC5
		public override int GetHashCode()
		{
			if (this.def == null)
			{
				return 0;
			}
			return this.def.GetHashCode();
		}

		// Token: 0x0400984F RID: 38991
		[TranslationHandle]
		public ScenPartDef def;

		// Token: 0x04009850 RID: 38992
		public bool visible = true;

		// Token: 0x04009851 RID: 38993
		public bool summarized;
	}
}
