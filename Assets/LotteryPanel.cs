using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LotteryPanel : MonoBehaviour {

    // Use this for initialization
    public List<Text> textList;
    public List<Text> textList1;
    public List<Text> textList2;
    public List<Text> textList3;
    public float H;

    public AnimationCurve line;
    public Text goldText;
    public Image icon;
    public int randomInt;

    public List<int> currentList = new List<int>();
    //int current = 0;
    List<List<Text>> textLists = new List<List<Text>>();

    List<int> randomList = new List<int>();
    void Start () {
       

    }

    private void OnEnable()
    {
        //current = 0;
        textLists.Clear();
        AddTextList(ref textLists, textList);
        AddTextList(ref textLists, textList1);
        AddTextList(ref textLists, textList2);
        AddTextList(ref textLists, textList3);

        currentList.Clear();
        int R = 1;
        for (int i=0;i< textLists.Count; i++) {
            currentList.Add(0);
            R *= 10;
        }
        H = textList[0].GetComponent<RectTransform>().sizeDelta.y;

        //SetRandomList(Random.Range(0, R));
        SetRandomList(randomInt);
        StartCoroutine(OnInit());
    }

    void AddTextList(ref List<List<Text>> textLists,List<Text>tlist) {
        if (tlist != null && tlist.Count > 0)
        {
            textLists.Add(tlist);
        }
    }

    public void SetRandomList(int num ) {
        //int i = 0;
        Debug.Log("Random " + num);
        randomList.Clear();
        do
        {
            int n = num % 10;
            randomList.Add((n-2+10)%10);
            num = num / 10;
        } while (num > 0);
        for (int i= randomList.Count;i < textLists.Count; i++) {
            randomList.Add(8);
        }
    }

    IEnumerator OnInit() {
        yield return null;
        
       
        int counting = 0;
        for (int li = 0;li< textLists.Count;li++) {
            List<Text> tlist = textLists[li];
            float t = 100 + ((randomList.Count > li) ? (randomList[li]) :8);//  Random.Range(0, 10);
            int current = currentList[li];
            int index = li;
            counting++;
            DOTween.To(() => { return 0f; }, (float x) =>
            {
                int tcurrent = (current + (int)Mathf.Floor(x)) % 10;
                for (int i = 0; i < tlist.Count; i++)
                {
                    
                    tlist[i].GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(0, -H * line.Evaluate(x) -  H + i * H);
                    
                    tlist[i].text = ((tcurrent + i) % 10) + "";
                }
            }, t, t * 0.01f).SetEase(Ease.InCubic).onComplete = () =>
            {

                current = (current + (int)(Mathf.Floor(t))) % 10;
                Debug.Log("-----"+current+" "+t);
                for (int i = 0; i < tlist.Count; i++)
                {
                    tlist[i].text = ((current + i) % 10) + "";
                //Debug.Log("eeeee:"+( -H * line.Evaluate(x) + 2*H - index * H));
                }
                //Debug.Log(index+" "+current);
                currentList[index] = current;
                counting--;
                if (counting<=0) {
                    OnRollComplete();
                }
            };
        }
        //SetDelay(0)
       
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnRollComplete() {
        for (int i = 0; i < currentList.Count; i++)
        {
            Debug.Log(i + " " + (currentList[i]+2)%10);
        }

        StartCoroutine(Close());
    }

    public void OnClick() {
        for (int i = 0; i < textLists.Count; i++)
        {
            currentList[i]=0;
            //R *= 10;
        }
        SetRandomList(randomInt);
        StartCoroutine(OnInit());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(1f);
        //gameObject.SetActive(false);
    }
}
