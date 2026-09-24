using UnityEngine;

namespace Ach.Units.Enemies
{
    public class EnemyRandomizer : MonoBehaviour
    {
        [SerializeField] private GameObject[] models;

        private void Awake()
        {
            foreach (var model in models) model.SetActive(false);
            int index = Random.Range(0, models.Length);
            models[index].SetActive(true);
        }
    }
}

