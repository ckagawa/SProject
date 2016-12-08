
//player state singleton
public class PlayerState : StateMachine {
    //might think of some use for this eventually
    private static PlayerState ths = PlayerBuilder.Load();
    //will eventually remove builders and stick a file load here instead
    public static PlayerState generate() { return ths; }
}
