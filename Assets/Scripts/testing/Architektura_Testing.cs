using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING
{
    public class Architektura_Testing : MonoBehaviour
    {
        DialougeSystem ds;
        TexktArchitekt architekt;

        public TexktArchitekt.BuildMethod bm = TexktArchitekt.BuildMethod.instant;

        string[] lines = new string[2]
        {
             "This is a random",
             "line for testing"
        };
        // Start is called before the first frame update
        void Start()
        {
            ds = DialougeSystem.instance;
            architekt = new TexktArchitekt(ds.dialogueContainer.dialogueText);
            architekt.buildMethod = TexktArchitekt.BuildMethod.fade;
            architekt.speed = 0.5f; 
        }

        // Update is called once per frame
        void Update()
        {
            if (bm != architekt.buildMethod)
            {
                architekt.buildMethod = bm;
                architekt.Stop();
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                architekt.Stop();
            }

            string loongLine = "this is a very long line that andn srfk wndap djs nqwp jqd nqdw [dk k[dwqdk [kd qwd[pdk  p[wkd [qp k[pdkq [pw [kdqwk[d fkslldklsklkllj aaaaaaaaaaa jjjjjjjjjjj kkkkdkkskkkdll mmmmmmmmmmmmmmmmmmmmmmmm loooooooooooooooolllll kdkfkdkfdkf kkfdfk kf" ;
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (architekt.isBuilding)
                {
                    if (!architekt.hurryUp)
                        architekt.hurryUp = true;
                     else 
                        architekt.ForceComplete();
                }
                else
                    architekt.Build(loongLine);
                // architekt.Build(lines[Random.Range(0, lines.Length)]);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                architekt.Build(loongLine);
                // architekt.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}