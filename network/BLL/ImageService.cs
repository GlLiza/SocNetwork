using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;

namespace network.BLL
{
    public class ImageService
    {
        NetworkContext db = new NetworkContext();

        private IImagesRepository imagesRepository;
 
        public ImageService()
        {
            imagesRepository = new ImagesRepository(db);
        }

        public IEnumerable<Images> GetImages()
        {
            var images = from s in db.Images
                select s;
            return images;
        }

        public void InsertImage(Images image)
        {
            imagesRepository.AddImage(image);
            imagesRepository.Save();
        }

        public void EditUser(Images image)
        {
            imagesRepository.UpdateImage(image);
            imagesRepository.Save();

        }

        public void DeleteImage(Images image)
        {
            Images img = imagesRepository.GetImageById(image.Id);
            imagesRepository.DeleteImage(img.Id);
            imagesRepository.Save();
        }

        public Images SearchImg(int? id)
        {
            Images img = imagesRepository.GetImageById(id);
            return img;
        }




    }
}