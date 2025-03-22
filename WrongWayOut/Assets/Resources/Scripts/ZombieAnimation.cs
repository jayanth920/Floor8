using UnityEngine;

namespace SkinlessZombie
{
    public class ZombieAnimation : MonoBehaviour
    {
        public Animator zombieAnimator; // Assign in Inspector
        public AnimationClip zombieAnimation; // Assign a single animation in Inspector
        public GameObject weaponPrefab; // Assign the axe prefab in Inspector
        public Transform weaponPivot; // The point where the weapon should be attached
        public Transform rightHand; // Assign the right hand Transform

        private GameObject instantiatedWeapon;

        void Start()
        {
            PlayZombieAnimation();
            EquipWeapon();
        }

        private void PlayZombieAnimation()
        {
            if (zombieAnimator != null && zombieAnimation != null)
            {
                zombieAnimator.Play(zombieAnimation.name, 0, 0f); // Play from the start
            }
            else
            {
                Debug.LogError("Animator or AnimationClip not assigned!");
            }
        }

        private void EquipWeapon()
        {
            if (weaponPrefab != null && rightHand != null)
            {
                instantiatedWeapon = Instantiate(weaponPrefab, rightHand);
                instantiatedWeapon.transform.position = weaponPivot.position;
                instantiatedWeapon.transform.rotation = weaponPivot.rotation;
            }
        }
    }
}
