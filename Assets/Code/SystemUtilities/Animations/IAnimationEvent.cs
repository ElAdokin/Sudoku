using System;

public interface IAnimationEvent
{
	event Action<int> OnCallAnimationEvent;
}
