using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using THREE;

// inner class
// https://github.com/mrdoob/three.js/blob/34dc2478c684066257e4e39351731a93c6107ef5/src/extras/core/Curve.js
public class FrenetFrames
{
    public Vector3[] tangents;
    public Vector3[] normals;
    public Vector3[] binormals;

    // For computing of Frenet frames, exposing the tangents, normals and binormals the spline
    public FrenetFrames(Curve path, int segments, bool closed)
    {
        //Vector3 tangent = new Vector3 ();
        Vector3 normal = new Vector3();
        //Vector3 binormal = new Vector3 ();

        Vector3 vec = new Vector3();
        //Matrix4x4 mat = new Matrix4x4 ();

        int numpoints = segments + 1;
        float theta;
        float epsilon = THREE.Setting.EPSILON_S;
        float smallest;

        float tx, ty, tz;
        float u;
        //float v;

        this.tangents = new Vector3[numpoints];
        this.normals = new Vector3[numpoints];
        this.binormals = new Vector3[numpoints];

        // compute the tangent vectors for each segment on the path
        for (int i = 0; i < numpoints; i++)
        {

            u = (float)i / (numpoints - 1);

            //tangents [i] = path.getTangentAt(u);
            //tangents [i].Normalize ();
            Vector3 t_vec = path.getTangentAt(u);
            t_vec.Normalize();
            //tangents.Add( t_vec );
            tangents[i] = (t_vec);
        }

        //initialNormal3();

        //void initialNormal3() {
        // select an initial normal vector perpenicular to the first tangent vector,
        // and in the direction of the smallest tangent xyz component

        //normals.Add(new Vector3());
        //binormals.Add(new Vector3());

        smallest = Mathf.Infinity;
        tx = Mathf.Abs(tangents[0].x);
        ty = Mathf.Abs(tangents[0].y);
        tz = Mathf.Abs(tangents[0].z);

        if (tx <= smallest)
        {
            smallest = tx;
            normal = new Vector3(1, 0, 0);
        }

        if (ty <= smallest)
        {
            smallest = ty;
            normal = new Vector3(0, 1, 0);
        }

        if (tz <= smallest)
        {
            normal = new Vector3(0, 0, 1);
        }

        vec = Vector3.Cross(tangents[0], normal).normalized;

        normals[0] = (Vector3.Cross(tangents[0], vec));
        binormals[0] = (Vector3.Cross(tangents[0], normals[0]));
        //}

        // compute the slowly-varying normal and binormal vectors for each segment on the path

        // js233
        for (int i = 1; i < numpoints; i++)
        {
            normals[i] = clone(normals[i - 1]);
            binormals[i] = clone(binormals[i - 1]);

            vec = Vector3.Cross(tangents[i - 1], tangents[i]);

            if (vec.magnitude > epsilon)
            {
                vec.Normalize();

                //theta = Mathf.Acos( Mathf.Clamp( tangents[ i-1 ].dot( tangents[ i ] ), -1, 1 ) ); // clamp for floating pt errors
                float dot = Vector3.Dot(tangents[i - 1], tangents[i]);
                theta = Mathf.Acos(Mathf.Clamp(dot, -1, 1)); // clamp for floating pt errors

                //normals[ i ].applyMatrix4( mat.makeRotationAxis( vec, theta ) );
                Quaternion rot = Quaternion.AngleAxis(Mathf.Rad2Deg * (theta), vec);
                normals[i] = rot * normals[i];
                //vecs[i] = vec;
            }
            //binormals[ i ].crossVectors( tangents[ i ], normals[ i ] );
            binormals[i] = Vector3.Cross(tangents[i], normals[i]);
        }

        // if the curve is closed, postprocess the vectors so the first and last normal vectors are the same
        if (closed)
        {
            Debug.LogWarning("close---------");
            float dot = Vector3.Dot(normals[0], normals[numpoints - 1]);
            theta = Mathf.Acos(Mathf.Clamp(dot, -1, 1));
            theta /= (float)(numpoints - 1);

            Vector3 vec0 = Vector3.Cross(normals[0], normals[numpoints - 1]);
            float dot0 = Vector3.Dot(tangents[0], vec0);
            if (dot0 > 0)
            {
                //if ( tangents[ 0 ].dot( vec.crossVectors( normals[ 0 ], normals[ numpoints-1 ] ) ) > 0 ) {
                theta = -theta;
            }

            for (int i = 1; i < numpoints; i++)
            {
                // twist a little...
                //normals[ i ].applyMatrix4( mat.makeRotationAxis( tangents[ i ], theta * i ) );
                //binormals[ i ].crossVectors( tangents[ i ], normals[ i ] );

                Quaternion rot = Quaternion.AngleAxis(Mathf.Rad2Deg * (theta * i), tangents[i]);
                normals[i] = rot * normals[i];
                binormals[i] = Vector3.Cross(tangents[i], normals[i]);
            }
        }
    }

    public Vector3 clone(Vector3 vec)
    {
        return Utils.clone(vec);
    }
}