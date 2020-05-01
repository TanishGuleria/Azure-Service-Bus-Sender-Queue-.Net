using System;
using System.Threading;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Send_User_Servicebus_Queue
{
    class Program
    {
        static IQueueClient QueueClient;
        //static ITopicClient topic;

        const string ServiceBusConnectionString = "Endpoint=sb://studentservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=O5BCsQW4K2UvesE/JzS7WOMI5UdbsAC3MKzRfb+qIik="; // enter primary key name here
        const string QueueName = "studentqqueue"; // enter queue name here

        static async Task Main()
        {

           
            QueueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            await SendStudentMessage();

            Console.ReadKey();

            await QueueClient.CloseAsync();

        }


        static async Task SendStudentMessage()   //method to send the actual data
        {
            Program p = new Program();
            List<Student> list = p.GetdataTobeSent(); // geting data

            var serializeStudent = JsonConvert.SerializeObject(list); // serializing the data before sending it

            var messageType = "StudentMessage";
            var messageID = Guid.NewGuid().ToString();


            ServiceBusMessage serviceobj = new ServiceBusMessage
            {



                Id = messageID,
                Type = messageType,
                MassageContainer = serializeStudent
            
            };

            var finalseriaize = JsonConvert.SerializeObject(serviceobj);

            //send data to bus

            var busdata = new Message(Encoding.UTF8.GetBytes(finalseriaize)); // convert to bytes
            busdata.UserProperties.Add("type", messageType);
            busdata.MessageId = messageID;

            await QueueClient.SendAsync(busdata);

            Console.WriteLine("Message Has Been Sent");

            
        }
          

        


        public class Student         //Data to be sent as a message
        {
            public int Id { get; set; }
            
            public string StudentName { get; set; }
        }

        public class ServiceBusMessage     // a container of sent message
        {
            public string Id { get; set; }

            public string Type { get; set; }
            public string MassageContainer { get; set; }
        }

        public List<Student> GetdataTobeSent() // take raw input of data to be sent 
        {
            List<Student> list = new List<Student>();

            int NumberOfMessage = 10;
            for(int i=1;i<=NumberOfMessage;i++)
            {
                Student s = new Student();
                s.Id = i;
                s.StudentName = "DummyName" + i;

                list.Add(s);
            }
            return list;
        }



        





    }
}
