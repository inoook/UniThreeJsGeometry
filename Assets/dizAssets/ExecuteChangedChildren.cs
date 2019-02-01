using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[ExecuteInEditMode]
public class ExecuteChangedChildren : MonoBehaviour {

	bool HasChangedChildren() {
		foreach(Transform t in this.transform) {
            if (t.hasChanged)
            {
                t.hasChanged = false;
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        bool changed = HasChangedChildren();
        if (changed) {
            ExecuteEvents.Execute<IRecieveMessage>(
              target: this.gameObject,
              eventData: null,
              functor: (reciever, eventData) =>
              {
                  reciever.OnValidate();
              }
            );
        }
    }
}
