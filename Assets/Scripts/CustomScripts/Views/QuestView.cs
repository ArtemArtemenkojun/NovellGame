using System.Collections;
using System.Collections.Generic;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class QuestView : CustomUI
    {
        [SerializeField] private Slider questProgressSlider;
        [SerializeField] private Image starImage;
        [SerializeField] private QuestItemView[] questItemViews;
        
        private Dictionary<string, QuestItemView> _questItemDictionary;

        protected override void OnEnable()
        {
            base.OnEnable();
            _questItemDictionary = new Dictionary<string, QuestItemView>();
            foreach (var quest in questItemViews) 
                _questItemDictionary.Add(quest.Key, quest);
        }
        
        public void ActivateQuestItem(string id)
        {
            foreach (var questItem in questItemViews) 
                questItem.SetActive(false);

            _questItemDictionary[id].SetActive(true);
            
            starImage.gameObject.SetActive(false);
            questProgressSlider.value = 0;
        }
        
        public void CompleteQuest() => StartCoroutine(AnimateSliderFill(1f));

        public IEnumerator AnimateSliderFill(float duration)
        {
            float startValue = questProgressSlider.value;

            for (float time = 0; time < 1; time += Time.deltaTime / duration)
            {
                questProgressSlider.value = Mathf.Lerp(startValue, 1, time);
                yield return null;
            }

            questProgressSlider.value = 1;
            starImage.gameObject.SetActive(true);
        }
    }
}