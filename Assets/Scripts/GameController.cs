using UnityEngine;
using cc = ClickChange;
public class GameController : MonoBehaviour
{
    private void OnMouseDown()
    {
        if(!cc.End)
        { 
            cc.TimeClick = Time.time;
            cc.HintFalse();
            if (cc.HintCube)
                cc.HintCube.transform.localScale = new Vector3(1, 1, 1);
            if (cc.FirstClick)
            {
                cc.FirstColor = GetComponent<Renderer>().material.color;
                gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                cc.FirstCube = gameObject;
                cc.Clicked();
            }
            else if(gameObject.Equals(cc.FirstCube))
            {
                cc.Clicked();
                cc.FirstCube.transform.localScale = Vector3.one;
            }
            else if((cc.CheckClick(cc.FirstCube, gameObject) && cc.CheckAllow(gameObject, cc.FirstColor, cc.FirstCube))||
                    (cc.CheckClick(gameObject, cc.FirstCube) && cc.CheckAllow(cc.FirstCube, GetComponent<Renderer>().material.color, gameObject)))
            {
                cc.FirstCube.transform.localScale = Vector3.one;
                cc.SecondColor = GetComponent<Renderer>().material.color;
                cc.FirstCube.GetComponent<Renderer>().material.color = cc.SecondColor;
                GetComponent<Renderer>().material.color = cc.FirstColor;
                cc.Clicked();
                cc.DeleteCube();
            }
        }
    }
}
