// https://github.com/mrdoob/three.js/blob/master/examples/js/ParametricGeometries.js
/*
 * @author zz85
 *
 * Experimenting of primitive geometry creation using Surface Parametric equations
 *
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace THREE
{
	public class ParametricGeometries
	{

		public static Vector3 klein (float v, float u)
		{
			u *= Mathf.PI;
			v *= 2 * Mathf.PI;
			
			u = u * 2;
			float x, y, z;
			if (u < Mathf.PI) {
				x = 3 * Mathf.Cos (u) * (1 + Mathf.Sin (u)) + (2 * (1 - Mathf.Cos (u) / 2)) * Mathf.Cos (u) * Mathf.Cos (v);
				z = -8 * Mathf.Sin (u) - 2 * (1 - Mathf.Cos (u) / 2) * Mathf.Sin (u) * Mathf.Cos (v);
			} else {
				x = 3 * Mathf.Cos (u) * (1 + Mathf.Sin (u)) + (2 * (1 - Mathf.Cos (u) / 2)) * Mathf.Cos (v + Mathf.PI);
				z = -8 * Mathf.Sin (u);
			}
			
			y = -2 * (1 - Mathf.Cos (u) / 2) * Mathf.Sin (v);
			
			return new Vector3 (x, y, z);
		}

		public static Func<float, float, Vector3> plane (float width, float height) {
			return (u, v) => {
				float x = u * width;
				float y = 0;
				float z = v * height;
				
				return new Vector3(x, y, z);
			};
		}

//		public static Vector3 plane (float u, float v) {
//
//			float x = u * 200;
//			float y = 0;
//			float z = v * 200;
//			
//			return new Vector3(x, y, z);
//		}
		
		public static Vector3 mobius (float u, float t)
		{
			// flat mobius strip
			// http://www.wolframalpha.com/input/?i=M%C3%B6bius+strip+parametric+equations&lk=1&a=ClashPrefs_*Surface.MoebiusStrip.SurfaceProperty.ParametricEquations-
			u = u - 0.5f;
			float v = 2 * Mathf.PI * t;
			
			float x, y, z;
			
			float a = 2;
			x = Mathf.Cos (v) * (a + u * Mathf.Cos (v / 2));
			y = Mathf.Sin (v) * (a + u * Mathf.Cos (v / 2));
			z = u * Mathf.Sin (v / 2);
			return new Vector3 (x, y, z);
		}

		public static Vector3 mobius3d(float u, float t) {
			// volumetric mobius strip
			u *= Mathf.PI;
			t *= 2 * Mathf.PI;
			
			u = u * 2;
			float phi = u / 2.0f;
			float major = 2.25f, a = 0.125f, b = 0.65f;
			float x, y, z;
			x = a * Mathf.Cos(t) * Mathf.Cos(phi) - b * Mathf.Sin(t) * Mathf.Sin(phi);
			z = a * Mathf.Cos(t) * Mathf.Sin(phi) + b * Mathf.Sin(t) * Mathf.Cos(phi);
			y = (major + x) * Mathf.Sin(u);
			x = (major + x) * Mathf.Cos(u);
			return new Vector3(x, y, z);
		}

		/*********************************************
		 *
		 * Parametric Replacement for TubeGeometry
		 *
		 *********************************************/
		public class TubeGeometry
		{
			Path path;
			int segments;
			float radius;
			//int segmentsRadius;
			//bool closed;

			List<Vector3> tangents;
			List<Vector3> normals;
			List<Vector3> binormals;

			int numpoints;

			public Geometry geo;

			public TubeGeometry(Path path, int segments = 64, float radius = 1, int segmentsRadius = 8, bool closed = false)
			{
				this.path = path;
				this.segments = segments;
				this.radius = radius;
				//this.segmentsRadius = segmentsRadius;
				//this.closed = closed;

				numpoints = this.segments + 1;
				/*
				var frames = new THREE.TubeGeometry();
				frames.FrenetFrames(path, segments, closed);
				*/
				var frames = new THREE.TubeGeometry.FrenetFrames(path, segments, closed);
				//List<Vector3> tangents = frames.tangents;
				List<Vector3> normals = frames.normals;
				List<Vector3> binormals = frames.binormals;
				
				// proxy internals
				//this.tangents = tangents;
				this.normals = normals;
				this.binormals = binormals;

				geo = new ParametricGeometry(paramFunc, segments, segmentsRadius);
			}

			public Vector3 paramFunc(float u, float v) {
				v *= 2 * Mathf.PI;
				
				//int i = (u * (numpoints - 1));
				int i = Mathf.FloorToInt(u * (numpoints - 1));
				
				Vector3 pos2 = path.getPointAt(u);
				
				//Vector3 tangent = tangents[i];
				Vector3 normal = normals[i];
				Vector3 binormal = binormals[i];

				float cx = -radius * Mathf.Cos(v); // TODO: Hack: Negating it so it faces outside.
				float cy = radius * Mathf.Sin(v);
				
				//pos2.copy(pos);
				pos2.x += cx * normal.x + cy * binormal.x;
				pos2.y += cx * normal.y + cy * binormal.y;
				pos2.z += cx * normal.z + cy * binormal.z;
				
				//return pos2.clone();
				return pos2;
			}
		}

		//
		/*********************************************
		*
		* Parametric Replacement for SphereGeometry
		*
		*********************************************/
		//Geometry SphereGeometry(float size, float u, float v) {

		public class ParaSphereGeometry
		{
			private float size;
			public Geometry geo;
			public ParaSphereGeometry (float size, float u, float v)
			{
				this.size = size;
				geo = new ParametricGeometry(paramFunc, (int)u, (int)v);
			}

			public Vector3 paramFunc (float u, float v)
			{
				u *= Mathf.PI;
				v *= 2 * Mathf.PI;
				
				float x = size * Mathf.Sin (u) * Mathf.Cos (v);
				float y = size * Mathf.Sin (u) * Mathf.Sin (v);
				float z = size * Mathf.Cos (u);
				
				return new Vector3 (x, y, z);
			}
		}
	}

}