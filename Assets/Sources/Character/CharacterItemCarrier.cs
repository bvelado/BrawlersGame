using UnityEngine;

public class CharacterItemCarrier : MonoBehaviour {

    private bool isCarrying;
    public bool IsCarrying
    {
        get { return isCarrying; }
    }
    private bool canCarry;
    public bool CanCarry
    {
        get
        {
            return canCarry;
        }
    }

    private float canCarryCooldown;
    private float canCarryTimer;

    private GameObject itemModel;

    private void Start()
    {
        canCarryCooldown = GameManager.Instance.Parameters.COOLDOWN_CARRY;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            var otherCarrier = collision.gameObject.GetComponent<CharacterItemCarrier>();

            if(otherCarrier != null)
            {
                // Steal the other carrier
                if (otherCarrier.CanCarry && otherCarrier.IsCarrying)
                {
                    otherCarrier.SetCarrying(false);
                    SetCarrying(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("NeutralItem"))
        {
            SetCarrying(true);
            Destroy(collision.transform.root.gameObject);
        }
    }

    private void Update()
    {
        if (!canCarry)
        {
            if (canCarryTimer < canCarryCooldown)
                canCarryTimer += Time.deltaTime;
            else
                canCarry = true;
        }

        if (IsCarrying)
        {
            itemModel.SetActive(true);
            itemModel.transform.Rotate(Vector3.up, Time.deltaTime, Space.Self);
        }
    }

    public void SetCarrying(bool isCarrying)
    {
        this.isCarrying = isCarrying;

        if (!isCarrying)
        {
            Destroy(itemModel);
        } else
        {
            itemModel = Instantiate(GameManager.Instance.Parameters.ITEM_PREFAB, transform.position + Vector3.up * 2f, Quaternion.identity, transform);
        }

        canCarry = false;
        canCarryTimer = 0f;
    }
}
