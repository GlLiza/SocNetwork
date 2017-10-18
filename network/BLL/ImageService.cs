using System.Collections.Generic;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;

namespace network.BLL
{
    public class ImageService
    {
        private readonly IImagesRepository _imagesRepository;

        public ImageService()
        {
        }

        public ImageService(ImagesRepository imgRepository)
        {
            _imagesRepository = imgRepository;
        }


       
        public IEnumerable<Images> GetImages()
        {
            var images = _imagesRepository.GetImages();
            return images;
        }

        public void InsertImage(Images image)
        {
            _imagesRepository.AddImage(image);
        }

        public void EditUser(Images image)
        {
            _imagesRepository.UpdateImage(image);
        }

        public void DeleteImage(Images image)
        {
            Images img = _imagesRepository.GetImageById(image.Id);
            _imagesRepository.DeleteImage(img.Id);
        }

        public Images SearchImg(int? id)
        {
            Images img = _imagesRepository.GetImageById(id);
            return img;
        }

        public byte[] ReturnImage(int id)
        {
            byte[] imageData = _imagesRepository.GetImageById(id).Data;
            return imageData;
        }
        
    }
}