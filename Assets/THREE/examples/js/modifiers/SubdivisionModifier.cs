using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	@author zz85 / http://twitter.com/blurspline / http://www.lab4games.net/zz85/blog 
 *
 *	Subdivision Geometry Modifier 
 *		using Loop Subdivision Scheme
 *
 *	References:
 *		http://graphics.stanford.edu/~mdfisher/subdivision.html
 *		http://www.holmes3d.net/graphics/subdivision/
 *		http://www.cs.rutgers.edu/~decarlo/readings/subdiv-sg00c.pdf
 *
 *	Known Issues:
 *		- currently doesn't handle UVs
 *		- currently doesn't handle "Sharp Edges"
 *
 */

namespace THREE
{
	public class SubdivisionModifier {

		class Edge{
	//		a: vertexA, // pointer reference
	//		b: vertexB,
	//		newEdge: null,
	//			// aIndex: a, // numbered reference
	//			// bIndex: b,
	//		faces: [] // pointers to face

			public Vector3 a;
			public Vector3 b;
			public int newEdge;
			public List<Face3> faces = new List<Face3>();

			public string key;
		}

		class MetaVertex{
			public List<Edge> edges = new List<Edge>();
		}


		int subdivisions;
		public SubdivisionModifier(int subdivisions = 1)
		{
			this.subdivisions = subdivisions;
		}

		public void modify(Geometry geometry)
		{
			int repeats = this.subdivisions;
			
			while ( repeats-- > 0 ) {
				this.smooth( geometry );
			}

			Debug.Log("TODO: UVS");
			
            // smoothing
            geometry.SetFaceSmooth();
        }

		// ---------------------------------

		// Some constants
		bool WARNINGS = !true; // Set to true for development
//		string[] ABC = new string[]{ "a", "b", "c" };

		Edge getEdge(int a, int b, Dictionary<string, Edge> map ) {
			
			int vertexIndexA = Mathf.Min( a, b );
			int vertexIndexB = Mathf.Max( a, b );
			
			string key = vertexIndexA + "_" + vertexIndexB;
			
			return map[ key ];
			
		}

		void processEdge(int a, int b, List<Vector3> vertices, Dictionary<string, Edge> map, Face3 face, MetaVertex[] metaVertices ) {

			int vertexIndexA = Mathf.Min( a, b );
			int vertexIndexB = Mathf.Max( a, b );
			
			string key = vertexIndexA + "_" + vertexIndexB;
			
			Edge edge;

			//if ( key in map ) {
			if ( map.ContainsKey(key) ) {
				edge = map[ key ];
			} else {
				
				Vector3 vertexA = vertices[ vertexIndexA ];
				Vector3 vertexB = vertices[ vertexIndexB ];

				edge = new Edge();
				edge.a = vertexA;
				edge.b = vertexB;
				edge.newEdge = -1;
				edge.faces = new List<Face3>();

				edge.key = key;
				
				map[ key ] = edge;
				
			}
			
			edge.faces.Add( face );
			
			metaVertices[ a ].edges.Add( edge );
			metaVertices[ b ].edges.Add( edge );

		}

		//void generateLookups(List<Vector3> vertices, List<Face3> faces, List<MetaVertex> metaVertices, List<Edge> edges ) {
		void generateLookups(List<Vector3> vertices, List<Face3> faces, MetaVertex[] metaVertices, Dictionary<string, Edge>  edges ) {
			Face3 face;
			
			for (int i = 0, il = vertices.Count; i < il; i++ ) {
				//metaVertices[ i ] = { edges: [] };
				metaVertices[ i ] = new MetaVertex();
			}
			
			for (int i = 0, il = faces.Count; i < il; i++ ) {
				face = faces[ i ];

				processEdge( face.a, face.b, vertices, edges, face, metaVertices );
				processEdge( face.b, face.c, vertices, edges, face, metaVertices );
				processEdge( face.c, face.a, vertices, edges, face, metaVertices );
				
			}
		}

