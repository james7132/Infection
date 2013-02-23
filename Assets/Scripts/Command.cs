using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public abstract class Command
	{
		protected bool finished;
		
		public Command()
		{
			finished = false;
		}
		
		public void executeCommand(GameObject gameObject)
		{
			if(!finished)
			{
				executeCommandImpl(gameObject);
			}
		}
		
		protected abstract void executeCommandImpl(GameObject gameObject);
		
		public void cancel()
		{
			finished = true;
		}
	}
	
	public class MovementCommand : Command
	{
		private Vector3 targetLocation;
		private float movementSpeed;
		
		public MovementCommand(Vector3 targetLocation, float movemenSpeed)
		{
			this.targetLocation = targetLocation;
			this.movementSpeed = movemenSpeed;
		}
		
		protected override void executeCommandImpl(GameObject gameObject)
		{
			Transform transform = gameObject.transform;
			Vector3 movementVector = -movementSpeed *(transform.localPosition - targetLocation).normalized;
			if((transform.localPosition - targetLocation).magnitude < movementVector.magnitude)
			{
				transform.localPosition = targetLocation;
				finished = true;
			}
			else
			{
				transform.localPosition += movementVector;
			}
		}
	}
}

