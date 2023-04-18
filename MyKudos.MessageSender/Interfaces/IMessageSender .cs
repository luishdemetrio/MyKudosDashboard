namespace MyKudos.MessageSender.Interfaces;

public interface IMessageSender
{

    Task CreateQueueIfNotExistsAsync(string topicName);

    Task CreateTopicIfNotExistsAsync(string topicName);

    Task SendTopic(object queueMessage, string topic, string subject);

    Task SendQueue(object queueMessage, string queueName);



}