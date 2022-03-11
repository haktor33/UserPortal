using System.Collections.Generic;
using System.Threading.Tasks;
using UserPortal.Entities;
using UserPortal.Models;

namespace UserPortal.Interfaces
{
    public interface IKafkaService
    {
        public Task ResponseForTopicMesseageType(TopicMessageModel model);

        public  Task<List<User>> GetUserList(PageRequest request);

        public  Task<User> RegisterApprovement(RegisterApprovementRequest request);
  
    }
}
