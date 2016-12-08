
/// <summary>
/// Class for observing player behavior
/// </summary>
public class Assessor  {

    // TODO --remember to fix one of these
    private static Judgment judge = JudgementBuilder.Load();
    private static Assessor ths = new Assessor();
    public Evaluator Out { get; set; }
    private Assessor()
    {
    }
    /// <summary>
    /// singleton getter
    /// </summary>
    /// <returns></returns>
    public static Assessor getInstance()
    {
        return ths;
    }
    /// <summary>
    /// UM feed stage data to assessor here
    /// </summary>
    /// <param name="notice">UserFeed containing player info</param>
    public void update(UserFeed notice)
    {
        if (Out == null) ;//should throw error here
        else
        {
            if (Out.overloaded()) DH.ping("queue overflow");
            else
            {
                Out.vote(judge.assess(notice));
            }
        }
    }
    public void cheat(int value, int array)
    {
        judge.cheat(value, array);
    }
    /// <summary>
    /// get the current predicted player goal
    /// </summary>
    /// <returns></returns>
    public PlayerModel.PlayMode target()
    {
        return judge.target();
    }
    public class Package
    {
        public int[] data { get; set; }
        public string src { get; set; }
    }
}
