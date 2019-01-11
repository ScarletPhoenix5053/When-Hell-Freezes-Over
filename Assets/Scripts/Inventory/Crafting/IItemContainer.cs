public interface IItemContainer
{
    
    int ItemCount(GenericItem item);
    bool ContainsItem(GenericItem item);
    bool RemoveItem(GenericItem item);
    bool AddItem(GenericItem item);
    bool IsFull();
}
