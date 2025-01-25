using GGJ.Manager;
using GGJ.Prop;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace GGJ.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Transform _feet;

        [SerializeField]
        private GameObject _readyText;

        private Vector2 _mov;
        private Vector2 _direction = Vector2.up;
        private Rigidbody2D _rb;

        private int _money;

        public bool IsReady { private set; get; }

        public ITakeable CarriedObject { private set; get; }

        public Color Color { set; get; }
        public Vector2 SpawnPoint { set; private get; }

        #region Unity methods
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _readyText.SetActive(false);
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
                var colls = Physics2D.OverlapCircleAll(center, ResourceManager.Instance.GameInfo.InteractionSize, LayerMask.GetMask("Prop"));
                if (colls.Any())
                {
                    var valids = colls.Where(x =>
                    {
                        if (x.TryGetComponent<IInteractible>(out var interact))
                        {
                            return interact.CanInteract(this);
                        }
                        return false;
                    });
                    if (valids.Any())
                    {
                        var takeable = valids.FirstOrDefault(x => x.TryGetComponent<ITakeable>(out var _));
                        if (takeable != null) takeable.GetComponent<IInteractible>().Interact(this);
                        else valids.First().GetComponent<IInteractible>().Interact(this);
                    }
                }
                else if (CarriedObject != null) // We carry smth and there is empty space in front of us
                {
                    CarriedObject.GameObject.transform.position = center;
                    CarriedObject.GameObject.SetActive(true);
                    CarriedObject = null;
                }
            }
        }
        #endregion Inputs

        public void Ready()
        {
            if (GameManager.Instance.GamePhase == GamePhase.LobbyPreparation)
            {
                _readyText.SetActive(true);
                IsReady = true;
                PlayerManager.Instance.CheckAllReady();
            }
        }

        public void UnreadyForGameStart()
        {
            _readyText.SetActive(false);
            IsReady = false;
        }

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

        public void ResetAll()
        {
            _money = 0;
            if (CarriedObject != null)
            {
                Destroy(CarriedObject.GameObject);
                CarriedObject = null;
            }
            transform.position = SpawnPoint;
        }

        public override string ToString()
        {
            return $"Player (Money: {_money})";
        }
    }
}
