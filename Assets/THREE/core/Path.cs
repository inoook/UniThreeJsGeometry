/**
 * @author zz85 / http://www.lab4games.net/zz85/blog
 * Creates free form 2d path using series of points, lines or curves.
 *
 **/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public enum PathActions
	{
		MOVE_TO,
		LINE_TO,
		QUADRATIC_CURVE_TO,
		BEZIER_CURVE_TO,
		CSPLINE_THRU,
		ARC,
		ELLIPSE
	}

	public class PAction
	{
		public PathActions action;
		public List<float> args;
		public bool aClockwise = false;

		public PAction (PathActions action, List<float> pos)
		{
			this.action = action;
			this.args = pos;
		}
	}

	public class Path : CurvePath
	{

		public List<PAction> actions;
		public List<Vector2> pointList;
		public bool useSpacedPoints = false;

		public Path (List<Vector2> points = null)
		{
			this.actions = new List<PAction> ();
			pointList = new List<Vector2> ();
		
			if (points != null) {
				this.fromPoints (points);
			}
		}

		// TODO Clean up PATH API
		// Create path using straight lines to connect all points
		// - vectors: array of Vector2
		void fromPoints (List<Vector2> vectors)
		{
			this.moveTo (vectors [0].x, vectors [0].y);
			for (int v = 1, vlen = vectors.Count; v < vlen; v ++) {
				this.lineTo (vectors [v].x, vectors [v].y);
			}	
		}
		// startPath() endPath()?
	
		public void moveTo (float x, float y)
		{
			//var args = Array.prototype.slice.call( arguments );
			PAction act = new PAction (PathActions.MOVE_TO, new List<float> (new float[] {
				x,
				y
			}));
			this.actions.Add (act);

            //this.pointList.Add(new Vector2(x, y));
			//this.actions.Add( { action: THREE.PathActions.MOVE_TO, args: args } );	
		}

		public void lineTo (float x, float y)
		{
			//var args = Array.prototype.slice.call( arguments );
		
			List<float> lastargs = this.actions [this.actions.Count - 1].args;
		
			float x0 = lastargs [lastargs.Count - 2];
			float y0 = lastargs [lastargs.Count - 1];
		
			LineCurve curve = new LineCurve (new Vector3 (x0, y0), new Vector3 (x, y));
			this.curves.Add (curve);

			PAction act = new PAction (PathActions.LINE_TO, new List<float> (new float[] {
				x,
				y
			}));
			this.actions.Add (act);
		}

		public void quadraticCurveTo (float aCPx, float aCPy, float aX, float aY)
		{
			List<float> lastargs = this.actions [this.actions.Count - 1].args;
		
			float x0 = lastargs [lastargs.Count - 2];
			float y0 = lastargs [lastargs.Count - 1];
		
			QuadraticBezierCurve curve = new QuadraticBezierCurve (new Vector3 (x0, y0),
		                                           new Vector3 (aCPx, aCPy),
		                                           new Vector3 (aX, aY));
			this.curves.Add (curve);
		
			//this.actions.push( { action: THREE.PathActions.QUADRATIC_CURVE_TO, args: args } );
			PAction act = new PAction (PathActions.QUADRATIC_CURVE_TO, new List<float> (new float[] {
				aCPx,
				aCPy,
				aX,
				aY
			}));
			this.actions.Add (act);
		
		}

		public void bezierCurveTo (float aCP1x, float aCP1y,
	                   float aCP2x, float aCP2y,
	                   float aX, float aY)
		{
			List<float> lastargs = this.actions [this.actions.Count - 1].args;
		
			float x0 = lastargs [lastargs.Count - 2];
			float y0 = lastargs [lastargs.Count - 1];

			CubicBezierCurve curve = new CubicBezierCurve (new Vector3 (x0, y0),
		                                       new Vector3 (aCP1x, aCP1y),
		                                       new Vector3 (aCP2x, aCP2y),
		                                       new Vector3 (aX, aY));
			this.curves.Add (curve);
		
			//this.actions.Add( { action: THREE.PathActions.BEZIER_CURVE_TO, args: args } );
			PAction act = new PAction (PathActions.BEZIER_CURVE_TO, new List<float> (new float[] {
				aCP1x,
				aCP1y,
				aCP2x,
				aCP2y,
				aX,
				aY
			}));
			this.actions.Add (act);
		}

		// FUTURE: Change the API or follow canvas API?
	
		public void arc (float aX, float aY, float aRadius, float aStartAngle, float aEndAngle, bool aClockwise)
		{
		
			Vector2 lastPoint = pointList [this.pointList.Count - 1];
			float x0 = lastPoint.x;
			float y0 = lastPoint.y;
		
			this.absarc (aX + x0, aY + y0, aRadius,
		            aStartAngle, aEndAngle, aClockwise);
		
		}

		public void absarc (float aX, float aY, float aRadius, float aStartAngle, float aEndAngle, bool aClockwise)
		{
            this.moveTo(aX+aRadius, aY);
			this.absellipse (aX, aY, aRadius, aRadius, aStartAngle, aEndAngle, aClockwise);
		}

		public void ellipse (float aX, float aY, float xRadius, float yRadius, float aStartAngle, float aEndAngle, bool aClockwise)
		{
            Vector2 lastPoint = pointList [this.pointList.Count - 1];
            float x0 = lastPoint.x;
            float y0 = lastPoint.y;

            this.absellipse (aX - x0, aY + y0, xRadius, yRadius, aStartAngle, aEndAngle, aClockwise);
		}

		public void absellipse (float aX, float aY, float xRadius, float yRadius, float aStartAngle, float aEndAngle, bool aClockwise)
		{

			EllipseCurve curve = new EllipseCurve (aX, aY, xRadius, yRadius,
		                                   aStartAngle, aEndAngle, aClockwise);
			this.curves.Add (curve);
		
			Vector3 lastPoint = curve.getPoint (1.0f);
			float lastX = lastPoint.x;
			float lastY = lastPoint.y;
            //args.push(lastPoint.x);
            //args.push(lastPoint.y);

            //this.actions.push( { action: THREE.PathActions.ELLIPSE, args: args } );
            PAction act = new PAction (PathActions.ELLIPSE, new List<float> (new float[] {
                aX,
				aY,
				xRadius,
				yRadius,
				aStartAngle,
				aEndAngle,
				1.0f,
				lastX,
				lastY
			}));
			act.aClockwise = aClockwise;
			this.actions.Add (act);
		}

		public override List<Vector3> getSpacedPoints (float divisions = 40, bool closedPath = true)
		{
			List<Vector3> points = new List<Vector3> ();
		
			for (int i = 0; i < divisions; i ++) {
				points.Add (this.getPoint ((float)i / divisions));
			}
		
			// if ( closedPath ) {
			//
			// 	points.push( points[ 0 ] );
			//
			// }
		
			return points;
		}

		/* Return an array of vectors based on contour of the path */
	
		public override List<Vector3> getPoints (float divisions = 12, bool closedPath = true)
		{
			if (this.useSpacedPoints) {
				return this.getSpacedPoints (divisions, closedPath);
			}
		
			List<Vector3> points = new List<Vector3> ();

			PAction item;
			PathActions action;
			List<float> args;
			bool aClockwise;

			float cpx, cpy, cpx2, cpy2, cpx1, cpy1, cpx0, cpy0;

			Vector3 laste;
			List<float> lasteArgs;
			float t, tx, ty;
			
			for (int i = 0, il = this.actions.Count; i < il; i ++) {
			
				item = this.actions [i];
			
				action = item.action;
				args = item.args;
				aClockwise = item.aClockwise;
			
				switch (action) {
				
				case PathActions.MOVE_TO:
					THREE.Utils.DebugLog ("MOVE_TO");
					points.Add (new Vector3 (args [0], args [1]));
				
					break;
				
				case PathActions.LINE_TO:
					THREE.Utils.DebugLog ("LINE_TO");
					points.Add (new Vector2 (args [0], args [1]));
				
					break;
				
				case PathActions.QUADRATIC_CURVE_TO:
					THREE.Utils.DebugLog ("QUADRATIC_CURVE_TO");
					cpx = args [2];
					cpy = args [3];
				
					cpx1 = args [0];
					cpy1 = args [1];

					cpx0 = 0;
					cpy0 = 0;
				
					if (points.Count > 0) {
						laste = points [points.Count - 1];
					
						cpx0 = laste.x;
						cpy0 = laste.y;
					
					} else {
						lasteArgs = this.actions [i - 1].args;
					
						cpx0 = lasteArgs [lasteArgs.Count - 2];
						cpy0 = lasteArgs [lasteArgs.Count - 1];
					
					}
				
					for (int j = 1; j <= divisions; j ++) {
					
						t = (float)j / divisions;
					
						tx = Shape.UtilsShape.b2 (t, cpx0, cpx1, cpx);
						ty = Shape.UtilsShape.b2 (t, cpy0, cpy1, cpy);
					
						points.Add (new Vector3 (tx, ty));
					}
					break;

				case PathActions.BEZIER_CURVE_TO:
					THREE.Utils.DebugLog ("BEZIER_CURVE_TO");
					cpx = args [4];
					cpy = args [5];
			
					cpx1 = args [0];
					cpy1 = args [1];
			
					cpx2 = args [2];
					cpy2 = args [3];
			
					if (points.Count > 0) {
				
						laste = points [points.Count - 1];
					
						cpx0 = laste.x;
						cpy0 = laste.y;
				
					} else {
					
						lasteArgs = this.actions [i - 1].args;
					
						cpx0 = lasteArgs [lasteArgs.Count - 2];
						cpy0 = lasteArgs [lasteArgs.Count - 1];
				
					}
				
				
					for (int j = 1; j <= divisions; j ++) {
					
						t = (int)j / divisions;
					
						tx = Shape.UtilsShape.b3 (t, cpx0, cpx1, cpx2, cpx);
						ty = Shape.UtilsShape.b3 (t, cpy0, cpy1, cpy2, cpy);
					
						points.Add (new Vector2 (tx, ty));
						
					}
					break;

				case PathActions.CSPLINE_THRU:
					THREE.Utils.DebugLog ("CSPLINE_THRU no implement");
				/*
				lasteArgs = this.actions[ i - 1 ].args;
				
				Vector3 last = new Vector3( lasteArgs[ lasteArgs.Count - 2 ], lasteArgs[ lasteArgs.Count - 1 ], 0 );
				//List<Vector3> spts = [ last ];
				List<Vector3> spts = new List<Vector3>();
				spts.Add(last);
				
				var n = divisions * args[ 0 ].Count;

				//spts = spts.concat( args[ 0 ] );
				spts = spts.Add(args[0]);
				
				SplineCurve spline = new SplineCurve( spts );
				
				for (int j = 1; j <= n; j ++ ) {
					
					points.Add( spline.getPointAt( j / n ) ) ;
					
				}
				*/
					break;

				case PathActions.ARC:
					THREE.Utils.DebugLog ("ARC");
					float aX = args [0], aY = args [1],
					aRadius = args [2],
					aStartAngle = args [3], aEndAngle = args [4];
				//aClockwise = args[ 5 ];
				//aClockwise = !!args[ 5 ];
				
					float deltaAngle = aEndAngle - aStartAngle;
					float angle;
					float tdivisions = divisions * 2;
				
					for (int j = 1; j <= tdivisions; j ++) {
					
						t = (float)j / tdivisions;

						if (! aClockwise) {
							t = 1.0f - t;
						}
					
						angle = aStartAngle + t * deltaAngle;
					
						tx = aX + aRadius * Mathf.Cos (angle);
						ty = aY + aRadius * Mathf.Sin (angle);
					
						//console.log('t', t, 'angle', angle, 'tx', tx, 'ty', ty);
					
						points.Add (new Vector2 (tx, ty));
					
					}
				
				//console.log(points);
				
					break;

				case PathActions.ELLIPSE:
					THREE.Utils.DebugLog ("ELLIPSE");
					ELLIPSE (args, divisions, points, aClockwise);
				
					//console.log(points);
					
					break;
				//
				}
			}

			// Normalize to remove the closing point by default.
			Vector3 lastPoint = points [points.Count - 1];
			float EPSILON = THREE.Setting.EPSILON;
			//float EPSILON = 0.0001f;

			if (Mathf.Abs (lastPoint.x - points [0].x) < EPSILON && Mathf.Abs (lastPoint.y - points [0].y) < EPSILON) {
				points.RemoveAt (points.Count - 1);
			}

			if (closedPath) {
				Debug.Log("CLOSED PATH");
				points.Add (points [0]);
			}
			
			return points;
		}

		void ELLIPSE (List<float> args, float divisions, List<Vector3> points, bool aClockwise)
		{
			float aX = args [0], aY = args [1],
			xRadius = args [2],
			yRadius = args [3],
			aStartAngle = args [4], aEndAngle = args [5];
            //aClockwise = args[ 6 ];

			float deltaAngle = aEndAngle - aStartAngle;
			float angle;
			float tdivisions = divisions * 2.0f;

            for (int j = 1; j <= tdivisions; j++) {
			
				float t = (float)j / tdivisions;
			
				if (! aClockwise) {
					t = 1.0f - t;
				}
			
				angle = aStartAngle + t * deltaAngle;
			
				float tx = aX + xRadius * Mathf.Cos (angle);
				float ty = aY + yRadius * Mathf.Sin (angle);
			
				//console.log('t', t, 'angle', angle, 'tx', tx, 'ty', ty);
			
				points.Add (new Vector3 (tx, ty, 0));
			}
		}

		//
		// Breaks path into shapes
		//
		//	Assumptions (if parameter isCCW==true the opposite holds):
		//	- solid shapes are defined clockwise (CW)
		//	- holes are defined counterclockwise (CCW)
		//
		//	If parameter noHoles==true:
		//  - all subPaths are regarded as solid shapes
		//  - definition order CW/CCW has no relevance
		//
		void toShapes (bool isCCW, bool noHoles)
		{
			Debug.LogWarning ("toShapes not implement");
		}

		List<Path> extractSubpaths (List<PAction> inActions)
		{ // not use yet
			Debug.LogError ("extractSubpaths not implement");

			List<Path> subPaths = new List<Path> ();
			Path lastPath = new Path ();
		
			for (int i = 0, il = inActions.Count; i < il; i ++) {
			
				PAction item = inActions [i];
			
				List<float> args = item.args;
				PathActions action = item.action;
				bool aClockwise = item.aClockwise;
			
				if (action == PathActions.MOVE_TO) {
				
					if (lastPath.actions.Count != 0) {
					
						subPaths.Add (lastPath);
						lastPath = new Path ();
					}
				
				}

				// js 477
				//lastPath[ action ].apply( lastPath, args ); 
				Debug.LogError ("######### TODO: no implement ############");

				if (action == PathActions.MOVE_TO) {
					lastPath.moveTo (args [0], args [1]);
				} else if (action == PathActions.LINE_TO) {
					lastPath.lineTo (args [0], args [1]);
				} else if (action == PathActions.ARC) {
					//lastPath.arc(args[0], args[1], args[2], args[3], args[4], args[5]);
				} else if (action == PathActions.ELLIPSE) {
					lastPath.ellipse (args [0], args [1], args [2], args [3], args [4], args [5], aClockwise);
				} else if (action == PathActions.QUADRATIC_CURVE_TO) {
					lastPath.quadraticCurveTo (args [0], args [1], args [2], args [3]);
				} else {

				}
			

			}
		
			if (lastPath.actions.Count != 0) {
			
				subPaths.Add (lastPath);
			
			}
		
			// console.log(subPaths);
		
			return	subPaths;
		}
	}
}
