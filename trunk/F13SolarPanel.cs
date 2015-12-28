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

class F13SolarPanel : Part {
	float panelExtended, lastExtended;
	float panelRotation, lastRotation;
	Transform panel1, panel2, panel3, rotator;
	bool TRACE = true;

	private void trace(string msg) {
		if (TRACE)
			print("F13> " + msg);
	}
	private void debug(string msg) {
		print("F13> " + msg);
	}
	private void makeCollider(Transform t, Transform d) {
		MeshRenderer mr = (MeshRenderer)t.renderer;
		MeshCollider mc = d.gameObject.AddComponent<MeshCollider>();
		mc.sharedMesh = UnityEngine.Object.Instantiate(t.GetComponent<MeshFilter>().mesh) as Mesh;
		mc.convex = true;
		DestroyObject(mr);
	}
	protected override void onPartLoad() {
		trace("onPartLoad()");
		F13Utils.dumpTransformTree(transform);
		F13Utils.fixupColliders(transform);
		panelExtended = 0.0f;
	}
	protected override void onPartAwake() {
		trace("onPartAwake()");
		rotator = transform.Find("model/obj_anchor/obj_rotator");
		panel1 = rotator.Find("obj_panel1");
		panel2 = panel1.Find("obj_panel2");
		panel3 = panel2.Find("obj_panel3");		
	}
	protected override void onPartUpdate() {
		if (Input.GetKey(KeyCode.O)) {
			panelExtended -= 0.01f;
			if (panelExtended < 0.0f)
				panelExtended = 0.0f;
		}
		if (Input.GetKey(KeyCode.P)) {
			panelExtended += 0.01f;
			if (panelExtended > 1.0f)
				panelExtended = 1.0f;
		}
		if (Input.GetKey(KeyCode.V)) {
			panelRotation -= 0.01f;
			if (panelRotation < 0.0f)
				panelRotation = 0.0f;
		}
		if (Input.GetKey(KeyCode.B)) {
			panelRotation += 0.01f;
			if (panelRotation > 1.0f)
				panelRotation = 1.0f;
		}
		if (panelExtended != lastExtended) {
			lastExtended = panelExtended;
			Vector3 v;
			v.x = 0f;
			v.y = 0f;
			v.z = -90.0f * panelExtended;
			panel1.localEulerAngles = v;
			v.z = 180.0f * panelExtended;
			panel2.localEulerAngles = v;
			v.z = -180.0f * panelExtended;
			panel3.localEulerAngles = v;
		}
		if (panelRotation != lastRotation) {
			lastRotation = panelRotation;
			Vector3 v;
			v.x = 180.0f * panelRotation;
			v.y = 0f;
			v.z = 0f;
			rotator.localEulerAngles = v;
		}
	}
	protected override void onFlightStart() {
		trace("onFlightStart()");
	}
	public override void onFlightStateSave(System.Collections.Generic.Dictionary<string,KSPParseable> state)
{
		state.Add("panelExtended", new KSPParseable(panelExtended, KSPParseable.Type.FLOAT));
}
	public override void onFlightStateLoad(System.Collections.Generic.Dictionary<string, KSPParseable> state) {
		if (state.ContainsKey("panelExtended"))
			panelExtended = state["panelExtended"].value_float;
	}
}
