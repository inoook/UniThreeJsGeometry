// https://github.com/mrdoob/three.js/blob/master/src/extras/geometries/ParametricGeometry.js
/**
 * @author zz85 / https://github.com/zz85
 * Parametric Surfaces Geometry
 * based on the brilliant article by @prideout http://prideout.net/blog/?p=44
 *
 * new THREE.ParametricGeometry( parametricFunction, uSegments, ySegements, useTris );
 *
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace THREE
{
	public class ParametricGeometry : Geometry
	{

		// Use this for initialization
		public ParametricGeometry (Func<float, float, Vector3> func, int slices, int stacks)
		{
			List<Vector3> verts = this.vertices;
			List<Face3> faces = this.faces;
//			List<List<Vector2>> uvs = this.faceVertexUvs;
		
			//int i, j;
			Vector3 p;
			float u, v;
		
			//int stackCount = stacks + 1;
			int sliceCount = slices + 1;
		
			for (int i = 0; i <= stacks; i ++) {
			
				v = (float)i / stacks;
			
				for (int j = 0; j <= slices; j ++) {
				
					u = (float)j / slices;
				
					p = func (u, v);
					verts.Add (p);
				
				}
			}
		
			int a, b, c, d;
			Vector2 uva, uvb, uvc, uvd;
		
			for (int i = 0; i < stacks; i ++) {
			
				for (int j = 0; j < slices; j ++) {
				
					a = i * sliceCount + j;
					b = i * sliceCount + j + 1;
					c = (i + 1) * sliceCount + j + 1;
					d = (i + 1) * sliceCount + j;
				
					uva = new Vector2 ((float)j / slices, (float)i / stacks);
					uvb = new Vector2 ((float)(j + 1) / slices, (float)i / stacks);
					uvc = new Vector2 ((float)(j + 1) / slices, (float)(i + 1) / stacks);
					uvd = new Vector2 ((float)j / slices, (float)(i + 1) / stacks);

					Face3 face0 = new Face3 (a, b, d);
					face0.uvs = new Vector2[]{ uva, uvb, uvd };
					faces.Add (face0);
				
					Face3 face1 = new Face3 (b, c, d);
					face1.uvs = new Vector2[]{ (uvb), uvc, (uvd) };
					faces.Add (face1);
				}
			}
		
			// magic bullet
			//var diff = this.mergeVertices();
			//Debug.Log("removed "+ diff+ " vertices by merging");
			
            this.SetFaceSmooth();
		}
	}
}
