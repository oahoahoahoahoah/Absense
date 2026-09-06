using System;
using System.Reflection;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200002C RID: 44
internal class PullV3
{
	// Token: 0x06000146 RID: 326 RVA: 0x00017F44 File Offset: 0x00016144
	public static void Update()
	{
		bool flag3 = !Settings.PullV3Enabled;
		if (!flag3)
		{
			bool pullV3ToggleMode = Settings.PullV3ToggleMode;
			if (pullV3ToggleMode)
			{
				bool flag = Settings.IsPressed(Settings.PullV3Activation);
				bool flag4 = flag && !PullV3.toggleHeld;
				if (flag4)
				{
					PullV3.active = !PullV3.active;
				}
				PullV3.toggleHeld = flag;
			}
			else
			{
				PullV3.active = Settings.IsPressed(Settings.PullV3Activation);
			}
			PullV3.tpTimer += Time.deltaTime;
			PullV3.resetTimer += Time.deltaTime;
			GTPlayer instance = GTPlayer.Instance;
			bool flag5 = instance == null;
			if (!flag5)
			{
				Rigidbody component = instance.GetComponent<Rigidbody>();
				bool flag6 = component == null;
				if (!flag6)
				{
					float num = instance.maxJumpSpeed * Settings.PullV3Threshold;
					bool flag2 = true;
					bool pullV3MidPullRequired = Settings.PullV3MidPullRequired;
					if (pullV3MidPullRequired)
					{
						float num2 = Vector3.Distance(instance.LeftHand.controllerTransform.position, instance.headCollider.transform.position);
						float num3 = Vector3.Distance(instance.RightHand.controllerTransform.position, instance.headCollider.transform.position);
						flag2 = num2 > 0.5f || num3 > 0.5f;
					}
					int num4 = ((PullV3.active && Settings.PullV3LeftHand && PullV3.leftWasColliding) ? ((!instance.LeftHand.wasColliding) ? 1 : 0) : 0);
					int num5 = ((PullV3.active && Settings.PullV3RightHand && PullV3.rightWasColliding) ? ((!instance.RightHand.wasColliding) ? 1 : 0) : 0);
					int num6 = (num4 | num5) & (flag2 ? 1 : 0);
					Vector3 val = PullV3.GetVelocity(component);
					bool flag7 = (num6 & (((val.magnitude / instance.scale > num) | (instance.disableMovement && (Vector3.Distance(instance.LeftHand.controllerTransform.position, instance.headCollider.transform.position) > 1f || Vector3.Distance(instance.RightHand.controllerTransform.position, instance.headCollider.transform.position) > 1f))) ? 1 : 0)) != 0;
					if (flag7)
					{
						PullV3.tpTimer = 0f;
						PullV3.resetTimer = 0f;
						int num7 = PullV3.stacks;
						PullV3.stacks = Mathf.Min(num7 + 1, Settings.PullV3MaxStacks);
					}
					else
					{
						bool flag8 = PullV3.resetTimer > Settings.PullV3ResetTime;
						if (flag8)
						{
							PullV3.stacks = 0;
						}
					}
					float num8 = 0f;
					int num9 = PullV3.stacks;
					bool flag9 = num9 > 0;
					if (flag9)
					{
						int num10 = PullV3.stacks;
						num8 = (float)(num10 - 1) * 0.25f;
					}
					float num11 = Settings.PullV3Strength * Mathf.Clamp01(num8);
					bool flag10 = PullV3.tpTimer < Settings.PullV3TpTime;
					if (flag10)
					{
						bool disableMovement = instance.disableMovement;
						if (disableMovement)
						{
							Vector3 val2 = (instance.LeftHand.controllerTransform.position + instance.RightHand.controllerTransform.position) / 2f - instance.headCollider.transform.position;
							val2.y = 0f;
							val2 = -val2.normalized;
							Transform transform = instance.transform;
							transform.position += val2 * Settings.PullV3FreezeStrength * num11 * (Settings.PullV3TpTime * 4f);
						}
						else
						{
							Transform transform2 = instance.transform;
							transform2.position += val * num11 * (Settings.PullV3TpTime * 4f);
						}
					}
					PullV3.leftWasColliding = instance.LeftHand.wasColliding;
					PullV3.rightWasColliding = instance.RightHand.wasColliding;
				}
			}
		}
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00018330 File Offset: 0x00016530
	private static Vector3 GetVelocity(Rigidbody body)
	{
		bool flag = body == null;
		Vector3 vector;
		if (flag)
		{
			vector = Vector3.zero;
		}
		else
		{
			bool flag2 = !PullV3.velocityReflectionReady;
			if (flag2)
			{
				PullV3.velocityReflectionReady = true;
				Type typeFromHandle = typeof(Rigidbody);
				PullV3.velocityProperty = typeFromHandle.GetProperty("linearVelocity", BindingFlags.Instance | BindingFlags.Public);
				bool flag3 = PullV3.velocityProperty == null;
				if (flag3)
				{
					Type typeFromHandle2 = typeof(Rigidbody);
					PullV3.velocityProperty = typeFromHandle2.GetProperty("velocity", BindingFlags.Instance | BindingFlags.Public);
				}
			}
			bool flag4 = PullV3.velocityProperty != null;
			if (flag4)
			{
				try
				{
					return (Vector3)PullV3.velocityProperty.GetValue(body);
				}
				catch
				{
				}
			}
			vector = Vector3.zero;
		}
		return vector;
	}

	// Token: 0x0400026E RID: 622
	private static bool active = false;

	// Token: 0x0400026F RID: 623
	private static bool toggleHeld = false;

	// Token: 0x04000270 RID: 624
	private static float tpTimer = 999f;

	// Token: 0x04000271 RID: 625
	private static float resetTimer = 999f;

	// Token: 0x04000272 RID: 626
	private static int stacks = 0;

	// Token: 0x04000273 RID: 627
	private static bool leftWasColliding;

	// Token: 0x04000274 RID: 628
	private static bool rightWasColliding;

	// Token: 0x04000275 RID: 629
	private static PropertyInfo velocityProperty = null;

	// Token: 0x04000276 RID: 630
	private static bool velocityReflectionReady = false;
}
