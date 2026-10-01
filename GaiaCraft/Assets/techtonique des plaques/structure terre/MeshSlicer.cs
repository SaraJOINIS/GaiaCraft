using System;
using System.Collections.Generic;
using UnityEngine;

public struct VertexData
{
    public Vector3 Position;
    public Vector2 Uv;
    public Vector3 Normal;
    public bool Side;
}

public class MeshSlicer {

    class MeshContructionHelper
    {
        //All the properites we need to contruct a mesh in the end
        private List<Vector3> _vertices;
        private List<int> _triangles;
        private List<Vector2> _uvs;
        private List<Vector3> _normals;

        public MeshContructionHelper()
        {
            _triangles = new List<int>();
            _vertices = new List<Vector3>();
            _uvs = new List<Vector2>();
            _normals = new List<Vector3>();
        }

        //This function will actually produce the mesh
        //How exiting!
        public Mesh ConstructMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = _vertices.ToArray();
            mesh.triangles = _triangles.ToArray();
            mesh.normals = _normals.ToArray();
            mesh.uv = _uvs.ToArray();
            return mesh;
        }

        public void AddMeshSection(VertexData vertexA, VertexData vertexB, VertexData vertexC)
        {
            int indexA = TryAddVertex(vertexA);
            int indexB = TryAddVertex(vertexB);
            int indexC = TryAddVertex(vertexC);

            AddTriangle(indexA, indexB, indexC);
        }


        private void AddTriangle(int indexA, int indexB, int indexC)
        {
            _triangles.Add(indexA);
            _triangles.Add(indexB);
            _triangles.Add(indexC);
        }

