using System;

namespace Logic
{
    public class Message
    {
        public string MessageText { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string QueueName { get; set; }

        public Message(string message, string queueName)
        {
            MessageText = message;
            QueueName = queueName;
            ReceivedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"\tText: {MessageText} \n\tDate: {ReceivedDate} \n\tName: {QueueName}";
        }
    }
}
