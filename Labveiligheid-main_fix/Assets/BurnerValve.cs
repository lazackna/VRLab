using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnerValve : MonoBehaviour
{

    private float angle = 0;                //angle of hingejoint stored
    private int bunsenState = 1;            //current bunsen burner state
    private bool taskCompleted = false;     //stores if task is completed of bunsen burner
    public HingeJoint hingeJoint;           //the hingejoint of the valve
    public ParticleSystem burnSource;       //the particle system of the flame
    public AudioSource burnAudio;           //the audio source of the flame

    //see bunsen burner states (https://en.wikipedia.org/wiki/Bunsen_burner)
    public Gradient gradState1;             //Bunsen burner state 1
    public Gradient gradState2;             //Bunsen burner state 2
    public Gradient gradState3;             //Bunsen burner state 3
    public Gradient gradState4;             //Bunsen burner state 4

    private bool gasFlow = false;           //gasflow presence
    private bool isOn = false;              //gasflow and fire presence: flame on

    private float min;                      // min angle hingejoint
    private float max;                      // max angle hingejoint
    private float range;                    // range angle hingejoint
    private float step;                     // size of step bunsen burner state

    private int[] flameStatesAchieved = new int[4]; //stores if all 4 bunsen burner states are achieved

    // Start is called before the first frame update
    void Start()
    {
        //store and process hingejoint data
        this.angle = this.hingeJoint.angle;
        this.min = this.hingeJoint.limits.min;
        this.max = this.hingeJoint.limits.max;
        this.range = max - min;
        this.step = range / 4.0f;

        //get and stop particle system
        this.burnSource.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //state burner should be on
        if (this.gasFlow && this.isOn) {
            //if not playing already, play burn and audio
            if (!this.burnSource.isPlaying) { this.burnSource.Play(); this.burnAudio.Play(); }

            //set bunsen burner state
            this.setBunsenState(this.getState(this.hingeJoint.angle));
            this.flameStatesAchieved[this.bunsenState - 1] = 1;

            //heat up above
            this.heatRaycast();

        } else {

            //if burner is not on, stop particle system and audio source of flame
            if (this.burnSource.isPlaying) {
                this.burnSource.Stop();
                this.burnAudio.Stop();
            }
        }

        //if angle changed, process it
        if (this.angle != this.hingeJoint.angle) {
            this.angle = this.hingeJoint.angle;

            //see bunsen burner states (https://en.wikipedia.org/wiki/Bunsen_burner)

            //get bunsen state and set color accordingly
            if (this.setBunsenState(this.getState(this.angle))) {
                var col = this.burnSource.colorOverLifetime;
                col.enabled = true;
                switch (this.bunsenState)
                {
                    case 1:
                        col.color = this.gradState1;
                        break;
                    case 2:
                        col.color = this.gradState2;
                        break;
                    case 3:
                        col.color = this.gradState3;
                        break;
                    case 4:
                        col.color = this.gradState4;
                        break;
                    default:
                        break;
                }

                //if burner on, set current bunsen state to achieved
                if (this.gasFlow && this.isOn) {
                        this.flameStatesAchieved[this.bunsenState - 1] = 1;
                    }
            }

            //if all bunsen states achieved, communicate it to OverallManager. finished.
            if (
                this.flameStatesAchieved[0] == 1 &&
                this.flameStatesAchieved[1] == 1 &&
                this.flameStatesAchieved[2] == 1 &&
                this.flameStatesAchieved[3] == 1 &&
                !this.taskCompleted
            ) {
                this.taskCompleted = true;
                Debug.Log("Task completed: went through all the bunsen burner states");
                if(GameObject.Find("OverallManager") != null){
                    GameObject.Find("OverallManager").GetComponent<OverallManager>().finishTask("use the different flames with the burner");
                }
            }
        }
    }

    // Calculates which portion of angle the hingejoint is in and returns bunsen state accordingly.
    // see bunsen burner states (https://en.wikipedia.org/wiki/Bunsen_burner)
    int getState(float angle) {
        if (this.angle <= this.min + 1.0f * this.step) {
            //first quarter is state 1
            return 1;

        } else if (this.angle <= this.min + 2.0f * this.step) {
            //second quarter is state 2
            return 2;

        } else if (this.angle <= this.min + 3.0f * this.step) {
            //third quarter is state 3
            return 3;

        } else if (this.angle <= this.max) {
            //the rest, fourth quarter is state 4
            return 4;
        }
        return 0;
    }

    // Casts a ray up relative to the burners orientation to look for things to heat up
    private void heatRaycast() {
        //raycast up
        Ray ray = new Ray(this.burnSource.transform.position, Vector3.up);
        RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 10);

        if (hits != null){

            //goes through every object hit
            foreach (RaycastHit hit in hits)
            {
                //(debug, current iteration only beaker has LiquidManager)
                if (hit.collider.gameObject.name.Contains("beaker")) Debug.Log("HIT: " + hit.collider.gameObject.name);
                
                //if gameobject hit has LiquidManager, start their heat up
                hit.transform.gameObject.GetComponent<LiquidManager>()?.startHeatUp();
            }
        }
    }

    // Sets the bunsen state only if valid, safety check
    public bool setBunsenState(int state) {
        if (this.bunsenState != state && state > 0 && state <= 4) {
            this.bunsenState = state;
            return true;
        }
        return false;
    }

    // Gets gasflow for public use
    public bool getGasFlow() {
        return this.gasFlow;
    }

    // Set gasflow for public use
    public void setGasFlow(bool gasFlowing) {
        this.gasFlow = gasFlowing;
        if (!gasFlowing) this.isOn = false;
    }

    // Lucifer tries to make a fire
    public void lit() {
        //Debug.Log("Lit called");
        this.isOn = this.gasFlow;
    }
}
