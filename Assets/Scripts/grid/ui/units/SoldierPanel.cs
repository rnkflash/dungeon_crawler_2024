using System.Collections.Generic;
using grid.entities.units;
using grid.ui.mouse;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace grid.ui.units
{
    public class SoldierPanel: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image background;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> sounds;

        private bool currentlyActive;
        private Marine marine;

        private void Start()
        {
            SetActive(currentlyActive);
        }

        public void Initialize(Marine marine)
        {
            currentlyActive = false;
            SetActive(currentlyActive);
            this.marine = marine;
        }

        public void SetActive(bool active)
        {
            currentlyActive = active;
            
            if (active)
            {
                SetOpaqueBackground();
            }
            else
                SetTransparentBackground();
        }

        public Marine GetUnit()
        {
            return marine;
        }

        private void SetOpaqueBackground()
        {
            var tempColor = background.color;
            tempColor.a = 0.95f;
            background.color = tempColor;
        }

        private void SetTransparentBackground()
        {
            var tempColor = background.color;
            tempColor.a = 0.25f;
            background.color = tempColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UnitPanel.Instance.SoldierPanelClicked(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            MouseCursor.Instance.OnMouseEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MouseCursor.Instance.OnMouseExit();
        }
    }
}