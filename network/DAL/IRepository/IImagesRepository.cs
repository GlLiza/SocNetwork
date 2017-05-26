using System;
using System.Linq;
using network.BLL.EF;
using WebGrease.Css.ImageAssemblyAnalysis;

namespace network.DAL.IRepository
{
    interface IImagesRepository:IDisposable
    {
        IQueryable<Images> GetImages ();
        void AddImage (Images images);
        void DeleteImage (int id);
        void UpdateImage (Images images);
        void Save ();

  
        Images GetImageById(int? id);

        byte[] ReturnImage(int id);
    }
}
