/**
 * @author zz85 / http://www.lab4games.net/zz85/blog
 *
 * Creates extruded geometry from a path shape.
 *
 * parameters = {
 *
 *  curveSegments: <int>, // number of points on the curves
 *  steps: <int>, // number of points for z-side extrusions / used for subdividing segements of extrude spline too
 *  amount: <int>, // Depth to extrude the shape
 *
 *  bevelEnabled: <bool>, // turn on bevel
 *  bevelThickness: <float>, // how deep into the original shape bevel goes
 *  bevelSize: <float>, // how far from shape outline is bevel
 *  bevelSegments: <int>, // number of bevel layers
 *
 *  extrudePath: <THREE.CurvePath> // 3d spline path to extrude shape along. (creates Frames if .frames aren't defined)
 *  frames: <THREE.TubeGeometry.FrenetFrames> // containing arrays of tangents, normals, binormals
 *
 *  material: <int> // material index for front and back faces
 *  extrudeMaterial: <int> // material index for extrusion and beveled faces
 *  uvGenerator: <Object> // object that provides UV generator functions
 *
 * }
 **/
// https://github.com/mrdoob/three.js/blob/master/src/extras/geometries/ExtrudeGeometry.js

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class ExtrudeGeometry : Geometry
	{
		public class Option{
			public int amount = 100;
			public float bevelThickness = 6;
			//public int bevelSize = bevelThickness - 2;
			public float bevelSize = 4;
			public int bevelSegments = 3;

			public bool bevelEnabled = true;

			public int curveSegments = 12;

			public int steps = 1;

			public Curve extrudePath;

			//public Material material;
			//public Material extrudeMaterial;

			//public WorldUVGenerator UVGenerator ExtrudeGeometry.WorldUVGenerator;
			public WorldUVGenerator UVGenerator;

			public THREE.TubeGeometry.FrenetFrames frames;
		}


		public BoundingBox shapebb;

		// Use this for initialization
		public ExtrudeGeometry (List<Shape> shapes, Option options )
		{
			//shapes = (shapes != null) ? shapes : new List<Shape>();
			this.shapebb = shapes[ shapes.Count - 1 ].getBoundingBox();
			
			this.addShapeList(shapes, options );
			
			this.computeFaceNormals();
			this.copyFaceNormalToVertexNormals();
			
			// can't really use automatic vertex normals
			// as then front and back sides get smoothed too
			// should do separate smoothing just for sides
			
			//this.computeVertexNormals();
		}

		public ExtrudeGeometry (Shape shape, Option options )
		{
			this.shapebb = shape.getBoundingBox();
			
			this.addShape( shape, options );
		}

		void addShapeList(List<Shape>  shapes, Option options ) {
			int sl = shapes.Count;
			
			for ( int s = 0; s < sl; s ++ ) {
				Shape shape = shapes[ s ];
				this.addShape( shape, options );
			}
		}

		void addShape(Shape shape, Option options ) {

			int amount = options.amount;
			
			float bevelThickness = options.bevelThickness;
			float bevelSize = options.bevelSize;
			int bevelSegments = options.bevelSegments;
			
			bool bevelEnabled = options.bevelEnabled;
			
			int curveSegments = options.curveSegments;
			
			int steps = options.steps;
			
			Curve extrudePath = options.extrudePath;
			
			//Material material = options.material;
			//Material extrudeMaterial = options.extrudeMaterial;
			
			// Use default WorldUVGenerator if no UV generators are specified.
			WorldUVGenerator uvgen = options.UVGenerator;
			if(uvgen == null){
				Debug.LogWarning("CREATE WorldUVGenerator");
				uvgen = new WorldUVGenerator();
			}

			List<Vector3> extrudePts = new List<Vector3>();// ???
			bool extrudeByPath = false;
			//BoundingBox shapebb = this.shapebb;

			THREE.TubeGeometry.FrenetFrames splineTube = null;
			Vector3 binormal, normal, position2;

			if ( extrudePath != null) {
				
				extrudePts = extrudePath.getSpacedPoints( steps );	
				extrudeByPath = true;
				bevelEnabled = false; // bevels not supported for path extrusion
				
				// SETUP TNB variables
				
				// Reuse TNB from TubeGeomtry for now.
				// TODO1 - have a .isClosed in spline?
				splineTube = options.frames;
				if(splineTube == null){
					Debug.LogWarning("CREATE TubeGeometry.FrenetFrame");
					splineTube = new THREE.TubeGeometry.FrenetFrames(extrudePath, steps, false);
				}
				
				// console.log(splineTube, 'splineTube', splineTube.normals.length, 'steps', steps, 'extrudePts', extrudePts.length);
				
				binormal = new Vector3();
				normal = new Vector3();
				position2 = new Vector3();

			}else{
				Debug.Log("no extrudePath");
			}

			// Safeguards if bevels are not enabled
			
			if ( ! bevelEnabled ) {
				bevelSegments = 0;
				bevelThickness = 0;
				bevelSize = 0;
			}

			// Variables initalization

			//var ahole, h, hl; // looping of holes
			//var scope = this;
			List<Vector3> ahole;
			//List<Vector3> bevelPoints = new List<Vector3>();
			
			int shapesOffset = this.vertices.Count;
			
			ShapeAndHoleObject shapePoints = shape.extractPoints( curveSegments );
			
			List<Vector3> vertices = shapePoints.shape;
			List<List<Vector3>> holes = shapePoints.holes;

			//Debug.LogWarning(vertices.Count + " / " + holes.Count + " / "+curveSegments);
			
			bool reverse = !Shape.Utils.isClockWise( vertices ) ;

			if ( reverse ) {
				vertices.Reverse();

				for (int h = 0, hl = holes.Count; h < hl; h ++ ) {
					
					ahole = holes[ h ];
					
					if ( Shape.Utils.isClockWise( ahole ) ) {
						//holes[ h ] = ahole.reverse();
						ahole.Reverse();
						holes[ h ] = ahole;
					}
				}
				
				reverse = false; // If vertices are in order now, we shouldn't need to worry about them again (hopefully)!
			}

			List<List<int>> faces = Shape.Utils.triangulateShape ( vertices, holes );
			//Debug.LogWarning("faces: "+ faces.Count + " vertices: "+vertices.Count + " / "+holes[0].Count);
			
			/* Vertices */
			
			//List<Vector3> contour = vertices; // vertices has all points but contour has only points of circumference
			List<Vector3> contour = new List<Vector3>();
			contour.AddRange(vertices);

//			// Debug
//			for(int i = 0; i < contour.Count; i++){
//				Debug.Log(i + " / "+ contour[i]);
//			}
			
			for (int h = 0, hl = holes.Count;  h < hl; h ++ ) {
				ahole = holes[ h ];
				//vertices = vertices.concat( ahole );
				vertices.AddRange( ahole );
			}


			float t;
			float z, bs;

			Vector3 vert;
			int vlen = vertices.Count;
			//Face3 face;
			int flen = faces.Count;
			//var cont;
			//int clen = contour.Count;

			// Find directions for point movement
			
			//float RAD_TO_DEGREES = 180 / Mathf.PI;

			// js 290
			//List<Vector3> contourMovements = new List<Vector3>(new Vector3[contour.Count]);
			//Debug.LogWarning("contour.Count: "+contour.Count);
			List<Vector3> contourMovements = new List<Vector3>();
			for ( int i = 0, il = contour.Count, j = il - 1, k = i + 1; i < il; i ++, j ++, k ++ ) {
				int t_i = i;
				//if ( i == 0 ) { t_i = il-2; }
				if ( j == il ) j = 0;
				if ( k == il ) k = 0;
				
				//  (j)---(i)---(k)
				// console.log('i,j,k', i, j , k)
				
//				Vector3 pt_i = contour[ i ];
//				Vector3 pt_j = contour[ j ];
//				Vector3 pt_k = contour[ k ];
				//Debug.LogWarning(i + "> t_i, j, k: "+t_i +", "+j+", "+k );
				Vector3 bevelVec = getBevelVec( contour[ t_i ], contour[ j ], contour[ k ] );
				//Debug.LogWarning("bevelVec: "+bevelVec+" // "+contour[t_i] + " / "+contour[j] + " / "+contour[k] + " / "+bevelVec.magnitude);
				contourMovements.Add( bevelVec );
				//contourMovements[ i ] = bevelVec;
			}

			//Debug.LogWarning("contourMovements.Count: "+contourMovements.Count + " / vlen: "+vlen);

			List<List<Vector3>> holesMovements = new List<List<Vector3>>();
			List<Vector3> oneHoleMovements;

			//verticesMovements = contourMovements.concat(); // TODO: Check /////////
			List<Vector3> verticesMovements = new List<Vector3>();
			verticesMovements.AddRange( contourMovements ); // COPY????

			for (int h = 0, hl = holes.Count; h < hl; h ++ ) {
				
				ahole = holes[ h ];
				
				oneHoleMovements = new List<Vector3>();
				
				for (int i = 0, il = ahole.Count, j = il - 1, k = i + 1; i < il; i ++, j ++, k ++ ) {
					
					if ( j == il ) j = 0;
					if ( k == il ) k = 0;
					
					//  (j)---(i)---(k)
					//oneHoleMovements[ i ] = getBevelVec( ahole[ i ], ahole[ j ], ahole[ k ] );
					Vector3 bevelVec = getBevelVec( ahole[ i ], ahole[ j ], ahole[ k ] );
					oneHoleMovements.Add ( bevelVec );
				}
				
				holesMovements.Add( oneHoleMovements );
				//verticesMovements = verticesMovements.concat( oneHoleMovements );
				verticesMovements.AddRange( oneHoleMovements );
			}

			// Loop bevelSegments, 1 for the front, 1 for the back
			for (int b = 0; b < bevelSegments; b ++ ) {
				//for ( b = bevelSegments; b > 0; b -- ) {
				
				t = (float)b / bevelSegments;
				z = bevelThickness * ( 1 - t );
				
				//z = bevelThickness * t;
				bs = bevelSize * ( Mathf.Sin ( t * Mathf.PI/2 ) ) ; // curved
				//bs = bevelSize * t ; // linear
				
				// contract shape
				
				for (int i = 0, il = contour.Count; i < il; i ++ ) {
					//Debug.Log(">> contract shape");
					vert = scalePt2( contour[ i ], contourMovements[ i ], bs );
					
					addVertex( vert.x, vert.y,  - z );
				}
				
				// expand holes
				
				for (int h = 0, hl = holes.Count; h < hl; h++ ) {
					
					ahole = holes[ h ];
					oneHoleMovements = holesMovements[ h ];
					
					for (int i = 0, il = ahole.Count; i < il; i++ ) {
						//Debug.Log(">> expand holes");
						vert = scalePt2( ahole[ i ], oneHoleMovements[ i ], bs );
						
						addVertex( vert.x, vert.y,  -z );
						
					}
					
				}
				
			}

			bs = bevelSize;
			
			// Back facing vertices
			for (int i = 0; i < vlen; i ++ ) {
				
				vert = bevelEnabled ? scalePt2( vertices[ i ], verticesMovements[ i ], bs ) : vertices[ i ];
				
				if ( !extrudeByPath && splineTube == null ) {
					//Debug.Log(">> !extrudeByPath");
					addVertex( vert.x, vert.y, 0 );

				} else {
					//Debug.Log(">> extrudeByPath");
					normal = splineTube.normals[0] * (vert.x);
					binormal = splineTube.binormals[0] * (vert.y);
					
					position2 = ( extrudePts[0] ) + (normal) + (binormal);
					
					addVertex( position2.x, position2.y, position2.z );
					
				}
				
			}

			// Add stepped vertices...
			// Including front facing vertices
			
			int s;
			
			for ( s = 1; s <= steps; s ++ ) {
				
				for (int i = 0; i < vlen; i ++ ) {
					
					vert = bevelEnabled ? scalePt2( vertices[ i ], verticesMovements[ i ], bs ) : vertices[ i ];
					
					if ( !extrudeByPath ) {
						//Debug.Log("--->> Add stepped vertices / !extrudeByPath: "+ vert + " / "+vertices[ i ] +" / "+verticesMovements[ i ] + " / "+bs);
						addVertex( vert.x, vert.y, (float)amount / steps * s );
						
					} else {
						normal = splineTube.normals[s] * ( vert.x );
						binormal = ( splineTube.binormals[s] ) * ( vert.y );
						position2 = ( extrudePts[s] ) + ( normal ) + ( binormal );

						//Debug.Log(">>>> --->> Add stepped vertices / extrudeByPath / s: " + s);
						addVertex( position2.x, position2.y, position2.z );
					}
				}
			}


			// Add bevel segments planes
			
			//for (int b = 1; b <= bevelSegments; b ++ ) {
			for (int b = bevelSegments - 1; b >= 0; b -- ) {
				
				t = (float)b / bevelSegments;
				z = bevelThickness * ( 1.0f - t );
				//bs = bevelSize * ( 1-Math.sin ( ( 1 - t ) * Math.PI/2 ) );
				bs = bevelSize * Mathf.Sin ( t * Mathf.PI/2.0f ) ;
				
				// contract shape
				
				for (int i = 0, il = contour.Count; i < il; i ++ ) {
					//Debug.Log(">> Add bevel segments planes");
					vert = scalePt2( contour[ i ], contourMovements[ i ], bs );
					addVertex( vert.x, vert.y,  (float)amount + z );
				}
				
				// expand holes
				
				for (int h = 0, hl = holes.Count; h < hl; h ++ ) {
					
					ahole = holes[ h ];
					oneHoleMovements = holesMovements[ h ];
					
					for (int i = 0, il = ahole.Count; i < il; i ++ ) {
						
						vert = scalePt2( ahole[ i ], oneHoleMovements[ i ], bs );
						
						if ( !extrudeByPath ) {
							//Debug.Log(">> expand holes / !extrudeByPath");
							addVertex( vert.x, vert.y,  (float)amount + z );
							
						} else {
							//Debug.Log(">> expand holes / extrudeByPath");
							addVertex( vert.x, vert.y + extrudePts[ steps - 1 ].y, extrudePts[ steps - 1 ].x + z );
							
						}
						
					}
					
				}
				
			}

			/* Faces */

			// Top and bottom faces

			buildLidFaces(vlen, faces, flen, steps, bevelSegments, bevelEnabled, shapesOffset, shape, uvgen, options);
			
			// Sides faces
			
			buildSideFaces(vlen, contour, holes, steps, bevelSegments, shapesOffset, uvgen, shape, options);

		}

		Vector3 scalePt2 (Vector3 pt, Vector3 vec, float size ) {
			
			//if ( vec == null){ Debug.Log( "die" ) ;}
			
			//return vec.clone().multiplyScalar( size ).add( pt );
			return (vec * size) + ( pt );
			
		}

		Vector3 getBevelVec(Vector3 inPt, Vector3 inPrev, Vector3 inNext ) {

			float EPSILON = THREE.Setting.EPSILON;

			// computes for inPt the corresponding point inPt' on a new contour
			//   shiftet by 1 unit (length of normalized vector) to the left
			// if we walk along contour clockwise, this new contour is outside the old one
			//
			// inPt' is the intersection of the two lines parallel to the two
			//  adjacent edges of inPt at a distance of 1 unit on the left side.
			
			float v_trans_x, v_trans_y, shrink_by = 1;		// resulting translation vector for inPt
			
			// good reading for geometry algorithms (here: line-line intersection)
			// http://geomalgorithms.com/a05-_intersect-1.html
			
			float v_prev_x = inPt.x - inPrev.x;
			float v_prev_y = inPt.y - inPrev.y;
			float v_next_x = inNext.x - inPt.x;
			float v_next_y = inNext.y - inPt.y;
			
			float v_prev_lensq = ( v_prev_x * v_prev_x + v_prev_y * v_prev_y );
			float v_nextv_lensq = ( v_next_x * v_next_x + v_next_y * v_next_y );// add inok
			
			// check for colinear edges
			float colinear0 = ( v_prev_x * v_next_y - v_prev_y * v_next_x );

			if ( Mathf.Abs( colinear0 ) > EPSILON ) {		// not colinear
				
				// length of vectors for normalizing
				
				float v_prev_len = Mathf.Sqrt( v_prev_lensq );
				float v_next_len = Mathf.Sqrt( v_nextv_lensq );
				
				// shift adjacent points by unit vectors to the left
				
				float ptPrevShift_x = ( inPrev.x - v_prev_y / v_prev_len );
				float ptPrevShift_y = ( inPrev.y + v_prev_x / v_prev_len );
				
				float ptNextShift_x = ( inNext.x - v_next_y / v_next_len );
				float ptNextShift_y = ( inNext.y + v_next_x / v_next_len );
				
				// scaling factor for v_prev to intersection point
				
				float sf = (  ( ptNextShift_x - ptPrevShift_x ) * v_next_y - ( ptNextShift_y - ptPrevShift_y ) * v_next_x    ) / ( v_prev_x * v_next_y - v_prev_y * v_next_x );
				
				// vector from inPt to intersection point
				
				v_trans_x = ( ptPrevShift_x + v_prev_x * sf - inPt.x );
				v_trans_y = ( ptPrevShift_y + v_prev_y * sf - inPt.y );
				
				// Don't normalize!, otherwise sharp corners become ugly
				//  but prevent crazy spikes
				float v_trans_lensq = ( v_trans_x * v_trans_x + v_trans_y * v_trans_y );
				if ( v_trans_lensq <= 2 ) {
					return	new Vector2( v_trans_x, v_trans_y );
				} else {
					shrink_by = Mathf.Sqrt( v_trans_lensq / 2.0f );
				}
				
			} else {		// handle special case of colinear edges

				bool direction_eq = false;		// assumes: opposite
				if ( v_prev_x > EPSILON ) {
					if ( v_next_x > EPSILON ) { direction_eq = true; }
				} else {
					if ( v_prev_x < -EPSILON ) {
						if ( v_next_x < -EPSILON ) { direction_eq = true; }
					} else {
						if ( Mathf.Sign(v_prev_y) == Mathf.Sign(v_next_y) ) { direction_eq = true; }
					}
				}
				
				if ( direction_eq ) {
					// console.log("Warning: lines are a straight sequence");
					v_trans_x = -v_prev_y;
					v_trans_y =  v_prev_x;
					shrink_by = Mathf.Sqrt( v_prev_lensq );
				} else {
					// console.log("Warning: lines are a straight spike");
					v_trans_x = v_prev_x;
					v_trans_y = v_prev_y;
					shrink_by = Mathf.Sqrt( v_prev_lensq / 2.0f );
				}

			}

			float t_x = v_trans_x / shrink_by;
			float t_y = v_trans_y / shrink_by;

			if( float.IsNaN(t_x) ){ t_x = 0; }
			if( float.IsNaN(t_y) ){ t_y = 0; }

			return	new Vector3( t_x, t_y, 0 );
		}


		/////  Internal functions
		
		void buildLidFaces(int vlen, List<List<int>> faces, int flen, int steps, int bevelSegments, bool bevelEnabled, int shapesOffset, Shape shape, WorldUVGenerator uvgen, Option options) {
			
			if ( bevelEnabled ) {
				
				int layer = 0 ; // steps + 1
				int offset = vlen * layer;
				
				// Bottom faces
				
				for (int i = 0; i < flen; i ++ ) {
					
					List<int> face = faces[ i ];
					//f3( face[ 2 ] + offset, face[ 1 ]+ offset, face[ 0 ] + offset, true );
					f3( face[2] + offset, face[1]+ offset, face[0] + offset, shape, true, shapesOffset, uvgen, options);
					
				}
				
				layer = steps + bevelSegments * 2;
				offset = vlen * layer;
				
				// Top faces
				
				for (int i = 0; i < flen; i ++ ) {
					
					List<int> face = faces[ i ];
					//f3( face[ 0 ] + offset, face[ 1 ] + offset, face[ 2 ] + offset, false );
					f3( face[0] + offset, face[1] + offset, face[2] + offset, shape, false, shapesOffset, uvgen, options);
					
				}
				
			} else {
				
				// Bottom faces
				
				for (int i = 0; i < flen; i++ ) {
					
					List<int> face = faces[ i ];
					//f3( face[ 2 ], face[ 1 ], face[ 0 ], true );
					f3( face[2], face[1], face[0], shape, true, shapesOffset, uvgen, options);
					
				}
				
				// Top faces
				
				for (int i = 0; i < flen; i ++ ) {
					
					List<int> face = faces[ i ];
					//f3( face[ 0 ] + vlen * steps, face[ 1 ] + vlen * steps, face[ 2 ] + vlen * steps, false );
					f3( face[0] + vlen * steps, face[1] + vlen * steps, face[2] + vlen * steps, shape, false, shapesOffset, uvgen, options);
					
				}
			}
			
		}
		
		// Create faces for the z-sides of the shape
		
		void buildSideFaces(int vlen, List<Vector3> contour, List<List<Vector3>> holes, int steps, int bevelSegments, int shapesOffset, WorldUVGenerator uvgen, Shape shape, Option options) {
			
			int layeroffset = 0;
			sidewalls(vlen, contour, layeroffset, steps, bevelSegments, shapesOffset, uvgen, shape, options);
			layeroffset += contour.Count;

			// holes sideWall
			for (int h = 0, hl = holes.Count;  h < hl; h ++ ) {
				List<Vector3> ahole = holes[ h ];
				sidewalls(vlen, ahole, layeroffset, steps, bevelSegments, shapesOffset, uvgen, shape, options);
				//, true
				layeroffset += ahole.Count;
			}
			
		}
		
		void sidewalls(int vlen, List<Vector3>  contour, int layeroffset, int steps, int bevelSegments, int shapesOffset, WorldUVGenerator uvgen, Shape shape, Option options) {
			
			int j, k;
			int i = contour.Count;
			
			//while ( --i >= 0 ) {
			while ( i > 0 ) {
				i --;

				j = i;
				k = i - 1;
				if ( k < 0 ) k = contour.Count - 1;
				
				//console.log('b', i,j, i-1, k,vertices.length);
				
				int s = 0, sl = steps + bevelSegments * 2;
				
				for ( s = 0; s < sl; s ++ ) {
					
					int slen1 = vlen * s;
					int slen2 = vlen * ( s + 1 );
					
					int a = layeroffset + j + slen1,
					b = layeroffset + k + slen1,
					c = layeroffset + k + slen2,
					d = layeroffset + j + slen2;
					
					f4( a, b, c, d, shape, contour, s, sl, j, k, shapesOffset, uvgen, options );
					
				}
			}
			
		}
//		
		
		void addVertex(float x, float y, float z ) {

			Vector3 vertex = new Vector3( x, y, z );
			this.vertices.Add( vertex );
		}
		
		void f3(int a, int b, int c, Shape shape, bool isBottom, int shapesOffset, WorldUVGenerator uvgen, Option options) {
			
			a += shapesOffset;
			b += shapesOffset;
			c += shapesOffset;
			
			// normal, color, material
			this.faces.Add( new Face3( a, b, c ) );

			List<Vector2> uvs = isBottom ? uvgen.generateBottomUV( this, shape, a, b, c ) : uvgen.generateTopUV( this, shape, a, b, c );
			this.faceVertexUvs.Add( new List<Vector2>( uvs ));

			//this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{ Vector2.one, Vector2.one, Vector2.one } )); // Debug
			
		}
		
		void f4(int a, int b, int c, int d, Shape shape, List<Vector3> wallContour, int stepIndex, int stepsLength, int contourIndex1, int contourIndex2, int shapesOffset , WorldUVGenerator uvgen, Option options) {
			
			a += shapesOffset;
			b += shapesOffset;
			c += shapesOffset;
			d += shapesOffset;
			
			this.faces.Add( new Face3( a, b, d ) );
			this.faces.Add( new Face3( b, c, d ) );
			
			List<Vector2> uvs = uvgen.generateSideWallUV( this, shape, wallContour, a, b, c, d,
                                   stepIndex, stepsLength, contourIndex1, contourIndex2 );
			
			this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{ uvs[ 0 ], uvs[ 1 ], uvs[ 3 ] } ));
			this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{ uvs[ 1 ], uvs[ 2 ], uvs[ 3 ] } ));

