public interface IItemContainer
{

    //int ItemCount(GenericItem item);
    int ItemCount(string itemID);
    GenericItem RemoveItem(string itemID);
    //bool ContainsItem(GenericItem item);
    bool RemoveItem(GenericItem item);
    bool AddItem(GenericItem item);
    bool IsFull();
}
