using Confluent.Kafka;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserPortal.Entities;
using UserPortal.Models;

namespace UserPortal.Interfaces
{
    public interface IKafkaService
    {
        public Task<DeliveryResult<Null, string>> ResponseForTopicMesseageType(TopicMessageModel model);
        public Task<List<User>> GetUserList(PageRequest request);
        public Task<User> RegisterApprovement(RegisterApprovementRequest request);
        public Task<User> ChangeStatus(ChangeStatusRequest request);
    }
}
