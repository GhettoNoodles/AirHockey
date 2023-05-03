using System;
using UnityEngine;

public class Puck : MonoBehaviour
{
    [SerializeField] public Rigidbody2D puck;
    public bool playerGoal = false;
    public bool enemyGoal = false;
    private float _portalCooldown;
    private bool _portalHit = false;
    public int lastHitBy=3;
    public bool eFoul = false;
    public bool pFoul = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (_portalHit)
        {
            _portalCooldown += Time.deltaTime;
        }

        if (_portalCooldown >= 1f)
        {
            _portalHit = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Goal Left"))
        {
            playerGoal = true;
        }

        if (other.gameObject.CompareTag("Goal Right"))
        {
            enemyGoal = true;
        }

        if (other.gameObject.CompareTag("Portal"))
        {
            if (_portalHit == false)
            {
                var pIn = other.gameObject;
                var portals = GameObject.FindGameObjectsWithTag("Portal");
                GameObject pOut;
                pOut = portals[0] == pIn ? portals[1] : portals[0];
                string inLoc;
                inLoc = DeterminePos(pIn);
                string outLoc;
                outLoc = DeterminePos(pOut);
                Debug.Log(inLoc);
                Debug.Log(outLoc);
                float rotation = 0f;
                var newVel = puck.velocity;
                if (inLoc == "Top")
                {
                    if (outLoc == "Right")
                    {
                        rotation = 90f;
                    }
                    else if (outLoc == "Bot")
                    {
                        rotation = 0f;
                    }
                    else if (outLoc == "Left")
                    {
                        rotation = -90f;
                    }
                    else if (outLoc == "Top")
                    {
                        rotation = 180f;
                    }
                }
                else if (inLoc == "Right")
                {
                    if (outLoc == "Bot")
                    {
                        rotation = 90f;
                    }
                    else if (outLoc == "Left")
                    {
                        rotation = 0f;
                    }
                    else if (outLoc == "Top")
                    {
                        rotation = -90f;
                    }
                    else if (outLoc == "Right")
                    {
                        rotation = 180f;
                    }
                }
                else if (inLoc == "Bot")
                {
                    if (outLoc == "Left")
                    {
                        rotation = 90f;
                    }
                    else if (outLoc == "Top")
                    {
                        rotation = 0f;
                    }
                    else if (outLoc == "Right")
                    {
                        rotation = -90f;
                    }
                    else if (outLoc == "Bot")
                    {
                        rotation = 180f;
                    }
                }
                else if (inLoc == "Left")
                {
                    if (outLoc == "Top")
                    {
                        rotation = 90f;
                    }
                    else if (outLoc == "Right")
                    {
                        rotation = 0f;
                    }
                    else if (outLoc == "Bot")
                    {
                        rotation = -90f;
                    }
                    else if (outLoc == "Left")
                    {
                        rotation = 180f;
                    }
                }

                if (Math.Abs(rotation - 90f) < 2f)
                {
                    puck.velocity = new Vector2(-1 * newVel.y, newVel.x);
                    Debug.Log("90 rotation");
                }
                else if (Math.Abs(rotation - (-90f)) < 2f)
                {
                    puck.velocity = new Vector2(newVel.y, -1 * newVel.x);
                    Debug.Log("-90 rotation");
                }
                else if (Math.Abs(rotation - 180f) < 2f)
                {
                    puck.velocity = -1 * newVel;
                    Debug.Log("180 rotation");
                }
                else
                {
                    Debug.Log("no rotation");
                }

                puck.position = pOut.transform.position + pOut.transform.right *1.1f;

                _portalHit = true;
            }
        }
    }

    private string DeterminePos(GameObject p)
    {
        if (Math.Abs(p.transform.position.x - 8.86f) < 0.2f)
        {
            return "Right";
        }
        else if (Math.Abs(p.transform.position.x - (-8.86f)) < 0.2f)
        {
            return "Left";
        }
        else if (Math.Abs(p.transform.position.y - 4.73f) < 0.2f)
        {
            return "Top";
        }
        else if (Math.Abs(p.transform.position.y - (-4.73f)) < 0.2f)
        {
            return "Bot";
        }
        else
        {
            return "Nope";
        }
    }

    private void FixedUpdate()
    {
         if (puck.velocity.magnitude > 20f)
         {
             var puckDir = puck.velocity.normalized;
             puck.velocity = 20f * puckDir;
         }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (lastHitBy ==1)
            {
                eFoul = true;
            }
            lastHitBy = 1;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (lastHitBy==2)
            {
                pFoul = true;
            }

            lastHitBy = 2;

        }
    }
}