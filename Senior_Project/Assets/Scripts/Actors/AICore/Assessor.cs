
/// <summary>
/// Class for observing player behavior
/// Intended to observe player and estimate their intention based on their actions
/// and fluctuations in their status
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
    /// Getter method, only proper way to receive an Assessor
    /// </summary>
    /// <returns>Instance of Assessor</returns>
    public static Assessor getInstance()
    {
        return ths;
    }
    /// <summary>
    /// Receive Data Packets from Unit Manager
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
    /// <summary>
    /// Method used for Debugging
    /// Allows modification of assessments
    /// Would not exist in a fully functional Assessor
    /// </summary>
    /// <param name="value"></param>
    /// <param name="array"></param>
    public void cheat(int value, int array)
    {
        judge.cheat(value, array);
    }
    /// <summary>
    /// Get Current player goal
    /// </summary>
    /// <returns>PlayMode for current player goal</returns>
    public PlayerModel.PlayMode target()
    {
        return judge.target();
    }
    /// <summary>
    /// Helper for storing values related to assessor
    /// </summary>
    public class Package
    {
        public int[] data { get; set; }
        public string src { get; set; }
    }
}
