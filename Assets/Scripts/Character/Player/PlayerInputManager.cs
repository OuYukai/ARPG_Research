using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;

        // THINK ABOUT GOALS IN STEPS
        
        // 2. MOVE CHARACTER BASED ON THOSE VALUES
        PlayerControls playerControls;
        [SerializeField] Vector2 movementInput;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            // WHEN THE SCENE CHANGES, RUN THIS LOGIC
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            // IF WE ARE LOADING INTO OUR WORLD SCENE, ENABLE OUR PLAYERS CONTROLS
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetworldSceneIndex())
            {
                instance.enabled = true;
            }
            // OTHERWISE WE MUST BE AT THE MAIN MENU, DISABLE OUR PLAYERS CONTROLS
            // THIS IS SO OUR PLAYER CANT MOVE AROUND IF WE ENTER LIKE A CHARACTER CREATION MENU ECT
            else
            {
                instance.enabled = false;
            }
        }

        // 1. FIND A WAY TO READ THE VALUES OF A JOY STICK
        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }
    
        private void OnDestroy()
        {
            // IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THIS ENET
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}

