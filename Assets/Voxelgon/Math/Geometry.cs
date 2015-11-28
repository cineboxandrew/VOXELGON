using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Voxelgon;

namespace Voxelgon.Math {
	public static class Geometry {

		//0: points are colinear
		//1: points are wound clockwise
		//2: points are wound counter-clockwise
		public static int WindingOrder(Vector2 p1, Vector2 p2, Vector2 p3) {
			float val = (p2.y - p1.y) * (p3.x - p2.x) - (p2.x - p1.x) * (p3.y - p2.y); //evil vector dot product hacking

			if (Mathf.Approximately(val, 0.0f)) {
				return 1;
			}

			return (val > 0) ? 1:2;
		}

		//merges a list of meshes into a single mesh (limited at 65534 vertices)
		public static Mesh MergeMeshes(List<Mesh> meshes) {
			Mesh compoundMesh = new Mesh();

			List<Vector3> compoundVertices = new List<Vector3>();
			List<Vector3> compoundNormals = new List<Vector3>();
			List<Color> compoundColors = new List<Color>();
			List<int> compoundTriangles = new List<int>();
			
			foreach (Mesh m in meshes) {
				if (compoundVertices.Count + m.vertices.Length > 65534) {
					break;
				}
				compoundVertices.AddRange(m.vertices);
				compoundNormals.AddRange(m.normals);
				compoundColors.AddRange(m.colors);
				compoundTriangles.AddRange(m.triangles);
				
				if (compoundTriangles.Count - m.triangles.Length != 0) {
					for (int t = 1; t <= m.triangles.Length; t++) {
						compoundTriangles[compoundTriangles.Count - t] += compoundVertices.Count - m.vertices.Length;
					}
				}
			}

			compoundMesh.SetVertices(compoundVertices);
			compoundMesh.SetNormals(compoundNormals);
			compoundMesh.SetColors(compoundColors);
			compoundMesh.SetTriangles(compoundTriangles, 0);
			compoundMesh.RecalculateBounds();
			compoundMesh.RecalculateNormals();
			
			return compoundMesh;
		}

		//public static List<Mesh> MergeMeshesWithOverflow(List<Mesh> meshes, int vertexMax) {



	}
}