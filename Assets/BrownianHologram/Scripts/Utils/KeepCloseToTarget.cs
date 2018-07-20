using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepCloseToTarget : MonoBehaviour {

    public Transform tracked;
    public Transform target;
    [Tooltip("Parent of the object in the scene hierarchy, that will be pulled so that the tracked object is close to the target")]
    public Transform root;
    public AnimationCurve pullStrength;
    public float maxDistance = 0.5f;

    void OnEnable() {
        SetupPullStrength();
        EnsureRootIsTrackedParent();
    }

    void EnsureRootIsTrackedParent() {
        var parent = tracked;
        while (parent != null) {
            if (parent == root)
                return;
            parent = parent.parent;
        }
        throw new System.Exception("Target object MUST be a descendant of Root or Root itself");
    }

    void SetupPullStrength() {
        if (pullStrength == null)
            pullStrength = AnimationCurve.EaseInOut(0, 0, 1, 1);

        pullStrength.preWrapMode = WrapMode.Clamp;
        pullStrength.postWrapMode = WrapMode.Clamp;
    }

    void Update() {
        var distance = Vector3.Distance(tracked.position, target.position) / maxDistance;
        var strength = pullStrength.Evaluate(distance);
        if (strength <= 0)
            return;

        var difference = tracked.position - target.position;
        root.position -= Vector3.Lerp(Vector3.zero, difference, Time.deltaTime * strength);
    }
}
