/// <summary>
/// Judgement must be able to take a user feed
/// and produce an output suitable for Evaluator.vote
/// </summary>
public interface Judgment {
    //should be guessing what the player might be doing
    Assessor.Package assess(UserFeed input);
    PlayerModel.PlayMode target();
    void cheat(int i, int index);//shouldnt exist
}
