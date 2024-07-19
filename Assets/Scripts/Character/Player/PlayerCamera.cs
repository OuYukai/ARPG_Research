using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SG
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;

        //  CHANGE THESE TO TWEAK CAMERA PERFORMANCE
        [Header("Camera Settings")]
        [SerializeField] float cameraSmoothSpeed = 1 ; //   THE BIGGER THIS NUMBER, THE LONGER FOR THE CAMERA TO REACH ITS POSITION DURING MOVEMENT
        [SerializeField] float leftAndRightRotationSpeed = 220;
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30; //   THE LOWEST POINT YOU ARE ABLE TO LOOK DOWN
        [SerializeField] float maximumpivot = 60; //    THE HIGHEST POINT YOU ARE ABLE TO LOOK UP
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        // JUST DISPLAY CAMERA VALUES
        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition;   //  USED FOR CAMERA COLLISION (MOVE THE CAMERA OBJECT TO THIS POSITION UPON COLLIDING)
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPoaition;    //  VALUES USED FOR CAMERA COLLISIONS
        private float targetCameraZPosition;     //  VALUES USED FOR CAMERA COLLISIONS

        [Header("Lock On")] 
        [SerializeField] float lockOnRadius = 20;
        [SerializeField] float minimumViewableAngle = -50;
        [SerializeField] float maximumViewableAngle = 50;
        [SerializeField] float maximumLockOnDistance = 20;

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
            cameraZPoaition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraAction()
        {
            if (player != null)
            {
                HandleFollowTarget();   //  FOLLOW THE PLAYER
                
                HandleRotation();       //  ROTATE AROUND THE PLAYER
                
                HandleCollision();      //  COLLIDE WITH OBJECTS
            }
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotation()
        {
            //  IF LOCKED ON, FORCE ROTATION TOWARDS TARGET
            //  ESLE ROTATE REGULARY

            //  ROTATE LEFT AND RIGHT BASED ON HORIZONAL MOVEMENT ON THE RIGHT JOYSTICK
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;

            //  ROTATE UP AND DOWN BASED ON VERTICAL MOVEMENT ON THE RIGHT JOYSTICK
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;

            //  CLAMP THE UP AND DOWN LOOK ANGLE BETWEEN MIN AND MAX VALUE
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumpivot);


            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            //  ROATATE THIS GAMEOBJECT LEFT AND RIGHT
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            //  ROTATE THE PIVOT GAMEOBJECT UP AND DOWN
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollision()
        {
            targetCameraZPosition = cameraZPoaition;

            RaycastHit hit;
            //  DIRECTION FOR COLLISION CHECK
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            //  WE CHECK IF THERE IS AN OBJECT IN FRONT OF OUR DESIRED DIRECTION ^ (SEE ABOVE)
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                //  IF THERE IS, WE GET OUR DISTANCE FORM IT
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                //  WE THEN EQUATE OUR TARGET Z POSITION TO THE FOLLOWINT
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            //  IF OUR TARGET POSITION IS LESS THEN OUR COLLISION RADIUS, WE SUBTRACT OUR COLLISION RADIUS (MAKING IT SNAP BACK)
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            //  WE THEN APPLY OUR FINAL POSITION USING A LERP OVER A TIME OF 0.2F
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }

        public void HandleLocatingLockOnTargets()
        {
            float shortestDistance = Mathf.Infinity;   //  WILL BE USED TO DETERMINE THE TARGET CLOSEST TO US
            
            //  (Closest target to the right of current target)
            float shortestDistanceOfRightTarget = Mathf.Infinity;  //  WILL BE USE TO DETERMINE SHORTEST DISTANCE ON ONE AXIS TO THE RIGHT OF CURRENT TARGET (+)
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;  //  WILL BE USE TO DETERMINE SHORTEST DISTANCE ON ONE AXIS TO THE LEFT OF CURRENT TARGET (-)
            
            // TODO : USE A LAYER MASK
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.instance.GetCharacterLayers());

            for (int i = 0 ; i < colliders.Length ; i ++)
            {
                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

                if (lockOnTarget != null)
                {
                    //  CHECK IF THEY ARE WITHIN OUR FIELD OF VIEW
                    Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);
                    
                    //  IF TARGET IS DEAD, CHECK THE NEXT POTENTIAL TARGET
                    if (lockOnTarget.isDead.Value)
                        continue;
                    
                    //  IF TARGET IS US, CHECK THE NEXT POTENTIAL TARGET
                    if (lockOnTarget.transform.root == player.transform.root)
                        continue;
                    
                    //  IF THE TARGET IS TOO FAR AWAY, CHECK THE NEXT POTENTIAL TARGET
                    if (distanceFromTarget > maximumLockOnDistance)
                        continue;

                    if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;
                        
                        //  TODO : ADD LAYER MASK FOR ENVIRO LAYERS ONLY
                        if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position,
                                lockOnTarget.characterCombatManager.lockOnTransform.position, out hit, WorldUtilityManager.instance.GetEnviroLayerMask()))
                        {
                            //  WE HIT SOMETHING, WE CANNOT SEE OUR LOCK ON TARGET
                            continue;
                        }
                        else
                        {
                            Debug.Log("WE MADE IT");
                        }
                    }
                }
                    
            }
        }
    }
}

