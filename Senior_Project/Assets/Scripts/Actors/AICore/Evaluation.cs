/// <summary>
/// Interprets fitness
/// class should take judgments and use them to alter AI
/// </summary>
public interface Evaluation {
    //in practice should typically require a command set to do anything useful
    void Evaluate(PlayerModel.PlayMode mode,int[] value,string src);
    void setAI(UnifiedAI commands);
}
