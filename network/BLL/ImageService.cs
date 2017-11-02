using System.Collections.Generic;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;

namespace network.BLL
{
    public class ImageService
    {
        private readonly IImagesRepository _imagesRepository;
        private readonly IUserRepository _userRepository;

        public ImageService()
        {
        }

        public ImageService(ImagesRepository imgRepository,UserRepository userRepository)
        {
            _imagesRepository = imgRepository;
            _userRepository = userRepository;
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

        public byte[] ReturnImage(int? id)
        {
            byte[] imageData = _imagesRepository.GetImageById(id).Data;
            return imageData;
        }
        
               
        public byte[] GetProfilesPhoto(int UserId)
        {
            var user = _userRepository.GetUserById(UserId);
            var image = _imagesRepository.GetImageById(user.ImagesId);
            return image.Data;
        }

    }
}