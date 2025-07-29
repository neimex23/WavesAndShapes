using UnityEngine;

public class LevelAttackManager : MonoBehaviour
{
    [Header("Prefabs")]
    public EnemyBall ballPrefab;
    public EnemyBall ballSpikePrefab;

    public void SpawnEnemyLine(Vector2 start, Vector2 end, EnemyLine.LineType type, float warningTime = 1.5f)
    {
        // Crear un GameObject vacío para la línea
        GameObject go = new GameObject("EnemyLine");
        go.transform.position = Vector3.zero;

        // Añadir componentes requeridos
        LineRenderer lr = go.AddComponent<LineRenderer>();
        BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
        EnemyLine newLine = go.AddComponent<EnemyLine>();

        // Configurar parámetros básicos
        newLine.lineType = type;
        newLine.warningDuration = warningTime;
        newLine.SetPositions(start, end);

        // Configuración visual mínima
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
}
