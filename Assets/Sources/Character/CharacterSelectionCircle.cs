using UnityEngine;

public class CharacterSelectionCircle : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer circle;
    [SerializeField]
    private float offset = 0.2f;
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private float maxScale = 2f;
    [SerializeField]
    private float minScale = 0.6f;

    private RaycastHit hit;

	void Update () {
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 100f, groundLayerMask.value))
        {
            if(hit.point != null)
            {
                circle.transform.position = hit.point + Vector3.up * offset;
                circle.transform.localScale = 1f / Mathf.Clamp(hit.distance / 2f, minScale, maxScale) * Vector3.one;
            }
        }
	}

    public void SetColor(Color color)
    {
        circle.color = color;
    }
}
