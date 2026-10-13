using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B0B RID: 6923
	public abstract class ThingSetMaker
	{
		// Token: 0x06009A88 RID: 39560 RVA: 0x002CEAF8 File Offset: 0x002CCCF8
		public List<Thing> Generate()
		{
			return this.Generate(default(ThingSetMakerParams));
		}

		// Token: 0x06009A89 RID: 39561 RVA: 0x002CEB14 File Offset: 0x002CCD14
		public List<Thing> Generate(ThingSetMakerParams parms)
		{
			List<Thing> list = new List<Thing>();
			ThingSetMaker.thingsBeingGeneratedNow.Add(list);
			try
			{
				ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
				this.Generate(parms2, list);
				this.PostProcess(list);
			}
			catch (Exception ex)
			{
				string str = "Exception while generating thing set: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				for (int i = list.Count - 1; i >= 0; i--)
				{
					list[i].Destroy(DestroyMode.Vanish);
					list.RemoveAt(i);
				}
			}
			finally
			{
				ThingSetMaker.thingsBeingGeneratedNow.Remove(list);
			}
			return list;
		}

		// Token: 0x06009A8A RID: 39562 RVA: 0x002CEBBC File Offset: 0x002CCDBC
		public bool CanGenerate(ThingSetMakerParams parms)
		{
			ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
			return this.CanGenerateSub(parms2);
		}

		// Token: 0x06009A8B RID: 39563 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return true;
		}

		// Token: 0x06009A8C RID: 39564
		protected abstract void Generate(ThingSetMakerParams parms, List<Thing> outThings);

		// Token: 0x06009A8D RID: 39565 RVA: 0x002CEBD8 File Offset: 0x002CCDD8
		public IEnumerable<ThingDef> AllGeneratableThingsDebug()
		{
			return this.AllGeneratableThingsDebug(default(ThingSetMakerParams));
		}

		// Token: 0x06009A8E RID: 39566 RVA: 0x002CEBF4 File Offset: 0x002CCDF4
		public IEnumerable<ThingDef> AllGeneratableThingsDebug(ThingSetMakerParams parms)
		{
			if (!this.CanGenerate(parms))
			{
				yield break;
			}
			ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
			foreach (ThingDef thingDef in this.AllGeneratableThingsDebugSub(parms2).Distinct<ThingDef>())
			{
				yield return thingDef;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06009A8F RID: 39567 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float ExtraSelectionWeightFactor(ThingSetMakerParams parms)
		{
			return 1f;
		}

		// Token: 0x06009A90 RID: 39568
		protected abstract IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms);

		// Token: 0x06009A91 RID: 39569 RVA: 0x002CEC0C File Offset: 0x002CCE0C
		private void PostProcess(List<Thing> things)
		{
			if (things.RemoveAll((Thing x) => x == null) != 0)
			{
				Type type = base.GetType();
				Log.Error(((type != null) ? type.ToString() : null) + " generated null things.");
			}
			this.ChangeDeadPawnsToTheirCorpses(things);
			for (int i = things.Count - 1; i >= 0; i--)
			{
				if (things[i].Destroyed)
				{
					Type type2 = base.GetType();
					Log.Error(((type2 != null) ? type2.ToString() : null) + " generated destroyed thing " + things[i].ToStringSafe<Thing>());
					things.RemoveAt(i);
				}
				else if (things[i].stackCount <= 0)
				{
					string[] array = new string[5];
					int num = 0;
					Type type3 = base.GetType();
					array[num] = ((type3 != null) ? type3.ToString() : null);
					array[1] = " generated ";
					array[2] = things[i].ToStringSafe<Thing>();
					array[3] = " with stackCount=";
					array[4] = things[i].stackCount.ToString();
					Log.Error(string.Concat(array));
					things.RemoveAt(i);
				}
			}
			this.Minify(things);
		}

		// Token: 0x06009A92 RID: 39570 RVA: 0x002CED3C File Offset: 0x002CCF3C
		private void Minify(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].def.Minifiable)
				{
					int stackCount = things[i].stackCount;
					things[i].stackCount = 1;
					MinifiedThing minifiedThing = things[i].MakeMinified(DestroyMode.Vanish);
					minifiedThing.stackCount = stackCount;
					things[i] = minifiedThing;
				}
			}
		}

		// Token: 0x06009A93 RID: 39571 RVA: 0x002CEDA4 File Offset: 0x002CCFA4
		private void ChangeDeadPawnsToTheirCorpses(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].ParentHolder is Corpse)
				{
					things[i] = (Corpse)things[i].ParentHolder;
				}
			}
		}

		// Token: 0x06009A94 RID: 39572 RVA: 0x002CEDF0 File Offset: 0x002CCFF0
		private ThingSetMakerParams ApplyFixedParams(ThingSetMakerParams parms)
		{
			ThingSetMakerParams result = this.fixedParams;
			Gen.ReplaceNullFields<ThingSetMakerParams>(ref result, parms);
			return result;
		}

		// Token: 0x06009A95 RID: 39573 RVA: 0x002CEE0D File Offset: 0x002CD00D
		public virtual void ResolveReferences()
		{
			if (this.fixedParams.filter != null)
			{
				this.fixedParams.filter.ResolveReferences();
			}
		}

		// Token: 0x06009A96 RID: 39574 RVA: 0x002CEE2C File Offset: 0x002CD02C
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x04006A9A RID: 27290
		public ThingSetMakerParams fixedParams;

		// Token: 0x04006A9B RID: 27291
		public static List<List<Thing>> thingsBeingGeneratedNow = new List<List<Thing>>();
	}
}
