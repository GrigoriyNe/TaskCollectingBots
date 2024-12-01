using System.Collections.Generic;
using UnityEngine;

public class BaseStorage : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;
    [SerializeField] private BaseStorageView _viewResource;

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
        if (resource.TryGetComponent(out Wood _))
        {
            _woodCounter++;
            _viewResource.ChangeViewWood(_woodCounter);
        }
        else if (resource.TryGetComponent(out Metal _))
        {
            _metalCounter++;
            _viewResource.ChangeViewMetal(_woodCounter);
        }
        else if (resource.TryGetComponent(out Bullet _))
        {
            _bulletCounter++;
            _viewResource.ChangeViewBullet(_woodCounter);
        }
        else if (resource.TryGetComponent(out Money _))
        {
            _moneyCounter++;
            _viewResource.ChangeViewMoney(_woodCounter);
        }
        else if (resource.TryGetComponent(out Weapon _))
        {
            _weaponCounter++;
            _viewResource.ChangeViewWeapon(_woodCounter);
        }
    }
}
