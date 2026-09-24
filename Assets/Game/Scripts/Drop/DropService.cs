using System.Linq;
using Ach.Weapons;
using RPool;
using UnityEngine;

namespace Ach.Drop
{
    public class DropService
    {
        private PoolService _pool;
        private DropListSO _dropList;

        public DropService(DropListSO dropList, PoolService pool)
        {
            _dropList = dropList;
            _pool = pool;
        }

        public void DropWeapon(WeaponConfigSO config, DropWeaponInfo dropInfo, Vector3 position)
        {
            var item =
                _dropList.Weapons.FirstOrDefault(x => x != null && x.Weapon == config);
            if (item == null) return;

            var drop = _pool.Get(item, position, Quaternion.Euler(0, Random.Range(0, 360f), 0));
            drop.SetDropInfo(dropInfo);
        }
    }
}

