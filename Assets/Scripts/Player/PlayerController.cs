using GGJ.Manager;
using GGJ.Prop;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace GGJ.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Transform _feet;

        private Vector2 _mov;
        private Vector2 _direction = Vector2.up;
        private Rigidbody2D _rb;

        private int _money;

        public ITakeable CarriedObject { private set; get; }

        #region Unity methods
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            PlayerManager.Instance.Register(this);
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _mov * ResourceManager.Instance.GameInfo.Speed;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (ResourceManager.Instance != null)
            {
                Gizmos.DrawWireSphere(_feet.transform.position + (Vector3)_direction * ResourceManager.Instance.GameInfo.InteractionDistance, ResourceManager.Instance.GameInfo.InteractionSize);
            }
        }
        #endregion Unity methods

        #region Inputs
        public void OnMovement(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>();
            if (_mov.magnitude > 0f)
            {
                _direction = _mov;
            }
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started)
            {
                var center = _feet.transform.position + (Vector3)_direction * ResourceManager.Instance.GameInfo.InteractionDistance;
                var coll = Physics2D.OverlapCircle(center, ResourceManager.Instance.GameInfo.InteractionSize, LayerMask.GetMask("Prop"));
                if (coll != null)
                {
                    if (coll.TryGetComponent<IInteractible>(out var interact) && interact.CanInteract(this))
                    {
                        interact.Interact(this);
                    }
                }
                else if (CarriedObject != null) // We carry smth and there is empty space in front of us
                {
                    CarriedObject.GameObject.SetActive(true);
                    CarriedObject.GameObject.transform.position = center;
                    CarriedObject = null;
                }
            }
        }
        #endregion Inputs

        /// <summary>
        /// Attempt to carry an object
        /// </summary>
        /// <returns>Could the object be carried</returns>
        public bool Carry(ITakeable takeable)
        {
            if (CarriedObject != null) return false;

            CarriedObject = takeable;
            CarriedObject.GameObject.SetActive(false);
            return true;
        }

        /// <summary>
        /// Delete object carried
        /// </summary>
        public void DiscardCarry()
        {
            Assert.IsNotNull(CarriedObject);
            Destroy(CarriedObject.GameObject);
            CarriedObject = null;
        }

        public void GainMoney(int amount)
        {
            _money += amount;
        }

        public override string ToString()
        {
            return $"Player (Money: {_money})";
        }
    }
}
