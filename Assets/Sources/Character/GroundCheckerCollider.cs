using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GroundCheckerCollider : MonoBehaviour {

    BoxCollider col;
    [SerializeField]
    CharacterPhysics characterPhysics;

    [Header("Ground check")]
    [SerializeField]
    private float groundCheckDistance = 0.4f;
    [SerializeField]
    private LayerMask groundcheckLayerMask;

    private RaycastHit[] hits;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == 8)
    //        characterPhysics.IsGrounded = true;
    //}

    ////private void OnTriggerStay(Collider other)
    ////{
    ////    if (other.gameObject.layer == 8)
    ////        characterPhysics.IsGrounded = true;
    ////}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == 8)
    //        characterPhysics.IsGrounded = false;
    //}

    private void Update()
    {
        RaycastCheck();
    }

    public void RaycastCheck()
    {
        if (Physics.CheckBox(transform.position, col.bounds.extents, transform.rotation, groundcheckLayerMask.value))
        {
            characterPhysics.IsGrounded = true;
        }
        else
        {
            characterPhysics.IsGrounded = false;
        }
    }

    public float GetGroundDistance()
    {
        hits = Physics.BoxCastAll(transform.position, col.bounds.extents, Vector3.down, Quaternion.identity, 100f, groundcheckLayerMask.value);
        if(hits.Length > 0)
        {
            return hits[0].distance;
        }
        return 999f;
    }
}
