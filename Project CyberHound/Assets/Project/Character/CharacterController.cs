using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Transform bodyTransform;

    [Header("Projectile Logic")] 
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float firePower = 500f;

    

    private Rigidbody rb;
    private Vector2 m_Move;
    private Vector2 m_Look;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void FixedUpdate()
    {
        // Get rotation input
        var lookDir = new Vector3(m_Look.x, 0, m_Look.y);
        
        if (lookDir.magnitude > 0.1f)
        {
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, Quaternion.LookRotation(lookDir), rotateSpeed * Time.fixedDeltaTime);
        }

        // Move the character
        var moveDir = new Vector3(m_Move.x, 0, m_Move.y);

        if (moveDir.magnitude > 0.1f)
        {
            moveDir = moveDir.normalized;
            if (Vector3.Dot(transform.forward, moveDir) < .95)
            {
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), rotateSpeed * Time.fixedDeltaTime));
            }
            rb.MovePosition(transform.position + transform.forward * (Vector3.Dot(transform.forward, moveDir) * moveSpeed * Time.fixedDeltaTime));
        }
    }

    // 'Move' input action has been triggered.
    public void OnMove(InputValue value)
    {
        m_Move = value.Get<Vector2>();
    }

    // 'Look' input action has been triggered.
    public void OnAim(InputValue value)
    {
        m_Look = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        var go = Instantiate(projectile, firePoint);

        if (go.TryGetComponent(out Rigidbody pRigidbody))
        {
            pRigidbody.AddForce(firePoint.forward * firePower, ForceMode.VelocityChange);
        }
    }
}
