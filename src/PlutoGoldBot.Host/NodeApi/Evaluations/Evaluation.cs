namespace PlutoGoldBot.Host.NodeApi.Evaluations;

public class Evaluation
{
    public JsonElement Message { get; set; }

    public EvaluationResult Result { get; set; } = new();
}