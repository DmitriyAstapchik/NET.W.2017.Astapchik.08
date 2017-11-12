namespace BookSystem
{
    interface IBookStorageService
    {
        void LoadFromStorage(string path);
        void SaveToStorage(string path);
    }
}
