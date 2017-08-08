using System;
using System.Collections.Generic;
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

        public void NewRequest(Requests request)
        {
            context.Requests.Add(request);
        }

        public Requests SearchById(int id)
        {
            return context.Requests.Find(id);
        }

        public Requests SearchByUsersId(string idIng, string idEd)
        {
            var request =
                context.Requests.FirstOrDefault(s => s.Requesting_user_id == idIng && s.Requested_user_id == idEd);
            return request;
        }

        public IQueryable<Requests> SearchRequests(string id)
        {
            var request = context.Requests.
                Where(s => s.Requesting_user_id == id && s.Status_id == 1);
            return request;
        }


        public void CancelRequests(Requests request)
        {
            Requests req = context.Requests.Find(request.Id);
            req.FriendStatuses.Name = "Active";
            context.Entry(req).State=EntityState.Modified;

        }


        public void Update(Requests requests)
        {
            Requests req = context.Requests.Find(requests.Id);
            //context.Entry(requests).State=EntityState.Modified;
            context.Entry(requests).CurrentValues.SetValues(requests);
        }


        public IQueryable<Requests> ShowNewRequests(string id)
        {
            var list = context.Requests
                .Where(s => s.Requesting_user_id == id && s.Status_id == 1);
            return list;

        }

        public Requests ReturnRequests(string idEd, string idIng)
        {
            return context.Requests
                .FirstOrDefault(s => s.Requested_user_id == idEd && s.Requesting_user_id == idIng && s.Status_id==1);
        }


    }
}