// interface for functions that are written to interact with objects through touching and using

public interface ObjectInteractionFunctionsInterface
{
    // don't forget that classes can inherit several interfaces
    void useObject();

    void unuseObject();

    void touchObject();

    void untouchObject();
}