//			this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{ Vector2.one, Vector2.one, Vector2.one, Vector2.one } )); // Debug
//			this.faceVertexUvs.Add( new List<Vector2>( new Vector2[]{ Vector2.one, Vector2.one, Vector2.one, Vector2.one } )); // Debug
		}
		
		//
		public class WorldUVGenerator
		{

			public List<Vector2> generateTopUV(THREE.Geometry geometry, Shape extrudedShape, int indexA,  int indexB,  int indexC )
			{
				float ax = geometry.vertices[ indexA ].x;
				float ay = geometry.vertices[ indexA ].y;
				
				float bx = geometry.vertices[ indexB ].x;
				float by = geometry.vertices[ indexB ].y;
				
				float cx = geometry.vertices[ indexC ].x;
				float cy = geometry.vertices[ indexC ].y;
				
				return new List<Vector2>(new Vector2[]{
				        new Vector2( ax, ay ),
				        new Vector2( bx, by ),
						new Vector2( cx, cy ) } );
			}

			public List<Vector2> generateBottomUV(THREE.Geometry geometry, Shape extrudedShape, int indexA, int indexB, int indexC )
			{
				return this.generateTopUV( geometry, extrudedShape, indexA, indexB, indexC );
			}

			public List<Vector2> generateSideWallUV(THREE.Geometry geometry, Shape extrudedShape, List<Vector3> wallContour, 
			                                 int indexA, int indexB, int indexC, int indexD, int stepIndex, int stepsLength,
			                                 int contourIndex1, int contourIndex2 ) {
				
				float ax = geometry.vertices[ indexA ].x,
				ay = geometry.vertices[ indexA ].y,
				az = geometry.vertices[ indexA ].z,
				
				bx = geometry.vertices[ indexB ].x,
				by = geometry.vertices[ indexB ].y,
				bz = geometry.vertices[ indexB ].z,
				
				cx = geometry.vertices[ indexC ].x,
				cy = geometry.vertices[ indexC ].y,
				cz = geometry.vertices[ indexC ].z,
				
				dx = geometry.vertices[ indexD ].x,
				dy = geometry.vertices[ indexD ].y,
				dz = geometry.vertices[ indexD ].z;
				
				if ( Mathf.Abs( ay - by ) < 0.01f ) {
					return new List<Vector2>(new Vector2[]{
					        new Vector2( ax, 1 - az ),
					        new Vector2( bx, 1 - bz ),
					        new Vector2( cx, 1 - cz ),
					        new Vector2( dx, 1 - dz )
					});
				} else {
					return new List<Vector2>(new Vector2[]{
					        new Vector2( ay, 1 - az ),
					        new Vector2( by, 1 - bz ),
					        new Vector2( cy, 1 - cz ),
					        new Vector2( dy, 1 - dz )
					});
				}
			}


		}

	}
}
