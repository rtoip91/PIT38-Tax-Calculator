using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataAccess.Interfaces
{
    public interface IFileDataAccess
    {
        void SetFileName(string fileName);
        string GetFileName();
        string GetFileNameWithoutExtension();
    }
}