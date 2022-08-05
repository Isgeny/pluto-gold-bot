namespace PlutoGoldBot.Host.NodeApi.Evaluations;

public class EvaluationExpression
{
    public string Expr { get; set; } = string.Empty;

    public static implicit operator EvaluationExpression(string expression) => new() { Expr = expression };
}