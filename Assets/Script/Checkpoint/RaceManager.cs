using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public List<CarPosition> cars = new List<CarPosition>(); // List of all cars in the race
    public TextMeshProUGUI positionText; // UI text to display the position
    public FinishLine2PController finishLineTriggerPlayer1; // Finish line trigger for Player 1
    public FinishLine2PController finishLineTriggerPlayer2; // Finish line trigger for Player 2
    public FinishLineController finishLinePlayer1; // Finish line trigger for Player 1
    private void Update()
    {
        // Check if both players have completed 3 laps
        if (finishLineTriggerPlayer1.lapCount >= 3 && finishLineTriggerPlayer2.lapCount >= 3)
        {
            // Display the finish screen
            FinishGame.instanceClass.SetStatus(true);
        }
        // Check if both players have completed 3 laps
        if (finishLinePlayer1.lapCount >= 3)
        {
            // Display the finish screen
            FinishGame.instanceClass.SetStatus(true);
        }
        // Sort cars by their distance to the finish line 
        cars.Sort((a, b) => b.distanceTravelled.CompareTo(a.distanceTravelled));

        // Update the leaderboard UI
        UpdateLeaderboard();
    }

    // Update the leaderboard text
    private void UpdateLeaderboard()
    {
        string position = "Position:\n";
        for (int i = 0; i < cars.Count; i++)
        {
            position += $"{i + 1}. {cars[i].carName}\n";
        }
        positionText.text = position;
    }

    // Get the sorted leaderboard data
    public List<CarPosition> GetLeaderboard()
    {
        cars.Sort((a, b) => b.distanceTravelled.CompareTo(a.distanceTravelled));
        return cars;
    }

}
// Class to store car information
[System.Serializable]
public class CarPosition
{
    public string carName; // Name of the car 
    public float distanceTravelled; // Distance travelled by the car
    public Transform carTransform; // Reference to the car's transform
    public TimerController timerController; // Reference to the TimerController for lap times
}
