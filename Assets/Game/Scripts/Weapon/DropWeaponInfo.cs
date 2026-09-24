namespace Ach.Weapons
{
    public struct DropWeaponInfo
    {
        public int Ammo;
        public int AmmoInClip;

        public DropWeaponInfo(int ammo, int ammoInClip)
        {
            Ammo = ammo;
            AmmoInClip = ammoInClip;
        }
    }
}