/**
 * @author jonobr1 / http://jonobr1.com
 *
 * Creates a one-sided polygonal geometry from a path shape. Similar to
 * ExtrudeGeometry.
 *
 * parameters = {
 *
 *	curveSegments: <int>, // number of points on the curves. NOT USED AT THE MOMENT.
 *
 *	material: <int> // material index for front and back faces
 *	uvGenerator: <Object> // object that provides UV generator functions
 *
 * }
 **/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace THREE
{
	public class ShapeGeometry : Geometry {

		public class Option{
			public ExtrudeGeometry.IUVGenerator uvGenerator;
		}
		
		public BoundingBox shapebb;

		public ShapeGeometry(List<Shape> shapes, Option options ){

			this.shapebb = shapes[ shapes.Count - 1 ].getBoundingBox();
			this.addShapeList( shapes, options );
            //this.SetFaceNormals(); // TODO: Check 不要？　または Geometryの設定で行う様にする。
        }

		public ShapeGeometry(Shape shape, Option options ){
			this.shapebb = shape.getBoundingBox();
			this.addShape( shape, options );

            //this.SetFaceNormals(); // TODO: Check 不要？　または Geometryの設定で行う様にする。
        }
        public ShapeGeometry(Shape shape) {
            this.shapebb = shape.getBoundingBox();
            this.addShape(shape, new Option());
        }

        /**
		 * Add an array of shapes to THREE.ShapeGeometry.
		 */
        ShapeGeometry addShapeList(List<Shape> shapes, Option options ) {
			for ( int i = 0, l = shapes.Count; i < l; i++ ) {
				this.addShape( shapes[ i ], options );
			}
			return this;
		}

		/**
		 * Adds a shape to THREE.ShapeGeometry, based on THREE.ExtrudeGeometry.
		 */
		void addShape(Shape shape, Option options ) {

            int curveSegments = shape.curveSegments;
			
			ExtrudeGeometry.IUVGenerator uvgen = options.uvGenerator;
			if(uvgen == null){
				uvgen = new ExtrudeGeometry.WorldUVGenerator();
			}

			//BoundingBox shapebb = this.shapebb;
			int i, l;

            int shapesOffset = this.vertices.Count;
			shape.extractPoints();
			
			List<Vector3> shapeVertices = shape.shapeVertices;
			List<List<Vector3>> holes = shape.holesList;
			
			bool reverse = !Shape.UtilsShape.isClockWise( shapeVertices );
			
			if ( reverse ) {
				//vertices = vertices.reverse();
				shapeVertices.Reverse();
				
				// Maybe we should also check if holes are in the opposite direction, just to be safe...
				for ( i = 0, l = holes.Count; i < l; i++ ) {
					
					List<Vector3> hole = holes[ i ];
					
					if ( Shape.UtilsShape.isClockWise( hole ) ) {
						//holes[ i ] = hole.reverse();
						hole.Reverse();
						holes[ i ] = hole;
					}
				}
				reverse = false;
			}
			
			List<List<int>> faces = Shape.UtilsShape.triangulateShape( shapeVertices, holes );

			// Vertices
			//var contour = vertices;
			for ( i = 0, l = holes.Count; i < l; i++ ) {
				List<Vector3> hole = holes[ i ];
				//vertices = vertices.concat( hole );
				shapeVertices.AddRange(hole);
			}

			Vector3 vert;
			int vlen = shapeVertices.Count;
			List<int> face;
			int flen = faces.Count;

			//var cont;
			//int clen = contour.Count;
			for ( i = 0; i < vlen; i++ ) {
				vert = shapeVertices[ i ];
				this.vertices.Add( new Vector3( vert.x, vert.y, 0 ) );
			}

			for ( i = 0; i < flen; i++ ) {
				face = faces[ i ];

				int a = face[0] + shapesOffset;
				int b = face[1] + shapesOffset;
				int c = face[2] + shapesOffset;

				Face3 f = new Face3( a, b, c );
				f.uvs = uvgen.generateBottomUV( this, shape, a, b, c );
				this.faces.Add(f);
			}
		}
	}
}