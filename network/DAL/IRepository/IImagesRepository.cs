using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;

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
        byte[] ReturnImage(string id);

        Images CompareDate(List<Images> list);
    }
}
