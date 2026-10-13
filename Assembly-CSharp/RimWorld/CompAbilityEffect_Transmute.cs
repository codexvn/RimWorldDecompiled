using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200229D RID: 8861
	public class CompAbilityEffect_Transmute : CompAbilityEffect
	{
		// Token: 0x17001D7D RID: 7549
		// (get) Token: 0x0600C0D1 RID: 49361 RVA: 0x0037D51A File Offset: 0x0037B71A
		public new CompProperties_Transmute Props
		{
			get
			{
				return (CompProperties_Transmute)this.props;
			}
		}

		// Token: 0x0600C0D2 RID: 49362 RVA: 0x0037D528 File Offset: 0x0037B728
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Thing thing = target.Thing;
			if (thing.Stuff != null)
			{
				string value = Find.ActiveLanguageWorker.WithIndefiniteArticle(thing.LabelShort, false, false);
				string value2 = Find.ActiveLanguageWorker.WithIndefiniteArticle(Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1), false, false);
				ThingDef stuff = thing.Stuff;
				ThingDef thingDef;
				using (new RandBlock(thing.thingIDNumber))
				{
					thingDef = this.Props.outcomeStuff.RandomElement<ThingDef>();
				}
				float num = (float)thing.HitPoints / (float)thing.MaxHitPoints;
				thing.SetStuffDirect(thingDef);
				StatDefOf.MaxHitPoints.Worker.ClearCacheForThing(thing);
				thing.Notify_ColorChanged();
				thing.DirtyMapMesh(thing.Map);
				thing.HitPoints = Mathf.CeilToInt((float)thing.MaxHitPoints * num);
				UnfinishedThing unfinishedThing = thing as UnfinishedThing;
				if (unfinishedThing != null)
				{
					int num2 = 0;
					int num3 = 100;
					foreach (Thing thing2 in unfinishedThing.ingredients)
					{
						num2 += thing2.stackCount;
					}
					unfinishedThing.ingredients.Clear();
					while (num2 > 0 && num3-- > 0)
					{
						Thing thing3 = ThingMaker.MakeThing(thingDef, null);
						thing3.stackCount = Mathf.Min(num2, thingDef.stackLimit);
						unfinishedThing.ingredients.Add(thing3);
						num2 -= thing3.stackCount;
					}
					if (num3 <= 0)
					{
						Debug.LogError(string.Concat(new string[]
						{
							"Attempted to transmute an unfinished thing but could not generate enough replacement items (",
							stuff.label,
							" to ",
							thingDef.label,
							"). Check if ",
							thingDef.label,
							" is stackable."
						}));
					}
				}
				string text;
				if (target.Thing.stackCount > 1)
				{
					text = "MessageTransmutedStuffPlural".Translate(this.parent.pawn.Named("PAWN"), value2, thing.Named("TRANSMUTED"));
				}
				else
				{
					text = "MessageTransmutedStuff".Translate(this.parent.pawn.Named("PAWN"), value, thing.Named("TRANSMUTED"));
				}
				Messages.Message(text, this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
				return;
			}
			CompProperties_Transmute.ElementRatio elementRatio;
			this.TryGetElementFor(thing.def, out elementRatio);
			int stackCount = thing.stackCount;
			int num4 = Mathf.Max(Mathf.FloorToInt((float)thing.stackCount * elementRatio.ratio), 1);
			Thing thing4 = ThingMaker.MakeThing(this.Props.outcomeItems.RandomElement<ThingDef>(), null);
			thing4.stackCount = num4;
			IntVec3 positionHeld = thing.PositionHeld;
			thing.Destroy(DestroyMode.Vanish);
			GenPlace.TryPlaceThing(thing4, positionHeld, this.parent.pawn.Map, ThingPlaceMode.Direct, null, null, null, 1);
			string arg = string.Format("{0} {1}", stackCount, target.Thing.LabelNoCount);
			string arg2 = string.Format("{0} {1}", num4, thing4.LabelNoCount);
			Messages.Message("MessageTransmutedItem".Translate(this.parent.pawn.Named("PAWN"), arg.Named("ORIGINAL"), arg2.Named("TRANSMUTED")), this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600C0D3 RID: 49363 RVA: 0x0037D8E0 File Offset: 0x0037BAE0
		public override bool CanApplyOn(GlobalTargetInfo target)
		{
			CompProperties_Transmute.ElementRatio elementRatio;
			return target.HasThing && this.TryGetElementFor(target.Thing.Stuff ?? target.Thing.def, out elementRatio);
		}

		// Token: 0x0600C0D4 RID: 49364 RVA: 0x0037D91C File Offset: 0x0037BB1C
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			CompProperties_Transmute.ElementRatio elementRatio;
			bool flag = target.HasThing && this.TryGetElementFor(target.Thing.Stuff ?? target.Thing.def, out elementRatio);
			if (!flag)
			{
				Messages.Message(this.Props.failedMessage.Formatted(target.Thing.Label), target.Thing, MessageTypeDefOf.NeutralEvent, true);
			}
			return flag;
		}

		// Token: 0x0600C0D5 RID: 49365 RVA: 0x0037D99C File Offset: 0x0037BB9C
		private bool TryGetElementFor(ThingDef stuff, out CompProperties_Transmute.ElementRatio ratio)
		{
			ratio = this.Props.elementRatios.FirstOrDefault((CompProperties_Transmute.ElementRatio x) => x.sourceStuff == stuff);
			return ratio != null;
		}

		// Token: 0x0600C0D6 RID: 49366 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return false;
		}
	}
}
