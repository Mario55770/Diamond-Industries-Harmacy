using System;
using Verse;
using RimWorld;

namespace DI_Harmacy
{
	// Token: 0x0200029A RID: 666
	public class ScarChance : HediffCompProperties
	{
		// Token: 0x060012A5 RID: 4773 RVA: 0x0006C72C File Offset: 0x0006A92C
		public ScarChance()
		{
			this.compClass = typeof(ScarChance);
		}

		// Token: 0x04000E23 RID: 3619
		public float becomePermanentChanceFactor = 1f;

		// Token: 0x04000E24 RID: 3620
		[MustTranslate]
		public string permanentLabel;
		//Should be in line with literally all other values then.
		public float chanceToBecomePermant=0.01f;
		// Token: 0x04000E25 RID: 3621
		[MustTranslate]
		public string instantlyPermanentLabel;
	}
}
