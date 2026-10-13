using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000BAE RID: 2990
	public sealed class LetterStack : IExposable
	{
		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06004C68 RID: 19560 RVA: 0x00184387 File Offset: 0x00182587
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06004C69 RID: 19561 RVA: 0x0018438F File Offset: 0x0018258F
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06004C6A RID: 19562 RVA: 0x00184397 File Offset: 0x00182597
		public BundleLetter BundleLetter
		{
			get
			{
				if (this.bundleLetterCache == null)
				{
					this.bundleLetterCache = (BundleLetter)LetterMaker.MakeLetter(LetterDefOf.BundleLetter);
				}
				return this.bundleLetterCache;
			}
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x001843BC File Offset: 0x001825BC
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null, string debugInfo = null, int delayTicks = 0, bool playSound = true)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction, quest, hyperlinkThingDefs);
			this.ReceiveLetter(let, debugInfo, delayTicks, playSound);
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x001843E8 File Offset: 0x001825E8
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, string debugInfo = null, int delayTicks = 0, bool playSound = true)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, null, null);
			this.ReceiveLetter(let, debugInfo, delayTicks, playSound);
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x00184410 File Offset: 0x00182610
		public void ReceiveLetter(Letter let, string debugInfo = null, int delayTicks = 0, bool playSound = true)
		{
			let.debugInfo = debugInfo;
			if (delayTicks > 0)
			{
				let.arrivalTick = Find.TickManager.TicksGame + delayTicks;
				this.letterQueue.Add(let);
				return;
			}
			if (!let.CanShowInLetterStack)
			{
				return;
			}
			if (playSound)
			{
				let.def.arriveSound.PlayOneShotOnCamera(null);
			}
			if (Prefs.AutomaticPauseMode >= let.def.pauseMode)
			{
				Find.TickManager.Pause();
			}
			else if (let.def.forcedSlowdown)
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			let.arrivalTime = Time.time;
			let.arrivalTick = Find.TickManager.TicksGame;
			this.letters.Add(let);
			Find.Archive.Add(let);
			let.Received();
		}

		// Token: 0x06004C6E RID: 19566 RVA: 0x001844D8 File Offset: 0x001826D8
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x001844F0 File Offset: 0x001826F0
		public void LettersOnGUI(float baseY)
		{
			float num = baseY;
			float num2 = baseY - Find.Alerts.AlertsHeight;
			float num3 = 42f;
			int num4 = Mathf.FloorToInt(num2 / num3);
			int num5 = Math.Max(this.letters.Count - num4, 0);
			if (num5 > 0)
			{
				num5++;
			}
			for (int i = this.letters.Count - 1; i >= num5; i--)
			{
				num -= 30f;
				this.letters[i].DrawButtonAt(num);
				num -= 12f;
			}
			if (num5 > 0)
			{
				this.tmpBundledLetters.Clear();
				this.tmpBundledLetters.AddRange(this.letters.Take(num5));
				num -= 30f;
				this.BundleLetter.SetLetters(this.tmpBundledLetters);
				this.BundleLetter.DrawButtonAt(num);
				num -= 12f;
				this.tmpBundledLetters.Clear();
			}
			this.lastTopYInt = num;
			if (Event.current.type == EventType.Repaint)
			{
				num = baseY;
				for (int j = this.letters.Count - 1; j >= num5; j--)
				{
					num -= 30f;
					this.letters[j].CheckForMouseOverTextAt(num);
					num -= 12f;
				}
				if (num5 > 0)
				{
					num -= 30f;
					this.BundleLetter.CheckForMouseOverTextAt(num);
					num -= 12f;
				}
			}
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x00184648 File Offset: 0x00182848
		public void OpenAutomaticLetters()
		{
			if (Find.WindowStack.WindowsForcePause)
			{
				return;
			}
			foreach (Letter letter in this.letters)
			{
				if (letter.ShouldAutomaticallyOpenLetter)
				{
					letter.OpenLetter();
					break;
				}
			}
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x001846B4 File Offset: 0x001828B4
		public void LetterStackTick()
		{
			this.OpenAutomaticLetters();
			for (int i = this.letterQueue.Count - 1; i >= 0; i--)
			{
				if (Find.TickManager.TicksGame >= this.letterQueue[i].arrivalTick)
				{
					this.ReceiveLetter(this.letterQueue[i], null, 0, true);
					this.letterQueue.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004C72 RID: 19570 RVA: 0x00184720 File Offset: 0x00182920
		public void LetterStackUpdate()
		{
			if (this.mouseoverLetterIndex >= 0 && this.mouseoverLetterIndex < this.letters.Count)
			{
				this.letters[this.mouseoverLetterIndex].lookTargets.TryHighlight(true, true, false);
			}
			this.mouseoverLetterIndex = -1;
			for (int i = this.letters.Count - 1; i >= 0; i--)
			{
				if (!this.letters[i].CanShowInLetterStack)
				{
					this.RemoveLetter(this.letters[i]);
				}
			}
		}

		// Token: 0x06004C73 RID: 19571 RVA: 0x001847AB File Offset: 0x001829AB
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		// Token: 0x06004C74 RID: 19572 RVA: 0x001847C0 File Offset: 0x001829C0
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.letters.Count; i++)
			{
				if (this.letters[i].relatedFaction == faction)
				{
					this.letters[i].relatedFaction = null;
				}
			}
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x0018480C File Offset: 0x00182A0C
		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Letter>(ref this.letterQueue, "letterQueue", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.letterQueue == null)
				{
					this.letterQueue = new List<Letter>();
				}
				this.letters.RemoveAll((Letter x) => x == null);
				this.letterQueue.RemoveAll((Letter x) => x == null);
			}
		}

		// Token: 0x04003676 RID: 13942
		private List<Letter> letters = new List<Letter>();

		// Token: 0x04003677 RID: 13943
		private List<Letter> letterQueue = new List<Letter>();

		// Token: 0x04003678 RID: 13944
		private int mouseoverLetterIndex = -1;

		// Token: 0x04003679 RID: 13945
		private float lastTopYInt;

		// Token: 0x0400367A RID: 13946
		private BundleLetter bundleLetterCache;

		// Token: 0x0400367B RID: 13947
		private const float LettersBottomY = 350f;

		// Token: 0x0400367C RID: 13948
		public const float LetterSpacing = 12f;

		// Token: 0x0400367D RID: 13949
		private List<Letter> tmpBundledLetters = new List<Letter>();
	}
}
