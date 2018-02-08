using System;
using System.Collections.Generic;
using DAL.EF;

namespace DAL.IRepository
{
    public interface IImagesRepository:IDisposable
    {
        void Add (Images images);
        void Delete (int id);
        void Update (Images images);

        IEnumerable<Images> GetImages();
        Images GetById(int? id);
        byte[] ReturnImage(string id);

        Images CompareDate(List<Images> list);
        
    }
}
