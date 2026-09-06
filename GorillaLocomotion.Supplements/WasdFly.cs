using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000042 RID: 66
public static class WasdFly
{
	// Token: 0x060001EB RID: 491 RVA: 0x00020B4C File Offset: 0x0001ED4C
	public static void Apply()
	{
		bool flag = !Settings.WASDFlyEnabled || !Settings.IsPressed(Settings.WASDFlyKeybind);
		if (!flag)
		{
			try
			{
				GorillaTagger tagger = GorillaTagger.Instance;
				bool flag2 = tagger == null;
				if (!flag2)
				{
					bool flag3 = WasdFly.flyBody == null;
					if (flag3)
					{
						WasdFly.flyBody = tagger.rigidbody;
					}
					bool flag4 = WasdFly.flyBody == null;
					if (!flag4)
					{
						Keyboard keyboard = Keyboard.current;
						bool flag5 = keyboard == null;
						if (!flag5)
						{
							Camera main = Camera.main;
							bool flag6 = main == null;
							if (!flag6)
							{
								Vector3 forward = main.transform.forward;
								forward.y = 0f;
								forward.Normalize();
								Vector3 right = main.transform.right;
								right.y = 0f;
								right.Normalize();
								Vector3 move = Vector3.zero;
								bool isPressed = keyboard.wKey.isPressed;
								if (isPressed)
								{
									move += forward;
								}
								bool isPressed2 = keyboard.sKey.isPressed;
								if (isPressed2)
								{
									move -= forward;
								}
								bool isPressed3 = keyboard.dKey.isPressed;
								if (isPressed3)
								{
									move += right;
								}
								bool isPressed4 = keyboard.aKey.isPressed;
								if (isPressed4)
								{
									move -= right;
								}
								bool isPressed5 = keyboard.spaceKey.isPressed;
								if (isPressed5)
								{
									move += Vector3.up;
								}
								bool isPressed6 = keyboard.leftShiftKey.isPressed;
								if (isPressed6)
								{
									move -= Vector3.up;
								}
								WasdFly.flyBody.useGravity = false;
								WasdFly.flyBody.velocity = move * Settings.WASDFlySpeed;
								WasdFly.flying = true;
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00020D40 File Offset: 0x0001EF40
	public static void Stop()
	{
		bool flag = !WasdFly.flying;
		if (!flag)
		{
			bool flag2 = WasdFly.flyBody != null;
			if (flag2)
			{
				WasdFly.flyBody.useGravity = true;
			}
			WasdFly.flying = false;
		}
	}

	// Token: 0x040004A1 RID: 1185
	private static Rigidbody flyBody;

	// Token: 0x040004A2 RID: 1186
	private static bool flying;
}
