namespace BookSystem
{
    /// <summary>
    /// methods for work with a book storage
    /// </summary>
    internal interface IBookStorageService
    {
        /// <summary>
        /// load info from a storage
        /// </summary>
        /// <param name="path">path to storage file</param>
        void LoadFromStorage(string path);

        /// <summary>
        /// save info to storage
        /// </summary>
        /// <param name="path">path to storage file</param>
        void SaveToStorage(string path);
    }
}
