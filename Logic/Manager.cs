using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logic
{
    public class Manager
    {
        DoubleLinkedList<Message> messagesSystem;
        HashTable<string, LinkedListQueue<Message>> contactsHash;

        public Manager()
        {
            this.messagesSystem = new DoubleLinkedList<Message>();
            this.contactsHash = new HashTable<string, LinkedListQueue<Message>>();
        }


        //הצג שמות (מפתחות) של כל הקבוצות הקיימות במערכת

        /// <summary>
        /// Returns a list of all keys in the hash table
        /// </summary>
        /// <returns>list of keys</returns>
        public List<string> ShowContactNames() // O(n)
        {
            if (this.messagesSystem.Count == 0)
                throw new Exception("No contacts found!");

            else
                return this.contactsHash.GetKeys();
        }


        //הכנס הודעה לתור לפי מפתח, אם המפתח קיים - הטקסט ייכנס לתור שלו, אם המפתח לא קיים אז הוא מתווסף למערכת והטקסט ייכנס לתור שלו

        /// <summary>
        /// Adds a message to a contact's queue. 
        /// If there's no such contact in the hash table, it will be created with a new queue. 
        /// Once the queue exists, the message will be added to it.
        /// </summary>
        /// <param name="contactName">key in hash table</param>
        /// <param name="message">message that will be added to the queue(value) of the contact(key)</param>
        public void SendMessageToContact(string contactName, string message) // O(1) 
        {
            LinkedListQueue<Message> contactMessageQueue;
            // tries to get the value(message queue) for the key entered (contact name)
            try
            {
                contactMessageQueue = this.contactsHash.GetValue(contactName.ToLower());
            }

            // if there's no such key (contact name), this will add it to the hash table with it's value (new queue)
            catch
            {
                contactMessageQueue = new LinkedListQueue<Message>();
                this.contactsHash.Add(contactName.ToLower(), contactMessageQueue);
            }

            // if the queue already exists, this will add a message to it
            if (contactMessageQueue != null)
            {
                Message messageToContact = new Message(message, contactName);
                contactMessageQueue.EnQueue(messageToContact);
                messagesSystem.AddLast(messageToContact);
            }
        }


        // שלוף(הוצא) הודעה מהתור לפי מפתח – מוציא את הודעה הוותיקה ביותר בתור הזה 
        // אם המפתח המבוקש לא קיים או אין הודעות בתור המבוקש  - פונקציה לסמן כישלון  

        /// <summary>
        /// Removes the oldest message from the contact queue. If the hashtable contains such contact name,
        /// the oldest message will be displayed using Peek() and then removed form the queue using DeQueue.
        /// It will also be removed from the messages system.
        /// </summary>
        /// <param name="contactName">key in hash table</param>
        /// <returns>Text of the oldest message</returns>
        /// <exception cref="Exception">execption raised if there's a queue with no messages or if there's no such contact</exception>
        public string GetAndRemoveOldestMessageFromContactQueue(string contactName) //Get + Remove // O(1)
        {
            if (this.contactsHash.ContainsKey(contactName.ToLower()))
            {
                LinkedListQueue<Message> contactMessageQueue = this.contactsHash.GetValue(contactName.ToLower());

                Message oldestMessage = contactMessageQueue.Peek();
                contactMessageQueue.DeQueue();

                RemoveMessageFromMessagesSystem(oldestMessage);

                if (oldestMessage == null) 
                    throw new Exception("There are no more messages for this contact!");
                
                else 
                    return oldestMessage.MessageText;

            }
            else
            {
                throw new Exception("This contact doesn't exist!");
            }
        }

        /// <summary>
        /// Private method that removes a message from the double linked list of messages system.
        /// Using RemoveAt() method which gets access to a specific node at given 'index' and removes it from the list.
        /// This method is called by the public method: GetAndRemoveOldestMessageFromContactQueue. 
        /// This is done since we want to remove the deleted message in queue from the messages system as well.
        /// The way it works is by comapring datetime of removed message from queue and messages in messages system, if datetime match, message will be deleted from messages system.
        /// </summary>
        /// <param name="message">message in queue that we compare to message in system by date</param>
        private void RemoveMessageFromMessagesSystem(Message message)
        {
            if (message == null) 
                return;
            
            DoubleLinkedList<Message>.Node messageToDelete = this.messagesSystem.GetFirstNode();

            int index = 1;
            
            do
            {
                if (messageToDelete.value.ReceivedDate == message.ReceivedDate)
                {
                    this.messagesSystem.RemoveAt(index);
                    break;
                }

                messageToDelete = messageToDelete.next;
                index++;
            }
            while (messageToDelete.next != null);
         
        }

        // שלוף את ההודעה הותיקה ביותר בכל המערכת

        /// <summary>
        /// Gets and removes the oldest message from the messages system and also from the apropriate queue of that contact.
        /// </summary>
        /// <returns>shows the message that is removed</returns>
        public Message GetAndRemoveOldestMessageFromMessagesSystem() //Get + Remove // O(1)
        {
            if (messagesSystem.Count != 0)
            {
                Message message = messagesSystem.GetFirstValue();
                messagesSystem.RemoveFirst();

                LinkedListQueue<Message> contactMessageQueue = this.contactsHash.GetValue(message.QueueName.ToLower());
                contactMessageQueue.DeQueue();

                return message;
            }
            else
                throw new Exception("The Messages System is empty, no messages to delete!");
        }


        // החזר בלי למחוק מהמערכת את איקס ההודעות הכי ישנות או הכי חדשות

        /// <summary>
        /// Gets the newest or the oldest n messages from the messages system
        /// </summary>
        /// <param name="num">number of messages to show</param>
        /// <param name="isNewest">if it's true - newest messages will be shown, if it's false oldest messages will be shown</param>
        /// <returns>list of newest or oldest messages</returns>
        public List<Message> GetNMessagesFromSystem(int num, bool isNewest)
        {
            List<Message> systemMessages = new List<Message>();

            DoubleLinkedList<Message>.Node message = isNewest ? this.messagesSystem.GetLastNode() : this.messagesSystem.GetFirstNode();

            if (message == null)
                throw new Exception("No messages to show!");

            for (int i = 0; i < num; i++)
            {
                if (message == null)
                    break;

                systemMessages.Add(message.value);

                message = isNewest ? message.previous : message.next;
            }
            return systemMessages;
        }

        // החזר בלי למחוק מהמערכת את כל ההודעות החדשות ביותר/ישנות ביותר מתאריך נתון והחזר - הודעה עצמה, תאריך בו התקבלה, לאיזה תור נשלחה במקור

        /// <summary>
        /// Returns all oldest or newest messages than a given date from the system
        /// </summary>
        /// <param name="date">date from which we want newest or oldest messages</param>
        /// <param name="isNewest">if it's true - newest messages will be shown, if it's false oldest messages will be shown</param>
        /// <returns>list of messages oldest or newest than a given datr</returns>
        public List<Message> GetMessagesByDateFromSystem(DateTime date, bool isNewest)
        {
            List<Message> messagesByDate = new List<Message>();

            DoubleLinkedList<Message>.Node message = isNewest ? this.messagesSystem.GetLastNode() : this.messagesSystem.GetFirstNode();

            while (message != null)
            {
                if ((isNewest && message.value.ReceivedDate > date) || (!isNewest && message.value.ReceivedDate < date))
                    messagesByDate.Add(message.value);

                message = isNewest ? message.previous : message.next;
            }

            if (messagesByDate.Count == 0)
                throw new Exception("No messages found :(");

            return messagesByDate;
        }

        //מצא בתור מסוים (לפי מפתח) את כל ההודעות שמכילות מילה מסוימת והחזר: הודעה עצמה, תאריך בו התקבלה, לאיזה תור נשלחה במקןר

        /// <summary>
        /// Finds all messages that contain a specific word in a queue of a specific contact.
        /// </summary>
        /// <param name="contactName">name of the key in the hash table which queue we want to access</param>
        /// <param name="word">the word the user wants to find</param>
        /// <returns>list of found words in a specific queue</returns>
        /// <exception cref="Exception">in case the queue is empty from messages/no contact was found, an exeption will be raised.</exception>
        public List<Message> SearchContactMessagesByWord(string contactName, string word)
        {
            if (this.contactsHash.ContainsKey(contactName.ToLower()))
            {
                LinkedListQueue<Message> contactMessageQueue = this.contactsHash.GetValue(contactName.ToLower());

                string regexPattern = string.Format(@"\b{0}\b", Regex.Escape(word));
                Regex regex = new Regex(regexPattern, RegexOptions.IgnoreCase);


                DataStructures.LinkedListNode<Message> message = contactMessageQueue.GetFirstNode();
                List<Message> foundMessages = new List<Message>();

                while (message != null)
                {
                    Message currentMessage = message.Value;
                    string textMessage = currentMessage.MessageText;

                    if (regex.IsMatch(textMessage))
                        foundMessages.Add(currentMessage);

                    message = message.next;
                }

                if (foundMessages.Count == 0 || word == "")
                    throw new Exception("No messages containing this word were found :(");

                return foundMessages;
            }
            else
                throw new Exception("This contact doesn't exist :(");
        }
    }
}
