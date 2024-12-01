using TMPro;
using UnityEngine;

public class BaseStorageView : MonoBehaviour
{
    const string Wood = "Wood: ";
    const string Metal = "Metal: ";
    const string Bullets = "Bullets: ";
    const string Money = "Money: ";
    const string Weapon = "Weapon: ";

    [SerializeField] private TextMeshProUGUI _woodCounterText;
    [SerializeField] private TextMeshProUGUI _metalCounterText;
    [SerializeField] private TextMeshProUGUI _bulletCounterText;
    [SerializeField] private TextMeshProUGUI _moneyCounterText;
    [SerializeField] private TextMeshProUGUI _weaponCounterText;

    public void ChangeViewWood(int value)
    {
        string viewText = Wood + value.ToString();
        _woodCounterText.text = viewText;
    }

    public void ChangeViewMetal(int value)
    {
        string viewText = Metal + value.ToString();
        _metalCounterText.text = viewText;
    }

    public void ChangeViewBullet(int value)
    {
        string viewText = Bullets + value.ToString(); 
        _bulletCounterText.text = viewText;
    }

    public void ChangeViewMoney(int value)
    {
        string viewText = Money + value.ToString();
        _moneyCounterText.text = viewText;
    }

    public void ChangeViewWeapon(int value)
    {
        string viewText = Weapon + value.ToString();
        _weaponCounterText.text = viewText;
    }
}