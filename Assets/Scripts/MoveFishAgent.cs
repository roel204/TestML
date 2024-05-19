using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveFishAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private FishControl fishControl;
    private float cumulativeReward = 0f;
    private float borderPenalty = -0.1f;
    private float proximityReward = 0.01f;
    private float contactReward = 1f;

    public override void Initialize()
    {
        Time.timeScale = 1;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        targetTransform.position = new Vector3(-8, -8, 0);

        Debug.Log("Episode ended. Total reward: " + cumulativeReward);
        cumulativeReward = 0f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float rotationInput = actions.ContinuousActions[0];
        float speedInput = actions.ContinuousActions[1];

        // Move the fish using the FishControl script
        fishControl.Move(rotationInput, speedInput);

        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(fishControl.transform.position, targetTransform.position);

        // Add a small reward based on the distance to the target
        AddReward(proximityReward / distanceToTarget);
        cumulativeReward += proximityReward / distanceToTarget;

        // Add reward for contacting the target
        if (distanceToTarget < 0.5f)
        {
            AddReward(contactReward);
            cumulativeReward += contactReward;
        }

        // Check if the fish is out of bounds and apply penalty
        if (fishControl.IsOutOfBounds())
        {
            AddReward(borderPenalty);
            cumulativeReward += borderPenalty;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Mathf.Clamp(Input.GetAxisRaw("Horizontal"), -1f, 1f); // Rotation control
        continuousActions[1] = Mathf.Clamp(Input.GetAxisRaw("Vertical"), fishControl.minMoveSpeed, fishControl.maxMoveSpeed); // Speed control
    }
}
