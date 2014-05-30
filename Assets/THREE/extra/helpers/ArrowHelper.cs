using UnityEngine;
using System.Collections;

namespace THREE
{
	public class ArrowHelper : Geometry {
		
		public ArrowHelper(Vector3 dir, Vector3 origin, float length, Color color, float headLength = 0.2f, float headWidth = 0.2f) : base()
		{
			this.vertices.Add( origin );
			this.vertices.Add( origin + dir.normalized * length );

			this.verticexColors.Add( color );
			this.verticexColors.Add( color );
		}
		
	}
}
