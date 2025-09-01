using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour, IAnimationEvent
{
	public event Action<int> OnCallAnimationEvent;

	public void CallAnimationEvent(int index)
	{
		OnCallAnimationEvent?.Invoke(index);
	}
}
