// Copyright 2012 F13 Aerospace Development Team
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using UnityEngine;

class F13Utils {
	static public void debug(string msg) {
		Debug.Log("F13> " + msg);
	}
	static public void dumpTransformTree(Transform t) {
		dumpTransformTree(t, "/");
	}
	static public void dumpTransformTree(Transform t, string prefix) {
		prefix = prefix + "/" + t.name;
		debug(prefix);
		string info = "  [";
		MeshFilter mf = t.GetComponent<MeshFilter>();
		if (mf)
			info += " MeshFilter";
		if (t.collider)
			info += " Collider";
		if (t.renderer)
			info += " Renderer";
		if (t.rigidbody)
			info += " RigidBody";
		info += " ]";
		debug(info);
		foreach (Transform c in t)
			dumpTransformTree(c, prefix);
	}

	/* KSP only processes one mesh ("node_collider") as a collider
	 * 
	 * For additional colliders (recognized by "node_collider" in their name
	 * we need to grab the mesh from the Renderer, create a Collider,
	 * and delete the Renderer.
	 */
	static public void fixupColliders(Transform t) {
		if (t.name.StartsWith("boundbox")) {
			debug("fixing up '"+t.name+"'");
			MeshRenderer mr = (MeshRenderer)t.renderer;
			MeshCollider mc = t.gameObject.AddComponent<MeshCollider>();
			mc.sharedMesh = UnityEngine.Object.Instantiate(t.GetComponent<MeshFilter>().mesh) as Mesh;
			mc.convex = true;
			UnityEngine.Object.DestroyObject(mr);
		}
		foreach (Transform c in t)
			fixupColliders(c);
	}
}
