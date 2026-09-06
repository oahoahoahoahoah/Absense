using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
public static class TickRate
{
	// Token: 0x060001C5 RID: 453 RVA: 0x0001F73D File Offset: 0x0001D93D
	public static void Apply()
	{
		Time.timeScale = Mathf.Clamp(Settings.TickRate, 0.1f, 10f);
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0001F75A File Offset: 0x0001D95A
	public static void Reset()
	{
		Time.timeScale = 1f;
	}
}
