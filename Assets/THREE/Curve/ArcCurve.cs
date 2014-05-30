using UnityEngine;
using System.Collections;

namespace THREE
{
	public class ArcCurve : EllipseCurve
	{

		public ArcCurve (float aX, float aY, float aRadius, float aStartAngle, float aEndAngle, bool aClockwise) : 
				base(aX, aY, aRadius, aRadius, aStartAngle, aEndAngle, aClockwise)
		{
	
		}

	}
}
