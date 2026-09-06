using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000017 RID: 23
public static class KayFlock
{
	// Token: 0x06000087 RID: 135 RVA: 0x0000BEC4 File Offset: 0x0000A0C4
	public static void Apply()
	{
		bool flag = !Settings.KayFlockEnabled || !Settings.IsPressed(Settings.KayFlockKeybind);
		if (!flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = !(instance == null);
				if (flag2)
				{
					KayFlock.timer += Time.deltaTime;
					Transform transform = GorillaTagger.Instance.headCollider.transform;
					Rigidbody rigidbody = GorillaTagger.Instance.rigidbody;
					bool flag3 = rigidbody != null;
					if (flag3)
					{
						rigidbody.linearVelocity = transform.forward * Settings.KayFlockSpeed * instance.scale;
					}
					float num = 1.5f * instance.scale;
					float num2 = KayFlock.timer;
					Vector3 val = default(Vector3);
					val..ctor(Mathf.Sin(num2 * 14f), Mathf.Sin(num2 * 14f + 1.1f), Mathf.Cos(num2 * 14f + 2.3f));
					bool flag4 = val.sqrMagnitude < 0.01f;
					if (flag4)
					{
						val = Vector3.left;
					}
					val = val.normalized;
					Vector3 val2 = default(Vector3);
					val2..ctor(Mathf.Sin(num2 * 14f + 3.1f), Mathf.Sin(num2 * 14f + 4.7f), Mathf.Cos(num2 * 14f * 1.5f + 0.6f));
					bool flag5 = val2.sqrMagnitude < 0.01f;
					if (flag5)
					{
						val2 = Vector3.right;
					}
					val2 = val2.normalized;
					Vector3 position = transform.position;
					instance.LeftHand.controllerTransform.position = position + val * num;
					instance.RightHand.controllerTransform.position = position + val2 * num;
					instance.LeftHand.controllerTransform.rotation = Quaternion.Euler(Mathf.Sin(num2 * 14f * 1.6f) * 180f, Mathf.Sin(num2 * 14f + 1.4f) * 180f, Mathf.Sin(num2 * 14f + 2.8f) * 180f);
					instance.RightHand.controllerTransform.rotation = Quaternion.Euler(Mathf.Sin(num2 * 14f + 5f) * 180f, Mathf.Sin(num2 * 14f * 1.9f) * 180f, Mathf.Sin(num2 * 14f + 3.9f) * 180f);
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x040001B9 RID: 441
	private static float timer;
}
