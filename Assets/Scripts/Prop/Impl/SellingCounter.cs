using GGJ.Manager;
using GGJ.Player;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GGJ.Prop.Impl
{
    public class SellingCounter : MonoBehaviour
    {
        [SerializeField]
        private GameObject _textCanvas;
        [SerializeField]
        private TMP_Text _priceText;
        [SerializeField]
        private TMP_Text _priceVariationText;
        [SerializeField]
        private Sprite _closedSprite;
        [SerializeField]
        private Sprite _boughtSprite;
        [SerializeField]
        private GameObject _sellVFX;
        [SerializeField]
        private Sprite _money1Sprite, _money2Sprite, _money3Sprite;
        [SerializeField]
        private SpriteRenderer _moneySpriteRenderer;

        private Sprite _openSprite;
        public int AmountSold { set; get; }
        public float Variation { set; get; } = 1f;

        private float _boughtTimer = 0f;

        private SpriteRenderer _sr;
        private int _lastPrice;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _openSprite = _sr.sprite;
        }

        private void Start()
        {
            EconomyManager.Instance.Register(this);
            _lastPrice = Mathf.RoundToInt(EconomyManager.Instance.CurrentPrice * Variation);
            UpdateUI();
        }

        private void Update()
        {
            if (_boughtTimer > 0f)
            {
                _boughtTimer -= Time.deltaTime;
                if (_boughtTimer <= 0f && GameManager.Instance.GamePhase != GamePhase.GameEnded)
                {
                    _sr.sprite = _openSprite;
                }
            }
        }

        public string AddSign(int nb)
        {
            if (nb >= 0f) return $"+{nb}";
            else return $"-{Mathf.Abs(nb)}";
        }
        public void UpdateUI()
        {
            if (GameManager.Instance.GamePhase == GamePhase.GameEnded)
            {
                _textCanvas.SetActive(false);
                _sr.sprite = _closedSprite;
            }
            else
            {
                _priceText.text = $" = {Mathf.RoundToInt(Mathf.Max(0, EconomyManager.Instance.CurrentPrice * Variation))}";
                _priceVariationText.text = $"({AddSign(Mathf.RoundToInt(EconomyManager.Instance.CurrentPrice * Variation) - _lastPrice)})";
                _textCanvas.SetActive(true);
                _sr.sprite = _openSprite;

                _lastPrice = Mathf.RoundToInt(EconomyManager.Instance.CurrentPrice * Variation);
                
                UpdateMoneySprite();
            }
        }

        void UpdateMoneySprite()
        {
            int money = Mathf.RoundToInt(EconomyManager.Instance.CurrentPrice * Variation);
            float money01 = (float)money / ResourceManager.Instance.GameInfo.MinMaxPrice.Max;

            if      (money01 < 0.333f) _moneySpriteRenderer.sprite = _money1Sprite;
            else if (money01 < 0.666f) _moneySpriteRenderer.sprite = _money2Sprite;
            else                       _moneySpriteRenderer.sprite = _money3Sprite;
        }

        public void UpdateVariation(float average)
        {
            Variation = (1f + Variation) / 2f;
            Variation += (average - AmountSold) * ResourceManager.Instance.GameInfo.PercSellVariation * 0.01f;
        }

        public void Interact(PlayerController pc)
        {
            if ((GameManager.Instance.GamePhase == GamePhase.PriceRaise || GameManager.Instance.GamePhase == GamePhase.PriceCrash) && pc.Sellables.Any())
            {
                foreach (var item in pc.Sellables)
                {
                    int money = Mathf.RoundToInt(EconomyManager.Instance.CurrentPrice * Variation);
                    if (item is CutedPlant &&
                        ((CutedPlant)item).PlayerId != pc.Id)
                        money = (int)(money * ResourceManager.Instance.GameInfo.OtherPlayerPlantPriceCoef);

                    pc.GainMoney(money);
                }
                AmountSold += pc.Sellables.Count;
                pc.DiscardSellablesCarry();
                AudioManager.Instance.PlaySell();
                Destroy(Instantiate(_sellVFX, transform.position, Quaternion.identity), 1f);
                _boughtTimer = .5f;
                _sr.sprite = _boughtSprite;
            }
        }
    }
}
