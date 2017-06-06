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
        void DeleteImage (string id);
        void UpdateImage (Images images);
        void Save ();

  
        Images GetImageById(string id);

        byte[] ReturnImage(string id);
    }
}
