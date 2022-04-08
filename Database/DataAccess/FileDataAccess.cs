using System.IO;
using Database.DataAccess.Interfaces;
using Database.Repository;

namespace Database.DataAccess
{
    public class FileDataAccess : IFileDataAccess
    {
        private readonly IDataRepository _dataRepository;

        public FileDataAccess(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void SetFileName(string fileName)
        {
            _dataRepository.InputFileName = fileName;
        }

        public string GetFileName()
        {
            return _dataRepository.InputFileName;
        }

        public string GetFileNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(_dataRepository.InputFileName);
        }
    }
}