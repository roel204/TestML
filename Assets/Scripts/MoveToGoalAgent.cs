using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    private float minMoveSpeed = 1.0f;
    private float maxMoveSpeed = 4.0f;
    private float moveSpeed;
    private float turnSpeed = 200f;
    private float cumulativeReward = 0f;
    private float borderPenalty = -0.01f;
    private float proximityReward = 0.01f;
    private float contactReward = 0.1f;

    private Vector2 minBounds = new Vector2(-16f, -9f);
    private Vector2 maxBounds = new Vector2(16f, 9f);

    public override void Initialize()
    {
        Time.timeScale = 1;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        targetTransform.position = new Vector3(5, 2, 0);
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
        float rotation = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        moveSpeed = Mathf.Clamp(actions.ContinuousActions[1], minMoveSpeed, maxMoveSpeed);

        // Apply rotation
        transform.Rotate(Vector3.forward, rotation * turnSpeed * Time.deltaTime);

        // Move forward
        transform.position += transform.right * moveSpeed * Time.deltaTime;

        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

        // Add a small reward based on the distance to the target
        AddReward(proximityReward / distanceToTarget);
        cumulativeReward += proximityReward / distanceToTarget;

        // Add reward for contacting the target
        if (distanceToTarget < 0.5f)
        {
            Debug.Log("Reached target - reward");
            AddReward(contactReward);
            cumulativeReward += contactReward;
        }

        // Check if the fish is within bounds
        if (transform.position.x > maxBounds.x || transform.position.x < minBounds.x || transform.position.y > maxBounds.y || transform.position.y < minBounds.y)
        {
            // Penalize for hitting the border
            Debug.Log("Hit the border - penalty");
            AddReward(borderPenalty);
            cumulativeReward += borderPenalty;

            // Keep the fish within bounds
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
                Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),
                transform.position.z);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Mathf.Clamp(Input.GetAxisRaw("Horizontal"), -1f, 1f); // Rotation control
        continuousActions[1] = Mathf.Clamp(Input.GetAxisRaw("Vertical"), minMoveSpeed, maxMoveSpeed); // Speed control
    }
}
