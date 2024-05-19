using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveFishAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private FishControl fishControl;

    // Rewards and Penalties
    private float borderPenalty = -0.1f;
    private float proximityReward = 0.01f;
    //private float contactReward = 1f;

    private float cumulativeReward = 0f;
    private Vector3 initialPosition;

    public override void Initialize()
    {
        Time.timeScale = 1;
        initialPosition = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        // Reset positions with random values
        transform.position = initialPosition + new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        //targetTransform.position = new Vector3(Random.Range(-8, 8), -8, 0);

        // Show the total reward from the previous run
        Debug.Log("Episode ended. Total reward: " + cumulativeReward);
        cumulativeReward = 0f;
    }

    // Give the AI info about the environment
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(targetTransform.position);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        // The inputs from the AI
        float rotationInput = actions.ContinuousActions[0];
        float speedInput = actions.ContinuousActions[1];
        Debug.Log("AI Rotation:" + rotationInput);
        Debug.Log("AI Speed:" + speedInput);

        // Move the fish using the FishControl script
        fishControl.Move(rotationInput, speedInput);

        CalcReward();
    }

    // Calculate all the rewards & penalties
    private void CalcReward()
    {
        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(fishControl.transform.position, targetTransform.position);

        // Add a small reward based on the distance to the target
        Debug.Log("Distance Reward:" + proximityReward / distanceToTarget);
        AddReward(proximityReward / distanceToTarget);
        cumulativeReward += proximityReward / distanceToTarget;

        // Add reward for maintaining a horizontal orientation
        float horizontalReward = Mathf.Abs(Mathf.Cos(fishControl.transform.eulerAngles.z * Mathf.Deg2Rad));
        Debug.Log("Rotation Reward:" + horizontalReward * 0.01f);
        AddReward(horizontalReward * 0.01f); // Adjust reward scale as needed
        cumulativeReward += horizontalReward * 0.01f;

        // Add reward for contacting the target
        //if (distanceToTarget < 0.5f)
        //{
        //    AddReward(contactReward);
        //    cumulativeReward += contactReward;
        //}

        // Check if the fish is out of bounds and apply penalty
        if (fishControl.IsOutOfBounds())
        {
            Debug.Log("Border Penalty:" + borderPenalty);
            AddReward(borderPenalty);
            cumulativeReward += borderPenalty;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        // Get input from the player
        float rotationInput = Input.GetAxisRaw("Horizontal");
        float speedInput = Input.GetAxisRaw("Vertical");

        // Clamp the input values
        float clampedRotation = Mathf.Clamp(-rotationInput, -1f, 1f);
        float clampedSpeed = Mathf.Clamp(speedInput, -1f, 1f);

        // Assign the actions
        continuousActions[0] = clampedRotation; // Rotation control
        continuousActions[1] = clampedSpeed; // Speed control
    }
}