		void newFace(List<Face3> newFaces, int a, int b, int c ) {
			newFaces.Add( new Face3( a, b, c ) );
		}


		/////////////////////////////
		
		// Performs one iteration of Subdivision
		void smooth(Geometry geometry ) {

			Vector3 tmp = new Vector3();

			List<Vector3> oldVertices;
			List<Face3> oldFaces;
			List<Vector3> newVertices;
			List<Face3> newFaces; // newUVs = [];
			
			int n;
			//List<Edge> sourceEdges;
			Dictionary<string, Edge> sourceEdges;
			//var metaVertices, sourceEdges;
			
			// new stuff.
			//var newSourceVertices;
			
			oldVertices = geometry.vertices; // { x, y, z}
			oldFaces = geometry.faces; // { a: oldVertex1, b: oldVertex2, c: oldVertex3 }
			
			/******************************************************
			 *
			 * Step 0: Preprocess Geometry to Generate edges Lookup
			 *
			 *******************************************************/

			MetaVertex[] metaVertices = new MetaVertex[ oldVertices.Count ];
			//sourceEdges = {}; // Edge => { oldVertex1, oldVertex2, faces[]  }
			//sourceEdges = new List<Edge>();
			sourceEdges = new Dictionary<string, Edge>();
			
			generateLookups(oldVertices, oldFaces, metaVertices, sourceEdges);
			
			
			/******************************************************
			 *
			 *	Step 1. 
			 *	For each edge, create a new Edge Vertex,
			 *	then position it.
			 *
			 *******************************************************/
			
			List<Vector3> newEdgeVertices = new List<Vector3>();
			Vector3 other = new Vector3();
			//Edge currentEdge;
			Vector3 newEdge;
			Face3 face;
			float edgeVertexWeight;
			float adjacentVertexWeight;
			int connectedFaces;
			
			//for ( i in sourceEdges ) {
			foreach(Edge currentEdge in sourceEdges.Values){
				//currentEdge = sourceEdges[ i ];
				newEdge = new Vector3();
				
				edgeVertexWeight = 3.0f / 8;
				adjacentVertexWeight = 1.0f / 8;
				
				connectedFaces = currentEdge.faces.Count;
				
				// check how many linked faces. 2 should be correct.
				if ( connectedFaces != 2 ) {
					
					// if length is not 2, handle condition
					edgeVertexWeight = 0.5f;
					adjacentVertexWeight = 0;
					
					if ( connectedFaces != 1 ) {
						if (WARNINGS) {
							Debug.Log("Subdivision Modifier: Number of connected faces != 2, is: " + connectedFaces + " / "+ currentEdge);
						}
					}
				}
				
				//newEdge.addVectors( currentEdge.a, currentEdge.b ).multiplyScalar( edgeVertexWeight );
				newEdge = ( currentEdge.a + currentEdge.b ) * ( edgeVertexWeight );
				
				//tmp.set( 0, 0, 0 );
				tmp = new Vector3(0,0,0);
				
				for (int j = 0; j < connectedFaces; j++ ) {
					
					face = currentEdge.faces[ j ];
					
					for (int k = 0; k < 3; k++ ) {
						
						//other = oldVertices[ face[ ABC[k] ] ];
						int index;
						if( k == 0 ){
							index = face.a;
						}else if(k == 1){
							index = face.b;
						}else{
							index = face.c;
						}
						other = oldVertices[ index ];

						if (other != currentEdge.a && other != currentEdge.b ) break;
						
					}
					tmp += ( other );
				}
				
				tmp *= ( adjacentVertexWeight );
				newEdge += ( tmp );
				
				currentEdge.newEdge = newEdgeVertices.Count;
				newEdgeVertices.Add(newEdge);
				// console.log(currentEdge, newEdge);
			}
			
			/******************************************************
			 *
			 *	Step 2. 
			 *	Reposition each source vertices.
			 *
			 *******************************************************/
			float beta = 0.0f;
			float sourceVertexWeight;
			float connectingVertexWeight;
			Edge connectingEdge;
			List<Edge> connectingEdges;
			Vector3 oldVertex;
			Vector3 newSourceVertex;
			List<Vector3> newSourceVertices = new List<Vector3>();
			
			for(int i = 0, il = oldVertices.Count; i < il; i++ ) {
				
				oldVertex = oldVertices[ i ];
				
				// find all connecting edges (using lookupTable)
				connectingEdges = metaVertices[ i ].edges;
				n = connectingEdges.Count;
				//beta;
				
				if ( n == 3 ) {
					
					beta = 3.0f / 16;
					
				} else if ( n > 3 ) {
					
					beta = 3.0f / ( 8 * n ); // Warren's modified formula
					
				}
				
				// Loop's original beta formula
				// beta = 1 / n * ( 5/8 - Math.pow( 3/8 + 1/4 * Math.cos( 2 * Math. PI / n ), 2) );
				
				sourceVertexWeight = 1 - n * beta;
				connectingVertexWeight = beta;
				
				if ( n <= 2 ) {
					
					// crease and boundary rules
					// console.warn('crease and boundary rules');
					
					if ( n == 2 ) {
						
						if (WARNINGS) { Debug.LogWarning("2 connecting edges" + connectingEdges); }
						sourceVertexWeight = 3.0f / 4;
						connectingVertexWeight = 1.0f / 8;
						
						// sourceVertexWeight = 1;
						// connectingVertexWeight = 0;
						
					} else if ( n == 1 ) {
						
						if (WARNINGS) { Debug.LogWarning("only 1 connecting edge"); }
						
					} else if ( n == 0 ) {
						
						if (WARNINGS) { Debug.LogWarning("0 connecting edges"); }
						
					}
					
				}
				
				//newSourceVertex = oldVertex.clone().multiplyScalar( sourceVertexWeight );
				newSourceVertex = oldVertex * ( sourceVertexWeight );
				
				tmp = Vector3.zero;
				
				for (int j=0; j < n; j++ ) {
					
					connectingEdge = connectingEdges[ j ];
					other = connectingEdge.a != oldVertex ? connectingEdge.a : connectingEdge.b;
					tmp += ( other );
					
				}
				
				tmp *= ( connectingVertexWeight );
				newSourceVertex += ( tmp );
				
				newSourceVertices.Add( newSourceVertex );
				
			}
			
			
			/******************************************************
			 *
			 *	Step 3. 
			 *	Generate Faces between source vertecies
			 *	and edge vertices.
			 *
			 *******************************************************/
			
			//newVertices = newSourceVertices.concat( newEdgeVertices );
			Debug.LogWarning("TODO: CHECK");
			//newVertices = newSourceVertices;
			newVertices = new List<Vector3>();
			newVertices.AddRange(newSourceVertices);
			newVertices.AddRange(newEdgeVertices);

			int sl = newSourceVertices.Count;
			int edge1, edge2, edge3;
			newFaces = new List<Face3>();

			for (int i = 0, il = oldFaces.Count; i < il; i++ ) {
				
				face = oldFaces[ i ];
				
				// find the 3 new edges vertex of each old face
				
				edge1 = getEdge( face.a, face.b, sourceEdges ).newEdge + sl;
				edge2 = getEdge( face.b, face.c, sourceEdges ).newEdge + sl;
				edge3 = getEdge( face.c, face.a, sourceEdges ).newEdge + sl;
				
				// create 4 faces.
				
				newFace( newFaces, edge1, edge2, edge3 );
				newFace( newFaces, face.a, edge1, edge3 );
				newFace( newFaces, face.b, edge2, edge1 );
				newFace( newFaces, face.c, edge3, edge2 );
				
			}
			
			// Overwrite old arrays
			geometry.vertices = newVertices;
			geometry.faces = newFaces;
			
			// console.log('done');
			
		}

	}
}