        private int TryAddVertex(VertexData vertex)
        {
            _vertices.Add(vertex.Position);
            _uvs.Add(vertex.Uv);
            _normals.Add(vertex.Normal);
            return _vertices.Count - 1;
        }
    }

    private static VertexData GetVertexData(Mesh mesh, Plane plane, int index)
    {
        Vector3 position = mesh.vertices[index];
        VertexData vertexData = new VertexData()
        {
            Position = position,
            Side = plane.GetSide(position),
            Uv = mesh.uv[index],
            Normal = mesh.normals[index]
        };
        return vertexData;
    }

    public static Vector3 ComputeNormal(VertexData vertexA, VertexData vertexB, VertexData vertexC)
    {
        Vector3 sideL = vertexB.Position - vertexA.Position;
        Vector3 sideR = vertexC.Position - vertexA.Position;

        Vector3 normal = Vector3.Cross(sideL, sideR);

        return normal.normalized;
    }

    public static Vector3 GetHalfwayPoint(List<VertexData> pointsAlongPlane)
    {
        if (pointsAlongPlane.Count > 0)
        {
            Vector3 firstPoint = pointsAlongPlane[0].Position;
            Vector3 furthestPoint = Vector3.zero;
            float distance = 0f;

            for (int index = 0; index < pointsAlongPlane.Count; index++)
            {
                Vector3 point = pointsAlongPlane[index].Position;
                float currentDistance = 0f;
                currentDistance = Vector3.Distance(firstPoint, point);

                if (currentDistance > distance)
                {
                    distance = currentDistance;
                    furthestPoint = point;
                }
            }

            return Vector3.Lerp(firstPoint, furthestPoint, 0.5f);
        }
        else
        {
            return Vector3.zero;
        }
    }

    public static Mesh[] SliceMesh(Mesh mesh, Vector3 normal, Vector3 origin)
    {

        // draw normal
        Debug.DrawRay(origin, normal, Color.yellow, 10);

        UnityEngine.Plane plane = new UnityEngine.Plane(normal, origin);
        MeshContructionHelper positiveMesh = new MeshContructionHelper();
        MeshContructionHelper negativeMesh = new MeshContructionHelper();

        int[] meshTriangles = mesh.triangles;
        List<VertexData> pointsAlongPlane = new List<VertexData>();
        for (int i = 0; i < meshTriangles.Length; i += 3)
        {
            VertexData vertexA = GetVertexData(mesh, plane, meshTriangles[i]);
            VertexData vertexB = GetVertexData(mesh, plane, meshTriangles[i + 1]);
            VertexData vertexC = GetVertexData(mesh, plane, meshTriangles[i + 2]);

            bool isABSameSide = vertexA.Side == vertexB.Side;
            bool isBCSameSide = vertexB.Side == vertexC.Side;

            if (isABSameSide && isBCSameSide)
            {
                MeshContructionHelper helper = vertexA.Side ? positiveMesh : negativeMesh;
                helper.AddMeshSection(vertexA, vertexB, vertexC);
            }
            else
            {
                //else we have to find intersection between triangle and cutting plane 
                VertexData intersectionD;
                VertexData intersectionE;
                //Determine appropirate helper for each triangle corner
                MeshContructionHelper helperA = vertexA.Side ? positiveMesh : negativeMesh;
                MeshContructionHelper helperB = vertexB.Side ? positiveMesh : negativeMesh;
                MeshContructionHelper helperC = vertexC.Side ? positiveMesh : negativeMesh;

                if (isABSameSide)
                {
                    intersectionD = GetIntersectionVertex(vertexA, vertexC, normal, origin);
                    intersectionE = GetIntersectionVertex(vertexB, vertexC, normal, origin);

                    helperA.AddMeshSection(vertexA, vertexB, intersectionE);
                    helperA.AddMeshSection(vertexA, intersectionE, intersectionD);
                    helperC.AddMeshSection(intersectionE, vertexC, intersectionD);
                }
                else if (isBCSameSide)
                {
                    intersectionD = GetIntersectionVertex(vertexB, vertexA, normal, origin);
                    intersectionE = GetIntersectionVertex(vertexC, vertexA, normal, origin);

                    helperB.AddMeshSection(vertexB, vertexC, intersectionE);
                    helperB.AddMeshSection(vertexB, intersectionE, intersectionD);
                    helperA.AddMeshSection(intersectionE, vertexA, intersectionD);
                }
                else
                {
                    intersectionD = GetIntersectionVertex(vertexA, vertexB, normal, origin);
                    intersectionE = GetIntersectionVertex(vertexC, vertexB, normal, origin);

                    helperA.AddMeshSection(vertexA, intersectionE, vertexC);
                    helperA.AddMeshSection(intersectionD, intersectionE, vertexA);
                    helperB.AddMeshSection(vertexB, intersectionE, intersectionD);
                }

                pointsAlongPlane.Add(intersectionD);
                pointsAlongPlane.Add(intersectionE);
            }
        }
        JoinPointsAlongPlane(ref positiveMesh, ref negativeMesh, normal, pointsAlongPlane);

        return new[] { positiveMesh.ConstructMesh(), negativeMesh.ConstructMesh() };
    }

    private static void JoinPointsAlongPlane(ref MeshContructionHelper positive, ref MeshContructionHelper negative, Vector3 cutNormal, List<VertexData> pointsAlongPlane)
    {
        VertexData halfway = new VertexData()
        {
            Position = GetHalfwayPoint(pointsAlongPlane)
        };

        for (int i = 0; i < pointsAlongPlane.Count; i += 2)
        {
            VertexData firstVertex = pointsAlongPlane[i];
            VertexData secondVertex = pointsAlongPlane[i + 1];

            Vector3 normal = ComputeNormal(halfway, secondVertex, firstVertex);
            halfway.Normal = Vector3.forward;

            float dot = Vector3.Dot(normal, cutNormal);
            //we check which side of our plane the calculated normal is
            //and we add new triangle to both construction helpers 
            //in different order do that they face diffewrent directions

            if (dot > 0)
            {
                //used if calculated normal aligns with plane normal                           
                positive.AddMeshSection(firstVertex, secondVertex, halfway);
                negative.AddMeshSection(secondVertex, firstVertex, halfway);
            }
            else
            {
                //used if calculated normal is opposite to plane normal
                negative.AddMeshSection(firstVertex, secondVertex, halfway);
                positive.AddMeshSection(secondVertex, firstVertex, halfway);
            }
        }
    }

    // function that will return intersection position if it exists
    public static bool PointIntersectsAPlane(Vector3 from, Vector3 to, Vector3 planeOrigin, Vector3 normal, out Vector3 result)
    {
        Vector3 translation = to - from;
        float dot = Vector3.Dot(normal, translation);
        if (Mathf.Abs(dot) > Single.Epsilon)
        {
            Vector3 fromOrigin = from - planeOrigin;
            float fac = -Vector3.Dot(normal, fromOrigin) / dot;
            translation = translation * fac;
            result = from + translation;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public static Vector2 InterpolateUvs(Vector2 uv1, Vector2 uv2, float distance)
    {
        Vector2 uv = Vector2.Lerp(uv1, uv2, distance);
        return uv;
    }

    private static VertexData GetIntersectionVertex(VertexData vertexA, VertexData vertexB, Vector3 planeNormal, Vector3 planeOrigin)
    {
        Vector3 result;
        PointIntersectsAPlane(vertexA.Position, vertexB.Position, planeOrigin , planeNormal, out result);
        float distanceA = Vector3.Distance(vertexA.Position, result);
        float distanceB = Vector3.Distance(vertexB.Position, result);
        float t = distanceA / (distanceA + distanceB);

        return new VertexData()
        {
            Position = result,
            Normal = planeNormal,
            Uv = InterpolateUvs(vertexA.Uv, vertexB.Uv, t)
        };
    }


}
