using System.Collections.Generic;
using System.Threading.Tasks;
using UserPortal.Entities;
using UserPortal.Interfaces;
using UserPortal.Models;
using System;
using System.Linq;
using UserPortal.Enums;
using System.Text.Json;
using Confluent.Kafka;

namespace UserPortalTest
{
    public class KafkaServiceFake : IKafkaService
    {
        public Task<List<User>> GetUserList(PageRequest request)
        {
            var list = DummyData.userList;
            return Task.FromResult(list);
        }

        public Task<User> RegisterApprovement(RegisterApprovementRequest request)
        {
            var user = DummyData.userList.FirstOrDefault(x => x.Id == request.UserId);
            if (user == null)
                throw new Exception("not exist");
            user.Approvement = request.Approvement;
            if (request.Approvement == false)
                DummyData.userList.Remove(user);
            return Task.FromResult(user);
        }

        public Task<User> ChangeStatus(ChangeStatusRequest request)
        {
            var user = DummyData.userList.FirstOrDefault(x => x.Id == request.UserId);
            if (user == null)
                throw new Exception("not exist");
            user.Active = request.Active;
            return Task.FromResult(user);
        }

        public Task<DeliveryResult<Null, string>> ResponseForTopicMesseageType(TopicMessageModel model)
        {
            switch (model.Type)
            {
                case (int)TopicMessageType.RegisterConfirm:
                    var registerApp = JsonSerializer.Deserialize<RegisterApprovementRequest>(model.Data.ToString());
                    var data = RegisterApprovement(registerApp);
                    model.Data = data;
                    break;
                case (int)TopicMessageType.ChangeStatus:
                    var changeStatusReq = JsonSerializer.Deserialize<ChangeStatusRequest>(model.Data.ToString());
                    var sdata = ChangeStatus(changeStatusReq);
                    model.Data = sdata;
                    break;
                case (int)TopicMessageType.UserList:
                    var request = JsonSerializer.Deserialize<PageRequest>(model.Data.ToString());
                    var list = GetUserList(request);
                    model.Data = list;
                    break;
                default:
                    break;
            }
            return null;
        }


    }
}