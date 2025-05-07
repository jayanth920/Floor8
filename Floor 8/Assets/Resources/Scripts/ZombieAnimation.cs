using UnityEngine;

namespace SkinlessZombie
{
    public class ZombieAnimation : MonoBehaviour
    {
        public Animator zombieAnimator; 
        public AnimationClip zombieAnimation;
        public GameObject weaponPrefab; 
        public Transform weaponPivot; 
        public Transform rightHand; 

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
                zombieAnimator.Play(zombieAnimation.name, 0, 0f); // play from start
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
