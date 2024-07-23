using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectibleAnim : MonoBehaviour
{
    // Duration of the animation
    public float duration = 2.0f;
    // Target position to move to
    public Vector3 targetPosition = new Vector3(5.0f, 0, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        // Animate the card to move to the target position
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear);
    }
}
