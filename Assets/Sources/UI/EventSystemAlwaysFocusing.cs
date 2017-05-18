using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemAlwaysFocusing : MonoBehaviour {

    EventSystem es;
    private GameObject mainFocus;

    private void Awake()
    {
        es = GetComponent<EventSystem>();
    }

    private void Start()
    {
        mainFocus = es.firstSelectedGameObject;

        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(mainFocus);
        mainFocus.GetComponent<Selectable>().Select();
        //mainFocus.GetComponent<Image>().sprite = mainFocus.GetComponent<Selectable>().spriteState.highlightedSprite;
    }

    void Update () {
        if (es.currentSelectedGameObject == null && !es.alreadySelecting)
        {
            es.SetSelectedGameObject(null);
            es.SetSelectedGameObject(mainFocus);
            mainFocus.GetComponent<Selectable>().Select();
            //mainFocus.GetComponent<Image>().sprite = mainFocus.GetComponent<Selectable>().spriteState.highlightedSprite;
        }
            
	}

    public void SetMainFocus(GameObject focus)
    {
        es.SetSelectedGameObject(null);
        mainFocus = focus;
        es.SetSelectedGameObject(mainFocus);
        mainFocus.GetComponent<Selectable>().Select();
        //mainFocus.GetComponent<Image>().sprite = mainFocus.GetComponent<Selectable>().spriteState.highlightedSprite;
    }

    public void RemoveFocus()
    {
        es.SetSelectedGameObject(null);
    }
}
