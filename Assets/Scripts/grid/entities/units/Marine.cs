using System;
using System.Collections.Generic;
using Cinemachine;
using grid.scene;
using MEC;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace grid.entities.units
{
    public class Marine : Entity
    {
        public CinemachineVirtualCamera virtualCamera;
        [HideInInspector] public MarineController controller;
        private Tilemap tilemap => TilemapManager.Instance.tilemap;

        private void Start()
        {
            controller = GetComponent<MarineController>();
        }

        public void Initialize()
        {
            DeactivateHumanControl();
        }

        public void ActivateHumanControl()
        {
            virtualCamera.gameObject.SetActive(true);
        }

        public void DeactivateHumanControl()
        {
            virtualCamera.gameObject.SetActive(false);
        }

        public void ShootAnimation(Entity target, Action callback)
        {
            //marine
            //  turn marine
            //  play shoot animation
            //  turn marine back
            //bullets
            //  create bullets,
            //  move bullets,
            //enemy
            //  hit enemy,
            //  play enemy animation
            var defaultPosition = transform.position + transform.rotation.eulerAngles;
            var enemyPosition = tilemap.CellToWorld(target.TilePosition); 
            //var turningCoroutineHandle = Timing.RunCoroutine(TurnToTarget(transform, enemyPosition, 1.0f));
            
        }
        
        public IEnumerator<float> TurnToTarget(Transform transform, Vector3 positionToLook, float timeToRotate)
        {
            var startRotation = transform.rotation;
            var direction = positionToLook - transform.position;
            var finalRotation = Quaternion.LookRotation(direction);
            var t = 0f;
            while (t <= 1f)
            {
                t += Time.deltaTime / timeToRotate;
                transform.rotation = Quaternion.Lerp(startRotation, finalRotation, t);
                yield return Timing.WaitForOneFrame;
            }
            transform.rotation = finalRotation;
        }
    }
}