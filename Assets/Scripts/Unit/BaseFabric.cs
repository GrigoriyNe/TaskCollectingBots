using UnityEngine;

public class BaseFabric : MonoBehaviour
{

    [SerializeField] private Base _basePrefab;
    [SerializeField] private PlayerInputController _input;

    private Quaternion _basePrefabRotation = new Quaternion(0f, 261.61f, 0f, -0.6f);

    public Base CreateBase(Vector3 bildPosition)
    {
        Base newBase = Instantiate(_basePrefab, bildPosition, _basePrefabRotation);

        if(newBase.TryGetComponent(out BaseCrafter crafter))
        {
            crafter.TakeInput(_input);
        }

        return newBase;
    }
}
