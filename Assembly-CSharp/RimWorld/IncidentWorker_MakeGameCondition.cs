using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6F RID: 7535
	public class IncidentWorker_MakeGameCondition : IncidentWorker
	{
		// Token: 0x0600A8E2 RID: 43234 RVA: 0x00305445 File Offset: 0x00303645
		public virtual GameConditionDef GetGameConditionDef(IncidentParms parms)
		{
			return this.def.gameCondition;
		}

		// Token: 0x0600A8E3 RID: 43235 RVA: 0x00305454 File Offset: 0x00303654
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = parms.target as Map;
			if (map != null)
			{
				using (List<GameCondition>.Enumerator enumerator = map.gameConditionManager.ActiveConditions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.preventIncidents)
						{
							return false;
						}
					}
				}
			}
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			if (gameConditionManager == null)
			{
				Log.ErrorOnce(string.Format("Couldn't find condition manager for incident target {0}", parms.target), 70849667);
				return false;
			}
			GameConditionDef gameConditionDef = this.GetGameConditionDef(parms);
			if (gameConditionDef == null)
			{
				return false;
			}
			if (gameConditionManager.ConditionIsActive(gameConditionDef))
			{
				return false;
			}
			List<GameCondition> activeConditions = gameConditionManager.ActiveConditions;
			for (int i = 0; i < activeConditions.Count; i++)
			{
				if (!gameConditionDef.CanCoexistWith(activeConditions[i].def))
				{
					return false;
				}
			}
			if (ModsConfig.OdysseyActive && gameConditionDef.requireFish)
			{
				Map map2 = parms.target as Map;
				if (map2 == null || map2.waterBodyTracker == null || !map2.waterBodyTracker.AnyBodyContainsFish)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A8E4 RID: 43236 RVA: 0x0030557C File Offset: 0x0030377C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			GameConditionDef gameConditionDef = this.GetGameConditionDef(parms);
			int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
			GameCondition gameCondition = GameConditionMaker.MakeCondition(gameConditionDef, duration);
			gameConditionManager.RegisterCondition(gameCondition);
			this.TryFireLetter(parms, gameCondition);
			return true;
		}

		// Token: 0x0600A8E5 RID: 43237 RVA: 0x003055D0 File Offset: 0x003037D0
		private void TryFireLetter(IncidentParms parms, GameCondition condition)
		{
			if (!this.def.letterLabel.NullOrEmpty() && !condition.def.letterText.NullOrEmpty())
			{
				Map map = parms.target as Map;
				if (map == null || !condition.HiddenByOtherCondition(map))
				{
					bool flag = false;
					foreach (Map map2 in Find.Maps)
					{
						if (condition.CanApplyOnMap(map2))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return;
					}
					parms.letterHyperlinkThingDefs = condition.def.letterHyperlinks;
					base.SendStandardLetter(this.def.letterLabel, condition.LetterText, this.def.letterDef, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
					return;
				}
			}
		}
	}
}
