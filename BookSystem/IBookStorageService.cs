using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSystem
{
    interface IBookStorageService
    {
        void LoadFromStorage(string path);
        void SaveToStorage(string path);
    }
}
