using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollerCoasterManager : MonoBehaviour
{
    [SerializeField] AudioClip rollerCoasterBack;
    [SerializeField] AudioClip goodSE;
    [SerializeField] AudioClip badSE;

    [SerializeField] int attractionsCode;
    [SerializeField] float time = 60.0f;
    [HideInInspector] public int goodNum = 0;

    /// <summary>
    /// ����� ������Ʈ
    /// </summary>
    [SerializeField] GameObject[] waitObj;
    /// <summary>
    /// ����� ��������Ʈ
    /// </summary>
    [SerializeField] Sprite[] waitSprite;

    /// <summary>
    /// ���� ����� ��ȣ
    /// </summary>
    int nowNum = 0;

    int lastNum = 0;
    /// <summary>
    /// ����
    /// </summary>
    float rollerCoasterScore = 0;
    /// <summary>
    /// ��ü ����� ����Ʈ
    /// </summary>
    List<int> waitList = new List<int>();
    /// <summary>
    /// ȭ�鿡 ������ �����
    /// </summary>
    Queue<int> waitQueue = new Queue<int>();

    [SerializeField] GameObject rideObj;
    [SerializeField] GameObject outObj;

    [Header("UI")]
    [SerializeField] Text goodText;
    [SerializeField] Text TimeText;
    [SerializeField] GameObject GameStartPanel;
    [SerializeField] Text GameStartText;

    [Header("Result")]
    [SerializeField] GameObject Result;
    [SerializeField] GameObject[] ResultScoreImage;

    [Header("Reaction")]
    [SerializeField] GameObject reaction;
    [SerializeField] Sprite reactionGood;
    [SerializeField] Sprite reactionBad;
    [SerializeField] Image reactionBar;



    void Start()
    {
        SoundManager.Instance.PlayBackSound(rollerCoasterBack);

        SceneController.Instance.FindObj();
        goodNum = 0;
        GameStartPanel.SetActive(true);
        reaction.SetActive(false);
        StartCoroutine("GameStart");
    }

    // Update is called once per frame
    void Update()
    {
        TimeText.text = time.ToString("00");
        reactionBar.fillAmount = rollerCoasterScore / 300;

        if (0 >= time)
        {
            OptionOn(Result);
            GetResult();
        }

    }


    public void RideBtn()
    {
        lastNum = waitQueue.Peek();
        if(waitQueue.Peek() == 2 || waitQueue.Peek() == 3)
        {
            StartCoroutine("RideAni");
            goodNum++;
            AddScore(10);
            SoundManager.Instance.PlayEffectSound(goodSE);
            GetReaction(true);
            waitQueue.Dequeue();
            WaitGen();
            nowNum++;
            WaitMatch();
        }
        else
        {
            StartCoroutine("RideAni");
            AddScore(-10);
            SoundManager.Instance.PlayEffectSound(badSE);
            GetReaction(false);
            waitQueue.Dequeue();
            WaitGen();
            nowNum++;
            WaitMatch();
        }
    }

    IEnumerator RideAni()
    {
        GameObject ride = Instantiate(rideObj as GameObject);
        ride.GetComponent<SpriteRenderer>().sprite = waitSprite[lastNum];
        yield return new WaitForSeconds(4.0f);
        Destroy(ride);
    }
    
    IEnumerator OutAni()
    {
        GameObject outo = Instantiate(outObj as GameObject);
        outo.GetComponent<SpriteRenderer>().sprite = waitSprite[lastNum];
        yield return new WaitForSeconds(4.0f);
        Destroy(outo);
    }

    /// <summary>
    /// �����߰�
    /// </summary>
    public void AddScore(int argNum)
    {
        rollerCoasterScore += argNum;
    }

    public void GetReaction(bool argBool)
    {
        StartCoroutine("ReactionActive", argBool);
    }

    /// <summary>
    /// ���׼� ���̱�
    /// </summary>
    /// <param name="argBool">���׼�Ȯ��</param>
    /// <returns></returns>
    IEnumerator ReactionActive(bool argBool)
    {
        reaction.SetActive(true);
        if (argBool)
        {
            reaction.GetComponent<Image>().sprite = reactionGood;
        }
        else
        {
            reaction.GetComponent<Image>().sprite = reactionBad;
        }

        yield return new WaitForSeconds(1.0f);
        reaction.SetActive(false);
    }

    public void GetOutBtn()
    {
        lastNum = waitQueue.Peek();
        if (waitQueue.Peek() != 2 && waitQueue.Peek() != 3)
        {
            StartCoroutine("OutAni");
            GetReaction(true);
            waitQueue.Dequeue();
            WaitGen();
            nowNum++;
            WaitMatch();
        }
        else
        {
            StartCoroutine("OutAni");
            AddScore(-10);
            SoundManager.Instance.PlayEffectSound(badSE);
            GetReaction(false);
            waitQueue.Dequeue();
            WaitGen();
            nowNum++;
            WaitMatch();
        }
    }

    IEnumerator GameStart()
    {
        GameStartText.text = "3";
        yield return new WaitForSeconds(1.0f);
        GameStartText.text = "2";
        yield return new WaitForSeconds(1.0f);
        GameStartText.text = "1";
        yield return new WaitForSeconds(1.0f);
        GameStartText.text = "���ӽ���";
        yield return new WaitForSeconds(1.0f);
        GameStartPanel.SetActive(false);
        StartCoroutine("Timer");

        for(int i = 0; i < 4; i++)
        {
            WaitGen();
        }
        WaitMatch();
    }

    void WaitGen()
    {
        int ranNum = Random.Range(0, 3);
        waitQueue.Enqueue(ranNum);
        waitList.Add(ranNum);   
    }

    void WaitMatch()
    {
        GetWaitSprite(nowNum, 0);
        GetWaitSprite(nowNum + 1, 1);
        GetWaitSprite(nowNum + 2, 2);
        GetWaitSprite(nowNum + 3, 3);
    }

    void GetWaitSprite(int num, int pos)
    {
        if (waitList[num] == 0)
        {
            waitObj[pos].GetComponent<SpriteRenderer>().sprite = waitSprite[0];
        }
        else if (waitList[num] == 1)
        {
            waitObj[pos].GetComponent<SpriteRenderer>().sprite = waitSprite[1];
        }
        else if (waitList[num] == 2)
        {
            waitObj[pos].GetComponent<SpriteRenderer>().sprite = waitSprite[2];
        }
        else if (waitList[num] == 3)
        {
            waitObj[pos].GetComponent<SpriteRenderer>().sprite = waitSprite[3];
        }
        else if (waitList[num] == 4)
        {
            waitObj[pos].GetComponent<SpriteRenderer>().sprite = waitSprite[4];
        }
    }

    public void OptionOn(GameObject Option)
    {
        Option.SetActive(true);
        Time.timeScale = 0;
    }

    public void OptionOff(GameObject Option)
    {
        Option.SetActive(false);
        Time.timeScale = 1;
    }

    public void Replay()
    {
        SceneController.Instance.LoadScene("RollerCoaster");
    }

    public void MiniGameExit()
    {
        SceneController.Instance.LoadScene("MainGame");
    }

    IEnumerator Timer()
    {
        time--;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine("Timer");
    }

    /// <summary>
    /// ��Ʈ���� ����
    /// </summary>
    public void EndGame()
    {
        GameValueManager.Instance.IsWorkimg = attractionsCode;
        ParkReputationManager.Instance.addReputation(5);
        AttractionPreferenceManager.Instance.AddPreference(attractionsCode - 1, 5);
        QuestManager.Instance.NpcQuestCheck(0, 1);
        SceneController.Instance.LoadScene("MainGame");
    }

    void GetResult()
    {
        goodText.text = "X " + goodNum.ToString();
        if (rollerCoasterScore <= 50)
        {
            ResultScoreImage[0].SetActive(false);
            ResultScoreImage[1].SetActive(false);
            ResultScoreImage[2].SetActive(false);
        }
        else if (rollerCoasterScore <= 100)
        {
            ResultScoreImage[1].SetActive(false);
            ResultScoreImage[2].SetActive(false);
        }
        else if (rollerCoasterScore <= 200)
        {
            ResultScoreImage[2].SetActive(false);
        }
        else
        {
            return;
        }
    }
}