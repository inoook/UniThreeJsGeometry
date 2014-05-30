using UnityEngine;
using System.Collections;

namespace THREE
{
	public class EllipseCurve : Curve
	{

		float aX;
		float aY;
		float xRadius;
		float yRadius;
		float aStartAngle;
		float aEndAngle;
		bool aClockwise;

		public EllipseCurve (float aX, float aY, float xRadius, float yRadius, float aStartAngle, float aEndAngle, bool aClockwise)
		{
		
			this.aX = aX;
			this.aY = aY;
		
			this.xRadius = xRadius;
			this.yRadius = yRadius;
		
			this.aStartAngle = aStartAngle;
			this.aEndAngle = aEndAngle;
		
			this.aClockwise = aClockwise;
		}

		public override Vector3 getPoint (float t)
		{
		
			float angle;
			float deltaAngle = this.aEndAngle - this.aStartAngle;
		
			if (deltaAngle < 0.0f)
				deltaAngle += Mathf.PI * 2.0f;
			if (deltaAngle > Mathf.PI * 2)
				deltaAngle -= Mathf.PI * 2.0f;
		
			if (this.aClockwise == true) {
			
				angle = this.aEndAngle + (1.0f - t) * (Mathf.PI * 2.0f - deltaAngle);
			
			} else {
			
				angle = this.aStartAngle + t * deltaAngle;
			
			}
		
			float tx = this.aX + this.xRadius * Mathf.Cos (angle);
			float ty = this.aY + this.yRadius * Mathf.Sin (angle);
		
			return new Vector3 (tx, ty);
		}

	}
}
