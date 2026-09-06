using System;
using System.Reflection;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000040 RID: 64
internal class WallPull
{
	// Token: 0x060001E1 RID: 481 RVA: 0x0002023C File Offset: 0x0001E43C
	public static void Update()
	{
		bool flag3 = !Settings.WallPullModEnabled;
		if (!flag3)
		{
			GTPlayer instance = GTPlayer.Instance;
			bool flag4 = instance == null;
			if (!flag4)
			{
				bool wallPullModToggleMode = Settings.WallPullModToggleMode;
				if (wallPullModToggleMode)
				{
					bool flag = Settings.IsPressed(Settings.WallPullModActivation);
					bool flag5 = flag && !WallPull.toggleHeld;
					if (flag5)
					{
						Settings.WallPullModToggleActive = !Settings.WallPullModToggleActive;
					}
					WallPull.toggleHeld = flag;
					bool flag6 = !Settings.WallPullModToggleActive;
					if (flag6)
					{
						WallPull.appliedOffset = Vector3.zero;
						WallPull.leftHandWasColliding = instance.LeftHand.wasColliding;
						WallPull.rightHandWasColliding = instance.RightHand.wasColliding;
						return;
					}
				}
				bool flag2 = Settings.WallPullModToggleMode || Settings.IsPressed(Settings.WallPullModActivation);
				WallPull.timer += Time.deltaTime;
				float num = 6.5f * Settings.WallPullModVelThreshold;
				bool flag7 = flag2;
				if (flag7)
				{
					bool wasColliding = instance.LeftHand.wasColliding;
					bool wasColliding2 = instance.RightHand.wasColliding;
					int num2 = ((WallPull.leftHandWasColliding && !wasColliding) ? 1 : (WallPull.rightHandWasColliding ? ((!wasColliding2) ? 1 : 0) : 0));
					Vector3 vector3_ = WallPull.GetVelocity(WallPull.GetRigidbody(instance));
					bool flag8 = (wasColliding || wasColliding2) && vector3_.magnitude / instance.scale > num;
					if (flag8)
					{
						WallPull.timer = 0f;
					}
					bool flag9 = num2 != 0 && vector3_.magnitude / instance.scale > num;
					if (flag9)
					{
						WallPull.timer = 0f;
					}
					float wallPullModTpTime = Settings.WallPullModTpTime;
					bool flag10 = WallPull.timer < wallPullModTpTime;
					if (flag10)
					{
						WallPull.ApplyPull(instance, vector3_);
					}
					else
					{
						WallPull.appliedOffset = Vector3.zero;
					}
				}
				else
				{
					WallPull.appliedOffset = Vector3.zero;
				}
				WallPull.leftHandWasColliding = instance.LeftHand.wasColliding;
				WallPull.rightHandWasColliding = instance.RightHand.wasColliding;
			}
		}
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x00020434 File Offset: 0x0001E634
	private static void ApplyPull(GTPlayer player, Vector3 delta)
	{
		Vector3 val = default(Vector3);
		float num3 = Mathf.Abs(delta.y);
		bool flag = num3 >= 0.5f;
		if (!flag)
		{
			num3 = delta.magnitude * 0.5f;
		}
		float num4 = num3 * Settings.WallPullModStrength * (Settings.WallPullModTpTime * 4f);
		val..ctor(0f, Mathf.Abs(num4), 0f);
		float num5 = Settings.WallPullModSmoothing;
		bool flag2 = num5 > 0.01f;
		if (flag2)
		{
			WallPull.appliedOffset += val;
			float num6 = Mathf.Lerp(1f, 0.15f, num5);
			Vector3 val2 = WallPull.appliedOffset * num6;
			Transform transform = player.transform;
			transform.position += val2;
			WallPull.appliedOffset -= val2;
			bool flag3 = WallPull.appliedOffset.magnitude < 0.001f;
			if (flag3)
			{
				WallPull.appliedOffset = Vector3.zero;
			}
		}
		else
		{
			Transform transform2 = player.transform;
			transform2.position += val;
		}
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x00020570 File Offset: 0x0001E770
	private static Rigidbody GetRigidbody(GTPlayer player)
	{
		bool flag = !(WallPull.cachedRigidbody == null);
		if (!flag)
		{
			WallPull.cachedRigidbody = player.GetComponent<Rigidbody>();
		}
		return WallPull.cachedRigidbody;
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x000205AC File Offset: 0x0001E7AC
	private static Vector3 GetVelocity(Rigidbody body)
	{
		bool flag = GTPlayer.Instance != null && GTPlayer.Instance.disableMovement;
		Vector3 vector;
		if (flag)
		{
			vector = GTPlayer.Instance.AveragedVelocity;
		}
		else
		{
			bool flag2 = body == null;
			if (flag2)
			{
				vector = Vector3.zero;
			}
			else
			{
				bool flag3 = !WallPull.velocityReflectionReady;
				if (flag3)
				{
					WallPull.velocityReflectionReady = true;
					Type typeFromHandle = typeof(Rigidbody);
					WallPull.velocityProperty = typeFromHandle.GetProperty("linearVelocity", BindingFlags.Instance | BindingFlags.Public);
					bool flag4 = WallPull.velocityProperty == null;
					if (flag4)
					{
						Type typeFromHandle2 = typeof(Rigidbody);
						WallPull.velocityProperty = typeFromHandle2.GetProperty("velocity", BindingFlags.Instance | BindingFlags.Public);
					}
				}
				bool flag5 = WallPull.velocityProperty != null;
				if (flag5)
				{
					try
					{
						return (Vector3)WallPull.velocityProperty.GetValue(body);
					}
					catch
					{
					}
				}
				vector = Vector3.zero;
			}
		}
		return vector;
	}

	// Token: 0x04000498 RID: 1176
	public static bool leftHandWasColliding;

	// Token: 0x04000499 RID: 1177
	public static bool rightHandWasColliding;

	// Token: 0x0400049A RID: 1178
	public static float timer = 35f;

	// Token: 0x0400049B RID: 1179
	private static PropertyInfo velocityProperty = null;

	// Token: 0x0400049C RID: 1180
	private static bool velocityReflectionReady = false;

	// Token: 0x0400049D RID: 1181
	private static Rigidbody cachedRigidbody = null;

	// Token: 0x0400049E RID: 1182
	private static Vector3 appliedOffset = Vector3.zero;

	// Token: 0x0400049F RID: 1183
	private static bool toggleHeld = false;
}
