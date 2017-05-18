using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemAlwaysFocusing : MonoBehaviour {

    EventSystem es;

    private void Awake()
    {
        es = GetComponent<EventSystem>();
    }

    void Update () {
        if (es.currentSelectedGameObject == null && !es.alreadySelecting)
            es.SetSelectedGameObject(es.firstSelectedGameObject);
	}
}
