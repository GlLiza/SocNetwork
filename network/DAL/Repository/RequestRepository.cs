using System;
using System.Data.Entity;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private NetworkContext context;

        public RequestRepository(NetworkContext context)
        {
            this.context = context;
        }

        

        private bool disposed = false;

        protected virtual void Dispose(bool disposed)
        {
            if (!this.disposed)
            {
                if (disposed)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }


        public void AddRequest(Requests request)
        {
            context.Requests.Add(request);
        }

        public void CancelRequests(Requests request)
        {
            if (request == null)
            {
                request.FriendStatuses.Name = "Active";
                context.Entry(request).State = EntityState.Modified;
            }
        }

        public void Update(Requests requests)
        {
            context.Entry(requests).CurrentValues.SetValues(requests);
        }

        public Requests SearchById(int id)
        {
            return context.Requests.Find(id);
        }
        
        public Requests SearchByUsersId(string idIng, string idEd)
        {
            return context.Requests.FirstOrDefault(s => s.Requesting_user_id == idIng && s.Requested_user_id == idEd);
        }
        

        //возвращает активный список запросов
        public IQueryable<Requests> GetActiveRequests(string id)
        {
            return context.Requests.Where(s => s.Requesting_user_id == id && s.Status_id == 1);
           
        }


        //возвращает активный запрос для определенных пользователей
        public Requests CheckRequests(string idEd, string idIng)
        {
            return context.Requests.FirstOrDefault(s => s.Requested_user_id == idEd && s.Requesting_user_id == idIng && s.Status_id==1);
        }


    }
}