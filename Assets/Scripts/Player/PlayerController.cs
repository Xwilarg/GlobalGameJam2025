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

        [SerializeField]
        private GameObject _scissors, _seeds, _wateringCan, _cutedPlant;

        [SerializeField]
        private SpriteRenderer _cutedPlantFlowerSprite;


        private Vector2 _mov;
        private Vector2 _direction = Vector2.up;
        private Rigidbody2D _rb;

        private int _money;

        public bool IsReady { private set; get; }

        public ITakeable CarriedObject { private set; get; }

        public Color Color { set; get; }
        public Vector2 SpawnPoint { set; private get; }

        public SpriteRenderer CutedPlantFlowerSprite { get => _cutedPlantFlowerSprite; }

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
                var size = ResourceManager.Instance.GameInfo.InteractionSize;
                var colls = Physics2D.OverlapCircleAll(center, size, LayerMask.GetMask("Prop"));
                if (colls.Any())
                {
                    if (colls.Any())
                    {
                        IInteractible target = null;

                        var takeable = colls.FirstOrDefault(x => x.TryGetComponent<ITakeable>(out var _));
                        if (takeable != null)
                        {
                            DropItem();
                            target = takeable.GetComponent<IInteractible>();
                            if (!target.CanInteract(this)) target = null; // Safeguard
                        }
                        else target = colls.FirstOrDefault(x => x.TryGetComponent<IInteractible>(out var interact) && interact.CanInteract(this))?.GetComponent<IInteractible>();

                        if (target != null) target.Interact(this);
                    }
                }
                else if (CarriedObject != null) // We carry smth and there is empty space in front of us
                {
                    if (Physics2D.OverlapCircle(center, size, LayerMask.GetMask("Wall")) == null) // Prevent dropping something inside a wall
                    {
                        DropItem();
                    }
                }
            }
        }
        #endregion Inputs

        public void DropItem()
        {
            if (CarriedObject != null)
            {
                var center = _feet.transform.position + (Vector3)_direction * ResourceManager.Instance.GameInfo.InteractionDistance;
                CarriedObject.GameObject.transform.position = center;
                CarriedObject.GameObject.SetActive(true);
            }
            CarriedObject = null;

            DesactiveAllItems();
        }

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

            DesactiveAllItems();

            if (takeable is Scissors) _scissors.SetActive(true);
            else if (takeable is Seeds) _seeds.SetActive(true);
            else if (takeable is WateringCan) _wateringCan.SetActive(true);
            else if (takeable is CutedPlant) _cutedPlant.SetActive(true);

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

            DesactiveAllItems();
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

            DesactiveAllItems();
        }

        public override string ToString()
        {
            return $"Player (Money: {_money})";
        }

        void DesactiveAllItems()
        {
            _scissors.SetActive(false);
            _seeds.SetActive(false);
            _wateringCan.SetActive(false);
            _cutedPlant.SetActive(false);
        }
    }
}
