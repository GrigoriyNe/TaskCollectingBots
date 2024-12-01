using TMPro;
using UnityEngine;

public class BaseStorageView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _woodCounterText;
    [SerializeField] private TextMeshProUGUI _metalCounterText;
    [SerializeField] private TextMeshProUGUI _bulletCounterText;
    [SerializeField] private TextMeshProUGUI _moneyCounterText;
    [SerializeField] private TextMeshProUGUI _weaponCounterText;

    public void ChangeViewWood(int value)
    {
        _woodCounterText.text = value.ToString();
    }

    public void ChangeViewMetal(int value)
    {
        _metalCounterText.text = value.ToString();
    }

    public void ChangeViewBullet(int value)
    {
        _bulletCounterText.text = value.ToString();
    }

    public void ChangeViewMoney(int value)
    {
        _moneyCounterText.text = value.ToString();
    }

    public void ChangeViewWeapon(int value)
    {
        _weaponCounterText.text = value.ToString();
    }

}