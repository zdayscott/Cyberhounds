using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Project.Game_Entities.Character
{
    public class PlayerController : GameEntity
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 5f;
        [SerializeField] private Transform bodyTransform;

        public int weaponIndex;
        public Weapon[] weapons;

        [FormerlySerializedAs("_playerInput")]
        [Header("Action References")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputActionReference fireActionReference;
        private InputAction _fireAction;

        private Rigidbody _rb;
        private Vector2 _moveVector;
        private Vector2 _lookVector;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            Debug.Log($"Name: {fireActionReference.action.name}");
            playerInput = GetComponent<PlayerInput>();
            _fireAction = playerInput.actions[fireActionReference.action.name];
            _fireAction.started += FireActionOnStarted;
            _fireAction.canceled += FireActionOnCanceled;
        }

        private void OnDisable()
        {
            _fireAction.started -= FireActionOnStarted;
            _fireAction.canceled -= FireActionOnCanceled;
        }

        private void FixedUpdate()
        {
            // Get rotation input
            var lookDir = new Vector3(_lookVector.x, 0, _lookVector.y);
        
            if (lookDir.magnitude > 0.1f)
            {
                bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, Quaternion.LookRotation(lookDir), rotateSpeed * Time.fixedDeltaTime);
            }

            // Move the character
            var moveDir = new Vector3(_moveVector.x, 0, _moveVector.y);

            if (moveDir.magnitude > 0.1f)
            {
                moveDir = moveDir.normalized;
                if (Vector3.Dot(transform.forward, moveDir) < .95)
                {
                    _rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), rotateSpeed * Time.fixedDeltaTime));
                }
                _rb.MovePosition(transform.position + transform.forward * (Vector3.Dot(transform.forward, moveDir) * moveSpeed * Time.fixedDeltaTime));
            }
        }

        // 'Move' input action has been triggered.
        public void OnMove(InputValue value)
        {
            _moveVector = value.Get<Vector2>();
        }

        // 'Look' input action has been triggered.
        public void OnAim(InputValue value)
        {
            _lookVector = value.Get<Vector2>();
        }

        public void OnCycleWeapon(InputValue value)
        {
            weapons[weaponIndex].StopFiring();
            weaponIndex++;
            if (weaponIndex >= weapons.Length)
                weaponIndex = 0;
        }
    
        private void FireActionOnStarted(InputAction.CallbackContext obj)
        {
            weapons[weaponIndex].StartFiring();
        }
    
        private void FireActionOnCanceled(InputAction.CallbackContext obj)
        {
            weapons[weaponIndex].StopFiring();
        }
    

    }
}