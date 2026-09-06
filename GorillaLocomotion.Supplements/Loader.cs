using System;
using UnityEngine;

namespace GorillaLocomotion.Supplements
{
	// Token: 0x02000044 RID: 68
	public class Loader
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x000211F0 File Offset: 0x0001F3F0
		public static void Init()
		{
			try
			{
				GameObject val = ((Camera.main != null) ? Camera.main.gameObject : null);
				bool flag = val == null;
				if (flag)
				{
					val = GameObject.Find("Player");
				}
				bool flag2 = val == null;
				if (flag2)
				{
					val = GameObject.Find("GorillaPlayer");
				}
				bool flag3 = val == null;
				if (flag3)
				{
					val = GameObject.Find("Main Camera");
				}
				bool flag4 = val == null;
				if (flag4)
				{
					val = new GameObject("AbsenseMod");
					Object.DontDestroyOnLoad(val);
				}
				bool flag5 = val.GetComponent<Plugin>() == null;
				if (flag5)
				{
					val.AddComponent<Plugin>();
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
