using System.Collections;
using UnityEngine;

public class LevelAttackManager : MonoBehaviour
{
    [Header("Prefabs")]
    public EnemyBall ballPrefab;
    public EnemyBall ballSpikePrefab;
    public ChargingEnemyBall chargingBallPrefab;

    public void SpawnEnemyLine(Vector2 start, Vector2 end, EnemyLine.LineType type, float warningTime = 1.5f)
    {
        GameObject go = new GameObject("EnemyLine");
        go.transform.position = Vector3.zero;

        LineRenderer lr = go.AddComponent<LineRenderer>();
        BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
        EnemyLine newLine = go.AddComponent<EnemyLine>();

        newLine.lineType = type;
        newLine.warningDuration = warningTime;
        newLine.SetPositions(start, end);

        lr.useWorldSpace = true;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = newLine.lineWidth;
        lr.endWidth = newLine.lineWidth;
    }

    public void SpawnEnemyBall(Vector3 position, Vector3 direction, float speed)
    {
        EnemyBall ball = Instantiate(ballPrefab, position, Quaternion.identity);
        ball.gameObject.tag = "EnemyBall";
        ball.SetDirectionToTarget(position + direction);
        ball.SetSpeed(speed);
    }

    public void SpawnSpikeEnemyBall(Vector3 position, Vector3 direction, float speed)
    {
        EnemyBall ball = Instantiate(ballSpikePrefab, position, Quaternion.identity);
        ball.gameObject.tag = "EnemyBall";
        ball.SetDirectionToTarget(position + direction);
        ball.SetSpeed(speed);
    }

    public void SpawnParryBall(Vector3 position, Vector3 direction, float speed)
    {
        EnemyBall parryBall = Instantiate(ballPrefab, position, Quaternion.identity);
        parryBall.gameObject.tag = "ParryBall"; // Tag especial para parry
        parryBall.triggerPlayer = true;          // Para que trackee al jugador si es necesario
        parryBall.SetDirectionToTarget(position + direction);
        parryBall.SetSpeed(speed);
    }

    public void SpawnChargingBall(Vector3 position, Vector3 direction, float speed, float chargeTime)
    {
        ChargingEnemyBall chargingBall = Instantiate(chargingBallPrefab, position, Quaternion.identity);

        // Configurar el tiempo de carga
        chargingBall.chargeTime = chargeTime;

        // Configurar dirección y velocidad (usando su EnemyBall interno o ajustando tras el pop)
        EnemyBall enemyBall = chargingBall.GetComponent<EnemyBall>();
        if (enemyBall != null)
        {
            enemyBall.SetDirectionToTarget(position + direction);
            enemyBall.SetSpeed(speed);
        }

        chargingBall.gameObject.tag = "EnemyBall";
    }

}
