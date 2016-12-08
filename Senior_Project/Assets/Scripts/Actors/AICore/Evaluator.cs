using System.Collections;
using System.Threading;
/// <summary>
/// Class in charge of evaluating fitness
/// note: In fully functional implementation should be abstract
/// </summary>
public class Evaluator{
    //
    private static int LOAD_LIMIT = 50;
    private static int sleepTimer = 200;
    private static Evaluator ths = new Evaluator();
    private static Evaluation logic = EvaluationBuilder.Load();
    //
    public UnifiedAI data;
    public Thread core;
    private Queue processQueue;
    public static PlayerModel.PlayMode[] checks = EvaluationBuilder.Set();//doesnt do anything right now
    public PlayerModel.PlayMode target;
    //singleton constructor
    private Evaluator()
    {
        processQueue = new Queue();
        core = new Thread(new ThreadStart(run));
        core.Priority = System.Threading.ThreadPriority.Highest;
    }
    /// <summary>
    /// Singleton getter
    /// </summary>
    /// <returns>instance of Evaluator</returns>
    public static Evaluator getInstance()
    {
        //would like to use pointers for this but apparently frowned upon
        return ths;
    }
    /// <summary>
    /// Evaluate fitness of data packet
    /// </summary>
    /// <param name="data">data to be evaluated</param>
    public void vote(Assessor.Package data)
    {
        if (data==null) return;//should be error here
        Evidence e = new Evidence();//create and log a new evidence
        if (data.src != null) e.src = data.src;
        e.type = target;//this line should be different but no time
        e.value = data.data;//this should also be different
        processQueue.Enqueue(e);
    }
    //thread method
    private void run()
    {
        while (true)
        {
            int x;
            Thread.Sleep(50);//creating issues
            lock(processQueue){ x = processQueue.Count; }
            if (x < 1) { Thread.Sleep(sleepTimer); }
            else
            {
                advance();
            }
            DH.ping("Evaluator Cycle");
        }
    }
    //basic check if there is anything to do
    private void advance()
    {
        Evidence next;
        lock (processQueue) { next = (Evidence)processQueue.Dequeue(); }
        if(next!=null)logic.Evaluate(next.type,next.value,next.src);
    }
    /// <summary>
    /// check if Evaluator can accept more inputs
    /// </summary>
    /// <returns>true if unable to accept new inputs, else false</returns>
    public bool overloaded()
    { return processQueue.Count > LOAD_LIMIT; }
    //check if is in a runnable state
    private bool working()
    {
        if (checks == null || checks.Length < 1||data==null) return false;
        return true;
    }
    /// <summary>
    /// used to link a stage's Unified AI
    /// note: may be called multiple times,but ideally no more than once per stage
    /// </summary>
    /// <param name="ai">Unified AI to be linked</param>
    public void link(UnifiedAI ai)
    {
        data = ai;
        logic.setAI(ai);
    }
    //Helper to hold data for fitness evaluation
    private class Evidence
    {
        public PlayerModel.PlayMode type { get; set; }//type of evaluation to be performed
        public int[] value { get; set; }//value to evaluate
        public string src { get; set; }
    }
}
