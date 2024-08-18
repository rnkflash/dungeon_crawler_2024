using grid.scene;
using UnityEngine;

namespace grid.entities.units
{
    public class UserInput : MonoBehaviour
    {
        private UnitManager unitManager;

        private void Start()
        {
            unitManager = GetComponent<UnitManager>();
        }

        private void Update()
        {
            if (unitManager.activeMarine == null) return;
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                unitManager.activeMarine.controller.TurnLeft();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                unitManager.activeMarine.controller.TurnRight();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                unitManager.activeMarine.controller.MoveForward();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                unitManager.activeMarine.controller.MoveBackward();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                unitManager.activeMarine.controller.MoveLeft();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                unitManager.activeMarine.controller.MoveRight();
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                unitManager.NextActiveUnit();
            }
        }
    }
}
