using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;
    [SerializeField] private StorageView _viewResource;

    private int _woodCounter = 0;
    private int _metalCounter = 0;
    private int _bulletCounter = 0;
    private int _moneyCounter = 0;
    private int _weaponCounter = 0;

    private void Start()
    {
        foreach (Unit unit in _units)
        {
            unit.IsCollect += OnCollect;
        }
    }

    private void OnCollect(Resource resource)
    {
        if (resource is Wood)
        {
            _woodCounter++;
            _viewResource.ChangeViewWood(_woodCounter);
        }
        else if (resource is Metal)
        {
            _metalCounter++;
            _viewResource.ChangeViewMetal(_metalCounter);
        }
        else if (resource is Bullet)
        {
            _bulletCounter++;
            _viewResource.ChangeViewBullet(_bulletCounter);
        }
        else if (resource is Money)
        {
            _moneyCounter++;
            _viewResource.ChangeViewMoney(_moneyCounter);
        }
        else if (resource is Weapon)
        {
            _weaponCounter++;
            _viewResource.ChangeViewWeapon(_weaponCounter);
        }
    }
}
