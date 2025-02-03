using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class QuestItemView : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private Text nameText;
        [SerializeField] private Text descriptionText;

        public string Key => key;

        public void SetActive(bool isActive)
        {
            nameText.enabled = isActive;
            descriptionText.enabled = isActive;
        }
    }
}