﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Voxelgon;
using Voxelgon.EventSystems;
using Voxelgon.ShipEditor;

namespace Voxelgon.ShipEditor {

public class ShipEditor : MonoBehaviour, IModeChangeHandler {

		//Fields

		private Wall tempWall;
		private List<Wall> walls;

		private Dictionary<Position, List<Wall>> wallVertices;

		private List<Vector3> nodes = new List<Vector3>();
		private bool nodesChanged = false;

		private BuildMode mode = BuildMode.polygon;

		//Properties

		public Wall TempWall {
			get { return tempWall; }
		}

		public List<Wall> Walls {
			get { return walls; }
		}

		public BuildMode Mode {
			get { return mode; }
			set { mode = value; }
		}

		//Enums

		public enum BuildMode {
			polygon,
			rectangle
		}

		//Methods

		public void OnModeChange (ModeChangeEventData eventData) {
		}

		public void Start() {
			tempWall = new Wall();
			walls = new List<Wall>();
			wallVertices = new Dictionary<Position, List<Wall>>();
		}

		public void Update() {
			if (Input.GetButtonDown("ChangeFloor")) {
				transform.Translate(Vector3.up * 2 * (int) Input.GetAxis("ChangeFloor"));
			}
		}


		private bool NodesChanged() {
			if (!nodesChanged) return false;
			return true;
		}

		private void UpdateSimpleMesh() {
			int vertCount = 0;
			int triCount = 0;
			foreach (Wall w in walls) {
				vertCount += w.VertexCount;
				triCount += w.VertexCount - 2;
			}

			Vector3[] meshVerts = new Vector3[vertCount];
			int[] meshTris = new int[triCount];


		}

		public bool AddNode(Vector3 node) {
			if (ValidNode(node)) {
				nodes.Add(node);
				nodesChanged = true;
				return true;
			}
			return false;
		}

		public bool RemoveNode(Vector3 node) {
			if (nodes.Contains(node)) {
				nodes.Remove(node);
				nodesChanged = true;
				return true;
			}
			return false;
		}

		public void AddWall(Wall wall) {
			foreach (Vector3 v in wall.Vertices) {
				Position p = (Position) v;
				if (!wallVertices.ContainsKey(p)) {
					wallVertices.Add(p, new List<Wall>());
				}
				wallVertices[p].Add(wall);
			}
			walls.Add(wall);
		}

		public void RemoveWall(Wall wall) {
			foreach (Vector3 v in wall.Vertices) {
				Position p = (Position) v;
				if (wallVertices.ContainsKey(p)) {
					wallVertices[p].Remove(wall);

					if (wallVertices[p].Count == 0) {
						wallVertices.Remove(p);
					}
				}
			}
			walls.Remove(wall);
		}

		public List<Wall> GetWallNeighbors(Wall wall) {
			List<Wall> lastList = wallVertices[(Position) wall.Vertices[wall.VertexCount - 1]];
			List<Wall> neighbors = new List<Wall>();

			foreach (Vector3 v in wall.Vertices) {
				Position p = (Position) v;
				foreach(Wall w in wallVertices[p]) {
					if (lastList.Contains(w)) {
						neighbors.Add(w);
					}
					lastList = wallVertices[p];
				}
			}
			return neighbors;
		}

		public bool UpdateTempWall() {
			if (nodesChanged && tempWall.UpdateVertices(nodes, mode)) {
				nodesChanged = false;
				return true;
			}
			return false;
		}

		public bool ValidNode(Vector3 node) {
			return (tempWall.ValidVertex(node) && !ContainsNode(node));
		}

		public bool ContainsNode(Vector3 node) {
			return nodes.Contains(node);
		}

		public static Vector3 GetEditCursorPos(float y) {
			Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			float xySlope = cursorRay.direction.y / cursorRay.direction.x;
			float zySlope = cursorRay.direction.y / cursorRay.direction.z;

			float deltaY = cursorRay.origin.y - y;

			float xIntercept = cursorRay.origin.x + deltaY / -xySlope;
			float zIntercept = cursorRay.origin.z + deltaY / -zySlope;

			Vector3 interceptPoint = new Vector3(xIntercept, y, zIntercept);

			return interceptPoint; 
		}

		public static Vector3 GetEditCursorPos() {
			return GetEditCursorPos(0);
		}

	}
}