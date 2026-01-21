using TMPro;
using UnityEngine;

namespace CodeBase.Logic.UI
{
    public class CoinView : MonoBehaviour
    {
        [field: SerializeField] private TMP_Text _coinText;

        public void SetCoin(int coin)
        {
            _coinText.text = coin.ToString();
        }
    }
}