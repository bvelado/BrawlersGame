using UnityEngine;
using DG.Tweening;

public class CameraTraveller : MonoBehaviour {

    [SerializeField]
    private float travelDuration;

    [SerializeField]
    private Transform[] travelPositions;

    private Sequence currentSequence = null;

    public void GoToTravelPosition(int index)
    {
        if (currentSequence != null)
        {
            currentSequence.Kill();
            currentSequence = null;
        }

        currentSequence = DOTween.Sequence();

        currentSequence
            .Append(transform.DOMove(travelPositions[index].position, travelDuration))
            .Insert(0f, transform.DORotate(travelPositions[index].eulerAngles, travelDuration));

        currentSequence.Play();
    }

}
