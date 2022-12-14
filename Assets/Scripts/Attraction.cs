using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{
    [SerializeField] AudioClip moveClip;
    [SerializeField] int attractionPosCode;
    [SerializeField] string sceneName;
    public LayerMask playerLayer;
    bool hit = false;

    [SerializeField] GameObject mark;

    void Update()
    {
        hit = Physics2D.OverlapCircle(transform.position, 0.9f, playerLayer);

        if (hit && !UIManager.Instance.night)
        {
            mark.SetActive(true);
            if (!GameManager.Instance.m_optionOpen)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                    if (hit.collider != null)
                    {
                        if (hit.collider.name == gameObject.name || hit.collider.name == "Player")
                        {
                            SoundManager.Instance.PlayEffectSound(moveClip);
                            GameValueManager.Instance.IsAttractionPosCode = attractionPosCode;
                            NPCManager.Instance.NPCFriendshipSave();
                            SceneController.Instance.LoadScene(sceneName);
                        }
                    }

                }
#else
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                    if (hit.collider != null)
                    {
                        if (hit.collider.name == gameObject.name || hit.collider.name == "Player")
                        {
                            SoundManager.Instance.PlayEffectSound(moveClip);
                            GameValueManager.Instance.IsAttractionPosCode = attractionPosCode;
                            NPCManager.Instance.NPCFriendshipSave();
                            SceneController.Instance.LoadScene(sceneName);
                        }
                    }
                }

#endif
            }
        }
        else
        {
            mark.SetActive(false);
            return;
        }
    }
}

